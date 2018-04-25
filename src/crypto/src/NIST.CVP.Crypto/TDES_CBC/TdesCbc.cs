using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Common.Symmetric.TDES.Enums;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CBC
{
    public class TdesCbc : ITDES_CBC
    {
        public const int EXPECTED_BLOCK_SIZE = 64;


        public SymmetricCipherResult BlockEncrypt(BitString keyBits, BitString data, BitString iv)
        {
            if (data.BitLength % EXPECTED_BLOCK_SIZE != 0)
            {
                return new SymmetricCipherResult($"Supplied data size {data.BitLength} is not in the proper block size {EXPECTED_BLOCK_SIZE}");
            }
            var plainTextBytes = data.ToBytes();
            byte[] output = new byte[plainTextBytes.Length];
            BitString vector = iv;

            for (int blockIdx = 0; blockIdx < plainTextBytes.Length / 8; blockIdx++)
            {
                byte[] input = new byte[8];
                Array.Copy(plainTextBytes, blockIdx * 8, input, 0, 8);
                
                var inputToBeXOR = new BitString(input); //Create input variable that we can XOR
                var xorInput = vector.XOR(inputToBeXOR);
                input = xorInput.ToBytes(); //Set input equal to the XOR'd input, in byte format

                var blockOutput = EncryptWorker(keyBits, input);
                Array.Copy(blockOutput, 0, output, blockIdx * 8, 8);
                vector = new BitString(blockOutput);
            }
            return new SymmetricCipherResult(new BitString(output));

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

        public SymmetricCipherResult BlockDecrypt(BitString keyBits, BitString cipherText, BitString iv)
        {
            if (cipherText.BitLength % EXPECTED_BLOCK_SIZE != 0)
            {
                return new SymmetricCipherResult($"Supplied data size {cipherText.BitLength} is not in the proper block size {EXPECTED_BLOCK_SIZE}");
            }
            var cipherTextBytes = cipherText.ToBytes();
            byte[] output = new byte[cipherTextBytes.Length];
            BitString vector = iv;

            for (int blockIdx = 0; blockIdx < cipherTextBytes.Length / 8; blockIdx++)
            {
                byte[] input = new byte[8];
                Array.Copy(cipherTextBytes, blockIdx * 8, input, 0, 8);
                var blockOutput = DecryptWorker(keyBits, input);

                var outputToBeXOR = new BitString(blockOutput); //Create output variable that we can XOR
                var XorOutput = vector.XOR(outputToBeXOR);
                blockOutput = XorOutput.ToBytes(); //Set output equal to the XOR'd output, in byte format

                Array.Copy(blockOutput, 0, output, blockIdx * 8, 8);
                vector = new BitString(input);
            }
            return new SymmetricCipherResult(new BitString(output));
        }

        private byte[] DecryptWorker(BitString keyBits, byte[] input)
        {
            var keys = new TDESKeys(keyBits);
            var context = new TDESContext(keys, FunctionValues.Decryption);
            byte[] interm1 = context.Schedule[2].Apply(input);
            byte[] interm2 = context.Schedule[1].Apply(interm1);
            byte[] output = context.Schedule[0].Apply(interm2);
            var data = new BitString(output);
            return output;
        }


    }
}
