using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Crypto.KES.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KES
{
    public class DiffieHellmanEcc : IDiffieHellman<EccDomainParameters, EccKeyPair>
    {
        public SharedSecretResponse GenerateSharedSecretZ(
            EccDomainParameters domainParameters,
            EccKeyPair dA,
            EccKeyPair qB
        )
        {
            var p = domainParameters.CurveE.Multiply(qB.PublicQ, dA.PrivateD);
            p = domainParameters.CurveE.Multiply(p, domainParameters.CurveE.CofactorH);

            if (p.Infinity)
            {
                return new SharedSecretResponse("Point is infinity");
            }

            var curveAttributes = CurveAttributesHelper.GetCurveAttribute(domainParameters.CurveE.CurveName);
            BitString z = SharedSecretZHelper.FormatEccSharedSecretZ(p.X, curveAttributes.DegreeOfPolynomial);

            return new SharedSecretResponse(z);
        }
    }
}
