using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.RSA
{
    public class PrimeGeneratorFactory
    {
        public PrimeGeneratorBase GetPrimeGenerator(string type)
        {
            if (type == "3.2")
            {
                return new RandomProvablePrimeGenerator();
            }
            else if (type == "3.3")
            {
                return new RandomProbablePrimeGenerator();
            }

            return null;
        }
    }
}
