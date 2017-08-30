using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NLog;
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
        private AllProvablePrimesWithConditionsGenerator _smallPrimeGen = new AllProvablePrimesWithConditionsGenerator(new HashFunction { Mode = ModeValues.SHA2, DigestSize = DigestSizes.d256 });
        private IRandom800_90 _rand = new Random800_90();

        // For Moq
        public TestGroupGeneratorGeneratedDataTest(RandomProbablePrimeGenerator gen, AllProvablePrimesWithConditionsGenerator gen2)
        {
            _primeGen = gen;
            _smallPrimeGen = gen2;
        }

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            var pubExpMode = RSAEnumHelpers.StringToPubExpMode(parameters.PubExpMode);

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
                                if(pubExpMode == PubExpModes.RANDOM)
                                {
                                    E = RSAEnumHelpers.GetEValue();
                                }
                                else
                                {
                                    E = new BitString(parameters.FixedPubExpValue).ToPositiveBigInteger();
                                }

                                // Use a tested PrimeGen for 1024-bit RSA
                                if(sigCapability.Modulo == 1024)
                                {
                                    _smallPrimeGen.SetBitlens(RSAEnumHelpers.GetBitlens(1024, KeyGenModes.B34));
                                    primeGenResult = _smallPrimeGen.GeneratePrimes(sigCapability.Modulo, E, RSAEnumHelpers.GetSeed(sigCapability.Modulo));
                                }
                                // Use a fast PrimeGen for other RSA
                                else
                                {
                                    primeGenResult = _primeGen.GeneratePrimes(sigCapability.Modulo, E, RSAEnumHelpers.GetSeed(sigCapability.Modulo));
                                }

                                if (!primeGenResult.Success)
                                {
                                    ThisLogger.Warn($"Error generating key for {sigCapability.Modulo}, {primeGenResult.ErrorMessage}");
                                }

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

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
