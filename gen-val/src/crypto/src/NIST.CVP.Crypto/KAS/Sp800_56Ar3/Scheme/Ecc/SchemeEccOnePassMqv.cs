using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Sp800_56Ar3.Scheme.Ecc
{
    internal class SchemeEccOnePassMqv : SchemeBaseEcc
    {
        private readonly IMqv<EccDomainParameters, EccKeyPair> _mqv;

        public SchemeEccOnePassMqv(
            SchemeParameters schemeParameters, 
            ISecretKeyingMaterial thisPartyKeyingMaterial, 
            IFixedInfoFactory fixedInfoFactory, 
            FixedInfoParameter fixedInfoParameter, 
            IKdfFactory kdfFactory,
            IKdfParameter kdfParameter, 
            IKeyConfirmationFactory keyConfirmationFactory, 
            MacParameters keyConfirmationParameter,
            IMqv<EccDomainParameters, EccKeyPair> mqv) 
            : base(
                schemeParameters, 
                thisPartyKeyingMaterial, 
                fixedInfoFactory, 
                fixedInfoParameter, 
                kdfFactory, 
                kdfParameter, 
                keyConfirmationFactory, 
                keyConfirmationParameter)
        {
            _mqv = mqv;
        }

        protected override BitString ComputeSharedSecret(ISecretKeyingMaterial otherPartyKeyingMaterial)
        {
            // Party U uses both its static and ephemeral keys, and Party V's static public key
            if (SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU)
            {
                return _mqv.GenerateSharedSecretZ(
                    (EccDomainParameters) ThisPartyKeyingMaterial.DomainParameters,
                    (EccKeyPair) ThisPartyKeyingMaterial.StaticKeyPair,
                    (EccKeyPair) otherPartyKeyingMaterial.StaticKeyPair,
                    (EccKeyPair) ThisPartyKeyingMaterial.EphemeralKeyPair,
                    (EccKeyPair) ThisPartyKeyingMaterial.EphemeralKeyPair,
                    (EccKeyPair) otherPartyKeyingMaterial.StaticKeyPair
                ).SharedSecretZ;
            }

            // Party V uses its static key, and party U's static and ephemeral keys
            return _mqv.GenerateSharedSecretZ(
                (EccDomainParameters) ThisPartyKeyingMaterial.DomainParameters,
                (EccKeyPair) ThisPartyKeyingMaterial.StaticKeyPair,
                (EccKeyPair) otherPartyKeyingMaterial.StaticKeyPair,
                (EccKeyPair) ThisPartyKeyingMaterial.StaticKeyPair,
                (EccKeyPair) ThisPartyKeyingMaterial.StaticKeyPair,
                (EccKeyPair) otherPartyKeyingMaterial.EphemeralKeyPair
            ).SharedSecretZ;
        }
    }
}