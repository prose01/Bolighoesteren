using System;
using FilterLibrary;

namespace Bolighoesteren
{
    class ConcreteFilterFactory : AbstractFilterFactory
    {
        public override AbstractFilter CreateFilter(NLog.Logger logger, FilterName filterName, string postcode)
        {
            try
            {
                switch (filterName)
                {
                    case FilterName.edc 
                        : return new Filter_edc(logger, postcode);

                    case FilterName.real
                        :
                        return new Filter_real(logger, postcode);

                    case FilterName.estate 
                        :
                        return new Filter_estate(logger, postcode);

                    case FilterName.boligsiden
                        :
                        return new Filter_boligsiden(logger, postcode);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Something bad happened");
                //Console.WriteLine(ex);
            }

            return new Filter_edc(logger, postcode);
        }
    }
}
