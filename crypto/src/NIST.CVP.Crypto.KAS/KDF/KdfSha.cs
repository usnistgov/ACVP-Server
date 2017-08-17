using System.Numerics;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KDF
{
    public class KdfSha : KdfBase
    {
        private readonly ISha _sha;

        public KdfSha(ISha sha)
        {
            _sha = sha;
        }

        protected override int OutputLength => _sha.HashFunction.OutputLen;
        protected override BigInteger MaxInputLength => _sha.HashFunction.MaxMessageLen;
        protected override BitString H(BitString message)
        {
            return _sha.HashMessage(message).Digest;
        }
    }
}