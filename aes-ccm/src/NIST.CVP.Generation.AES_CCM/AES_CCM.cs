using System;
using NIST.CVP.Generation.AES;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CCM
{
    public class AES_CCM : IAES_CCM
    {
        private readonly IAES_CCMInternals _iAesCccmInternals;
        private readonly IRijndaelFactory _iRijndaelFactory;

        public AES_CCM(IAES_CCMInternals iAesCccmInternals, IRijndaelFactory iRijndaelFactory)
        {
            _iAesCccmInternals = iAesCccmInternals;
            _iRijndaelFactory = iRijndaelFactory;
        }

        public EncryptionResult Encrypt(BitString keyBits, BitString nonce, BitString payload, BitString associatedData, int tagLength)
        {
            // @@@ TODO

            byte[] B = null;
            byte[] ctr = new byte[16];
            int r = 0;

            _iAesCccmInternals.CCM_format_80211(
                ref B, 
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

            var mac = cbcMacRijndael.BlockEncrypt(cbcMacCipher, cbcMacKey, B, (r + 1) * 128);

            // Encrypt Payload and MAC
            mac = mac.GetMostSignificantBits(16 * 8).GetDeepCopy();
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

        public DecryptionResult Decrypt(BitString key, BitString nonce, BitString cipherText, BitString associatedData, int tagLength)
        {
            byte[] ctr = null;

            _iAesCccmInternals.CCM_counter_80211(nonce.ToBytes(), nonce.BitLength, ref ctr);

            // Decrypt cipherText and MAC
            int Plen = cipherText.BitLength - tagLength;
            int m = (Plen + 127) / 128 + 1; // the length of encrypted payload + 1 block for MAC
            byte[] CT = new byte[m];

            // Put the tag into CT first
            int ctIndex = 0;
            for (int i = Plen / 8; i < cipherText.BitLength - tagLength / 8; i++)
            {
                CT[ctIndex++] = cipherText[i];
            }

            // Now put the cipherText into CT
            ctIndex = 16;
            for (int i = 0; i < Plen / 8; i++)
            {
                CT[ctIndex++] = cipherText[i];
            }

            var counterRijndael = _iRijndaelFactory.GetRijndael(ModeValues.Counter);
            var counterKey = counterRijndael.MakeKey(key.ToBytes(), DirectionValues.Encrypt);
            Cipher counterCipher = new Cipher()
            {
                BlockLength = 128,
                IV = new BitString(ctr),
                Mode = ModeValues.Counter
            };

            var PT = counterRijndael.BlockEncrypt(counterCipher, counterKey, cipherText.ToBytes(), cipherText.BitLength);

            // TODO
            throw new NotImplementedException();
        }


    }
}
 