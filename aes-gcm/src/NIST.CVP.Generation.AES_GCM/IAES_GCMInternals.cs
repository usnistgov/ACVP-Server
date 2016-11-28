using NIST.CVP.Generation.AES;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_GCM
{
    public interface IAES_GCMInternals
    {
        BitString Getj0(BitString h, BitString iv);
        int Ceiling(int numerator, int denominator);
        BitString GHash(BitString h, BitString x);
        BitString GCTR(BitString icb, BitString x, Key key);
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
        BitString LSB(int numBits, BitString x);
        BitString MSB(int numBits, BitString x);
        BitString BlockProduct(BitString x, BitString y);
    }
}