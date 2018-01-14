using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NLog;
using System.Collections.Generic;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Common.Hash.SHA2;

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

            foreach (var capability in parameters.Capabilities)
            {
                var sigType = capability.SigType;

                foreach (var moduloCap in capability.ModuloCapabilities)
                {
                    var modulo = moduloCap.Modulo;

                    foreach (var hashPair in moduloCap.HashPairs)
                    {
                        for (var i = 0; i < 3; i++)
                        {
                            // Get a key for the group
                            PrimeGeneratorResult primeGenResult = null;
                            BigInteger E;
                            do
                            {
                                if (pubExpMode == PubExpModes.RANDOM)
                                {
                                    E = RSAEnumHelpers.GetEValue();
                                }
                                else
                                {
                                    E = new BitString(parameters.FixedPubExpValue).ToPositiveBigInteger();
                                }

                                // Use a tested PrimeGen for 1024-bit RSA
                                if (modulo == 1024)
                                {
                                    _smallPrimeGen.SetBitlens(RSAEnumHelpers.GetBitlens(1024, KeyGenModes.B34));
                                    primeGenResult = _smallPrimeGen.GeneratePrimes(modulo, E, RSAEnumHelpers.GetSeed(modulo));
                                }
                                // Use a fast PrimeGen for other RSA
                                else
                                {
                                    primeGenResult = _primeGen.GeneratePrimes(modulo, E, RSAEnumHelpers.GetSeed(modulo));
                                }

                                if (!primeGenResult.Success)
                                {
                                    ThisLogger.Warn($"Error generating key for {modulo}, {primeGenResult.ErrorMessage}");
                                }

                            } while (!primeGenResult.Success);

                            var testGroup = new TestGroup
                            {
                                Mode = RSAEnumHelpers.StringToSigGenMode(sigType),
                                Modulo = modulo,
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
