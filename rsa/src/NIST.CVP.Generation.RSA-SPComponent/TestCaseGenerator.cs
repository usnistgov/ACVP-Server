using System;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NLog;

namespace NIST.CVP.Generation.RSA_SPComponent
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IKeyBuilder _keyBuilder;
        private readonly IRsa _rsa;
        private readonly IKeyComposerFactory _keyComposerFactory;

        public int NumberOfTestCasesToGenerate { get; private set; } = 30;

        public TestCaseGenerator(IRandom800_90 rand, IKeyBuilder keyBuilder, IRsa rsa, IKeyComposerFactory keyComposerFactory)
        {
            _rand = rand;
            _keyBuilder = keyBuilder;
            _rsa = rsa;
            _keyComposerFactory = keyComposerFactory;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 5;
            }

            // Chance of all 30 tests not having a single expected failure is one in one billion
            var failureTest = _rand.GetRandomInt(0, 2); // 0 or 1

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
                    .WithKeyComposer(_keyComposerFactory.GetKeyComposer(group.KeyFormat))
                    .Build();

            } while (!keyResult.Success);

            var key = keyResult.Key;

            BitString message;
            if (failureTest == 0)
            {
                // No failure, get a random 2048-bit value less than N
                message = new BitString(_rand.GetRandomBigInteger(key.PubKey.N), 2048);
            }
            else
            {
                // Yes failure, get a random 2048-bit value greater than N
                message = new BitString(_rand.GetRandomBigInteger(key.PubKey.N, NumberTheory.Pow2(2048)), 2048);
            }

            var testCase = new TestCase
            {
                Key = key,
                Message = message,
                FailureTest = (failureTest != 0)     // Failure test if m > N, meaning it can't be signed
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            if (!testCase.FailureTest)
            {
                DecryptionResult result;
                try
                {
                    result = _rsa.Decrypt(testCase.Message.ToPositiveBigInteger(), testCase.Key.PrivKey, testCase.Key.PubKey);

                    if (!result.Success)
                    {
                        ThisLogger.Error($"Error generating signature: {result.ErrorMessage}");
                        return new TestCaseGenerateResponse(result.ErrorMessage);
                    }
                }
                catch (Exception ex)
                {
                    ThisLogger.Error($"Exception generating signature: {ex.StackTrace}");
                    return new TestCaseGenerateResponse($"Exception generating signature: {ex.StackTrace}");
                }

                testCase.Signature = new BitString(result.PlainText, 2048);
            }
            
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
    }
}
