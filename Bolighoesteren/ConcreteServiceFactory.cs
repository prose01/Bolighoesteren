namespace Bolighoesteren
{
    class ConcreteServiceFactory : AbstractServiceFactory
    {
        public override AbstractService CreateService()
        {
            return new Service_edc();
        }
    }
}
