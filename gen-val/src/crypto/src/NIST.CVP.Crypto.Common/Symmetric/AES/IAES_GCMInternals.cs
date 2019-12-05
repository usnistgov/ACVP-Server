using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.AES
{
    public interface IAES_GCMInternals
    {
        BitString Getj0(BitString h, BitString iv);
        BitString GHash(BitString h, BitString x);
        BitString GCTR(BitString icb, BitString x, BitString key);
        /// <summary>
        /// NIST SP 800-38D
        /// Recommendation for Block Cipher Modes of Operation: Galois/Counter
        /// Mode (GCM) and GMAC
        /// Section 6: Mathematical Components of GCM
        ///
        /// Section 6.2: Incrementing Function
        /// increment the rightmost s bits of X modulo 2^s
        /// leave the leftmost len(X) - s bits unchanged
        /// </summary>
        /// <param name="s"></param>
        /// <param name="X"></param>
        /// <returns></returns>
        BitString inc_s(int s, BitString X);
        BitString BlockProduct(BitString x, BitString y);
    }
}