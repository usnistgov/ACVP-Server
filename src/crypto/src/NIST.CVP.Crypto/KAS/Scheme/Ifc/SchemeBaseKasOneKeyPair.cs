using System;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme.Ifc
{
    internal class SchemeBaseKasOneKeyPair : SchemeBaseKas
    {
        public SchemeBaseKasOneKeyPair
        (
            SchemeParametersIfc schemeParameters, 
            IFixedInfoFactory fixedInfoFactory,
            FixedInfoParameter fixedInfoParameter,
            IIfcSecretKeyingMaterial thisPartyKeyingMaterial,
            IKeyConfirmationFactory keyConfirmationFactory,
            MacParameters macParameters,
            IKdfVisitor kdfVisitor,
            IKdfParameter kdfParameter,
            IRsaSve rsaSve
            ) 
            : base(schemeParameters, fixedInfoFactory, fixedInfoParameter, thisPartyKeyingMaterial, keyConfirmationFactory, macParameters, kdfVisitor, kdfParameter, rsaSve)
        {
        }

        protected override BitString GetKeyingMaterial(IIfcSecretKeyingMaterial otherPartyKeyingMaterial)
        {
            switch (SchemeParameters.KeyAgreementRole)
            {
                case KeyAgreementRole.InitiatorPartyU:
                    // In this instance, party U creates a random Z and encrypts it using party V's public key.
                    // The keying material is derived partially via party U (the chosen random value Z) and party V via a nonce
                    // contributed via their otherPartyPublicInfo.
                    var otherPartyPublicKey = otherPartyKeyingMaterial.Key.PubKey;

                    var generateResult = _rsaSve.Generate(otherPartyPublicKey);

                    ThisPartyKeyingMaterial.C = generateResult.Ciphertext;
                    ThisPartyKeyingMaterial.Z = generateResult.SharedSecretZ;
                    _kdfParameter.Z = ThisPartyKeyingMaterial.Z;
                    break;
                case KeyAgreementRole.ResponderPartyV:
                    // In this instance, party V recovers the Z value chosen by party U, utilizing party V's RSA private key.
                    var thisPartyKeyPair = ThisPartyKeyingMaterial.Key;

                    var z = _rsaSve.Recover(thisPartyKeyPair, otherPartyKeyingMaterial.C).SharedSecretZ;
                    _kdfParameter.Z = z;
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(SchemeParameters.KeyAgreementRole)}");
            }
            
            var fixedInfo = GetFixedInfo(otherPartyKeyingMaterial);
            return _kdfParameter.AcceptKdf(_kdfVisitor, fixedInfo).DerivedKey;
        }

        protected override BitString GetEphemeralDataFromKeyContribution(IIfcSecretKeyingMaterial secretKeyingMaterial,
            KeyAgreementRole keyAgreementRole)
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