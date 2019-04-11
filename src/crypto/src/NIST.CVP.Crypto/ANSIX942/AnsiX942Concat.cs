using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.AnsiX942;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.ANSIX942
{
    public class AnsiX942Concat : IAnsiX942
    {
        private readonly ISha _sha;

        public AnsiX942Concat(ISha sha)
        {
            _sha = sha;
        }

        public KdfResult DeriveKey(BitString zz, int keyLen, BitString otherInfo)
        {
            if (keyLen <= 0 || keyLen > 65536)
            {
                return new KdfResult($"KeyLen must be between [1, 65536]. Value given was: {keyLen}");
            }

            var d = (int) System.Math.Ceiling(keyLen / (decimal) _sha.HashFunction.OutputLen);
            var counter = BitString.To32BitString(1);
            var h = new BitString(0);

            for (var i = 1; i <= d; i++)
            {
                // H[i] = Hash(ZZ || counter || otherInfo)
                h = h.ConcatenateBits(_sha.HashMessage(zz.ConcatenateBits(counter).ConcatenateBits(otherInfo)).Digest);

                counter = counter.BitStringAddition(BitString.One());
            }

            return new KdfResult(h.GetMostSignificantBits(keyLen));
        }
    }
}
