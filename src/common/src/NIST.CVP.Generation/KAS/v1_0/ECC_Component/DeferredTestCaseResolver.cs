using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.KAS.v1_0.ECC_Component
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

                        PrivateKeyServer = serverTestCase.KeyPairPartyServer.PrivateD,
                        PublicKeyServerX = serverTestCase.KeyPairPartyServer.PublicQ.X,
                        PublicKeyServerY = serverTestCase.KeyPairPartyServer.PublicQ.Y,

                        PublicKeyIutX = iutTestCase.KeyPairPartyIut.PublicQ.X,
                        PublicKeyIutY = iutTestCase.KeyPairPartyIut.PublicQ.Y,
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