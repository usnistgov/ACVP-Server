using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Enums;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.RSA2.PrimeGenerators
{
    public interface IPrimeGeneratorFactory
    {
        IPrimeGenerator GetPrimeGenerator(PrimeGenModes primeGen, ISha sha, IEntropyProvider entropyProvider, PrimeTestModes primeTest);
    }
}
