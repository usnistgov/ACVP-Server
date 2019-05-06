using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.KAS.v1_0.ECC_Component
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public int NumberOfTestCasesToGenerate => 25;

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            try
            {
                var result = await _oracle.GetKasEccComponentTestAsync(
                    new KasEccComponentParameters()
                    {
                        Curve = group.Curve,
                        IsSample = isSample
                    }
                );

                var testCase = new TestCase()
                {
                    PrivateKeyServer = result.PrivateKeyServer,
                    PublicKeyServerX = result.PublicKeyServerX,
                    PublicKeyServerY = result.PublicKeyServerY,

                    // These have values only when sample
                    Deferred = !isSample,
                    PrivateKeyIut = result.PrivateKeyIut,
                    PublicKeyIutX = result.PublicKeyIutX,
                    PublicKeyIutY = result.PublicKeyIutY,
                    Z = result.Z
                };

                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
            }
        }

        private static Logger Logger => LogManager.GetCurrentClassLogger();
    }
}
