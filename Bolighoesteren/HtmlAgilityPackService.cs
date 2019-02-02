using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Bolighoesteren
{
    public class HtmlAgilityPackService
    {
        private NLog.Logger _logger;

        public HtmlAgilityPackService(NLog.Logger logger)
        {
            _logger = logger;
        }

        public List<Property> GetAddress(HtmlAgilityPack.HtmlDocument doc, List<Property> properties)
        {
            try
            {
                // Address
                var addressHeaders = doc.DocumentNode.SelectNodes("//h2").ToList();

                foreach (var item in addressHeaders)
                {
                    if (item.InnerText.Any(char.IsDigit))
                    {
                        properties.Add(new Property { Address = item.InnerText });
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

        public List<Property> GetPrice(HtmlAgilityPack.HtmlDocument doc, List<Property> properties)
        {
            try
            {
                // Price
                var prices = doc.DocumentNode.SelectNodes("//div[@class='propertyitem__price']").ToList();

                int i = 0;
                foreach (var item in prices)
                {
                    var tempString = item.InnerText.Replace(".", string.Empty);
                    var price = Regex.Match(tempString, @"\d+").Value;

                    if (!string.IsNullOrEmpty(price))
                    {
                        price = price.Insert(price.Length - 3, ".");
                        if (price.Length > 7) price = price.Insert(price.Length - 7, ".");
                        properties[i].Price = price;
                        i++;
                        //Console.WriteLine(price);
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

        public List<Property> GetPhoto(HtmlAgilityPack.HtmlDocument doc, List<Property> properties, string photoFolderPath)
        {
            try
            {
                // Photo
                var photo = doc.DocumentNode.SelectNodes("//*[@class='propertyitem__body--view']/span/img").ToList();

                int i = 0;
                foreach (var item in photo)
                {
                    var path = item.Attributes["data-src"].Value;

                    if (!string.IsNullOrEmpty(path))
                    {
                        using (WebClient client = new WebClient())
                        {
                            if (path.StartsWith("/Static/Images"))
                            {
                                continue;
                            }

                            if (!path.StartsWith("http://")) path = @"http:" + path;

                            var lastIndex = path.LastIndexOf(@"/");
                            string fileName = path.Substring(lastIndex + 1);

                            client.DownloadFile(new Uri(path), $"{photoFolderPath}\\{fileName}");
                            properties[i].Photo = path;
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

        public List<Property> GetLink(HtmlAgilityPack.HtmlDocument doc, List<Property> properties)
        {
            try
            {
                // Links
                var links = doc.DocumentNode.SelectNodes("//*[@class='propertyitem propertyitem--list']/a").ToList();

                int i = 0;
                foreach (var item in links)
                {
                    var link = item.Attributes["href"].Value;
                    if (!link.StartsWith("https://")) link = @"https://www.edc.dk" + link;
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

        public List<Property> GetTableInfo(HtmlAgilityPack.HtmlDocument doc, List<Property> properties)
        {
            try
            {
                // Table info
                var tables = doc.DocumentNode.SelectNodes("//*[@class='propertyitem__wrapper']/table").ToList();

                int i = 0;
                foreach (var table in tables)
                {
                    for (int p = 0; p < table.ChildNodes[1].ChildNodes.Count; p++)
                    {
                        string key = string.Empty;
                        string value = string.Empty;

                        var th = table.ChildNodes[1].ChildNodes[p];
                        if (th.Name == "th")
                        {
                            key = th.InnerText;
                        }

                        var td = table.ChildNodes[2].ChildNodes[p];
                        if (td.Name == "td")
                        {
                            value = td.InnerText;
                        }

                        if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                        {
                            if (properties[i].tableInfo == null) properties[i].tableInfo = new Dictionary<string, string>();

                            properties[i].tableInfo.Add(key, value);
                        }
                    }

                    i++;
                }
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
