using System.Collections.Generic;

namespace FilterLibrary
{
    public interface IProperty
    {
        string Address { get; set; }
        string Price { get; set; }
        string Link { get; set; }
        Dictionary<string, string> tableInfo { get; set; }
        string Photo { get; set; }
    }
}