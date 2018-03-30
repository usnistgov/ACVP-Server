using System;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.AES_ECB
{
    public class AES_ECB : IAES_ECB
    {
        private readonly IRijndaelFactory _iRijndaelFactory;

        public AES_ECB(IRijndaelFactory iRijndaelFactory)
        {
            _iRijndaelFactory = iRijndaelFactory;
        }

        public SymmetricCipherResult BlockDecrypt(BitString keyBits, BitString cipherText, bool encryptUsingInverseCipher = false)
        {
            try
            {
                byte[] keyBytes = keyBits.ToBytes();
                ModeValues mode = ModeValues.ECB;
                var rijn = _iRijndaelFactory.GetRijndael(mode);
                var key = rijn.MakeKey(keyBytes, DirectionValues.Decrypt, encryptUsingInverseCipher);
                var cipher = new Cipher { BlockLength = 128, Mode = mode };

                var decryptBits = rijn.BlockEncrypt(cipher, key, cipherText.ToBytes(), cipherText.BitLength);
                
                return new SymmetricCipherResult(decryptBits);
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"keyLen:{keyBits.BitLength}; cipherTextLen:{cipherText.BitLength}");
                ThisLogger.Error(ex);
                return new SymmetricCipherResult(ex.Message);
            }
        }

        public SymmetricCipherResult BlockEncrypt(BitString keyBits, BitString data, bool encryptedUsingInverseCipher = false)
        {
            try
            {
                byte[] keyBytes = keyBits.ToBytes();
                ModeValues mode = ModeValues.ECB;
                var rijn = _iRijndaelFactory.GetRijndael(mode);
                var key = rijn.MakeKey(keyBytes, DirectionValues.Encrypt, encryptedUsingInverseCipher);
                var cipher = new Cipher { BlockLength = 128, Mode = mode };

                var encryptedBits = rijn.BlockEncrypt(cipher, key, data.ToBytes(), data.BitLength);

                return new SymmetricCipherResult(encryptedBits);
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"keyLen:{keyBits.BitLength}; dataLen:{data.BitLength}");
                ThisLogger.Error(ex);
                return new SymmetricCipherResult(ex.Message);
            }
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
