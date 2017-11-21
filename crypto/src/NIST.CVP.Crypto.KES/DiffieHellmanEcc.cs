using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.KES.Helpers;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.KES
{
    public class DiffieHellmanEcc : IDiffieHellman<EccDomainParameters, EccKeyPair>
    {
        public SharedSecretResponse GenerateSharedSecretZ(
            EccDomainParameters domainParameters, 
            EccKeyPair dA,
            EccKeyPair qB
        )
        {
            if (!domainParameters.CurveE.PointExistsOnCurve(dA.PublicQ))
            {
                return new SharedSecretResponse($"Point {nameof(qB)} does not exist on curve");
            }

            var p = domainParameters.CurveE.Multiply(qB.PublicQ, dA.PrivateD);
            p = domainParameters.CurveE.Multiply(p, domainParameters.CurveE.CofactorH);

            if (p.Infinity)
            {
                return new SharedSecretResponse("Point is infinity");
            }

            var pExactLength = domainParameters.CurveE.OrderN.ExactBitLength();
            BitString z = SharedSecretZHelper.FormatEccSharedSecretZ(p, pExactLength);

            return new SharedSecretResponse(z);
        }
    }
}