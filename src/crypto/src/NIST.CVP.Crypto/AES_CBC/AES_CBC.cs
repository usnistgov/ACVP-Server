using System;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.AES_CBC
{
    public class AES_CBC : IAES_CBC
    {
        private readonly IRijndaelFactory _iRijndaelFactory;

        public AES_CBC(IRijndaelFactory iRijndaelFactory)
        {
            _iRijndaelFactory = iRijndaelFactory;
        }

        public SymmetricCipherResult BlockDecrypt(BitString iv, BitString keyBits, BitString cipherText)
        {
            try
            {
                byte[] keyBytes = keyBits.ToBytes();
                ModeValues mode = ModeValues.CBC;
                var rijn = _iRijndaelFactory.GetRijndael(mode);
                var key = rijn.MakeKey(keyBytes, DirectionValues.Decrypt);
                var cipher = new Cipher { BlockLength = 128, Mode = mode, IV = iv};
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

        public SymmetricCipherResult BlockEncrypt(BitString iv, BitString keyBits, BitString data)
        {
            try
            {
                byte[] keyBytes = keyBits.ToBytes();
                ModeValues mode = ModeValues.CBC;
                var rijn = _iRijndaelFactory.GetRijndael(mode);
                var key = rijn.MakeKey(keyBytes, DirectionValues.Encrypt);
                var cipher = new Cipher { BlockLength = 128, Mode = mode, IV = iv};
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
