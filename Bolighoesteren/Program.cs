using FilterLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Bolighoesteren
{
    public class Program
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                // Get the execution directory
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

                _logger.Info("Starter Bolighoesteren.");

                string json = File.ReadAllText("appsettings.json");
                var settings = JsonConvert.DeserializeObject<Appsettings>(json);

                // Create data folders
                var dataFolderPath = Path.Combine(settings.DataDumpLocation, DateTime.Today.ToString("dd-MM-yyyy") + "-BoligData");

                var photoFolderPath = Path.Combine(dataFolderPath, "Photos");

                Directory.CreateDirectory(dataFolderPath);
                Directory.CreateDirectory(photoFolderPath);

                List<Postcode> postcodes = new List<Postcode>();

                foreach (var postcode in settings.Postnumre)
                {
                    Thread.Sleep(settings.ThreadSleep);

                    List<Ejendom> dbProperties = new List<Ejendom>();

                    using (var context = new Context())
                    {
                        if (int.TryParse(postcode, out int postnummer))
                        {
                            dbProperties = context.Ejendomme.OrderByDescending(p => p.Postnummer == postnummer).ToList();
                        }
                    }
                    
                    List<IEjendom> properties = new List<IEjendom>();

                    AbstractFilterFactory factory = new ConcreteFilterFactory();
                    FilterService client = new FilterService(_logger, factory, settings.FilterName, postcode);

                    properties = client.GetAddress(properties);

                    properties = client.GetPrice(properties);

                    properties = client.GetPhoto(properties, photoFolderPath);

                    properties = client.GetLink(properties);

                    properties = client.GetTableInfo(properties);

                    if (settings.SaveToDatabase)
                    {
                        foreach (var property in properties)
                        {
                            if (int.TryParse(postcode, out int postnummer))
                            {
                                property.Postnummer = postnummer;
                            }
                            var prop_json = JsonConvert.SerializeObject(property);
                            property.HashCode = prop_json.GetHashCode();

                            using (var context = new Context())
                            {
                                if (dbProperties.SingleOrDefault(p => p.Adresse == property.Adresse && p.HashCode == property.HashCode) != null)
                                {
                                    continue;
                                }

                                if (dbProperties.SingleOrDefault(p => p.Adresse == property.Adresse && p.HashCode != property.HashCode) != null)
                                {
                                    var dbProperty = dbProperties.SingleOrDefault(p => p.Adresse == property.Adresse && p.HashCode != property.HashCode);
                                    dbProperty.Pris = property.Pris;
                                    dbProperty.Link = property.Link;
                                    dbProperty.Foto = property.Foto;
                                    dbProperty.HashCode = property.HashCode;
                                    context.Ejendomme.Update(dbProperty);
                                    context.SaveChanges();
                                }
                                else
                                {
                                    context.Ejendomme.Add((Ejendom)property);
                                    context.SaveChanges();
                                }
                            }

                            if (settings.Console)
                            {
                                Console.WriteLine(JsonConvert.SerializeObject(property));
                                Console.ReadLine();
                            }
                        }
                    }
                    
                    if(settings.SaveToFile)
                    {
                        // Collect data
                        Postcode item = new Postcode() { Postnummer = postcode };

                        item.Ejendomme = properties;

                        postcodes.Add(item);                        
                    }
                }

                if(settings.SaveToFile)
                {
                    // Write json to file
                    using (StreamWriter writer = new StreamWriter(dataFolderPath + "\\" + DateTime.Today.ToString("dd -MM-yyyy") + "-BoligData.json"))
                    {
                        writer.Write(JsonConvert.SerializeObject(postcodes));
                    }
                }

                if (settings.Console)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(postcodes));
                    Console.ReadLine();
                }

                _logger.Info("Lukker Bolighoesteren.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Something bad happened");
            }
        }
    }
}
