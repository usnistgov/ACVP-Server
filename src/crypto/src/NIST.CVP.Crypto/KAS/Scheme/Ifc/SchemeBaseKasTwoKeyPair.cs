using System;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.Scheme.Ifc
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
            
            switch (SchemeParameters.KeyAgreementRole)
            {
                case KeyAgreementRole.InitiatorPartyU:
                    zU = ThisPartyKeyingMaterial.Z;
                    
                    // When party U, recover zV from cV
                    var recoverV = _rsaSve.Recover(ThisPartyKeyingMaterial.Key, otherPartyKeyingMaterial.C);
                    zV = recoverV.SharedSecretZ;
                    
                    break;
                case KeyAgreementRole.ResponderPartyV:
                    zV = ThisPartyKeyingMaterial.Z;
                    
                    // When party V, recover zU from cU
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
            KeyAgreementRole keyAgreementRole, bool excludeEphemeralData)
        {
            return secretKeyingMaterial.C;
        }
    }
}