using System.Collections;
using System.Numerics;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using NLog;

namespace NIST.CVP.Crypto.AES_GCM
{
    public class AES_GCMInternals : IAES_GCMInternals
    {
        private readonly IRijndaelFactory _iRijndaelFactory;

        public AES_GCMInternals(IRijndaelFactory iRijndaelFactory)
        {
            _iRijndaelFactory = iRijndaelFactory;
        }

        public virtual BitString Getj0(BitString h, BitString iv)
        {
            BitString j0 = null;
            //ThisLogger.Debug("Getj0");
            if (iv.BitLength == 96)
            {
                j0 = iv.ConcatenateBits(BitString.Zeroes(32));
                j0.Set(0, true);
            }
            //	If len(IV) != 96, then let s = 128 * ceil(len(IV)/128) - len(IV)
            //  and let J0 = GHASH_H(IV || 0^(s + 64) || [len(IV)]_64)
            else
            {
                int s = 128 * iv.BitLength.CeilingDivide(128) - iv.BitLength;
                var x = iv
                    .ConcatenateBits(BitString.Zeroes(s + 64))
                    .ConcatenateBits(BitString.To64BitString(iv.BitLength));
                j0 = GHash(h, x);
            }

            return j0;
        }

        public virtual BitString GHash(BitString h, BitString x)
        {
            //ThisLogger.Debug("GHash");
            if (h.BitLength != 128)
            {
                return null;
            }
            if (x.BitLength % 128 != 0 || x.BitLength == 0)
            {
                ThisLogger.Debug(x.BitLength);
                return null;
            }

            // Step 1: Let X1,...,Xm-1,Xm denote the unique sequence of blocks such that
            // X = X1 || X2 || ... || Xm-1 || Xm
            int m = x.BitLength / 128;

            // Step 2: Let Y0 be the "zero block" 0^128
            var y = BitString.Zeroes(128);

            // Step 3: For i = 1,...,m let Yi = (Yi-1 xor Xi) dot H
            for (int i = 0; i < m; ++i)
            {
                BitString xi = x.Substring((m - i - 1) * 128, 128);
                BitString YxorXi = BitString.XOR(y, xi);
                y = BlockProduct(YxorXi, h);
            }

            return y;
        }

        public virtual BitString GCTR(BitString icb, BitString x, Key key)
        {
            // ICB must be 128 bits long
            // ThisLogger.Debug("GCTR");
            if (icb.BitLength != 128)
            {
                ThisLogger.Warn($"icbLen:{icb.BitLength}");
                return null;
            }

            // Step 1: If X is the empty string, then return the empty string as Y
            if (x.BitLength == 0)
            {
                return new BitString(0);
            }

            // Step 2: Let n = ceil[ len(X)/128 ]
            int n = x.BitLength.CeilingDivide(128);
            
            // Step 3: Let X1,X2,...,Xn-1,Xn denote the unique sequence of bit
            // strings such that X = X1 || X2 || ... || Xn-1 || Xn*
            // X1, X2,...,Xn-1 are complete blocks
            // Xn* is either a complete block or a partial block

            // Step 4: Let CB1 = ICB
            // Step 5: For i = 2 to n, let CBi = inc32(CBi-1)
            // Step 6: For i = 1 to n-1, let Yi = Xi xor CIPH_K(CBi)
            ModeValues mode = ModeValues.ECB;
            var rijn = _iRijndaelFactory.GetRijndael(mode);
            var cipher = new Cipher { BlockLength = 128, Mode = mode };
            BitString cbi = icb;
            BitString Y = new BitString(0);
            int sx = x.BitLength - 128;
            for (int i = 1; i <= (n - 1); ++i, sx -= 128)
            {
                if (i > 1)
                {
                    cbi = inc_s(32, cbi);
                }
                BitString xi = x.Substring(sx, 128);
                var h = rijn.BlockEncrypt(cipher, key, cbi.ToBytes(), 128);//@@@ or k?
                BitString yi = BitString.XOR(xi, h);
                Y = Y.ConcatenateBits(yi);    // This is part of Step 8
            }

            // Step 7: Let Yn* = Xn* xor MSB_len(Xn*) (CIPH_K(CBn))
            // i == n case:
            if (n > 1)
            {
                cbi = inc_s(32, cbi);
            }

            var xn = x.Substring(0, 128 + sx);
            var h1 = rijn.BlockEncrypt(cipher, key, cbi.ToBytes(), 128);//@@@ or k?

            var yn = BitString.XOR(xn, h1.GetMostSignificantBits(xn.BitLength));
            Y = Y.ConcatenateBits(yn); // This is part of Step 8

            // Step 8: Let Y = Y1 || ... || Yn*

            // Step 9: Return Y
            return Y;
        }

        // NIST SP 800-38D
        // Recommendation for Block Cipher Modes of Operation: Galois/Counter
        // Mode (GCM) and GMAC
        // Section 6: Mathematical Components of GCM

        // Section 6.2: Incrementing Function
        // increment the rightmost s bits of X modulo 2^s
        // leave the leftmost len(X) - s bits unchanged
        public virtual BitString inc_s(int s, BitString X)
        {

            // inc_s(X) = MSB_len(X)-s || [int(LSB_s(X)) + 1 mod 2^s]_s
            if (X.BitLength < s)
            {
                return null;
            }
            BigInteger lsp1 = X.GetLeastSignificantBits(s).ToBigInteger() + new BigInteger(1);
            //ThisLogger.Debug($"lsp1: {lsp1}");
            lsp1 = lsp1 % (new BigInteger(1) << s);
            //ThisLogger.Debug($"lsp2: {lsp1}");

            var bitsToAppend = new BitString(lsp1, s);
            //ThisLogger.Debug($"BitsToAppendLength: {bitsToAppend.Length}");
            return X.GetMostSignificantBits(X.BitLength - s).ConcatenateBits(bitsToAppend);
        }

        public virtual BitString BlockProduct(BitString x, BitString y)
        {
            //ThisLogger.Debug("Block Product");
            // Since these are blocks, they must be 128 bits long
            if (x.BitLength != 128 || y.BitLength != 128)
            {
                return null;
            }

            // Let R be the bit string 11100001 || 0^120
            var R = new BitString(new byte[] { 225 }).ConcatenateBits(new BitString(120));


            // Step 1: Let x_0x_1...x_127 denote the sequence of bits in X
            // NOTE: Above notation makes x0 Most Significant Bit.  CAVS Bitstring
            // class notation has bit 0 as Least Significant Bit.  Therefore, x_i
            // will be accessed as X.bit(127 - i)

            // Step 2: Let Z_0 = 0^128 and V_0 = Y
            var z = new BitString(128);
            var v = y;

            // Step 3:  For i = 0 to 127, calculate blocks Z_i+1 and V_i+1 as follows:
            for (int i = 0; i < 128; ++i)
            {
                // Z_i+1 = Z_i,       if x_i == 0
                //         Z_i xor V,         if x_i == 1
                bool x_i = x.Bits[127 - i];

                if (x_i)
                {
                    z = BitString.XOR(z, v);
                }

                // V_i+1 =  V_i >> 1,        if LSB_1(V_i) == 0
                //         (V_i >> 1) xor R, if LSB_1(V_i) == 1
                bool lsbV = v.Bits[0];
                // Perform the 1-bit right shift operation:
                v = new BitString(1).ConcatenateBits(v.GetMostSignificantBits(127));

                // Perform the xor with R only if lsb of V_i is 1
                if (lsbV)
                {
                    v = BitString.XOR(v, R);
                }

            }

            return z;
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
