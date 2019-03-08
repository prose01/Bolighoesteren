using Bolighoesteren.Data;
using FilterLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
                        PropertyRepository repo = new PropertyRepository();

                        List<Ejendom> dbProperties = repo.GetPropertiesByPostCode(postcode);

                        dbProperties = repo.RemoveOrphanedProperties(dbProperties, properties);

                        foreach (var property in properties)
                        {
                            repo.SavePropertyToDatabase(property, dbProperties, postcode, settings.Console);
                        }
                    }
                    
                    if(settings.SaveToFile)
                    {
                        // Collect data by Postcode so its easier to read the json.
                        Postcode item = new Postcode() { Postnummer = postcode };

                        item.Ejendomme = properties;

                        postcodes.Add(item);                        
                    }
                }

                if(settings.SaveToFile)
                {
                    // Write json to file.
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
