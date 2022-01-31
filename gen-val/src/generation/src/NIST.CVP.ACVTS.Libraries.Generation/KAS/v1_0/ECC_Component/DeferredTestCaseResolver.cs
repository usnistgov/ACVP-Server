using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC_Component
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
