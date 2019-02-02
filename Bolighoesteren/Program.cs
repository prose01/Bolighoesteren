using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Bolighoesteren
{
    public class Program
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                // Get the execution directory
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

                logger.Info("Starter Bolighoesteren.");

                string json = File.ReadAllText("appsettings.json");
                var settings = JsonConvert.DeserializeObject<Appsettings>(json);

                // Create data folders
                var dataFolderPath = Path.Combine(settings.DataDumpLocation, DateTime.Today.ToString("dd-MM-yyyy") + "-BoligData");

                var photoFolderPath = Path.Combine(dataFolderPath, "Photos");

                Directory.CreateDirectory(dataFolderPath);
                Directory.CreateDirectory(photoFolderPath);

                // Collect data
                List<Postcode> postcodes = new List<Postcode>();

                foreach (var postcode in settings.Postnumre)
                {
                    Thread.Sleep(settings.ThreadSleep);

                    Postcode item = new Postcode() { Postnummer = postcode };

                    List<Property> properties = new List<Property>();

                    var url = settings.Url + "?postnr=" + postcode + "&antal=1000&side=1#lstsor";

                    HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
                    HtmlAgilityPack.HtmlDocument doc = web.Load(url);

                    HtmlAgilityPackService service = new HtmlAgilityPackService(logger);

                    properties = service.GetAddress(doc, properties);


                    // Abstract stuff
                    AbstractServiceFactory factory = new ConcreteServiceFactory();
                    Client client = new Client(factory);
                    var tt = client.GetAddress(doc, properties);


                    properties = service.GetPrice(doc, properties);

                    properties = service.GetPhoto(doc, properties, photoFolderPath);

                    properties = service.GetLink(doc, properties);

                    properties = service.GetTableInfo(doc, properties);

                    item.Ejendomme = properties;

                    postcodes.Add(item);
                }

                // Write json to file
                using (StreamWriter writer = new StreamWriter(dataFolderPath + "\\" + DateTime.Today.ToString("dd -MM-yyyy") + "-BoligData.json"))
                {
                    writer.Write(JsonConvert.SerializeObject(postcodes));
                }

                if (settings.Console)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(postcodes));

                    Console.ReadLine();
                }

                logger.Info("Lukker Bolighoesteren.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Something bad happened");
                //Console.WriteLine(ex);
            }
        }
    }
}
