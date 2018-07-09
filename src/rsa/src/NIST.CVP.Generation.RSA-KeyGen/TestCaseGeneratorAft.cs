using System;
using System.Numerics;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NLog;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCaseGeneratorAft : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly IRandom800_90 _rand;
        private readonly IKeyBuilder _keyBuilder;
        private readonly IKeyComposerFactory _keyComposerFactory;
        private readonly IShaFactory _shaFactory;

        public int NumberOfTestCasesToGenerate { get; private set; } = 25;

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 3;
            }

            if (group.InfoGeneratedByServer || isSample)
            {
                // Generate a full key
                var param = new RsaKeyParameters
                {
                    Modulus = group.Modulo,
                    KeyFormat = group.KeyFormat,
                    KeyMode = group.PrimeGenMode,
                    PrimeTest = group.PrimeTest,
                    HashAlg = group.HashAlg,
                    PublicExponentMode = group.PubExp,
                    PublicExponent = group.FixedPubExp
                };

                var result = _oracle.GetRsaKey(param);

                return response;
            }
            else
            {
                // Leave all the properties blank
                var testCase = new TestCase();
                if (group.PubExp == PublicExponentModes.Fixed)
                {
                    testCase.Key = new KeyPair { PubKey = new PublicKey { E = group.FixedPubExp.ToPositiveBigInteger() }};
                }

                testCase.Deferred = true;
                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
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
                    .WithSeed(testCase.Seed)
                    .Build();

                if (!keyResult.Success)
                {
                    ThisLogger.Warn(keyResult.ErrorMessage);
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(keyResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
            }

            testCase.Key = keyResult.Key;

            testCase.XP = new BitString(keyResult.AuxValues.XP);
            testCase.XP1 = new BitString(keyResult.AuxValues.XP1);
            testCase.XP2 = new BitString(keyResult.AuxValues.XP2);
            testCase.XQ = new BitString(keyResult.AuxValues.XQ);
            testCase.XQ1 = new BitString(keyResult.AuxValues.XQ1);
            testCase.XQ2 = new BitString(keyResult.AuxValues.XQ2);

            // TODO this really sucks, these bitstring values need slight modification because they don't always match the bitlens if they start with some 0s
            if (group.PrimeGenMode == PrimeGenModes.B36)
            {
                testCase.XP1 = new BitString(keyResult.AuxValues.XP1, testCase.Bitlens[0] % 8 == 0 ? testCase.Bitlens[0] : testCase.Bitlens[0] + 8 - testCase.Bitlens[0] % 8, false);
                testCase.XP2 = new BitString(keyResult.AuxValues.XP2, testCase.Bitlens[1] % 8 == 0 ? testCase.Bitlens[1] : testCase.Bitlens[1] + 8 - testCase.Bitlens[1] % 8, false);
                testCase.XQ1 = new BitString(keyResult.AuxValues.XQ1, testCase.Bitlens[2] % 8 == 0 ? testCase.Bitlens[2] : testCase.Bitlens[2] + 8 - testCase.Bitlens[2] % 8, false);
                testCase.XQ2 = new BitString(keyResult.AuxValues.XQ2, testCase.Bitlens[3] % 8 == 0 ? testCase.Bitlens[3] : testCase.Bitlens[3] + 8 - testCase.Bitlens[3] % 8, false);
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
