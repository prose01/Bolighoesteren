using System.Collections.Generic;

namespace Bolighoesteren
{
    class Client
    {
        private AbstractService _abstractService;

        public Client(AbstractServiceFactory factory)
        {
            _abstractService = factory.CreateService();
        }

        public List<Property> GetAddress(HtmlAgilityPack.HtmlDocument doc, List<Property> properties)
        {
            return _abstractService.GetAddress(doc, properties);
        }
    }
}
