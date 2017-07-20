using System;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_OFB
{
    public class TdesOfb : ITDES_OFB
    {
        public const int EXPECTED_BLOCK_SIZE = 64;

        public EncryptionResult BlockEncrypt(BitString keyBits, BitString data, BitString iv)
        {
            if (data.BitLength % EXPECTED_BLOCK_SIZE != 0)
            {
                return new EncryptionResult($"Supplied data size {data.BitLength} is not in the proper block size {EXPECTED_BLOCK_SIZE}");
            }
            var plainTextBytes = data.ToBytes();
            byte[] output = new byte[plainTextBytes.Length];
            BitString vector = iv;

            for (int blockIdx = 0; blockIdx < plainTextBytes.Length / 8; blockIdx++)
            {
                byte[] encryptionBlockInput = new byte[8];
                encryptionBlockInput = vector.ToBytes();

                var blockOutput = EncryptWorker(keyBits, encryptionBlockInput);
                vector = new BitString(blockOutput);

                byte[] plainTextBlock = new byte[8];
                Array.Copy(plainTextBytes, blockIdx * 8, plainTextBlock, 0, 8);

                var inputToBeXOR = new BitString(plainTextBlock);
                

                var xorOutput = inputToBeXOR.XOR(vector);

                Array.Copy(xorOutput.ToBytes(), 0, output, blockIdx * 8, 8);

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

        public DecryptionResult BlockDecrypt(BitString keyBits, BitString cipherText, BitString iv)
        {
            if (cipherText.BitLength % EXPECTED_BLOCK_SIZE != 0)
            {
                return new DecryptionResult($"Supplied data size {cipherText.BitLength} is not in the proper block size {EXPECTED_BLOCK_SIZE}");
            }
            var cipherTextBytes = cipherText.ToBytes();
            byte[] output = new byte[cipherTextBytes.Length];
            BitString vector = iv;

            for (int blockIdx = 0; blockIdx < cipherTextBytes.Length / 8; blockIdx++)
            {
                byte[] decryptionBlockInput = new byte[8];
                decryptionBlockInput = vector.ToBytes();

                var blockOutput = DecryptWorker(keyBits, decryptionBlockInput);
                vector = new BitString(blockOutput);

                byte[] cipherTextBlock = new byte[8];
                Array.Copy(cipherTextBytes, blockIdx * 8, cipherTextBlock, 0, 8);

                var inputToBeXOR = new BitString(cipherTextBlock);


                var xorOutput = inputToBeXOR.XOR(vector);

                Array.Copy(xorOutput.ToBytes(), 0, output, blockIdx * 8, 8);

            }
            return new DecryptionResult(new BitString(output));
        }

        private byte[] DecryptWorker(BitString keyBits, byte[] input)
        {
            var keys = new TDESKeys(keyBits);
            var context = new TDESContext(keys, FunctionValues.Encryption);
            byte[] interm1 = context.Schedule[0].Apply(input);
            byte[] interm2 = context.Schedule[1].Apply(interm1);
            byte[] output = context.Schedule[2].Apply(interm2);
            var data = new BitString(output);
            return output;
        }
    }
}
