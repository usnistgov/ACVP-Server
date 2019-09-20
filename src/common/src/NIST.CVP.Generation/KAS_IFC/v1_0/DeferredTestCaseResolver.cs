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
            KtsParameter ktsParameter = null;
            if (serverTestGroup.KtsConfiguration != null)
            {
                ktsParameter = new KtsParameter()
                {
                    KtsHashAlg = serverTestGroup.KtsConfiguration.KtsHashAlg,
                    AssociatedDataPattern = serverTestGroup.KtsConfiguration.AssociatedDataPattern,
                    Encoding = serverTestGroup.KtsConfiguration.Encoding
                };
            }

            MacParameters macParameter = null;
            if (serverTestGroup.MacConfiguration != null)
            {
                macParameter = new MacParameters(
                    serverTestGroup.MacConfiguration.MacType,
                    serverTestGroup.MacConfiguration.KeyLen,
                    serverTestGroup.MacConfiguration.MacLen
                );
            }
            
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
                KtsParameter = ktsParameter,
                MacParameter = macParameter,
                
                ServerKey = serverTestCase.ServerKey,
                ServerPartyId = serverTestGroup.ServerId,
                ServerNonce = serverTestCase.ServerNonce,
                ServerC = serverTestCase.ServerC,
                ServerZ = serverTestCase.ServerZ,
                
                IutKey = serverTestCase.IutKey,
                IutPartyId = serverTestGroup.IutId,
                IutNonce = iutTestCase.IutNonce,
                IutC = iutTestCase.IutC,
                IutZ = iutTestCase.ServerZ
            }); 
            
            return new KasResult(result.Result.Dkm, null, null, result.Result.Tag);
        }
    }
}