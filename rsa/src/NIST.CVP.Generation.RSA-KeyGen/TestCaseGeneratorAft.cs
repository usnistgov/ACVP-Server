using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Enums;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NLog;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCaseGeneratorAft : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IKeyBuilder _keyBuilder;
        private readonly IKeyComposerFactory _keyComposerFactory;
        private readonly IShaFactory _shaFactory;

        public int NumberOfTestCasesToGenerate { get; private set; } = 25;

        public TestCaseGeneratorAft(IRandom800_90 rand, IKeyBuilder keyBuilder, IKeyComposerFactory keyComposerFactory, IShaFactory shaFactory)
        {
            _rand = rand;
            _keyBuilder = keyBuilder;
            _keyComposerFactory = keyComposerFactory;
            _shaFactory = shaFactory;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 3;
            }

            if (group.InfoGeneratedByServer || isSample)
            {
                var seed = GetSeed(group.Modulo);
                var e = group.PubExp == PublicExponentModes.Fixed ? group.FixedPubExp.ToPositiveBigInteger() : GetEValue(32, 64);
                var bitlens = GetBitlens(group.Modulo, group.PrimeGenMode);

                var testCase = new TestCase
                {
                    Bitlens = bitlens,
                    Seed = seed,
                    Key = new KeyPair { PubKey = new PublicKey { E = e }}
                };

                return Generate(group, testCase);
            }
            else
            {
                var testCase = new TestCase();
                if (group.PubExp == PublicExponentModes.Fixed)
                {
                    testCase.Key = new KeyPair { PubKey = new PublicKey { E = group.FixedPubExp.ToPositiveBigInteger() }};
                }

                testCase.Deferred = true;
                return new TestCaseGenerateResponse(testCase);
            }
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            KeyResult keyResult = null;
            try
            {
                // TODO Not every group has a hash alg... Can use a default value perhaps?
                ISha sha = null;
                if (group.HashAlg != null)
                {
                    sha = _shaFactory.GetShaInstance(group.HashAlg);
                }

                var keyComposer = _keyComposerFactory.GetKeyComposer(group.KeyFormat);

                // Configure Entropy Provider
                var entropyProvider = new EntropyProvider(_rand);

                // Configure Prime Generator
                keyResult = _keyBuilder
                    .WithBitlens(testCase.Bitlens)
                    .WithEntropyProvider(entropyProvider)
                    .WithHashFunction(sha)
                    .WithNlen(group.Modulo)
                    .WithPrimeGenMode(group.PrimeGenMode)
                    .WithPrimeTestMode(group.PrimeTest)
                    .WithPublicExponent(testCase.Key.PubKey.E)
                    .WithKeyComposer(keyComposer)
                    .Build();

                if (!keyResult.Success)
                {
                    ThisLogger.Warn(keyResult.ErrorMessage);
                    return new TestCaseGenerateResponse(keyResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse(ex.Message);
            }

            testCase.Key = keyResult.Key;
            return new TestCaseGenerateResponse(testCase);
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
