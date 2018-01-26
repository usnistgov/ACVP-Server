using System;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.CMAC;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.CMAC
{
    public class CmacAes : ICmac
    {
        private readonly IRijndaelFactory _iRijndaelFactory;
        private const ModeValues Mode = ModeValues.CMAC;

        public int OutputLength => 128;

        public CmacAes(IRijndaelFactory iRijndaelFactory)
        {
            _iRijndaelFactory = iRijndaelFactory;
        }
        
        public MacResult Generate(BitString keyBits, BitString message, int macLength = 0)
        {
            try
            {
                byte[] keyBytes = keyBits.ToBytes();
                ModeValues mode = Mode;
                var rijn = _iRijndaelFactory.GetRijndael(mode);
                var key = rijn.MakeKey(keyBytes, DirectionValues.Encrypt);
                var iv = new BitString(Cipher._MAX_IV_BYTE_LENGTH * 8);
                var cipher = new Cipher { BlockLength = 128, Mode = mode, IV = iv };
                
                var encryptedBits = rijn.BlockEncrypt(cipher, key, message.ToBytes(), cipher.BlockLength);

                BitString mac;
                if (macLength != 0)
                {
                    mac = encryptedBits.GetMostSignificantBits(macLength);
                }
                else
                {
                    mac = encryptedBits.GetDeepCopy();
                }
                
                return new MacResult(mac);
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"keyLen:{keyBits.BitLength}; dataLen:{message.BitLength}");
                ThisLogger.Error(ex);
                return new MacResult(ex.Message);
            }
        }

        public MacResult Verify(BitString keyBits, BitString message, BitString macToVerify)
        {
            try
            {
                var mac = Generate(keyBits, message, macToVerify.BitLength);

                if (!mac.Success)
                {
                    return new MacResult(mac.ErrorMessage);
                }

                if (mac.Mac.Equals(macToVerify))
                {
                    return new MacResult(mac.Mac);
                }
                
                return new MacResult("CMAC did not match.");
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"keyLen:{keyBits.BitLength}; dataLen:{message.BitLength}");
                ThisLogger.Error(ex);
                return new MacResult(ex.Message);
            }
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
