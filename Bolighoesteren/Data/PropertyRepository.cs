using FilterLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bolighoesteren.Data
{
    public class PropertyRepository
    {

        public List<Ejendom> RemoveOrphanedProperties(List<Ejendom> dbProperties, List<IEjendom> properties)
        {
            var orphanedProperties = dbProperties.Where(n => !properties.Select(n1 => n1.Adresse).Contains(n.Adresse));

            if (orphanedProperties != null)
            {
                using (var context = new Context())
                {
                    context.Ejendomme.RemoveRange(orphanedProperties);
                    context.SaveChanges();
                }

                return dbProperties.Except(orphanedProperties).ToList();
            }

            return null;
        }

        public void SavePropertyToDatabase(IEjendom property, List<Ejendom> dbProperties, string postcode,  bool console)
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
                    return;
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

            if (console)
            {
                Console.WriteLine(JsonConvert.SerializeObject(property));
                Console.ReadLine();
            }
        }

        public List<Ejendom> GetPropertiesByPostCode(string postcode)
        {
            using (var context = new Context())
            {
                if (int.TryParse(postcode, out int postnummer))
                {
                    return context.Ejendomme.OrderByDescending(p => p.Postnummer == postnummer).ToList();
                }

                return null;
            }
        }
    }
}
