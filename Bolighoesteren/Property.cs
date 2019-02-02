using System.Collections.Generic;

namespace Bolighoesteren
{
    public class Property
    {
        public string Address { get; set; }
        public string Price { get; set; }
        public string Link { get; set; }
        public Dictionary<string, string> tableInfo { get; set; }
        public string Photo { get; set; }
    }
}
