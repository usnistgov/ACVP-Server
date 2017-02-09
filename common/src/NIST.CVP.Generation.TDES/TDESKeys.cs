using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES
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

           //MakeKeys(bitString);
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
                Array.Copy(keyBytes,8, key2, 0,8);
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

        //private void MakeKeys(BitString bitString)
        //{
        //    var rawBytes = bitString.ToBytes();
            
        //    Keys = new List<byte[]>();
        //    for (int keyIdx = 0; keyIdx < 3; keyIdx++)
        //    {
        //        byte[] keyBytes = new byte[7];
        //        Array.Copy(rawBytes, 0, keyBytes, 0, 7); //see if we need to split bytes kyIdx * 7
        //        var key64 = Key56to64(keyBytes);
        //        var withParityKey = key64.SetOddParityBitInSuppliedBytes();
        //        Keys.Add(withParityKey);
        //    }
        //}

        //public byte[] Key56to64(byte[] input)
        //{
        //    byte[] output = new byte[8];
        //    for (int i = 7; i >= 0; i--)
        //    {
        //        output[i] = (byte)(input[6] << 1);
        //        for (int j = 0; j < 7; j++)
        //        {
        //            var inputBitArray = new BitArray(input);
        //            inputBitArray = inputBitArray.BitShiftRight();
        //            input = inputBitArray.ToBytes();
        //        }
        //    }
        //    return output;
        //}



        
    }
}
