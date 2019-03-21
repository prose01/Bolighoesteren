using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace FilterLibrary
{
    public class Filter_real : AbstractFilter
    {
        private readonly NLog.Logger _logger;
        private readonly HtmlAgilityPack.HtmlWeb _web = new HtmlAgilityPack.HtmlWeb();
        private readonly HtmlAgilityPack.HtmlDocument _doc;

        public Filter_real(NLog.Logger logger, string postcode)
        {
            _logger = logger;
            var url = "https://www.realmaeglerne.dk/bolig?search=" + postcode + "&pricemin=&pricemax=&boligarealmin=&boligarealmax=&grundarealmin=&grundarealmax=&subsidymin=&subsidymax=";
            _doc = _web.Load(url);
        }

        public override List<IEjendom> GetProperties(string postcode, string photoFolderPath)
        {
            throw new NotImplementedException();
        }

        private List<IEjendom> GetAddress(List<IEjendom> properties)
        {
            try
            {
                // Address
                var addressHeaders = _doc.DocumentNode.SelectNodes("//div[@class='data']/h4").ToList();
                var city = _doc.DocumentNode.SelectNodes("//div[@class='data']/p[@data-attr='city']").ToList();

                if (addressHeaders.Count == city.Count)
                {
                    for (var i = 0; i < addressHeaders.Count; i++)
                    {
                        var adress = addressHeaders[i].InnerText + " " + city[i].InnerText;
                        properties.Add(new Ejendom { Adresse = adress.Replace("\r", string.Empty) });
                        //Console.WriteLine(item.InnerText);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Something bad happened");
                //Console.WriteLine(ex);
            }

            return properties;
        }

        private List<IEjendom> GetPrice(List<IEjendom> properties)
        {
            try
            {
                // Price
                var prices = _doc.DocumentNode.SelectNodes("//div[@class='price']/p/span[@data-attr='price']").ToList();

                int i = 0;
                foreach (var item in prices)
                {
                    properties[i].Pris = item.InnerText.Replace("kr.", string.Empty).Trim();
                    i++;
                    //Console.WriteLine(price);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Something bad happened");
                //Console.WriteLine(ex);
            }

            return properties;
        }

        private List<IEjendom> GetPhoto(List<IEjendom> properties, string photoFolderPath)
        {
            try
            {
                // Photo
                var photo = _doc.DocumentNode.SelectNodes("//*[@class='img-wrapper']/img").ToList();

                int i = 0;
                foreach (var item in photo)
                {
                    var path = item.Attributes["src"].Value;

                    if (!string.IsNullOrEmpty(path))
                    {
                        using (WebClient client = new WebClient())
                        {
                            string fileName = properties[i].Adresse;

                            client.DownloadFile(new Uri(path), $"{photoFolderPath}\\{properties[i].Adresse}.jpeg");
                            properties[i].Foto = path;
                            i++;
                        }
                    }

                    //Console.WriteLine(item.Attributes["data-src"].Value);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Something bad happened");
                //Console.WriteLine(ex);
            }

            return properties;
        }

        private List<IEjendom> GetLink(List<IEjendom> properties)
        {
            try
            {
                // Links
                var links = _doc.DocumentNode.SelectNodes("//*[@class='box']/a").ToList();

                int i = 0;
                foreach (var item in links)
                {
                    var link = item.Attributes["href"].Value;
                    if (!link.StartsWith("https://")) link = @"https://www.realmaeglerne.dk" + link;
                    properties[i].Link = link;
                    i++;

                    //Console.WriteLine(link);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Something bad happened");
                //Console.WriteLine(ex);
            }

            return properties;
        }

        private List<IEjendom> GetTableInfo(List<IEjendom> properties)
        {
            try
            {
                // Table info
                var Ejerudgifter = _doc.DocumentNode.SelectNodes("//div[@class='price']/p").ToList();

                int i = 0;

                foreach (var item in Ejerudgifter)
                {
                    var tempString = item.InnerText.Replace("Ejerudgift kr.", string.Empty).Trim();
                    tempString = tempString.Replace(".", string.Empty);
                    tempString = Regex.Match(tempString, @"\d+").Value;
                    tempString = tempString.Insert(tempString.Length - 3, ".");
                    properties[i].Ejerudgifter = tempString;

                    i++;
                    //Console.WriteLine(price);
                }
                
                var specs = _doc.DocumentNode.SelectNodes("//div[@class='data']/p[@data-attr='specs']").ToList();       // Kun nogle ejendomme har specs, og man skal trykke på "Se flere" før man kan se dem. Men htmlagilitypack kan IKKE klikke.
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Something bad happened");
                //Console.WriteLine(ex);
            }

            return properties;
        }
    }
}
