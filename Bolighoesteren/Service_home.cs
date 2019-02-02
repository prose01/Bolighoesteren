using System;
using System.Collections.Generic;
using System.Linq;

namespace Bolighoesteren
{
    class Service_home : AbstractService
    {
        public override List<Property> GetAddress(HtmlAgilityPack.HtmlDocument doc, List<Property> properties)
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
                //_logger.Error(ex, "Something bad happened");
                //Console.WriteLine(ex);
            }

            return properties;
        }
    }
}
