using System.Collections.Generic;

namespace Bolighoesteren
{
    abstract class AbstractService
    {
        public abstract List<Property> GetAddress(HtmlAgilityPack.HtmlDocument doc, List<Property> properties);
    }
}
