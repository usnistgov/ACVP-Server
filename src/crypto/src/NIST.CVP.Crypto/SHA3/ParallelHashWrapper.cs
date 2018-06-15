using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SHA3
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

        private void Update(BitString message)
        {
            _message = message;
        }

        private BitString Final(int digestSize, int capacity, int blockSize, bool xof, string customization)
        {
            var numberOfBlocks = ((_message.BitLength / 8) + blockSize - 1) / blockSize;     // ceiling

            var newMessage = CSHAKEHelpers.LeftEncode(IntToBitString(blockSize));

            BitString substring;
            for (int i = 0; i < numberOfBlocks; i++)
            {
                substring = _message.Substring(i * blockSize * 8, blockSize * 8);
                newMessage = BitString.ConcatenateBits(newMessage, _cSHAKE.HashMessage(substring, capacity, capacity));
            }

            newMessage = BitString.ConcatenateBits(newMessage, CSHAKEHelpers.RightEncode(IntToBitString(numberOfBlocks)));

            if (xof)
            {
                _message = BitString.ConcatenateBits(_message, CSHAKEHelpers.RightEncode(BitString.Zeroes(8)));
            }
            else
            {
                _message = BitString.ConcatenateBits(_message, CSHAKEHelpers.RightEncode(IntToBitString(digestSize)));
            }

            return _cSHAKE.HashMessage(_message, digestSize, capacity, "ParallelHash", customization);
        }

        private BitString IntToBitString(int num)
        {
            return new BitString(new System.Numerics.BigInteger(num));
        }
    }
}
