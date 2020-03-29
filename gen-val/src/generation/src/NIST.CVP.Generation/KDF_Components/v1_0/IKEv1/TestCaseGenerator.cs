using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.KDF_Components.v1_0.IKEv1
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate { get; private set; } = 100;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 10;
            }
            return new GenerateResponse();
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new IkeV1KdfParameters
            {
                AuthenticationMethod = group.AuthenticationMethod,
                HashAlg = group.HashAlg,
                NInitLength = group.NInitLength,
                NRespLength = group.NRespLength,
                GxyLength = group.GxyLength,
                PreSharedKeyLength = group.PreSharedKeyLength
            };

            try
            {
                var result = await _oracle.GetIkeV1KdfCaseAsync(param);

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
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
