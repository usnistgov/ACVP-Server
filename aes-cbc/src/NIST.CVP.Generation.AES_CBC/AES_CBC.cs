using System;
using NIST.CVP.Generation.AES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CBC
{
    public class AES_CBC : IAES_CBC
    {
        private readonly IRijndaelFactory _iRijndaelFactory;

        public AES_CBC(IRijndaelFactory iRijndaelFactory)
        {
            _iRijndaelFactory = iRijndaelFactory;
        }

        public DecryptionResult BlockDecrypt(BitString iv, BitString keyBits, BitString cipherText)
        {
            try
            {
                byte[] keyBytes = keyBits.ToBytes();
                ModeValues mode = ModeValues.CBC;
                var rijn = _iRijndaelFactory.GetRijndael(mode);
                var key = rijn.MakeKey(keyBytes, DirectionValues.Decrypt);
                var cipher = new Cipher { BlockLength = 128, Mode = mode, IV = iv};

                var ivPreRun = cipher.IV.ToBytes();
                var decryptBits = rijn.BlockEncrypt(cipher, key, cipherText.ToBytes(), cipherText.BitLength);
                var ivPostRun = cipher.IV.ToBytes();
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
                // @@@ TODO need to get changes of IV within CBC implementation to the caller.
                // @@@ TODO By ref?  Or update implementation of algorithm to work with a BitString instead of byte[]

                byte[] keyBytes = keyBits.ToBytes();
                ModeValues mode = ModeValues.CBC;
                var rijn = _iRijndaelFactory.GetRijndael(mode);
                var key = rijn.MakeKey(keyBytes, DirectionValues.Encrypt);
                var cipher = new Cipher { BlockLength = 128, Mode = mode, IV = iv};
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
