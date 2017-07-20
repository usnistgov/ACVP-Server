using System;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.AES_CFB8
{
    public class AES_CFB8 : IAES_CFB8
    {
        private readonly IRijndaelFactory _iRijndaelFactory;

        public AES_CFB8(IRijndaelFactory iRijndaelFactory)
        {
            _iRijndaelFactory = iRijndaelFactory;
        }

        public DecryptionResult BlockDecrypt(BitString iv, BitString keyBits, BitString cipherText)
        {
            try
            {
                byte[] keyBytes = keyBits.ToBytes();
                ModeValues mode = ModeValues.CFB8;
                var rijn = _iRijndaelFactory.GetRijndael(mode);
                var key = rijn.MakeKey(keyBytes, DirectionValues.Decrypt);
                var cipher = new Cipher { BlockLength = 128, Mode = mode, IV = iv, SegmentLength = 8};
                var decryptBits = rijn.BlockEncrypt(cipher, key, cipherText.ToBytes(), cipherText.BitLength);
                return new DecryptionResult(decryptBits);
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"keyLen:{keyBits.BitLength}; cipherTextLen:{cipherText.BitLength}");
                ThisLogger.Error(ex);
                return new DecryptionResult(ex.Message);
            }
        }

        public EncryptionResult BlockEncrypt(BitString iv, BitString keyBits, BitString data)
        {
            try
            {
                byte[] keyBytes = keyBits.ToBytes();
                ModeValues mode = ModeValues.CFB8;
                var rijn = _iRijndaelFactory.GetRijndael(mode);
                var key = rijn.MakeKey(keyBytes, DirectionValues.Encrypt);
                var cipher = new Cipher { BlockLength = 128, Mode = mode, IV = iv, SegmentLength = 8};
                var encryptedBits = rijn.BlockEncrypt(cipher, key, data.ToBytes(), data.BitLength);
                return new EncryptionResult(encryptedBits);
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"keyLen:{keyBits.BitLength}; dataLen:{data.BitLength}");
                ThisLogger.Error(ex);
                return new EncryptionResult(ex.Message);
            }
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
