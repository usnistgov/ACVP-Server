using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Math;
using NIST.CVP.ACVTS.Libraries.Crypto.KES.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KES
{
    public class MqvEcc : IMqv<EccDomainParameters, EccKeyPair>
    {
        public SharedSecretResponse GenerateSharedSecretZ(
            EccDomainParameters domainParameters,
            EccKeyPair xPrivateKeyPartyA,
            EccKeyPair yPublicKeyPartyB,
            EccKeyPair rPrivateKeyPartyA,
            EccKeyPair tPublicKeyPartyA,
            EccKeyPair tPublicKeyPartyB)
        {
            var exactBitSize = domainParameters.CurveE.OrderN.ExactBitLength();
            var associateValueQeA = AssociateValueFunction(exactBitSize, tPublicKeyPartyA);
            var associateValueQeB = AssociateValueFunction(exactBitSize, tPublicKeyPartyB);

            var implicitSigA =
                (rPrivateKeyPartyA.PrivateD + associateValueQeA
                * xPrivateKeyPartyA.PrivateD) % domainParameters.CurveE.OrderN;

            var p = domainParameters.CurveE.Multiply(yPublicKeyPartyB.PublicQ, associateValueQeB);
            p = domainParameters.CurveE.Add(p, tPublicKeyPartyB.PublicQ);
            p = domainParameters.CurveE.Multiply(p, implicitSigA);
            p = domainParameters.CurveE.Multiply(p, domainParameters.CurveE.CofactorH);

            if (p.Infinity)
            {
                return new SharedSecretResponse("Point is infinity");
            }

            var pExactLength = domainParameters.CurveE.FieldSizeQ.ExactBitLength();
            BitString z = SharedSecretZHelper.FormatEccSharedSecretZ(p.X, pExactLength);
            return new SharedSecretResponse(z);
        }

        public static BigInteger AssociateValueFunction(int qExactLength, EccKeyPair publicKey)
        {
            int f = (qExactLength + 1) / 2;

            BigInteger pow2 = NumberTheory.Pow2(f);

            return pow2 + (publicKey.PublicQ.X % pow2);
        }
    }
}
