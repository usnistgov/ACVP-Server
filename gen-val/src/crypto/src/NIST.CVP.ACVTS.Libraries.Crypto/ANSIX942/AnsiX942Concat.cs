using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.ANSIX942
{
    public class AnsiX942Concat : IAnsiX942
    {
        private readonly ISha _sha;

        public AnsiX942Concat(ISha sha)
        {
            _sha = sha;
        }

        public KdfResult DeriveKey(IAns942Parameters param)
        {
            if (!(param is ConcatAns942Parameters concatParams))
            {
                return new KdfResult("Unable to parse concat parameters");
            }

            if (concatParams.KeyLen <= 0 || concatParams.KeyLen > 65536)
            {
                return new KdfResult($"KeyLen must be between [1, 65536]. Value given was: {concatParams.KeyLen}");
            }

            var d = (int)System.Math.Ceiling(concatParams.KeyLen / (decimal)_sha.HashFunction.OutputLen);
            var counter = BitString.To32BitString(1);
            var h = new BitString(0);

            for (var i = 1; i <= d; i++)
            {
                // H[i] = Hash(ZZ || counter || otherInfo)
                var hashInput = concatParams.Zz.ConcatenateBits(counter).ConcatenateBits(concatParams.OtherInfo);
                h = h.ConcatenateBits(_sha.HashMessage(hashInput).Digest);

                counter = counter.BitStringAddition(BitString.One());
            }

            return new KdfResult(h.GetMostSignificantBits(concatParams.KeyLen));
        }
    }
}
