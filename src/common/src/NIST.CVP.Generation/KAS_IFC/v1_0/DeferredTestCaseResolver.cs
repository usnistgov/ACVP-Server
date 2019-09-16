using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KTS;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.KAS_IFC.v1_0
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, KasResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<KasResult> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var result = await _oracle.CompleteDeferredKasTestAsync(new KasAftDeferredParametersIfc()
            {
                L = serverTestGroup.L,
                Modulo = serverTestGroup.Modulo,
                Scheme = serverTestGroup.Scheme,
                KasMode = serverTestGroup.KasMode,
                KeyConfirmationDirection = serverTestGroup.KeyConfirmationDirection,
                IutKeyAgreementRole = serverTestGroup.KasRole,
                IutKeyConfirmationRole = serverTestGroup.KeyConfirmationRole,
                KdfParameter = serverTestCase.KdfParameter,
                KtsParameter = new KtsParameter()
                {
                    KtsHashAlg = serverTestGroup.KtsConfiguration.KtsHashAlg,
                    AssociatedDataPattern = serverTestGroup.KtsConfiguration.AssociatedDataPattern,
                    Encoding = serverTestGroup.KtsConfiguration.Encoding
                },
                MacParameter = new MacParameters(
                    serverTestGroup.MacConfiguration.MacType, 
                    serverTestGroup.MacConfiguration.KeyLen, 
                    serverTestGroup.MacConfiguration.MacLen
                ),
                ServerKey = serverTestCase.ServerKey,
                IutKey = serverTestCase.IutKey,
                ServerPartyId = serverTestGroup.ServerId,
                IutPartyId = serverTestGroup.IutId,
                ServerC = serverTestCase.ServerC,
                IutNonce = iutTestCase.IutNonce,
                ServerNonce = serverTestCase.ServerNonce,
                
                IutC = iutTestCase.IutC,
            }); 
            
            return new KasResult(result.Result.Dkm, null, null, result.Result.Tag);
        }
    }
}