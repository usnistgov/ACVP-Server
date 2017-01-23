using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class TdesEcb: ITDES_ECB
    {
        public const int EXPECTED_BLOCK_SIZE = 64;
      

        public EncryptionResult BlockEncrypt(BitString keyBits, BitString data)
        {
            if (data.BitLength % EXPECTED_BLOCK_SIZE != 0)
            {
                return new EncryptionResult($"Supplied data size {data.BitLength} is not in the proper block size {EXPECTED_BLOCK_SIZE}");
            }
            var plainTextBytes = data.ToBytes();
            byte[] output = new byte[plainTextBytes.Length];

            for (int blockIdx = 0; blockIdx < plainTextBytes.Length / 8; blockIdx++)
            {
                byte[] input = new byte[8];
                Array.Copy(plainTextBytes, blockIdx * 8, input, 0, 8);
                var blockOutput = EncryptWorker(keyBits, input);
                Array.Copy(blockOutput, 0, output, blockIdx * 8, 8);
            }
            return new EncryptionResult(new BitString(output));
           
        }

        private byte[] EncryptWorker(BitString keyBits, byte[] input)
        {
            var keys = new TDESKeys(keyBits);
            var context = new TDESContext(keys, FunctionValues.Encryption);
            byte[] interm1 = context.Schedule[0].Apply(input);
            byte[] interm2 = context.Schedule[1].Apply(interm1);
            byte[] output = context.Schedule[2].Apply(interm2);
            return output;
        }

        public DecryptionResult BlockDecrypt(BitString keyBits, BitString cipherText)
        {
            throw new NotImplementedException();
        }

       
    }
}
