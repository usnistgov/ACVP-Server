using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using System;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Signatures;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Crypto.RSA2.Signatures;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestCaseGeneratorGDT : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ISignatureBuilder _signatureBuilder;
        private readonly IKeyBuilder _keyBuilder;
        private readonly IPaddingFactory _paddingFactory;
        private readonly IShaFactory _shaFactory;
        private readonly IKeyComposerFactory _keyComposerFactory;

        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGeneratorGDT(IRandom800_90 random800_90, ISignatureBuilder signatureBuilder,
            IKeyBuilder keyBuilder, IPaddingFactory paddingFactory, IShaFactory shaFactory,
            IKeyComposerFactory keyComposerFactory)
        {
            _random800_90 = random800_90;
            _signatureBuilder = signatureBuilder;
            _keyBuilder = keyBuilder;
            _paddingFactory = paddingFactory;
            _shaFactory = shaFactory;
            _keyComposerFactory = keyComposerFactory;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 3;
            }

            // Make a single key for the group
            if (group.Key == null && isSample)
            {
                KeyResult keyResult;
                do
                {
                    keyResult = _keyBuilder
                        .WithPrimeTestMode(PrimeTestModes.C2)
                        .WithEntropyProvider(new EntropyProvider(_random800_90))
                        .WithNlen(group.Modulo)
                        .WithPrimeGenMode(PrimeGenModes.B33)
                        .WithPublicExponent(GetEValue(32, 64))
                        .WithKeyComposer(_keyComposerFactory.GetKeyComposer(PrivateKeyModes.Standard))
                        .Build();
                } while (!keyResult.Success);

                group.Key = keyResult.Key;
            }

            var testCase = new TestCase
            {
                Message = _random800_90.GetRandomBitString(group.Modulo / 2),
                IsSample = isSample
            };

            return isSample ? Generate(group, testCase) : new TestCaseGenerateResponse(testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            SignatureResult sigResult = null;
            try
            {
                var entropyProvider = new TestableEntropyProvider();
                testCase.Salt = _random800_90.GetRandomBitString(group.SaltLen * 8);
                entropyProvider.AddEntropy(testCase.Salt);

                var sha = _shaFactory.GetShaInstance(group.HashAlg);

                var paddingScheme = _paddingFactory.GetPaddingScheme(group.Mode, sha, entropyProvider, group.SaltLen);

                sigResult = _signatureBuilder
                    .WithKey(group.Key)
                    .WithMessage(testCase.Message)
                    .WithPaddingScheme(paddingScheme)
                    .WithDecryptionScheme(new Rsa(new RsaVisitor()))    // TODO: Can these be injected?
                    .BuildSign();

                if (!sigResult.Success)
                {
                    ThisLogger.Warn($"Error generating sample signature: {sigResult.ErrorMessage}");
                    return new TestCaseGenerateResponse($"Error generating sample signature: {sigResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error($"Exception generating sample signature: {ex.StackTrace}");
                return new TestCaseGenerateResponse($"Exception generating sample signature: {ex.StackTrace}");
            }

            testCase.Signature = new BitString(sigResult.Signature, group.Modulo);

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

                e = GetRandomBigIntegerOfBitLength(_random800_90.GetRandomInt(min, max) * 2);
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
            var bs = _random800_90.GetRandomBitString(len);
            return bs.ToPositiveBigInteger();
        }
    }
}
