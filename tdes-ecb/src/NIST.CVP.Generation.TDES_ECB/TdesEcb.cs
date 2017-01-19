using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class TdesEcb: ITDES_ECB
    {
        public const int EXPECTED_DATA_LENGTH = 64;
      

        public EncryptionResult BlockEncrypt(BitString keyBits, BitString data)
        {
            if (data.BitLength != EXPECTED_DATA_LENGTH)
            {
                return new EncryptionResult($"Supplied data size {data.BitLength} != {EXPECTED_DATA_LENGTH} bits");
            }
            var keys = new TDESKeys(keyBits);
            var context = new TDESContext(keys, FunctionValues.Encryption);
            var input = data.ToBytes();

            byte[] interm1 = context.Schedule[0].Apply(input);
            byte[] interm2 = context.Schedule[1].Apply(interm1);
            byte[] output = context.Schedule[2].Apply(interm2);

            return new EncryptionResult(new BitString(output));
           
        }

        public DecryptionResult BlockDecrypt(BitString keyBits, BitString cipherText)
        {
            throw new NotImplementedException();
        }

      

      
    }
}
