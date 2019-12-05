using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DRBG
{
    public class DrbgCounterAes : DrbgCounterBase
    {
        protected IModeBlockCipher<IModeBlockCipherResult> Cipher;

        public DrbgCounterAes(IEntropyProvider entropyProvider, IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory cipherFactory, DrbgParameters drbgParameters)
            : base(entropyProvider, drbgParameters)
        {
            Cipher = cipherFactory.GetStandardCipher(
                engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), BlockCipherModesOfOperation.Ecb);
        }

        protected override BitString BlockEncrypt(BitString K, BitString X)
        {
            var param = new ModeBlockCipherParameters(BlockCipherDirections.Encrypt, K, X);
            return Cipher.ProcessPayload(param).Result;
        }
    }
}