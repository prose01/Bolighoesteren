using FilterLibrary;

namespace Bolighoesteren
{
    abstract class AbstractFilterFactory
    {
        public abstract AbstractFilter CreateFilter(NLog.Logger logger, FilterName filterName, string postcode);
    }
}
