using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.HKDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.HKDF
{
    public class Hkdf : IHkdf
    {
        private readonly IHmac _hmac;

        public Hkdf(IHmac hmac)
        {
            _hmac = hmac;
        }

        public BitString Extract(BitString salt, BitString inputKeyingMaterial)
        {
            // An empty salt is converted to empty bytes 
            salt ??= new BitString(_hmac.OutputLength);

            return _hmac.Generate(salt, inputKeyingMaterial).Mac;
        }

        public BitString Expand(BitString pseudoRandomKey, BitString otherInfo, int keyLengthBytes)
        {
            // keyLength comes in as bytes
            var keyLengthBits = keyLengthBytes * 8;
            var n = keyLengthBits.CeilingDivide(_hmac.OutputLength);
            var t = new BitString(0);
            var counter = new BitString("00");
            var result = new BitString(0);

            for (short i = 1; i <= n; i++)
            {
                counter = counter.BitStringAddition(BitString.One());
                t = _hmac.Generate(pseudoRandomKey, t.ConcatenateBits(otherInfo).ConcatenateBits(counter)).Mac;
                result = result.ConcatenateBits(t);
            }

            return result.GetMostSignificantBits(keyLengthBits);
        }

        public KdfResult DeriveKey(BitString salt, BitString inputKeyingMaterial, BitString otherInfo, int keyLengthBytes)
        {
            var pseudoRandomKey = Extract(salt, inputKeyingMaterial);
            var derivedKey = Expand(pseudoRandomKey, otherInfo, keyLengthBytes);

            return new KdfResult(derivedKey);
        }
    }
}
