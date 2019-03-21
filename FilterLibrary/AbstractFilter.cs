using System.Collections.Generic;

namespace FilterLibrary
{
    public abstract class AbstractFilter
    {
        public abstract List<IEjendom> GetProperties(string postcode, string photoFolderPath);
    }
}
