using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF;
using NIST.CVP.Crypto.Common.KDF.Components.PBKDF;
using NIST.CVP.Crypto.Common.MAC.HMAC;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.PBKDF
{
    public class PbKdf : IPbKdf
    {
        private readonly ISha _sha;
        private readonly IHmac _hmac;
        
        public PbKdf(ISha sha, IHmacFactory hmacFactory)
        {
            _sha = sha;
            _hmac = hmacFactory.GetHmacInstance(_sha.HashFunction);
        }

        public KdfResult DeriveKey(BitString salt, string password, int c, int keyLen)
        {
            // Could check keyLen, but the allowed max is larger than an int allows, so we just force positive...
            if (keyLen <= 0)
            {
                return new KdfResult("KeyLen must be greater than 0");
            }

            var passwordBits = new BitString(Encoding.ASCII.GetBytes(password));
            var len = keyLen.CeilingDivide(_sha.HashFunction.OutputLen);
            var t = new BitString[len];
            
            Parallel.For(0, len, i =>
            {
                var u = salt.ConcatenateBits(BitString.To32BitString(i + 1));
                t[i] = new BitString(0);
                
                for (var j = 1; j <= c; j++)
                {
                    u = _hmac.Generate(passwordBits, u).Mac;
                    t[i] = t[i].XOR(u);
                }
            });
 
            var result = new BitString(0);
            result = t.Aggregate(result, (current, val) => current.ConcatenateBits(val));

            return new KdfResult(result.GetMostSignificantBits(keyLen));
        }
    }
}