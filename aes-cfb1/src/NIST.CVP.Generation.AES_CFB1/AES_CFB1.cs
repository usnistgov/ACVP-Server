using System;
using System.Collections.Generic;
using NIST.CVP.Generation.AES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CFB1
{
    public class AES_CFB1 : IAES_CFB1
    {
        private readonly IRijndaelFactory _iRijndaelFactory;

        public AES_CFB1(IRijndaelFactory iRijndaelFactory)
        {
            _iRijndaelFactory = iRijndaelFactory;
        }

        public DecryptionResult BlockDecrypt(BitString iv, BitString keyBits, BitString data)
        {
            try
            {
                byte[] keyBytes = keyBits.ToBytes();
                ModeValues mode = ModeValues.CFB1;
                var rijn = _iRijndaelFactory.GetRijndael(mode);
                var key = rijn.MakeKey(keyBytes, DirectionValues.Encrypt);
                var cipher = new Cipher { BlockLength = 128, Mode = mode, IV = iv, SegmentLength = 1};
                var decryptBits = rijn.BlockEncrypt(cipher, key, data.ToBytes(), cipher.SegmentLength * 8);
                return new DecryptionResult(decryptBits);
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"keyLen:{keyBits.BitLength}; cipherTextLen:{data.BitLength}");
                ThisLogger.Error(ex);
                return new DecryptionResult(ex.Message);
            }
        }

        public EncryptionResult BlockEncrypt(BitString iv, BitString keyBits, BitString data)
        {
            try
            {
                byte[] keyBytes = keyBits.ToBytes();
                ModeValues mode = ModeValues.CFB1;
                var rijn = _iRijndaelFactory.GetRijndael(mode);
                var key = rijn.MakeKey(keyBytes, DirectionValues.Encrypt);
                var cipher = new Cipher { BlockLength = 128, Mode = mode, IV = iv, SegmentLength = 1 };
                var encryptedBits = rijn.BlockEncrypt(cipher, key, data.ToBytes(), cipher.SegmentLength * 8);
                return new EncryptionResult(encryptedBits.GetMostSignificantBits(cipher.SegmentLength));
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
