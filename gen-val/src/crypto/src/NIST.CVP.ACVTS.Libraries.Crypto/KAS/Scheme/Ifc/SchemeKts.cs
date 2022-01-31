using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KTS;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Scheme.Ifc
{
    internal class SchemeKts : SchemeBase
    {
        private readonly IKtsFactory _ktsFactory;
        private readonly KtsParameter _ktsParameter;

        public SchemeKts(
            IEntropyProvider entropyProvider,
            SchemeParametersIfc schemeParameters,
            IFixedInfoFactory fixedInfoFactory,
            FixedInfoParameter fixedInfoParameter,
            IIfcSecretKeyingMaterialBuilder thisPartyKeyingMaterialBuilder,
            IKeyConfirmationFactory keyConfirmationFactory,
            MacParameters macParameters,
            IKtsFactory ktsFactory,
            KtsParameter ktsParameter
            )
            : base
            (
                entropyProvider,
                schemeParameters,
                fixedInfoFactory,
                fixedInfoParameter,
                thisPartyKeyingMaterialBuilder,
                keyConfirmationFactory,
                macParameters)
        {
            _ktsFactory = ktsFactory;
            _ktsParameter = ktsParameter;
        }

        protected override void BuildKeyingMaterialThisParty(IIfcSecretKeyingMaterialBuilder thisPartyKeyingMaterialBuilder,
            IIfcSecretKeyingMaterial otherPartyKeyingMaterial)
        {
            switch (SchemeParameters.KeyAgreementRole)
            {
                case KeyAgreementRole.InitiatorPartyU:
                    // Create a key of L length, wrap it with the other parties public key.
                    var keyToEncodeEncrypt = EntropyProvider.GetEntropy(SchemeParameters.KasAlgoAttributes.L);
                    var kts = _ktsFactory.Get(_ktsParameter.KtsHashAlg);

                    BitString fixedInfo = null;
                    if (!string.IsNullOrEmpty(_ktsParameter.AssociatedDataPattern))
                    {
                        ThisPartyKeyingMaterial = _thisPartyKeyingMaterialBuilder.Build(
                            SchemeParameters.KasAlgoAttributes.Scheme,
                            SchemeParameters.KasMode,
                            SchemeParameters.KeyAgreementRole,
                            SchemeParameters.KeyConfirmationRole,
                            SchemeParameters.KeyConfirmationDirection
                        );

                        fixedInfo = GetFixedInfo(otherPartyKeyingMaterial);
                    }

                    var c = kts.Encrypt(otherPartyKeyingMaterial.Key.PubKey, keyToEncodeEncrypt, fixedInfo).SharedSecretZ;

                    thisPartyKeyingMaterialBuilder.WithK(keyToEncodeEncrypt);
                    thisPartyKeyingMaterialBuilder.WithC(c);
                    break;
                case KeyAgreementRole.ResponderPartyV:
                    // Key should have been set outside the scope of the kas instance
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(SchemeParameters.KeyAgreementRole)}");
            }
        }

        protected override BitString GetKeyingMaterial(IIfcSecretKeyingMaterial otherPartyKeyingMaterial)
        {
            var kts = _ktsFactory.Get(_ktsParameter.KtsHashAlg);

            // Party U has the key, encrypts it with Party V's public key
            if (SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU)
            {
                return ThisPartyKeyingMaterial.K;
            }

            // Party V has the private key that is used to decrypt the key from party U.
            BitString fixedInfo = null;
            if (!string.IsNullOrEmpty(_ktsParameter.AssociatedDataPattern))
            {
                fixedInfo = GetFixedInfo(otherPartyKeyingMaterial, true);
            }
            var thisPartyKey = ThisPartyKeyingMaterial.Key;
            var otherPartyCiphertext = otherPartyKeyingMaterial.C;

            return kts.Decrypt(thisPartyKey, otherPartyCiphertext, fixedInfo).SharedSecretZ;
        }

        protected override BitString GetEphemeralDataFromKeyContribution(IIfcSecretKeyingMaterial secretKeyingMaterial, KeyAgreementRole keyAgreementRole, bool excludeEphemeralData)
        {
            // KTS         Party U        C
            // KTS         Party V        null

            if (excludeEphemeralData)
            {
                return null;
            }

            if (keyAgreementRole == KeyAgreementRole.InitiatorPartyU)
            {
                return secretKeyingMaterial.C;
            }

            return null;
        }
    }
}
