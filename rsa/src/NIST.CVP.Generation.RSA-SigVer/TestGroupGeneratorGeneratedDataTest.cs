using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class TestGroupGeneratorGeneratedDataTest : ITestGroupGenerator<Parameters>
    {
        public const string TEST_TYPE = "GDT";
        private RandomProbablePrimeGenerator _primeGen = new RandomProbablePrimeGenerator(EntropyProviderTypes.Random);
        private IRandom800_90 _rand = new Random800_90();

        // For Moq
        public TestGroupGeneratorGeneratedDataTest(RandomProbablePrimeGenerator gen)
        {
            _primeGen = gen;
        }

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var mode in parameters.SigGenModes)
            {
                foreach (var modulo in parameters.Moduli)
                {
                    foreach (var capability in parameters.Capabilities)
                    {
                        // Get a key for the group
                        PrimeGeneratorResult primeGenResult = null;
                        BigInteger E = 3;
                        do
                        {
                            E = GetEValue();
                            primeGenResult = _primeGen.GeneratePrimes(modulo, E, GetSeed(modulo));
                        } while (!primeGenResult.Success);

                        var testGroup = new TestGroup
                        {
                            Mode = RSAEnumHelpers.StringToSigGenMode(mode),
                            Modulo = modulo,
                            HashAlg = SHAEnumHelpers.StringToHashFunction(capability.HashAlg),
                            SaltLen = capability.SaltLen,
                            Key = new KeyPair(primeGenResult.P, primeGenResult.Q, E),

                            TestType = TEST_TYPE
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }

            return testGroups;
        }

        private BigInteger GetEValue()
        {
            BigInteger e;
            var min = BigInteger.Pow(2, 32) + 1;
            var max = BigInteger.Pow(2, 64);
            do
            {
                e = _rand.GetRandomBigInteger(min, max);
                if (e.IsEven)
                {
                    e++;
                }
            } while (e >= max);      // sanity check

            return e;
        }

        private BitString GetSeed(int modulo)
        {
            var security_strength = 0;
            if (modulo == 2048)
            {
                security_strength = 112;
            }
            else if (modulo == 3072)
            {
                security_strength = 128;
            }

            return _rand.GetRandomBitString(2 * security_strength);
        }
    }
}
