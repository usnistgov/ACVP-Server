using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH
{
    public interface IXecdh
    {
        /// <summary>
        /// Name of the curve as an enum
        /// </summary>
        Curve CurveName { get; }

        /// <summary>
        /// Field size, p = odd prime power
        /// </summary>
        BigInteger FieldSizeP { get; }

        /// <summary>
        /// U coordinate of the base point of the curve
        /// </summary>
        BigInteger BasePointG { get; }

        /// <summary>
        /// Used for X25519/X448. It is the length of a coordinate on the curve.
        /// </summary>
        int VariableBits { get; }

        /// <summary>
        /// Used for X25519/X448. It is equal to (A - 2) / 4.
        /// </summary>
        int VariableA24 { get; }

        BitString XECDH(BitString k, BitString i);

        XecdhKeyPairGenerateResult DeriveKeyPair(BitString privateKey);

        XecdhKeyPairGenerateResult GenerateKeyPair();

        XecdhKeyPairValidateResult ValidateKeyPair(XecdhKeyPair keyPair);
    }
}