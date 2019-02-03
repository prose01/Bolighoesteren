using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterLibrary
{
    public class Property : IProperty
    {
        public string Address { get; set; }
        public string Price { get; set; }
        public string Link { get; set; }
        public Dictionary<string, string> tableInfo { get; set; }
        public string Photo { get; set; }
    }
}
