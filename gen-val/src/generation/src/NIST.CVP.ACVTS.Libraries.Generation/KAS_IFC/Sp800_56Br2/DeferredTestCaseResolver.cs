using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KTS;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Br2;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_IFC.Sp800_56Br2
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
                    KtsHashAlg = serverTestGroup.KtsConfiguration.HashAlg,
                    AssociatedDataPattern = serverTestGroup.KtsConfiguration.AssociatedDataPattern,
                    Encoding = serverTestGroup.KtsConfiguration.Encoding,
                    Context = serverTestCase.KtsParameter.Context,
                    Label = serverTestCase.KtsParameter.Label,
                    AlgorithmId = serverTestCase.KtsParameter.AlgorithmId
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

            var kdfParameter = serverTestCase.KdfParameter;
            if (kdfParameter != null)
            {
                kdfParameter.AdditionalInitiatorNonce = ShouldSupplyInitiatorNonce(serverTestGroup)
                    ? iutTestCase.KdfParameter.AdditionalInitiatorNonce
                    : kdfParameter.AdditionalInitiatorNonce;
                kdfParameter.AdditionalResponderNonce = ShouldSupplyResponderNonce(serverTestGroup)
                    ? iutTestCase.KdfParameter.AdditionalResponderNonce
                    : kdfParameter.AdditionalResponderNonce;
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
                KdfParameter = kdfParameter,
                KtsParameter = ktsParameter,
                MacParameter = macParameter,

                ServerKey = serverTestCase.ServerKey,
                ServerPartyId = serverTestGroup.ServerId,
                ServerNonce = serverTestCase.ServerNonce,
                ServerC = serverTestCase.ServerC,
                ServerZ = serverTestCase.ServerZ,
                ServerK = serverTestCase.ServerK,

                IutKey = serverTestCase.IutKey,
                IutPartyId = serverTestGroup.IutId,
                IutNonce = iutTestCase.IutNonce,
                IutC = iutTestCase.IutC,
                IutZ = iutTestCase.IutZ,
                IutK = iutTestCase.IutK
            });

            if (!result.Result.Success)
            {
                return new KasResult(result.Result.ErrorMessage);
            }

            return new KasResult(result.Result.Dkm, null, null, result.Result.Tag);
        }

        /// <summary>
        /// When an AFT test if the IUT is partyU for this group, then the additional nonce is required.
        /// </summary>
        /// <returns></returns>
        private bool ShouldSupplyInitiatorNonce(TestGroup serverTestGroup)
        {
            if (serverTestGroup.TestType.Equals("VAL", StringComparison.OrdinalIgnoreCase) || serverTestGroup.KdfConfiguration == null)
            {
                return false;
            }

            return serverTestGroup.KdfConfiguration.RequiresAdditionalNoncePair && serverTestGroup.KasRole == KeyAgreementRole.InitiatorPartyU;
        }

        /// <summary>
        /// When an AFT test if the IUT is partyV for this group, then the additional nonce is required.
        /// </summary>
        /// <returns></returns>
        private bool ShouldSupplyResponderNonce(TestGroup serverTestGroup)
        {
            if (serverTestGroup.TestType.Equals("VAL", StringComparison.OrdinalIgnoreCase) || serverTestGroup.KdfConfiguration == null)
            {
                return false;
            }

            return serverTestGroup.KdfConfiguration.RequiresAdditionalNoncePair && serverTestGroup.KasRole == KeyAgreementRole.ResponderPartyV;
        }
    }
}
