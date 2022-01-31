using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Fips186_5.KeyGen
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 25;

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 3;
            }
            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
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
                    PublicExponent = group.FixedPubExp,
                    Standard = Fips186Standard.Fips186_5
                };

                try
                {
                    var result = await _oracle.GetRsaKeyAsync(param);

                    var testCase = new TestCase
                    {
                        Key = result.Key,
                        XP = new BitString(result.AuxValues.XP),
                        XQ = new BitString(result.AuxValues.XQ),
                        Bitlens = result.BitLens,
                        Seed = result.Seed
                    };

                    // Only one that actually needs these values
                    if (group.PrimeGenMode == PrimeGenModes.RandomProbablePrimesWithAuxiliaryProbablePrimes)
                    {
                        testCase.XP1 = new BitString(result.AuxValues.XP1, testCase.Bitlens[0], false);
                        testCase.XP2 = new BitString(result.AuxValues.XP2, testCase.Bitlens[1], false);
                        testCase.XQ1 = new BitString(result.AuxValues.XQ1, testCase.Bitlens[2], false);
                        testCase.XQ2 = new BitString(result.AuxValues.XQ2, testCase.Bitlens[3], false);
                    }

                    return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
                }
                catch (Exception ex)
                {
                    ThisLogger.Error(ex);
                    return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
                }
            }
            else
            {
                // Leave all the properties blank
                var testCase = new TestCase();
                if (group.PubExp == PublicExponentModes.Fixed)
                {
                    testCase.Key = new KeyPair { PubKey = new PublicKey { E = group.FixedPubExp.ToPositiveBigInteger() } };
                }

                testCase.Deferred = true;
                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
