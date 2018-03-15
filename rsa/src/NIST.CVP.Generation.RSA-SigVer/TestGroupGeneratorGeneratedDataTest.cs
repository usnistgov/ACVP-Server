using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NLog;
using System.Collections.Generic;
using System.Numerics;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.RSA_SigVer.TestCaseExpectations;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class TestGroupGeneratorGeneratedDataTest : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "GDT";
        
        private readonly IRandom800_90 _rand;
        private readonly IKeyBuilder _keyBuilder;
        private readonly IKeyComposerFactory _keyComposerFactory;
        private readonly IShaFactory _shaFactory;

        public TestGroupGeneratorGeneratedDataTest(IRandom800_90 rand, IKeyBuilder keyBuilder, IKeyComposerFactory keyComposerFactory, IShaFactory shaFactory)
        {
            _rand = rand;
            _keyBuilder = keyBuilder;
            _keyComposerFactory = keyComposerFactory;
            _shaFactory = shaFactory;
        }

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            var pubExpMode = EnumHelpers.GetEnumFromEnumDescription<PublicExponentModes>(parameters.PubExpMode);
            var keyFormat = EnumHelpers.GetEnumFromEnumDescription<PrivateKeyModes>(parameters.KeyFormat);

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
                            KeyResult keyResult = null;
                            var primeGenMode = PrimeGenModes.B33;
                            do
                            {
                                BigInteger e;
                                if (pubExpMode == PublicExponentModes.Fixed)
                                {
                                    e = new BitString(parameters.FixedPubExpValue).ToPositiveBigInteger();
                                }
                                else
                                {
                                    e = GetEValue(32, 64);
                                }

                                keyResult = _keyBuilder
                                    .WithSeed(GetSeed(modulo))
                                    .WithNlen(modulo)
                                    .WithEntropyProvider(new EntropyProvider(_rand))
                                    .WithKeyComposer(_keyComposerFactory.GetKeyComposer(keyFormat))
                                    .WithPublicExponent(e)
                                    .WithPrimeGenMode(primeGenMode)
                                    .Build();

                                if (!keyResult.Success)
                                {
                                    ThisLogger.Warn($"Error generating key for {modulo}, {keyResult.ErrorMessage}");
                                }

                            } while (!keyResult.Success);

                            var testGroup = new TestGroup
                            {
                                Mode = EnumHelpers.GetEnumFromEnumDescription<SignatureSchemes>(sigType),
                                Modulo = modulo,
                                HashAlg = ShaAttributes.GetHashFunctionFromName(hashPair.HashAlg),
                                SaltLen = hashPair.SaltLen,
                                Key = keyResult.Key,
                                TestCaseExpectationProvider = new TestCaseExpectationProvider(parameters.IsSample),

                                TestType = TEST_TYPE
                            };

                            testGroups.Add(testGroup);
                        }
                    }
                }
            }

            return testGroups;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();

        private BigInteger GetEValue(int minLen, int maxLen)
        {
            BigInteger e;
            BitString e_bs;
            do
            {
                var min = minLen / 2;
                var max = maxLen / 2;

                e = GetRandomBigIntegerOfBitLength(_rand.GetRandomInt(min, max) * 2);
                if (e.IsEven)
                {
                    e++;
                }

                e_bs = new BitString(e);
            } while (e_bs.BitLength >= maxLen || e_bs.BitLength < minLen);

            return e;
        }

        private BigInteger GetRandomBigIntegerOfBitLength(int len)
        {
            var bs = _rand.GetRandomBitString(len);
            return bs.ToPositiveBigInteger();
        }

        private BitString GetSeed(int modulo)
        {
            var security_strength = 0;
            if(modulo == 1024)
            {
                security_strength = 80;
            }
            else if (modulo == 2048)
            {
                security_strength = 112;
            }
            else if (modulo == 3072)
            {
                security_strength = 128;
            }

            return _rand.GetRandomBitString(2 * security_strength);
        }

        private int[] GetBitlens(int modulo, PrimeGenModes mode)
        {
            var bitlens = new int[4];
            var min_single = 0;
            var max_both = 0;

            // Min_single values were given as exclusive, we add 1 to make them inclusive
            if(modulo == 1024)
            {
                // Rough estimate based on existing test vectors
                min_single = 101;
                max_both = 236;
            }
            else if (modulo == 2048)
            {
                min_single = 140 + 1;

                if (mode == PrimeGenModes.B32 || mode == PrimeGenModes.B34)
                {
                    max_both = 494;
                }
                else
                {
                    max_both = 750;
                }
            }
            else if (modulo == 3072)
            {
                min_single = 170 + 1;

                if (mode == PrimeGenModes.B32 || mode == PrimeGenModes.B34)
                {
                    max_both = 1007;
                }
                else
                {
                    max_both = 1518;
                }
            }

            bitlens[0] = _rand.GetRandomInt(min_single, max_both - min_single);
            bitlens[1] = _rand.GetRandomInt(min_single, max_both - bitlens[0]);
            bitlens[2] = _rand.GetRandomInt(min_single, max_both - min_single);
            bitlens[3] = _rand.GetRandomInt(min_single, max_both - bitlens[2]);

            return bitlens;
        }
    }
}
