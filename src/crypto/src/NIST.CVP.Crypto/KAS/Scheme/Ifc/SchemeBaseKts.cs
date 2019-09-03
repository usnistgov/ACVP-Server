using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KTS;
using NIST.CVP.Math;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace NIST.CVP.Crypto.KAS.Scheme.Ifc
{
    public class SchemeBaseKts : SchemeBase
    {
        private readonly IKtsFactory _ktsFactory;
        private readonly KtsParameter _ktsParameter;

        protected SchemeBaseKts
        (
            SchemeParametersIfc schemeParameters, 
            IFixedInfoFactory fixedInfoFactory,
            FixedInfoParameter fixedInfoParameter,
            IIfcSecretKeyingMaterial thisPartyKeyingMaterial,
            IKeyConfirmationFactory keyConfirmationFactory,
            MacParameters macParameters,
            IKtsFactory ktsFactory,
            KtsParameter ktsParameter
            ) 
            : base(schemeParameters, fixedInfoFactory, fixedInfoParameter, thisPartyKeyingMaterial, keyConfirmationFactory, macParameters)
        {
            _ktsFactory = ktsFactory;
            _ktsParameter = ktsParameter;
        }

        protected override BitString GetKeyingMaterial(IIfcSecretKeyingMaterial otherPartyKeyingMaterial)
        {
            var kts = _ktsFactory.Get(_ktsParameter.KtsHashAlg);

            var fixedInfo = GetFixedInfo(otherPartyKeyingMaterial);
            
            // Party U has the key, encrypts it with Party V's public key
            if (SchemeParameters.KeyAgreementRole == KeyAgreementRole.InitiatorPartyU)
            {
                var otherPartyPublicKey = otherPartyKeyingMaterial.Key.PubKey;

                var keyToEncodeEncrypt = ThisPartyKeyingMaterial.K;

                var C = kts.Encrypt(otherPartyPublicKey, keyToEncodeEncrypt, fixedInfo).SharedSecretZ;
                ThisPartyKeyingMaterial.C = C.GetDeepCopy();
                
                return C;
            }

            // Party V has the private key that is used to decrypt the key from party U.
            var thisPartyKey = ThisPartyKeyingMaterial.Key;
            var otherPartyCiphertext = otherPartyKeyingMaterial.C;

            return kts.Decrypt(thisPartyKey, otherPartyCiphertext, fixedInfo).SharedSecretZ;
        }

        protected override BitString GetEphemeralDataFromKeyContribution(IIfcSecretKeyingMaterial secretKeyingMaterial, KeyAgreementRole keyAgreementRole)
        {
            // KTS         Party U        C
            // KTS         Party V        null

            if (keyAgreementRole == KeyAgreementRole.InitiatorPartyU)
            {
                return secretKeyingMaterial.C;
            }
            
            return null;
        }
    }
}