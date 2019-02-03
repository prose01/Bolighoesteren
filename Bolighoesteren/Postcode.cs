using FilterLibrary;
using System.Collections.Generic;

namespace Bolighoesteren
{
    public class Postcode
    {
        public string Postnummer { get; set; }
        public List<IProperty> Ejendomme { get; set; }
    }
}
