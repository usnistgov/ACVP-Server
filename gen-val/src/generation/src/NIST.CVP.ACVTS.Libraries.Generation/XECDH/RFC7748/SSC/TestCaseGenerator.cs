using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.XECDH.RFC7748.SSC
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
                XecdhSscResult result;
                if (isSample)
                {
                    result = await _oracle.GetXecdhSscTestAsync(
                        new XecdhSscParameters()
                        {
                            Curve = group.Curve
                        }
                    );
                }
                else
                {
                    result = await _oracle.GetDeferredXecdhSscTestAsync(
                        new XecdhSscParameters()
                        {
                            Curve = group.Curve
                        }
                    );
                }

                var testCase = new TestCase()
                {
                    KeyPairPartyServer = new XecdhKeyPair(result.PublicKeyServer, result.PrivateKeyServer),

                    // These have values only when sample
                    Deferred = !isSample,
                    KeyPairPartyIut = new XecdhKeyPair(result.PublicKeyIut, result.PrivateKeyIut),
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
