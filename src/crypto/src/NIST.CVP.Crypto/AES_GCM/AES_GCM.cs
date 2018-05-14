using System;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
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

        public SymmetricCipherResult BlockDecrypt(BitString keyBits, BitString cipherText, BitString iv, BitString aad, BitString tag)
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

                var plainText = GCTR(inc_s(32, j0), cipherText, keyBits);//rework Block to deal with only key or key bitstring

                int u = 128 * plainText.BitLength.CeilingDivide(128) - plainText.BitLength;
                int v = 128 * aad.BitLength.CeilingDivide(128) - aad.BitLength;

                var decryptedBits =
                    aad.ConcatenateBits(new BitString(v))
                        .ConcatenateBits(cipherText)
                        .ConcatenateBits(new BitString(u))
                        .ConcatenateBits(BitString.To64BitString(aad.BitLength))
                        .ConcatenateBits(BitString.To64BitString(cipherText.BitLength));

                var s = GHash(h,decryptedBits);
                var tagPrime = GCTR(j0, s, keyBits).GetMostSignificantBits(tag.BitLength);
                if (!tag.Equals(tagPrime))
                {
                    ThisLogger.Debug(plainText.ToHex());
                    ThisLogger.Debug($"tag :{tag}");
                    ThisLogger.Debug($"tag':{tagPrime}");
                    return new SymmetricCipherResult("Tags do not match");
                }

                return new SymmetricCipherResult(plainText);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new SymmetricCipherResult(ex.Message);
                }
            }
        }

        public SymmetricCipherAeadResult BlockEncrypt(BitString keyBits, BitString data, BitString iv, BitString aad, int tagLength)
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
                var cipherText = GCTR(inc_s(32, j0), data, keyBits);//rework Block to deal with only key or key bitstring
                ThisLogger.Debug($"cipherLen: {cipherText.BitLength}");
                ThisLogger.Debug($"aadLen: {aad.BitLength}");
                int u = 128 * cipherText.BitLength.CeilingDivide(128) - cipherText.BitLength;
                int v = 128 * aad.BitLength.CeilingDivide(128) - aad.BitLength;
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
                var tag = GCTR(j0, s, keyBits).GetMostSignificantBits(tagLength);
                ThisLogger.Debug($"Tag: {tag.ToHex()}");
                return new SymmetricCipherAeadResult(cipherText, tag);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new SymmetricCipherAeadResult(ex.Message);
                }
            }
        }

        private BitString Getj0(BitString h, BitString iv)
        {
            return _iAES_GCMInternals.Getj0(h, iv);
        }
        
        private BitString GHash(BitString h, BitString x)
        {
            return _iAES_GCMInternals.GHash(h, x);
        }

        private BitString GCTR(BitString icb, BitString x, BitString key)
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
