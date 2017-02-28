using System;
using System.Linq;
using NIST.CVP.Generation.AES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CCM
{
    public class AES_CCM : IAES_CCM
    {
        private readonly IAES_CCMInternals _iAesCcmInternals;
        private readonly IRijndaelFactory _iRijndaelFactory;

        public const string INVALID_TAG_MESSAGE = "Tags do not match";

        public AES_CCM(IAES_CCMInternals iAesCcmInternals, IRijndaelFactory iRijndaelFactory)
        {
            _iAesCcmInternals = iAesCcmInternals;
            _iRijndaelFactory = iRijndaelFactory;
        }

        public EncryptionResult Encrypt(BitString keyBits, BitString nonce, BitString payload, BitString associatedData, int tagLength)
        {
            try
            {
                byte[] b = null;
                byte[] ctr = new byte[16];
                int r = 0;

                _iAesCcmInternals.CCM_format_80211(
                    ref b,
                    nonce.ToBytes(),
                    nonce.BitLength,
                    payload.ToBytes(),
                    payload.BitLength,
                    associatedData.ToBytes(),
                    associatedData.BitLength,
                    tagLength,
                    ref ctr,
                    ref r
                );

                // MAC
                var cbcMacRijndael = _iRijndaelFactory.GetRijndael(ModeValues.CBCMac);
                var cbcMacKey = cbcMacRijndael.MakeKey(keyBits.ToBytes(), DirectionValues.Encrypt);

                BitString iv = new BitString(Cipher._MAX_IV_BYTE_LENGTH * 8);
                Cipher cbcMacCipher = new Cipher()
                {
                    BlockLength = 128,
                    IV = iv,
                    Mode = ModeValues.CBCMac
                };

                var mac = cbcMacRijndael.BlockEncrypt(cbcMacCipher, cbcMacKey, b, (r + 1) * 128);
                mac = new BitString(mac.ToBytes().Take(16).ToArray());

                // Encrypt Payload and MAC
                var counterRijndael = _iRijndaelFactory.GetRijndael(ModeValues.Counter);
                var counterKey = counterRijndael.MakeKey(keyBits.ToBytes(), DirectionValues.Encrypt);
                var counterCipher = new Cipher()
                {
                    BlockLength = 128,
                    IV = new BitString(ctr),
                    Mode = ModeValues.Counter
                };

                var T = counterRijndael.BlockEncrypt(counterCipher, counterKey, mac.ToBytes(), 128);

                int m = (payload.BitLength + 127) / 128;
                var ct = counterRijndael.BlockEncrypt(counterCipher, counterKey, payload.ToBytes(), m * 128);

                return new EncryptionResult(ct.ConcatenateBits(T));
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new EncryptionResult(ex.Message);
            }
        }

        public DecryptionResult Decrypt(BitString key, BitString nonce, BitString cipherText, BitString associatedData, int tagLength)
        {
            try
            {
                byte[] ctr = null;
                int r = 0;

                _iAesCcmInternals.CCM_counter_80211(nonce.ToBytes(), nonce.BitLength, ref ctr);

                // Decrypt cipherText and MAC
                int plen = cipherText.BitLength - tagLength;
                int m = (plen + 127) / 128 + 1; // the length of encrypted payload + 1 block for MAC

                var tagPortion = new BitString(cipherText.ToBytes().Skip(plen / 8).Take(tagLength / 8).ToArray());
                var cipherTextPortion = new BitString(cipherText.ToBytes().Take(plen / 8).ToArray());

                byte[] ct = tagPortion.ConcatenateBits(cipherTextPortion).ToBytes();

                var counterRijndael = _iRijndaelFactory.GetRijndael(ModeValues.Counter);
                var counterKey = counterRijndael.MakeKey(key.ToBytes(), DirectionValues.Encrypt);
                Cipher counterCipher = new Cipher()
                {
                    BlockLength = 128,
                    IV = new BitString(ctr),
                    Mode = ModeValues.Counter
                };

                var pt = counterRijndael.BlockEncrypt(counterCipher, counterKey, ct,
                    m * 128);

                // Payload starts at 16th byte of PT
                var payload = new BitString(pt.ToBytes().Skip(16).ToArray());

                // The tag is the first Tlen/8 bytes of the PT
                var T = new BitString(16 * 8).XOR(pt.GetMostSignificantBits(tagLength));

                // Format the data
                byte[] b = null;

                _iAesCcmInternals.CCM_format_80211(ref b, nonce.ToBytes(), nonce.BitLength, payload.ToBytes(), plen,
                    associatedData.ToBytes(), associatedData.BitLength, tagLength, ref ctr, ref r);

                // Calculate the MAC
                BitString iv = new BitString(16 * 8);
                var cbcMacRijndael = _iRijndaelFactory.GetRijndael(ModeValues.CBCMac);
                var cbcMacKey = cbcMacRijndael.MakeKey(key.ToBytes(), DirectionValues.Encrypt);
                var cbcMacCipher = new Cipher()
                {
                    BlockLength = 128,
                    IV = iv,
                    Mode = ModeValues.CBCMac
                };

                var mac = cbcMacRijndael.BlockEncrypt(cbcMacCipher, cbcMacKey, b, (r + 1) * 128);
                mac = new BitString(mac.ToBytes().Take(16).ToArray());

                if (!mac.Equals(T))
                {
                    return new DecryptionResult(INVALID_TAG_MESSAGE);
                }

                return new DecryptionResult(payload);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new DecryptionResult(ex.Message);
            }
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
 