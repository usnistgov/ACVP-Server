using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.AnsiX963;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.ANSIX963
{
    public class AnsiX963 : IAnsiX963
    {
        private readonly ISha _hash;

        public AnsiX963(ISha sha)
        {
            _hash = sha;
        }

        public KdfResult DeriveKey(BitString z, BitString sharedInfo, int keyLength)
        {
            var numBlocks = keyLength.CeilingDivide(_hash.HashFunction.OutputLen);

            var keyData = new BitString(0);
            for (var i = 1; i <= numBlocks; i++)
            {
                var counter = BitString.To32BitString(i);
                var hashInput = z.ConcatenateBits(counter).ConcatenateBits(sharedInfo);

                keyData = keyData.ConcatenateBits(_hash.HashMessage(hashInput).Digest);
            }

            return new KdfResult(keyData.GetMostSignificantBits(keyLength));
        }
    }
}
