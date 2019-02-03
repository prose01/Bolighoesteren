using System.Collections.Generic;

namespace FilterLibrary
{
    public abstract class AbstractFilter
    {
        public abstract List<IProperty> GetAddress(List<IProperty> properties);

        public abstract List<IProperty> GetPrice(List<IProperty> properties);

        public abstract List<IProperty> GetPhoto(List<IProperty> properties, string photoFolderPath);

        public abstract List<IProperty> GetLink(List<IProperty> properties);

        public abstract List<IProperty> GetTableInfo(List<IProperty> properties);
    }
}
