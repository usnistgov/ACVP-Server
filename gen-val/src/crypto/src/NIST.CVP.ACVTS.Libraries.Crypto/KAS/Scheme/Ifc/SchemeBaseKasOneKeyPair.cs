using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Scheme.Ifc
{
    internal class SchemeBaseKasOneKeyPair : SchemeBaseKas
    {
        public SchemeBaseKasOneKeyPair(
            IEntropyProvider entropyProvider,
            SchemeParametersIfc schemeParameters,
            IFixedInfoFactory fixedInfoFactory,
            FixedInfoParameter fixedInfoParameter,
            IIfcSecretKeyingMaterialBuilder thisPartyKeyingMaterialBuilder,
            IKeyConfirmationFactory keyConfirmationFactory,
            MacParameters macParameters,
            IKdfVisitor kdfVisitor,
            IKdfParameter kdfParameter,
            IRsaSve rsaSve)
            : base(
                entropyProvider,
                schemeParameters,
                fixedInfoFactory,
                fixedInfoParameter,
                thisPartyKeyingMaterialBuilder,
                keyConfirmationFactory,
                macParameters,
                kdfVisitor,
                kdfParameter,
                rsaSve)
        {
        }

        protected override void BuildKeyingMaterialThisParty(IIfcSecretKeyingMaterialBuilder thisPartyKeyingMaterialBuilder,
            IIfcSecretKeyingMaterial otherPartyKeyingMaterial)
        {
            // Note party ID should have been set on the builder outside of the scope of kas.
            switch (SchemeParameters.KeyAgreementRole)
            {
                case KeyAgreementRole.InitiatorPartyU:
                    // Create random Z, encrypt with IUT public key to arrive at C
                    var rsaSveResult = _rsaSve.Generate(otherPartyKeyingMaterial.Key.PubKey);
                    thisPartyKeyingMaterialBuilder.WithZ(rsaSveResult.SharedSecretZ);
                    thisPartyKeyingMaterialBuilder.WithC(rsaSveResult.Ciphertext);
                    break;
                case KeyAgreementRole.ResponderPartyV:
                    // Provides public key and nonce.  Public key should have been set on the builder outside the scope of the kas instance.
                    thisPartyKeyingMaterialBuilder.WithDkmNonce(
                        EntropyProvider.GetEntropy(SchemeParameters.KasAlgoAttributes.Modulo));
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(SchemeParameters.KeyAgreementRole)} for building keying material.");
            }
        }

        protected override BitString GetKeyingMaterial(IIfcSecretKeyingMaterial otherPartyKeyingMaterial)
        {
            BitString initiatorData = null;
            BitString responderData = null;

            BitString z = null;
            switch (SchemeParameters.KeyAgreementRole)
            {
                case KeyAgreementRole.InitiatorPartyU:
                    z = ThisPartyKeyingMaterial.Z;

                    initiatorData = ThisPartyKeyingMaterial.C;
                    responderData = otherPartyKeyingMaterial.DkmNonce;
                    break;
                case KeyAgreementRole.ResponderPartyV:
                    // In this instance, party V recovers the Z value chosen by party U, utilizing party V's RSA private key.
                    var thisPartyKeyPair = ThisPartyKeyingMaterial.Key;

                    z = _rsaSve.Recover(thisPartyKeyPair, otherPartyKeyingMaterial.C).SharedSecretZ;

                    initiatorData = otherPartyKeyingMaterial.C;
                    responderData = ThisPartyKeyingMaterial.DkmNonce;
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(SchemeParameters.KeyAgreementRole)}");
            }

            // Don't run the KDF if NoKdfNoKc, return Z directly, for SSC tests
            if (SchemeParameters.KasMode == KasMode.NoKdfNoKc)
            {
                return z;
            }

            _kdfParameter.Z = z;

            _kdfParameter.SetEphemeralData(initiatorData, responderData);
            var fixedInfo = GetFixedInfo(otherPartyKeyingMaterial);
            return _kdfParameter.AcceptKdf(_kdfVisitor, fixedInfo).DerivedKey;
        }

        protected override BitString GetEphemeralDataFromKeyContribution(IIfcSecretKeyingMaterial secretKeyingMaterial,
            KeyAgreementRole keyAgreementRole, bool excludeEphemeralData)
        {
            // KAS1        Party U        C
            // KAS1        Party V        NV

            if (keyAgreementRole == KeyAgreementRole.InitiatorPartyU)
            {
                return secretKeyingMaterial.C;
            }

            return secretKeyingMaterial.DkmNonce;
        }
    }
}
