using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.RSA_DPComponent
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IKeyBuilder _keyBuilder;
        private readonly IKeyComposerFactory _keyComposerFactory;
        private readonly IRsa _rsa;

        public int NumberOfTestCasesToGenerate { get; private set; } = 30;

        public TestCaseGenerator(IRandom800_90 rand, IKeyBuilder keyBuilder, IKeyComposerFactory keyComposerFactory, IRsa rsa)
        {
            _rand = rand;
            _keyBuilder = keyBuilder;
            _keyComposerFactory = keyComposerFactory;
            _rsa = rsa;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 5;

                KeyResult keyResult;
                do
                {
                    var e = GetEValue(32, 64);
                    keyResult = _keyBuilder
                        .WithPrimeGenMode(PrimeGenModes.B33)
                        .WithEntropyProvider(new EntropyProvider(_rand))
                        .WithNlen(group.Modulo)
                        .WithPublicExponent(e)
                        .WithPrimeTestMode(PrimeTestModes.C2)
                        .WithKeyComposer(_keyComposerFactory.GetKeyComposer(PrivateKeyModes.Standard))
                        .Build();

                } while (!keyResult.Success);

                var key = keyResult.Key;

                var testCase = new TestCase
                {
                    Key = key,
                    CipherText = _rand.GetRandomBitString(2048)
                };

                return Generate(group, testCase);
            }
            else
            {
                var testCase = new TestCase
                {
                    CipherText = _rand.GetRandomBitString(2048)
                };

                return new TestCaseGenerateResponse(testCase);
            }
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            throw new NotImplementedException();
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
