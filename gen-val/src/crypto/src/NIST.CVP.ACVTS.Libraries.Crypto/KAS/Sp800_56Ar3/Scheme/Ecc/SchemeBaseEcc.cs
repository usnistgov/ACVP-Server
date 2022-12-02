using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Crypto.KES.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Sp800_56Ar3.Scheme.Ecc
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
                var domainParam = (EccDomainParameters)secretKeyingMaterial.DomainParameters;
                var exactLength = CurveAttributesHelper.GetCurveAttribute(domainParam.CurveE.CurveName).DegreeOfPolynomial; ;

                var ephemKey = (EccKeyPair)secretKeyingMaterial.EphemeralKeyPair;

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
