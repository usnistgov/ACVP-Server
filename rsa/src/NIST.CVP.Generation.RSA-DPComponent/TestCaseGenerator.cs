using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NLog;

namespace NIST.CVP.Generation.RSA_DPComponent
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IKeyBuilder _keyBuilder;
        private readonly IKeyComposerFactory _keyComposerFactory;
        private readonly IRsa _rsa;

        // We want at least 1/3 of the total test cases to fail
        private int _numberOfCasesInATestCase = 30;
        private bool _isSample = false;

        public int NumberOfTestCasesToGenerate { get; } = 1;

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
                _numberOfCasesInATestCase = 6;
                _isSample = true;
            }

            var failureTestIndexes = GetFailureIndexes();
            var testCase = new TestCase();

            for (var i = 0; i < _numberOfCasesInATestCase; i++)
            {
                var algoArrayResponse = new AlgoArrayResponse();

                if (_isSample)
                {
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

                    algoArrayResponse.Key = keyResult.Key;
                }

                BitString cipherText;
                if (failureTestIndexes.Contains(i))
                {
                    // Try to force the value high by forcing the first few bits to be 1
                    cipherText = BitString.Ones(2).ConcatenateBits(_rand.GetRandomBitString(2046));
                }
                else
                {
                    // Normal random value
                    cipherText = _rand.GetRandomBitString(2048);
                }

                algoArrayResponse.CipherText = cipherText;
                testCase.ResultsArray.Add(algoArrayResponse);
            }

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            if (_isSample)
            {
                foreach (var algoArrayResponse in testCase.ResultsArray)
                {
                    if (algoArrayResponse.CipherText.ToPositiveBigInteger() < algoArrayResponse.Key.PubKey.N - 1)
                    {
                        try
                        {
                            var decryptionResult = _rsa.Decrypt(algoArrayResponse.CipherText.ToPositiveBigInteger(), algoArrayResponse.Key.PrivKey, algoArrayResponse.Key.PubKey);
                            if (decryptionResult.Success)
                            {
                                algoArrayResponse.PlainText = new BitString(decryptionResult.PlainText);
                            }
                            else
                            {
                                // This should never happen, the above check prevents it, but if it does, let's call it a failure test
                                algoArrayResponse.FailureTest = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            ThisLogger.Error($"Exception decrypting: {ex.StackTrace}");
                            return new TestCaseGenerateResponse($"Exception decrypting: {ex.StackTrace}");
                        }
                    }
                    else
                    {
                        algoArrayResponse.FailureTest = true;
                    }
                }

                if (testCase.ResultsArray.Count(ra => ra.FailureTest) < _numberOfCasesInATestCase / 3)
                {
                    return Generate(group, _isSample);
                    //return new TestCaseGenerateResponse("Didn't create enough failures");
                }
            }

            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();

        private int[] GetFailureIndexes()
        {
            return Enumerable.Range(0, _numberOfCasesInATestCase).OrderBy(a => Guid.NewGuid()).Take(_numberOfCasesInATestCase / 2).ToArray();
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
