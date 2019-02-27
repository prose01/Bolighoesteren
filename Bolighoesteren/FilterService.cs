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

        public List<IEjendom> GetAddress(List<IEjendom> properties)
        {
            return _abstractFilter.GetAddress(properties);
        }

        public List<IEjendom> GetPrice(List<IEjendom> properties)
        {
            return _abstractFilter.GetPrice(properties);
        }
        
        public List<IEjendom> GetPhoto(List<IEjendom> properties, string photoFolderPath)
        {
            return _abstractFilter.GetPhoto(properties, photoFolderPath);
        }

        public List<IEjendom> GetLink(List<IEjendom> properties)
        {
            return _abstractFilter.GetLink(properties);
        }

        public List<IEjendom> GetTableInfo(List<IEjendom> properties)
        {
            return _abstractFilter.GetTableInfo(properties);
        }
    }
}
