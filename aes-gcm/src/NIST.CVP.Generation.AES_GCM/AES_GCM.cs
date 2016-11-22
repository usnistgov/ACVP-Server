using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Generation.AES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_GCM
{
    public class AES_GCM : IAES_GCM
    {
        
        public DecryptionResult BlockDecrypt(BitString keyBits, BitString cipherText, BitString iv, BitString aad, BitString tag)
        {
            try
            {
                byte[] keyBytes = keyBits.ToBytes();
                var rijn = new RijndaelECB(new RijndaelInternals());//@@@inject?
                var key = rijn.MakeKey(keyBytes, DirectionValues.Enrypt);
                var cipher = new Cipher { BlockLength = 128, Mode = ModeValues.ECB };
                var h = rijn.BlockEncrypt(cipher, key, new byte[16], 128);
                var j0 = Getj0(h, iv);

                var plainText = GCTR(keyBits, inc_s(32, j0), cipherText, key);//rework Block to deal with only key or key bitstring

                int u = 128 * Ceiling(plainText.BitLength, 128) - plainText.BitLength;
                int v = 128 * Ceiling(aad.BitLength, 128) - aad.BitLength;

                var decryptedBits =
                    aad.ConcatenateBits(new BitString(v))
                        .ConcatenateBits(cipherText)
                        .ConcatenateBits(new BitString(u))
                        .ConcatenateBits(BitString.To64BitString(aad.BitLength))
                        .ConcatenateBits(BitString.To64BitString(cipherText.BitLength));

                var s = GHash(h,decryptedBits);
                var tagPrime = MSB(tag.BitLength, GCTR(keyBits, j0, s, key));
                if (!tag.Equals(tagPrime))
                {
                    ThisLogger.Debug(plainText.ToHex());
                    ThisLogger.Debug($"tag :{tag}");
                    ThisLogger.Debug($"tag':{tagPrime}");
                    return new DecryptionResult("Tags do not match");
                }

                return new DecryptionResult(plainText);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new DecryptionResult(ex.Message);
            }
        }

        public EncryptionResult BlockEncrypt(BitString keyBits, BitString data, BitString iv, BitString aad, int tagLength)
        {
            try

            {
                ThisLogger.Debug($"ivLen: {iv.BitLength}, klen: {keyBits.BitLength}");
                ThisLogger.Debug($"iv: {iv.ToHex()}");
                ThisLogger.Debug($"key: {keyBits.ToHex()}");
                byte[] keyBytes = keyBits.ToBytes();
                var rijn = new RijndaelECB(new RijndaelInternals());//@@@inject?
                var key = rijn.MakeKey(keyBytes, DirectionValues.Enrypt);
                var cipher = new Cipher { BlockLength = 128, Mode = ModeValues.ECB };
                var h = rijn.BlockEncrypt(cipher, key, new byte[16], 128);
                ThisLogger.Debug($"h: {h.ToHex()}");
                var j0 = Getj0(h, iv);
                ThisLogger.Debug($"j0: {j0.ToHex()}");
                //ThisLogger.Debug($"Cipher Text: {j0.Length}");
                var cipherText = GCTR(keyBits, inc_s(32, j0), data, key);//rework Block to deal with only key or key bitstring
                ThisLogger.Debug($"cipherLen: {cipherText.BitLength}");
                ThisLogger.Debug($"aadLen: {aad.BitLength}");
                int u = 128 * Ceiling(cipherText.BitLength, 128) - cipherText.BitLength;
                int v = 128 * Ceiling(aad.BitLength, 128) - aad.BitLength;
                ThisLogger.Debug($"u: {u}, v: {v}");
                var encryptedBits =
                    aad.ConcatenateBits(new BitString(v))
                        .ConcatenateBits(cipherText)
                        .ConcatenateBits(new BitString(u))
                        .ConcatenateBits(BitString.To64BitString(aad.BitLength))
                        .ConcatenateBits(BitString.To64BitString(cipherText.BitLength));
                ThisLogger.Debug($"encrBits: {encryptedBits.ToHex()}");
                var s = GHash(h, encryptedBits);
                ThisLogger.Debug($"s: {s.ToHex()}");
                var tag = MSB(tagLength, GCTR(keyBits, j0, s, key));
                ThisLogger.Debug($"Tag: {tag.ToHex()}");
                return new EncryptionResult(cipherText, tag);
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"keyLen:{keyBits.BitLength}; dataLen:{data.BitLength}; ivLen:{iv.BitLength}; aadLen:{aad.BitLength}, tagLength:{tagLength}");
                ThisLogger.Error(ex);
                return new EncryptionResult(ex.Message);
            }

            
        }

        private BitString Getj0(BitString h, BitString iv)
        {
            BitString j0 = null;
            //ThisLogger.Debug("Getj0");
            if (iv.BitLength == 96)
            {
                j0 = iv.ConcatenateBits(new BitString(new BitArray(32)));
                j0.Set(0, true);
            }
            //	If len(IV) != 96, then let s = 128 * ceil(len(IV)/128) - len(IV)
            //  and let J0 = GHASH_H(IV || 0^(s + 64) || [len(IV)]_64)
            else
            {
                int s = 128 * Ceiling(iv.BitLength, 128) - iv.BitLength;
                var x = iv.ConcatenateBits(new BitString(new BitArray(s + 64))).ConcatenateBits(BitString.To64BitString(iv.BitLength));
                j0 = GHash(h, iv.ConcatenateBits(x));
            }

            return j0;
        }

        private int Ceiling(int numerator, int denominator)
        {

            int c = numerator / denominator;
            if ((numerator % denominator) > 0)
            {
                ++c;
            }

            return c;
            //return (int) System.Math.Ceiling((double) numerator/denominator);
        }

        private BitString GHash(BitString h, BitString x)
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
            var y = new BitString(new BitArray(128));

            // Step 3: For i = 1,...,m let Yi = (Yi-1 xor Xi) dot H
            for (int i = 0; i < m; ++i)
            {
                BitString xi = x.Substring((m - i - 1) * 128, 128);
                BitString YxorXi = BitString.XOR(y, xi);
                y = BlockProduct(YxorXi, h);
            }

            return y;
        }

        private BitString GCTR(BitString k, BitString icb, BitString x, Key key)
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
            int n = Ceiling(x.BitLength, 128);


            // Step 3: Let X1,X2,...,Xn-1,Xn denote the unique sequence of bit
            // strings such that X = X1 || X2 || ... || Xn-1 || Xn*
            // X1, X2,...,Xn-1 are complete blocks
            // Xn* is either a complete block or a partial block

            // Step 4: Let CB1 = ICB
            // Step 5: For i = 2 to n, let CBi = inc32(CBi-1)
            // Step 6: For i = 1 to n-1, let Yi = Xi xor CIPH_K(CBi)
            var rijn = new RijndaelECB(new RijndaelInternals());//@@@inject?
            var cipher = new Cipher { BlockLength = 128, Mode = ModeValues.ECB };
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

            var yn = BitString.XOR(xn, MSB(xn.BitLength, h1));
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
        private BitString inc_s(int s, BitString X)
        {

            // inc_s(X) = MSB_len(X)-s || [int(LSB_s(X)) + 1 mod 2^s]_s
            if (X.BitLength < s)
            {
                return null;
            }
            BigInteger lsp1 = LSB(s, X).ToBigInteger() + new BigInteger(1);
            //ThisLogger.Debug($"lsp1: {lsp1}");
            lsp1 = lsp1 % (new BigInteger(1) << s);
            //ThisLogger.Debug($"lsp2: {lsp1}");

            var bitsToAppend = new BitString(lsp1, s);
            //ThisLogger.Debug($"BitsToAppendLength: {bitsToAppend.Length}");
            return X.GetMostSignificantBits(X.BitLength - s).ConcatenateBits(bitsToAppend);
        }

        private BitString LSB(int numBits, BitString x)
        {
            return x.GetLeastSignificantBits(numBits);
        }

        private BitString MSB(int numBits, BitString x)
        {
            return x.GetMostSignificantBits(numBits);
        }

        private BitString BlockProduct(BitString x, BitString y)
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
