using System;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme.Ifc
{
    public class SchemeBaseKasTwoKeyPair : SchemeBaseKas
    {
        public SchemeBaseKasTwoKeyPair
        (
            SchemeParametersIfc schemeParameters,
            IFixedInfoFactory fixedInfoFactory,
            FixedInfoParameter fixedInfoParameter,
            IIfcSecretKeyingMaterial thisPartyKeyingMaterial,
            IKeyConfirmationFactory keyConfirmationFactory,
            MacParameters macParameters,
            IKdfVisitor kdfVisitor,
            IKdfParameter kdfParameter,
            IRsaSve rsaSve) 
            : base(
                schemeParameters, 
                fixedInfoFactory, 
                fixedInfoParameter, 
                thisPartyKeyingMaterial, 
                keyConfirmationFactory, 
                macParameters, 
                kdfVisitor, 
                kdfParameter, 
                rsaSve)
        {
        }

        protected override BitString GetKeyingMaterial(IIfcSecretKeyingMaterial otherPartyKeyingMaterial)
        {
            BitString zU = null;
            BitString zV = null;
            
            switch (SchemeParameters.KeyAgreementRole)
            {
                case KeyAgreementRole.InitiatorPartyU:
                    // When party U, create a zU and cU, recover zV from cV
                    var generateU = _rsaSve.Generate(otherPartyKeyingMaterial.Key.PubKey);
                    zU = generateU.SharedSecretZ;
                    ThisPartyKeyingMaterial.Z = zU;
                    ThisPartyKeyingMaterial.C = generateU.Ciphertext;

                    var recoverV = _rsaSve.Recover(ThisPartyKeyingMaterial.Key, otherPartyKeyingMaterial.C);
                    zV = recoverV.SharedSecretZ;
                    
                    break;
                case KeyAgreementRole.ResponderPartyV:
                    // When party V, create a zV and cV, recover zU from cU
                    var generateV = _rsaSve.Generate(otherPartyKeyingMaterial.Key.PubKey);
                    zV = generateV.SharedSecretZ;
                    ThisPartyKeyingMaterial.Z = zV;
                    ThisPartyKeyingMaterial.C = generateV.Ciphertext;

                    var recoverU = _rsaSve.Recover(ThisPartyKeyingMaterial.Key, otherPartyKeyingMaterial.C);
                    zU = recoverU.SharedSecretZ;

                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(SchemeParameters.KeyAgreementRole)}");
            }
            
            var fixedInfo = GetFixedInfo(otherPartyKeyingMaterial);

            var z = zU.ConcatenateBits(zV);
            _kdfParameter.Z = z;

            return _kdfParameter.AcceptKdf(_kdfVisitor, fixedInfo).DerivedKey;
        }

        protected override BitString GetEphemeralDataFromKeyContribution(IIfcSecretKeyingMaterial secretKeyingMaterial,
            KeyAgreementRole keyAgreementRole)
        {
            return secretKeyingMaterial.C;
        }
    }
}