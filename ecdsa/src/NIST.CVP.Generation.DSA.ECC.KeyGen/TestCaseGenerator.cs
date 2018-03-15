using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random;
        private readonly IDsaEccFactory _eccDsaFactory;
        private IDsaEcc _eccDsa;
        private readonly IEccCurveFactory _curveFactory;

        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGenerator(IRandom800_90 rand, IDsaEccFactory eccDsaFactory, IEccCurveFactory curveFactory)
        {
            _random = rand;
            _eccDsaFactory = eccDsaFactory;
            _curveFactory = curveFactory;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 3;
                return Generate(group, new TestCase());
            }
            else
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase());
            }
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            EccKeyPairGenerateResult keyResult = null;
            try
            {
                // No hash function needed
                _eccDsa = _eccDsaFactory.GetInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));

                var curve = _curveFactory.GetCurve(group.Curve);
                var domainParams = new EccDomainParameters(curve);
                keyResult = _eccDsa.GenerateKeyPair(domainParams);
                if (!keyResult.Success)
                {
                    ThisLogger.Warn($"Error generating key: {keyResult.ErrorMessage}");
                    return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating key: {keyResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error($"Exception generating key: {ex.StackTrace}");
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Exception generating key: {ex.Message}");
            }

            testCase.KeyPair = keyResult.KeyPair;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
