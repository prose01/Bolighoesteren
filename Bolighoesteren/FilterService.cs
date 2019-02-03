using FilterLibrary;
using System.Collections.Generic;

namespace Bolighoesteren
{
    class FilterService
    {
        private AbstractFilter _abstractFilter;

        public FilterService(NLog.Logger logger, AbstractFilterFactory factory, FilterName filterName, string postcode)
        {
            _abstractFilter = factory.CreateFilter(logger, filterName, postcode);
        }

        public List<IProperty> GetAddress(List<IProperty> properties)
        {
            return _abstractFilter.GetAddress(properties);
        }

        public List<IProperty> GetPrice(List<IProperty> properties)
        {
            return _abstractFilter.GetPrice(properties);
        }
        
        public List<IProperty> GetPhoto(List<IProperty> properties, string photoFolderPath)
        {
            return _abstractFilter.GetPhoto(properties, photoFolderPath);
        }

        public List<IProperty> GetLink(List<IProperty> properties)
        {
            return _abstractFilter.GetLink(properties);
        }

        public List<IProperty> GetTableInfo(List<IProperty> properties)
        {
            return _abstractFilter.GetTableInfo(properties);
        }
    }
}
