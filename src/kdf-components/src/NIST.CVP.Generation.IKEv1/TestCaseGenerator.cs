using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace NIST.CVP.Generation.IKEv1
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 100;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 10;
            }

            var param = new IkeV1KdfParameters
            {
                AuthenticationMethod = group.AuthenticationMethod,
                HashAlg = group.HashAlg,
                NInitLength = group.NInitLength,
                NRespLength = group.NRespLength,
                GxyLength = group.GxyLength,
                PreSharedKeyLength = group.PreSharedKeyLength
            };

            IkeV1KdfResult result = null;
            try
            {
                result = _oracle.GetIkeV1KdfCase(param);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            var testCase = new TestCase
            {
                NInit = result.NInit,
                NResp = result.NResp,
                CkyInit = result.CkyInit,
                CkyResp = result.CkyResp,
                Gxy = result.Gxy,
                SKeyId = result.sKeyId,
                SKeyIdD = result.sKeyIdD,
                SKeyIdE = result.sKeyIdE,
                SKeyIdA = result.sKeyIdA,
                PreSharedKey = result.PreSharedKey
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
