using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.DSA.FFC.SigVer
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 15;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 5;
            }

            var keyParam = new DsaKeyParameters
            {
                DomainParameters = group.DomainParams
            };

            DsaKeyResult keyResult = null;
            try
            {
                keyResult = _oracle.GetDsaKey(keyParam);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex.StackTrace);
                return new TestCaseGenerateResponse<TestGroup, TestCase>("Unable to generate key");
            }

            var reason = group.TestCaseExpectationProvider.GetRandomReason();
            var param = new DsaSignatureParameters
            {
                HashAlg = group.HashAlg,
                DomainParameters = group.DomainParams,
                MessageLength = group.N,
                Key = keyResult.Key,
                Disposition = reason.GetReason()
            };

            DsaSignatureResult result = null;
            try
            {
                result = _oracle.GetDsaSignature(param);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex.StackTrace);
                return new TestCaseGenerateResponse<TestGroup, TestCase>("Unable to generate signature");
            }

            var testCase = new TestCase
            {
                Message = result.Message,
                Key = param.Key,
                Reason = reason,
                TestPassed = reason.GetReason() == DsaSignatureDisposition.None
            };

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
