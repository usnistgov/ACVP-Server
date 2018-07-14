using System;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Generation.Core;
using NLog;

namespace NIST.CVP.Generation.KAS.EccComponent
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public int NumberOfTestCasesToGenerate => 25;

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            try
            {
                var result = _oracle.GetKasEccComponentTest(
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

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            throw new NotImplementedException();
        }

        private static Logger Logger => LogManager.GetCurrentClassLogger();
    }
}
