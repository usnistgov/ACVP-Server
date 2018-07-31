using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.DSA.FFC.SigGen
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var param = new DsaSignatureParameters
            {
                HashAlg = group.HashAlg,
                MessageLength = group.L
            };

            DsaSignatureResult result = null;
            try
            {
                if (isSample)
                {
                    // If we are a sample, the group MUST have domain parameters
                    var keyParam = new DsaKeyParameters
                    {
                        DomainParameters = group.DomainParams
                    };

                    var keyResult = _oracle.GetDsaKey(keyParam);

                    param.Key = keyResult.Key;
                    param.DomainParameters = keyParam.DomainParameters;

                    result = _oracle.GetDsaSignature(param);
                    var testCase = new TestCase
                    {
                        Message = result.Message,
                        Key = param.Key,
                        Signature = result.Signature
                    };

                    return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
                }
                else
                {
                    result = _oracle.GetDeferredDsaSignature(param);
                    var testCase = new TestCase
                    {
                        Message = result.Message
                    };

                    return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex.StackTrace);
                return new TestCaseGenerateResponse<TestGroup, TestCase>("Unable to generate test case");
            }
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
