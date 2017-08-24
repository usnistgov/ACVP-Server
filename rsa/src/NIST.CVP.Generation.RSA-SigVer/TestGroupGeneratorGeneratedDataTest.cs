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

            foreach (var mode in parameters.SigVerModes)
            {
                foreach(var sigCapability in parameters.Capabilities)
                {
                    foreach(var hashPair in sigCapability.HashPairs)
                    {
                        // Make 3 identical groups with different keys
                        for (var i = 0; i < 3; i++)
                        {
                            // Get a key for the group
                            PrimeGeneratorResult primeGenResult = null;
                            BigInteger E;
                            do
                            {
                                E = RSAEnumHelpers.GetEValue();
                                primeGenResult = _primeGen.GeneratePrimes(sigCapability.Modulo, E, RSAEnumHelpers.GetSeed(sigCapability.Modulo));
                            } while (!primeGenResult.Success);

                            var testGroup = new TestGroup
                            {
                                Mode = RSAEnumHelpers.StringToSigGenMode(mode),
                                Modulo = sigCapability.Modulo,
                                HashAlg = SHAEnumHelpers.StringToHashFunction(hashPair.HashAlg),
                                SaltLen = hashPair.SaltLen,
                                Key = new KeyPair(primeGenResult.P, primeGenResult.Q, E),

                                TestType = TEST_TYPE
                            };

                            testGroups.Add(testGroup);
                        }
                    }
                }
            }

            return testGroups;
        }
    }
}
