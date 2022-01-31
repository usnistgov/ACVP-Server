using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DRBG
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
