using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random;
        private readonly IDsaFfc _ffcDsa;

        public int NumberOfTestCasesToGenerate { get { return 10; } }

        public TestCaseGenerator(IRandom800_90 rand, IDsaFfc ffcDsa)
        {
            _random = rand;
            _ffcDsa = ffcDsa;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                return Generate(group, new TestCase());
            }
            else
            {
                return new TestCaseGenerateResponse(new TestCase());
            }
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            FfcKeyPairGenerateResult keyResult = null;
            try
            {
                keyResult = _ffcDsa.GenerateKeyPair(group.DomainParams);
                if (!keyResult.Success)
                {
                    ThisLogger.Warn($"Error generating key: {keyResult.ErrorMessage}");
                    return new TestCaseGenerateResponse($"Error generating key: {keyResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error($"Exception generating key: {ex.StackTrace}");
                return new TestCaseGenerateResponse($"Exception generating key: {ex.Message}");
            }

            // TODO REMOVE once group level properties are allowed.
            // Adding group level properties PQG to test case,
            // for isSample only.  Currently result projection does not allow
            // group level properties, so PQG are being included on a per test case basis.
            testCase.DomainParams = group.DomainParams;
            testCase.Key = keyResult.KeyPair;
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
