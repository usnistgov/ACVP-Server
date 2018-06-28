using NIST.CVP.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.Crypto.CSHAKE;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.ParallelHash
{
    public class ParallelHashWrapper : IParallelHashWrapper
    {
        private BitString _message;
        private CSHAKEWrapper _cSHAKE;

        public virtual BitString HashMessage(BitString message, int digestSize, int capacity, int blockSize, bool xof, string customization = "")
        {
            Init();
            Update(message);
            return Final(digestSize, capacity, blockSize, xof, customization);
        }

        // These functions are for portability
        private void Init()
        {
            _message = new BitString(0);
            _cSHAKE = new CSHAKEWrapper();
        }

        private void Update(BitString newContent)
        {
            _message = BitString.ConcatenateBits(_message, newContent);
        }

        private BitString Final(int digestSize, int capacity, int blockSize, bool xof, string customization)
        {
            var newMessage = ParallelHashHelpers.FormatMessage(_message, _cSHAKE, digestSize, capacity, blockSize, customization, xof);
            return _cSHAKE.HashMessage(newMessage, digestSize, capacity, "ParallelHash", customization);
        }
    }
}
