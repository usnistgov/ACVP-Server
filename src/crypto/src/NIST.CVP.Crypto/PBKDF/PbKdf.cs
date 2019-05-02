using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF;
using NIST.CVP.Crypto.Common.KDF.Components.PBKDF;
using NIST.CVP.Crypto.Common.MAC.HMAC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.PBKDF
{
    public class PbKdf : IPbKdf
    {
        private readonly ISha _sha;
        private readonly IHmacFactory _hmacFactory;
        
        public PbKdf(ISha sha, IHmacFactory hmacFactory)
        {
            _sha = sha;
            _hmacFactory = hmacFactory;
        }

        public KdfResult DeriveKey(BitString salt, string password, int c, int keyLen)
        {
            // Could check keyLen, but the allowed max is larger than an int allows, so we just force positive...
            if (keyLen <= 0)
            {
                return new KdfResult("KeyLen must be greater than 0");
            }

            var hmac = _hmacFactory.GetHmacInstance(_sha.HashFunction);
            var passwordBits = new BitString(Encoding.ASCII.GetBytes(password));
            var len = (int)System.Math.Ceiling(keyLen / (decimal)_sha.HashFunction.OutputLen);
            var t = new BitString(0);
            
            for (var i = 1; i <= len; i++)
            {
                var t_i = new BitString(0);
                var u = salt.ConcatenateBits(BitString.To32BitString(i));

                for (var j = 1; j <= c; j++)
                {
                    u = hmac.Generate(passwordBits, u).Mac;
                    t_i = t_i.XOR(u);
                    t = t.ConcatenateBits(t_i);
                }
            }

            return new KdfResult(t.GetMostSignificantBits(keyLen));
        }
    }
}