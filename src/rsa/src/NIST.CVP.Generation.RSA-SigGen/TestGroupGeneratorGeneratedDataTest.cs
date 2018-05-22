using System;
using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using System.Numerics;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestGroupGeneratorGeneratedDataTest : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private readonly IKeyBuilder _keyBuilder;
        private readonly IRandom800_90 _rand;
        private readonly IKeyComposerFactory _keyComposerFactory;

        public TestGroupGeneratorGeneratedDataTest(IKeyBuilder keyBuilder, IRandom800_90 rand, IKeyComposerFactory keyComposerFactory)
        {
            _keyBuilder = keyBuilder;
            _rand = rand;
            _keyComposerFactory = keyComposerFactory;
        }

        public const string TEST_TYPE = "GDT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                var sigType = capability.SigType;

                foreach (var moduloCap in capability.ModuloCapabilities)
                {
                    var modulo = moduloCap.Modulo;

                    foreach (var hashPair in moduloCap.HashPairs)
                    {
                        var testGroup = new TestGroup
                        {
                            Mode = EnumHelpers.GetEnumFromEnumDescription<SignatureSchemes>(sigType),
                            Modulo = modulo,
                            HashAlg = ShaAttributes.GetHashFunctionFromName(hashPair.HashAlg),
                            SaltLen = hashPair.SaltLen,

                            TestType = TEST_TYPE
                        };

                        // Make a single key for the group
                        if (parameters.IsSample)
                        {
                            var keyResult = _keyBuilder
                                .WithPrimeTestMode(PrimeTestModes.C2)
                                .WithEntropyProvider(new EntropyProvider(_rand))
                                .WithNlen(testGroup.Modulo)
                                .WithPrimeGenMode(PrimeGenModes.B33)
                                .WithPublicExponent(GetEValue(32, 64))
                                .WithKeyComposer(_keyComposerFactory.GetKeyComposer(PrivateKeyModes.Standard))
                                .Build();

                            if (!keyResult.Success)
                            {
                                throw new Exception("Error generating key for group");
                            }

                            testGroup.Key = keyResult.Key;
                        }

                        testGroups.Add(testGroup);
                    }
                }
            }

            return testGroups;
        }

        
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
    }
}
