using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.Crypto.KES.Helpers;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.KAS.Sp800_56Ar3.Scheme.Ecc
{
    internal abstract class SchemeBaseEcc : SchemeBase
    {
        protected SchemeBaseEcc(
            SchemeParameters schemeParameters, 
            ISecretKeyingMaterial thisPartyKeyingMaterial, 
            IFixedInfoFactory fixedInfoFactory, 
            FixedInfoParameter fixedInfoParameter, 
            IKdfFactory kdfFactory, 
            IKdfParameter kdfParameter, 
            IKeyConfirmationFactory keyConfirmationFactory, 
            MacParameters keyConfirmationParameter) 
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
        }

        protected override BitString GetEphemeralDataFromKeyContribution(ISecretKeyingMaterial secretKeyingMaterial)
        {
            if (secretKeyingMaterial.EphemeralKeyPair != null)
            {
                var domainParam = (EccDomainParameters) secretKeyingMaterial.DomainParameters;
                var exactLength = domainParam.CurveE.OrderN.ExactBitLength();

                var ephemKey = (EccKeyPair) secretKeyingMaterial.EphemeralKeyPair;
                
                if (ephemKey.PublicQ.X != 0)
                {
                    return BitString.ConcatenateBits(
                        SharedSecretZHelper.FormatEccSharedSecretZ(ephemKey.PublicQ.X, exactLength),
                        SharedSecretZHelper.FormatEccSharedSecretZ(ephemKey.PublicQ.Y, exactLength)
                    );                    
                }
            }
            
            if (secretKeyingMaterial.EphemeralNonce != null && secretKeyingMaterial.EphemeralNonce?.BitLength != 0)
            {
                return secretKeyingMaterial.EphemeralNonce;
            }

            return secretKeyingMaterial.DkmNonce;
        }
    }
}