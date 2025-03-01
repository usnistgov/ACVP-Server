﻿using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.KeyGen
{
    public class TestCaseGeneratorGdt : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 25;

        public TestCaseGeneratorGdt(IOracle oracle)
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
            if (isSample)
            {
                // Generate a full key
                var param = new RsaKeyParameters
                {
                    Modulus = group.Modulo,
                    KeyFormat = group.KeyFormat,
                    KeyMode = RsaKeyGenAttributeConverter.GetPrimeGenFromSection(group.PrimeGenMode), // always B.3.3 probable
                    PrimeTest = RsaKeyGenAttributeConverter.GetPrimeTestFromSection(group.PrimeTest),
                    PublicExponentMode = group.PubExp,
                    PublicExponent = group.FixedPubExp,
                    Standard = Fips186Standard.Fips186_4
                };

                try
                {
                    var result = await _oracle.GetRsaKeyAsync(param);

                    var testCase = new TestCase
                    {
                        Key = result.Key,
                        Deferred = true
                    };

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
