using System.Collections.Generic;

namespace FilterLibrary
{
    public abstract class AbstractFilter
    {
        public abstract List<IEjendom> GetAddress(List<IEjendom> properties);

        public abstract List<IEjendom> GetPrice(List<IEjendom> properties);

        public abstract List<IEjendom> GetPhoto(List<IEjendom> properties, string photoFolderPath);

        public abstract List<IEjendom> GetLink(List<IEjendom> properties);

        public abstract List<IEjendom> GetTableInfo(List<IEjendom> properties);
    }
}
