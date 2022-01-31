using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF.OneStep
{
    public class KdfSha : KdfBase
    {
        private readonly ISha _sha;

        public KdfSha(ISha sha, bool useCounter)
        {
            UseCounter = useCounter;
            _sha = sha;
        }

        protected override int OutputLength => _sha.HashFunction.OutputLen;
        protected override BigInteger MaxInputLength => _sha.HashFunction.MaxMessageLen;
        protected override BitString H(BitString message, BitString salt)
        {
            return _sha.HashMessage(message).Digest;
        }
    }
}
