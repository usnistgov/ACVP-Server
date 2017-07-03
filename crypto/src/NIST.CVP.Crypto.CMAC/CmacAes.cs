using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.CMAC
{
    public class CmacAes : ICmac
    {
        private readonly IRijndaelFactory _iRijndaelFactory;
        private const ModeValues Mode = ModeValues.CMAC;

        public CmacAes(IRijndaelFactory iRijndaelFactory)
        {
            _iRijndaelFactory = iRijndaelFactory;
        }
        
        public CmacResult Generate(BitString keyBits, BitString message, int macLength)
        {
            try
            {
                byte[] keyBytes = keyBits.ToBytes();
                ModeValues mode = Mode;
                var rijn = _iRijndaelFactory.GetRijndael(mode);
                var key = rijn.MakeKey(keyBytes, DirectionValues.Encrypt);
                var iv = new BitString(Cipher._MAX_IV_BYTE_LENGTH * 8);
                var cipher = new Cipher { BlockLength = 128, Mode = mode, IV = iv };
                
                var encryptedBits = rijn.BlockEncrypt(cipher, key, message.ToBytes(), macLength);

                var mac = encryptedBits.GetMostSignificantBits(macLength);
                
                return new CmacResult(mac);
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"keyLen:{keyBits.BitLength}; dataLen:{message.BitLength}");
                ThisLogger.Error(ex);
                return new CmacResult(ex.Message);
            }
        }

        public CmacResult Verify(BitString keyBits, BitString message, BitString macToVerify)
        {
            try
            {
                var mac = Generate(keyBits, message, macToVerify.BitLength);

                if (!mac.Success)
                {
                    return new CmacResult(mac.ErrorMessage);
                }

                if (mac.ResultingMac.Equals(macToVerify))
                {
                    return new CmacResult(mac.ResultingMac);
                }
                
                return new CmacResult("CMAC did not match.");
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"keyLen:{keyBits.BitLength}; dataLen:{message.BitLength}");
                ThisLogger.Error(ex);
                return new CmacResult(ex.Message);
            }
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
