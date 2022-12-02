using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Scheme.Ifc
{
    internal class SchemeBaseKasTwoKeyPair : SchemeBaseKas
    {
        public SchemeBaseKasTwoKeyPair(
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
            // Public key should have been set on the builder outside the scope of the kas instance.
            // Create random Z, encrypt with IUT public key to arrive at C.
            var rsaSveResult = _rsaSve.Generate(otherPartyKeyingMaterial.Key.PubKey);
            thisPartyKeyingMaterialBuilder.WithZ(rsaSveResult.SharedSecretZ);
            thisPartyKeyingMaterialBuilder.WithC(rsaSveResult.Ciphertext);
        }

        protected override BitString GetKeyingMaterial(IIfcSecretKeyingMaterial otherPartyKeyingMaterial)
        {
            BitString zU = null;
            BitString zV = null;

            BitString initiatorData = null;
            BitString responderData = null;

            switch (SchemeParameters.KeyAgreementRole)
            {
                case KeyAgreementRole.InitiatorPartyU:
                    zU = ThisPartyKeyingMaterial.Z;

                    // When party U, recover zV from cV
                    var recoverV = _rsaSve.Recover(ThisPartyKeyingMaterial.Key, otherPartyKeyingMaterial.C);
                    zV = recoverV.SharedSecretZ;

                    initiatorData = ThisPartyKeyingMaterial.C;
                    responderData = otherPartyKeyingMaterial.C;
                    break;
                case KeyAgreementRole.ResponderPartyV:
                    zV = ThisPartyKeyingMaterial.Z;

                    // When party V, recover zU from cU
                    var recoverU = _rsaSve.Recover(ThisPartyKeyingMaterial.Key, otherPartyKeyingMaterial.C);
                    zU = recoverU.SharedSecretZ;

                    initiatorData = otherPartyKeyingMaterial.C;
                    responderData = ThisPartyKeyingMaterial.C;

                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(SchemeParameters.KeyAgreementRole)}");
            }
            var z = zU.ConcatenateBits(zV);

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
            return secretKeyingMaterial.C;
        }
    }
}
