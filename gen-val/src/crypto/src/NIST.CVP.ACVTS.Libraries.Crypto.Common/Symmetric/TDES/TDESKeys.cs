using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES
{
    public class TDESKeys
    {

        public List<byte[]> Keys { get; private set; }

        public List<BitString> KeysAsBitStrings
        {
            get
            {
                var list = new List<BitString>();
                foreach (var keyByteArray in Keys)
                {
                    list.Add(new BitString(keyByteArray));
                }
                return list;
            }
        }
        public TDESKeys(BitString bitString)
        {
            if (bitString.BitLength != 64 && bitString.BitLength != 128 && bitString.BitLength != 192)
            {
                throw new Exception("Invalid key length, expected 64, 128, 192 bits");
            }

            MakeKeySimple(bitString);
        }

        private void MakeKeySimple(BitString keyBits)
        {

            byte[] key1 = new byte[8];
            byte[] key2 = new byte[8];
            byte[] key3 = new byte[8];

            var keyBytes = keyBits.ToBytes();

            //we have one key, re-use for all three
            if (keyBytes.Length == 8)
            {
                Array.Copy(keyBytes, key1, 8);
                Array.Copy(keyBytes, key2, 8);
                Array.Copy(keyBytes, key3, 8);
            }
            if (keyBytes.Length == 16)
            {
                //we have two keys, key1 = key3
                Array.Copy(keyBytes, key1, 8);
                Array.Copy(keyBytes, key3, 8);
                Array.Copy(keyBytes, 8, key2, 0, 8);
            }
            if (keyBytes.Length == 24)
            {
                //we have three keys
                Array.Copy(keyBytes, key1, 8);
                Array.Copy(keyBytes, 8, key2, 0, 8);
                Array.Copy(keyBytes, 16, key3, 0, 8);
            }
            Keys = new List<byte[]>();
            Keys.Add(key1);
            Keys.Add(key2);
            Keys.Add(key3);

        }

        public KeyOptionValues KeyOption
        {
            get
            {
                if (KeysAsBitStrings.All(k => k.Equals(KeysAsBitStrings[0])))
                {
                    return KeyOptionValues.OneKey;
                }

                if (KeysAsBitStrings[0].Equals(KeysAsBitStrings[2]))
                {
                    return KeyOptionValues.TwoKey;
                }

                return KeyOptionValues.ThreeKey;
            }
        }
    }
}
