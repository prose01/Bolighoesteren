using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace FilterLibrary
{
    public class Filter_edc : AbstractFilter
    {
        private readonly NLog.Logger _logger;
        private readonly HtmlAgilityPack.HtmlWeb _web = new HtmlAgilityPack.HtmlWeb();
        private readonly HtmlAgilityPack.HtmlDocument _doc;

        public Filter_edc(NLog.Logger logger, string postcode)
        {
            _logger = logger;
            var url = "https://www.edc.dk/sog/?postnr=" + postcode + "&antal=1000&side=1#lstsor";
            _doc = _web.Load(url);
        }

        public override List<IEjendom> GetAddress(List<IEjendom> properties)
        {
            try
            {
                // Address
                var addressHeaders = _doc.DocumentNode.SelectNodes("//h2").ToList();

                foreach (var item in addressHeaders)
                {
                    if (item.InnerText.Any(char.IsDigit))
                    {
                        properties.Add(new Ejendom { Adresse = item.InnerText });
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

        public override List<IEjendom> GetPrice(List<IEjendom> properties)
        {
            try
            {
                // Price
                var prices = _doc.DocumentNode.SelectNodes("//div[@class='propertyitem__price']").ToList();

                int i = 0;
                foreach (var item in prices)
                {
                    var tempString = item.InnerText.Replace(".", string.Empty);
                    var price = Regex.Match(tempString, @"\d+").Value;

                    if (!string.IsNullOrEmpty(price))
                    {
                        price = price.Insert(price.Length - 3, ".");
                        if (price.Length > 7) price = price.Insert(price.Length - 7, ".");
                        properties[i].Pris = price.Trim();
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

        public override List<IEjendom> GetPhoto(List<IEjendom> properties, string photoFolderPath)
        {
            try
            {
                // Photo
                var photo = _doc.DocumentNode.SelectNodes("//*[@class='propertyitem__body--view']/span/img").ToList();

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

        public override List<IEjendom> GetLink(List<IEjendom> properties)
        {
            try
            {
                // Links
                var links = _doc.DocumentNode.SelectNodes("//*[@class='propertyitem propertyitem--list']/a").ToList();

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

        public override List<IEjendom> GetTableInfo(List<IEjendom> properties)
        {
            try
            {
                // Table info
                var tables = _doc.DocumentNode.SelectNodes("//*[@class='propertyitem__wrapper']/table").ToList();

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
                            if (int.TryParse(value, out int val)) { }

                            switch (key)
                            {
                                case "m²":
                                    properties[i].Areal = val;
                                    break;
                                case "Grund":
                                    properties[i].GrundAreal = val;
                                    break;
                                case "Rum":
                                    properties[i].Rum = val;
                                    break;
                                case "Byggeår":
                                    properties[i].Byggeår = val;
                                    break;
                                case "Liggetid":
                                    properties[i].Liggetid = value;
                                    break;
                                case "+/-":
                                    properties[i].Prisudvikling = value;
                                    break;
                                case "Pris/m²":
                                    properties[i].KvadratmeterPris = value;
                                    break;
                                case "Ejerudgifter pr. md.":
                                    properties[i].Ejerudgifter = value;
                                    break;
                                case "Boligyd. / Forbrugsafh.":
                                    properties[i].BoligydForbrugsafh = value;
                                    break;
                            }
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
