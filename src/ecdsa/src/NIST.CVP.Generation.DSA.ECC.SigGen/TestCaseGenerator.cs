using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.DSA.ECC.SigGen
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
            var param = new EcdsaSignatureParameters
            {
                Curve = group.Curve,
                HashAlg = group.HashAlg,
                PreHashedMessage = group.ComponentTest,
                Key = group.KeyPair
            };

            try
            {
                TestCase testCase = null;
                EcdsaSignatureResult result = null;
                if (isSample)
                {
                    result = _oracle.GetEcdsaSignature(param);
                    testCase = new TestCase
                    {
                        Message = result.Message,
                        Signature = result.Signature
                    };

                }
                else
                {
                    result = _oracle.GetDeferredEcdsaSignature(param);
                    testCase = new TestCase
                    {
                        Message = result.Message
                    };
                }

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex.StackTrace);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating case: {ex.Message}");
            }
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
