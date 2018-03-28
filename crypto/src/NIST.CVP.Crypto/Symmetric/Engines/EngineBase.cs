using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;

namespace NIST.CVP.Crypto.Symmetric.Engines
{
    public abstract class EngineBase<TSymmetricCipherResult> : IModeBlockCipher<TSymmetricCipherResult>
        where TSymmetricCipherResult : IModeBlockCipherResult
    {
        public abstract TSymmetricCipherResult ProcessMessage(IModeBlockCipherParameters param);

        protected virtual int GetNumberOfBlocks(int bitsPerBlock, int outputLengthBits)
        {
            return outputLengthBits / bitsPerBlock;
        }

        protected virtual byte[] GetOutputBuffer(int outputLengthInBits)
        {
            var byteLength = outputLengthInBits / 8 + (outputLengthInBits % 8 > 0 ? 1 : 0);
            return new byte[byteLength];
        }
    }
}