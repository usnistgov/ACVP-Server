using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class TDESKeys
    {
        public const int EXPECTED_BYTE_SIZE = 24;
        public List<byte[]> Keys { get; private set; }
        public TDESKeys(BitString bitString)
        {

           //MakeKeys(bitString);
           MakeKeySimple(bitString);
        }

        private void MakeKeySimple(BitString keyBits)
        {
            Keys = new List<byte[]>();
            var keyBytes = keyBits.ToBytes();
            var withParityKey = keyBytes; //.SetOddParityBitInSuppliedBytes();
            for (int keyIdx = 0; keyIdx < 3; keyIdx++)
            {
                Keys.Add(withParityKey);
            }

        }

        private void MakeKeys(BitString bitString)
        {
            var rawBytes = bitString.ToBytes();
            
            Keys = new List<byte[]>();
            for (int keyIdx = 0; keyIdx < 3; keyIdx++)
            {
                byte[] keyBytes = new byte[7];
                Array.Copy(rawBytes, 0, keyBytes, 0, 7); //see if we need to split bytes kyIdx * 7
                var key64 = Key56to64(keyBytes);
                var withParityKey = key64.SetOddParityBitInSuppliedBytes();
                Keys.Add(withParityKey);
            }
        }

        public byte[] Key56to64(byte[] input)
        {
            byte[] output = new byte[8];
            for (int i = 7; i >= 0; i--)
            {
                output[i] = (byte)(input[6] << 1);
                for (int j = 0; j < 7; j++)
                {
                    var inputBitArray = new BitArray(input);
                    inputBitArray = inputBitArray.BitShiftRight();
                    input = inputBitArray.ToBytes();
                }
            }
            return output;
        }



        
    }
}
