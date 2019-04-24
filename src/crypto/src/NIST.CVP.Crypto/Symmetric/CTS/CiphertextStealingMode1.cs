using NIST.CVP.Crypto.Common.Symmetric.CTS;
using NIST.CVP.Crypto.Common.Symmetric.Engines;

namespace NIST.CVP.Crypto.Symmetric.CTS
{
    /// <summary>
    /// Mode 1 - final blocks never swapped, return ciphertext as is.
    /// </summary>
    public class CiphertextStealingMode1 : ICiphertextStealingTransform
    {
        public void Transform(byte[] outBuffer, IBlockCipherEngine engine, int numberOfBlocks, int originalPayloadBitLength)
        {
            // No transformation
        }
    }
}