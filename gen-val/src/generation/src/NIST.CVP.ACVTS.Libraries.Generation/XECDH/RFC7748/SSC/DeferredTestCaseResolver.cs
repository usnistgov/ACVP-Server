using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.XECDH.RFC7748.SSC
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
                var result = await _oracle.CompleteDeferredXecdhSscTestAsync(
                    new XecdhSscDeferredParameters()
                    {
                        Curve = testGroup.Curve,

                        PrivateKeyServer = serverTestCase.KeyPairPartyServer.PrivateKey,
                        PublicKeyServer = serverTestCase.KeyPairPartyServer.PublicKey,

                        PublicKeyIut = iutTestCase.KeyPairPartyIut.PublicKey,
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
