using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.KAS.EccComponent
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, SharedSecretResponse>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<SharedSecretResponse> CompleteDeferredCryptoAsync(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            try
            {
                var result = await _oracle.CompleteDeferredKasComponentTestAsync(
                    new KasEccComponentDeferredParameters()
                    {
                        Curve = testGroup.Curve,

                        PrivateKeyServer = serverTestCase.PrivateKeyServer,
                        PublicKeyServerX = serverTestCase.PublicKeyIutX,
                        PublicKeyServerY = serverTestCase.PublicKeyIutY,

                        PublicKeyIutX = iutTestCase.PublicKeyIutX,
                        PublicKeyIutY = iutTestCase.PublicKeyIutY,
                    }
                );

                return new SharedSecretResponse(result.Z);
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