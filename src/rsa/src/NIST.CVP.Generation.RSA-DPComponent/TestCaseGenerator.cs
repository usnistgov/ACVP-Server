using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace NIST.CVP.Generation.RSA_DPComponent
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IKeyBuilder _keyBuilder;
        private readonly IKeyComposerFactory _keyComposerFactory;
        private readonly IRsa _rsa;

        public int NumberOfTestCasesToGenerate { get; } = 1;

        public TestCaseGenerator(IRandom800_90 rand, IKeyBuilder keyBuilder, IKeyComposerFactory keyComposerFactory, IRsa rsa)
        {
            _rand = rand;
            _keyBuilder = keyBuilder;
            _keyComposerFactory = keyComposerFactory;
            _rsa = rsa;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var failureTestIndexes = GetFailureIndexes(group.TotalTestCases, group.TotalFailingCases);
            var testCase = new TestCase()
            {
                ResultsArray = new List<AlgoArrayResponseSignature>()
            };

            for (var i = 0; i < group.TotalTestCases; i++)
            {
                var algoArrayResponse = new AlgoArrayResponseSignature();

                if (isSample)
                {
                    if (failureTestIndexes.Contains(i))
                    {
                        // Failure tests

                        // Pick a random ciphertext and force a leading '1' (so that it MUST be 2048 bits)
                        var cipherText = BitString.One().ConcatenateBits(_rand.GetRandomBitString(group.Modulo - 1));

                        // Pick a random n that is 2048 bits and less than the ciphertext
                        var n = _rand.GetRandomBigInteger(NumberTheory.Pow2(group.Modulo - 1), cipherText.ToPositiveBigInteger());
                        var e = GetEValue(32, 64);

                        algoArrayResponse.Key = new KeyPair {PubKey = new PublicKey {E = e, N = n}};
                        algoArrayResponse.CipherText = cipherText;
                        algoArrayResponse.FailureTest = true;
                    }
                    else
                    {
                        // Correct tests
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
                        algoArrayResponse.CipherText = new BitString(_rand.GetRandomBigInteger(1, algoArrayResponse.Key.PubKey.N - 1));
                    }
                }
                else
                {
                    BitString cipherText;
                    if (failureTestIndexes.Contains(i))
                    {
                        // Try to force the value high by forcing the first few bits to be 1
                        cipherText = BitString.Ones(2).ConcatenateBits(_rand.GetRandomBitString(group.Modulo - 2));
                    }
                    else
                    {
                        // Normal random value
                        cipherText = _rand.GetRandomBitString(group.Modulo);
                    }

                    algoArrayResponse.CipherText = cipherText;
                    algoArrayResponse.FailureTest = false;
                }

                testCase.ResultsArray.Add(algoArrayResponse);
            }

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            foreach (var algoArrayResponse in testCase.ResultsArray)
            {
                // Only samples have a key
                if (algoArrayResponse.Key != null)
                {
                    // Only run decryption if it's not a failure test
                    if (!algoArrayResponse.FailureTest)
                    {
                        DecryptionResult decryptionResult = null;
                        try
                        {
                            decryptionResult = _rsa.Decrypt(algoArrayResponse.CipherText.ToPositiveBigInteger(), algoArrayResponse.Key.PrivKey, algoArrayResponse.Key.PubKey);
                            if (!decryptionResult.Success)
                            {
                                ThisLogger.Error($"Error decrypting: {decryptionResult.ErrorMessage}");
                                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error decrypting: {decryptionResult.ErrorMessage}");
                            }
                        }
                        catch (Exception ex)
                        {
                            ThisLogger.Error($"Exception decrypting: {ex.StackTrace}");
                            return new TestCaseGenerateResponse<TestGroup, TestCase>($"Exception decrypting: {ex.StackTrace}");
                        }

                        algoArrayResponse.PlainText = new BitString(decryptionResult.PlainText);
                    }
                }
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();

        private int[] GetFailureIndexes(int total, int failing)
        {
            return Enumerable.Range(0, total).OrderBy(a => Guid.NewGuid()).Take(failing).ToArray();
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
