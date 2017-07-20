using System;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.AES_GCM
{
    public class AES_GCM : IAES_GCM
    {

        private readonly IAES_GCMInternals _iAES_GCMInternals;
        private readonly IRijndaelFactory _iRijndaelFactory;

        public AES_GCM(IAES_GCMInternals iAES_GCMInternals, IRijndaelFactory iRijndaelFactory)
        {
            _iAES_GCMInternals = iAES_GCMInternals;
            _iRijndaelFactory = iRijndaelFactory;
        }

        public DecryptionResult BlockDecrypt(BitString keyBits, BitString cipherText, BitString iv, BitString aad, BitString tag)
        {
            try
            {
                byte[] keyBytes = keyBits.ToBytes();
                ModeValues mode = ModeValues.ECB;
                var rijn = _iRijndaelFactory.GetRijndael(mode);
                var key = rijn.MakeKey(keyBytes, DirectionValues.Encrypt);
                var cipher = new Cipher { BlockLength = 128, Mode = mode };
                var h = rijn.BlockEncrypt(cipher, key, new byte[16], 128);
                var j0 = Getj0(h, iv);

                var plainText = GCTR(inc_s(32, j0), cipherText, key);//rework Block to deal with only key or key bitstring

                int u = 128 * Ceiling(plainText.BitLength, 128) - plainText.BitLength;
                int v = 128 * Ceiling(aad.BitLength, 128) - aad.BitLength;

                var decryptedBits =
                    aad.ConcatenateBits(new BitString(v))
                        .ConcatenateBits(cipherText)
                        .ConcatenateBits(new BitString(u))
                        .ConcatenateBits(BitString.To64BitString(aad.BitLength))
                        .ConcatenateBits(BitString.To64BitString(cipherText.BitLength));

                var s = GHash(h,decryptedBits);
                var tagPrime = BitString.GetMostSignificantBits(tag.BitLength, GCTR(j0, s, key));
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
                {
                    return new DecryptionResult(ex.Message);
                }
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
                ModeValues mode = ModeValues.ECB;
                var rijn = _iRijndaelFactory.GetRijndael(mode);
                var key = rijn.MakeKey(keyBytes, DirectionValues.Encrypt);
                var cipher = new Cipher { BlockLength = 128, Mode = mode };
                var h = rijn.BlockEncrypt(cipher, key, new byte[16], 128);
                ThisLogger.Debug($"h: {h.ToHex()}");
                var j0 = Getj0(h, iv);
                ThisLogger.Debug($"j0: {j0.ToHex()}");
                //ThisLogger.Debug($"Cipher Text: {j0.Length}");
                var cipherText = GCTR(inc_s(32, j0), data, key);//rework Block to deal with only key or key bitstring
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
                var tag = BitString.GetMostSignificantBits(tagLength, GCTR(j0, s, key));
                ThisLogger.Debug($"Tag: {tag.ToHex()}");
                return new EncryptionResult(cipherText, tag);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new EncryptionResult(ex.Message);
                }
            }
        }

        private BitString Getj0(BitString h, BitString iv)
        {
            return _iAES_GCMInternals.Getj0(h, iv);
        }

        private int Ceiling(int numerator, int denominator)
        {
            return _iAES_GCMInternals.Ceiling(numerator, denominator);
        }

        private BitString GHash(BitString h, BitString x)
        {
            return _iAES_GCMInternals.GHash(h, x);
        }

        private BitString GCTR(BitString icb, BitString x, Key key)
        {
            return _iAES_GCMInternals.GCTR(icb, x, key);
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
            return _iAES_GCMInternals.inc_s(s, X);
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
