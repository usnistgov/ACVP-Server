using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.AES_CFB1
{
    public class AES_CFB1 : IAES_CFB1
    {
        private readonly IRijndaelFactory _iRijndaelFactory;

        public AES_CFB1(IRijndaelFactory iRijndaelFactory)
        {
            _iRijndaelFactory = iRijndaelFactory;
        }

        public SymmetricCipherResult BlockDecrypt(BitString iv, BitString keyBits, BitString data)
        {
            try
            {
                byte[] keyBytes = keyBits.ToBytes();
                ModeValues mode = ModeValues.CFB1;
                var rijn = _iRijndaelFactory.GetRijndael(mode);
                var key = rijn.MakeKey(keyBytes, DirectionValues.Decrypt);
                var cipher = new Cipher { BlockLength = 128, Mode = mode, IV = iv, SegmentLength = 1};

                var paddedData = BitString.PadToNextByteBoundry(data);

                var decryptedBits = rijn.BlockEncrypt(cipher, key, paddedData.ToBytes(), data.BitLength);
                return new SymmetricCipherResult(decryptedBits.GetMostSignificantBits(data.BitLength));
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"keyLen:{keyBits.BitLength}; cipherTextLen:{data.BitLength}");
                ThisLogger.Error(ex);
                return new SymmetricCipherResult(ex.Message);
            }
        }

        public SymmetricCipherResult BlockEncrypt(BitString iv, BitString keyBits, BitString data)
        {
            try
            {
                byte[] keyBytes = keyBits.ToBytes();
                ModeValues mode = ModeValues.CFB1;
                var rijn = _iRijndaelFactory.GetRijndael(mode);
                var key = rijn.MakeKey(keyBytes, DirectionValues.Encrypt);
                var cipher = new Cipher { BlockLength = 128, Mode = mode, IV = iv, SegmentLength = 1 };

                var paddedData = BitString.PadToNextByteBoundry(data);

                var encryptedBits = rijn.BlockEncrypt(cipher, key, paddedData.ToBytes(), data.BitLength);
                return new SymmetricCipherResult(encryptedBits.GetMostSignificantBits(data.BitLength));
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
