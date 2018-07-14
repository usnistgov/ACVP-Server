using System;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Generation.Core;
using NLog;

namespace NIST.CVP.Generation.KAS.EccComponent
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolver<TestGroup, TestCase, SharedSecretResponse>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public SharedSecretResponse CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            try
            {
                return new SharedSecretResponse(_oracle.CompleteDeferredKasComponentTest(
                    new KasEccComponentDeferredParameters()
                    {
                        Curve = testGroup.Curve,

                        PrivateKeyServer = serverTestCase.PrivateKeyServer,
                        PublicKeyServerX = serverTestCase.PublicKeyIutX,
                        PublicKeyServerY = serverTestCase.PublicKeyIutY,

                        PublicKeyIutX = iutTestCase.PublicKeyIutX,
                        PublicKeyIutY = iutTestCase.PublicKeyIutY,
                    }
                ).Z);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new SharedSecretResponse(ex.Message);
            }
        }

        private static Logger Logger => LogManager.GetCurrentClassLogger();
    }
}