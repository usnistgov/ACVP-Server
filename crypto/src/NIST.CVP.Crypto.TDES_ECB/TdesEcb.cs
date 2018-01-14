using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_ECB
{
    public class TdesEcb : ITDES_ECB
    {
        public const int EXPECTED_BLOCK_SIZE = 64;


        public SymmetricCipherResult BlockEncrypt(BitString keyBits, BitString data, bool encryptUsingInverseCipher = false)
        {

            if (data.BitLength % EXPECTED_BLOCK_SIZE != 0)
            {
                return new SymmetricCipherResult($"Supplied data size {data.BitLength} is not in the proper block size {EXPECTED_BLOCK_SIZE}");
            }
            var plainTextBytes = data.ToBytes();
            byte[] output = new byte[plainTextBytes.Length];

            for (int blockIdx = 0; blockIdx < plainTextBytes.Length / 8; blockIdx++)
            {
                byte[] input = new byte[8];
                Array.Copy(plainTextBytes, blockIdx * 8, input, 0, 8);
                var blockOutput = EncryptWorker(keyBits, input, encryptUsingInverseCipher);
                Array.Copy(blockOutput, 0, output, blockIdx * 8, 8);
            }
            return new SymmetricCipherResult(new BitString(output));

        }

        private byte[] EncryptWorker(BitString keyBits, byte[] input, bool encryptUsingInverseCipher = false)
        {
            var keys = new TDESKeys(keyBits);
            TDESContext context; 
            byte[] output;
            if (!encryptUsingInverseCipher)
            {
                context = new TDESContext(keys, FunctionValues.Encryption);
                byte[] interm1 = context.Schedule[0].Apply(input);
                byte[] interm2 = context.Schedule[1].Apply(interm1);
                output = context.Schedule[2].Apply(interm2);
            }
            else
            {
                context = new TDESContext(keys, FunctionValues.Decryption);
                byte[] interm1 = context.Schedule[2].Apply(input);
                byte[] interm2 = context.Schedule[1].Apply(interm1);
                output = context.Schedule[0].Apply(interm2);
            }
            return output;
        }

        public SymmetricCipherResult BlockDecrypt(BitString keyBits, BitString cipherText, bool encryptUsingInverseCipher = false)
        {
            if (cipherText.BitLength % EXPECTED_BLOCK_SIZE != 0)
            {
                return new SymmetricCipherResult($"Supplied data size {cipherText.BitLength} is not in the proper block size {EXPECTED_BLOCK_SIZE}");
            }
            var cipherTextBytes = cipherText.ToBytes();
            byte[] output = new byte[cipherTextBytes.Length];

            for (int blockIdx = 0; blockIdx < cipherTextBytes.Length / 8; blockIdx++)
            {
                byte[] input = new byte[8];
                Array.Copy(cipherTextBytes, blockIdx * 8, input, 0, 8);
                var blockOutput = DecryptWorker(keyBits, input, encryptUsingInverseCipher);
                Array.Copy(blockOutput, 0, output, blockIdx * 8, 8);
            }
            return new SymmetricCipherResult(new BitString(output));
        }

        private byte[] DecryptWorker(BitString keyBits, byte[] input, bool encryptUsingInverseCipher = false)
        {
            var keys = new TDESKeys(keyBits);
            TDESContext context;
            byte[] output;
            if (!encryptUsingInverseCipher)
            {
                context = new TDESContext(keys, FunctionValues.Decryption);
                byte[] interm1 = context.Schedule[2].Apply(input);
                byte[] interm2 = context.Schedule[1].Apply(interm1);
                output = context.Schedule[0].Apply(interm2);
            }
            else
            {
                context = new TDESContext(keys, FunctionValues.Encryption);
                byte[] interm1 = context.Schedule[0].Apply(input);
                byte[] interm2 = context.Schedule[1].Apply(interm1);
                output = context.Schedule[2].Apply(interm2);
            }
            return output;
        }
    }
}
