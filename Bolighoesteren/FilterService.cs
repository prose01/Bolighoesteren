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

        public List<IEjendom> GetProperties(string postcode, string photoFolderPath)
        {
            return _abstractFilter.GetProperties(postcode, photoFolderPath);
        }
    }
}
