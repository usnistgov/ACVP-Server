using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_GCM
{
    public class AES_GCM : IAES_GCM
    {
        private  IRandom800_90 _randy = new Random800_90();
        public DecryptionResult BlockDecrypt(BitString keyBits, BitString cipherText, BitString iv, BitString aad, BitString tag)
        {
            return  new DecryptionResult(_randy.GetDifferentBitStringOfSameSize(cipherText));
        }

        public EncryptionResult BlockEncrypt(BitString keyBits, BitString data, BitString iv, BitString aad, int tagLength)
        {
           return new EncryptionResult(_randy.GetDifferentBitStringOfSameSize(data), _randy.GetRandomBitString(tagLength));
        }
    }
}
