using NIST.CVP.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Crypto.CSHAKE;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.ParallelHash
{
    public class ParallelHashWrapper : IParallelHashWrapper
    {
        private BitString _message;
        private CSHAKE.CSHAKE _cSHAKE;

        public virtual BitString HashMessage(BitString message, int digestLength, int capacity, int blockSize, bool xof, string customization = "")
        {
            Init();
            Update(message);
            return Final(digestLength, capacity, blockSize, xof, customization);
        }

        // These functions are for portability
        private void Init()
        {
            _message = new BitString(0);
            _cSHAKE = new CSHAKE.CSHAKE();
        }

        private void Update(BitString newContent)
        {
            _message = BitString.ConcatenateBits(_message, newContent);
        }

        private BitString Final(int digestLength, int capacity, int blockSize, bool xof, string customization)
        {
            var newMessage = ParallelHashHelpers.FormatMessage(_message, _cSHAKE, digestLength, capacity, blockSize, xof);
            return _cSHAKE.HashMessage(new Common.Hash.CSHAKE.HashFunction(digestLength, capacity), newMessage, customization, "ParallelHash").Digest;
        }

        #region BitString Customization
        public BitString HashMessage(BitString message, int digestLength, int capacity, int blockSize, bool xof, BitString customizationHex)
        {
            Init();
            Update(message);
            return Final(digestLength, capacity, blockSize, xof, customizationHex);
        }

        private BitString Final(int digestLength, int capacity, int blockSize, bool xof, BitString customizationHex)
        {
            var newMessage = ParallelHashHelpers.FormatMessage(_message, _cSHAKE, digestLength, capacity, blockSize, xof);
            return _cSHAKE.HashMessage(new Common.Hash.CSHAKE.HashFunction(digestLength, capacity), newMessage, customizationHex, "ParallelHash").Digest;
        }
        #endregion BitString Customization
    }
}
