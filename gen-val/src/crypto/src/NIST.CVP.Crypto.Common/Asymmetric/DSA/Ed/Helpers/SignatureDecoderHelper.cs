using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Helpers
{
    public static class SignatureDecoderHelper
    {
        public static (EdPoint R, BigInteger s) DecodeSig(EdDomainParameters domainParameters, EdSignature sig)
        {
            var rBits = sig.Sig.MSBSubstring(0, domainParameters.CurveE.VariableB);
            var sBits = sig.Sig.Substring(0, domainParameters.CurveE.VariableB);

            var R = domainParameters.CurveE.Decode(rBits);
            var s = BitString.ReverseByteOrder(sBits).ToPositiveBigInteger();

            return (R, s);
        }
    }
}
