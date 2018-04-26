using NIST.CVP.Crypto.Common.Symmetric.AES;

namespace NIST.CVP.Crypto.AES
{
    public class RijndaelCMAC : Rijndael
    {
        public RijndaelCMAC(IRijndaelInternals iRijndaelInternals) :
            base(iRijndaelInternals)
        {
        }

        protected override void BlockEncryptWorker(Cipher cipher, Key key, byte[] input, int numBlocks, byte[,] block,
            byte[] outBuffer)
        {
            switch (key.Direction)
            {
                case DirectionValues.Encrypt:
                    Encrypt(cipher, key, input, numBlocks, block, outBuffer);
                    break;
                case DirectionValues.Decrypt:
                    // CMAC contains different logic for setting up the block for decrypt, but uses the internal encrypt operation.
                    key.Direction = DirectionValues.Encrypt;
                    Decrypt(cipher, key, input, numBlocks, block, outBuffer);
                    break;
            }
        }

        private void Encrypt(Cipher cipher, Key key, byte[] input, int numBlocks, byte[,] block, byte[] outBuffer)
        {
            int j, t, i;
            bool leftover;
            int inputLenBits = input.Length * 8;
            numBlocks = input.Length * 8 / cipher.BlockLength;

            //  Set up L
            byte[] L = new byte[Cipher._MAX_IV_BYTE_LENGTH];

            for (j = 0; j < cipher.BlockLength / 32; j++)
            {
                for (t = 0; t < 4; t++)
                {
                    block[t, j] = 0x00;
                }
            }

            _iRijndaelInternals.EncryptSingleBlock(block, key);

            for (j = 0; j < cipher.BlockLength / 32; j++)
            {
                for (t = 0; t < 4; t++)
                {
                    L[4 * j + t] = block[t, j];
                }
            }

            //  Start chaining for MAC
            for (j = 0; j < cipher.BlockLength / 32; j++)
            {
                for (t = 0; t < 4; t++)
                {
                    block[t, j] = 0x00;
                }
            }

            if ((inputLenBits % cipher.BlockLength != 0) || input.Length == 0)
            {
                leftover = true;
            }
            else
            {
                leftover = false;
            }

            for (i = 0; i < (leftover ? numBlocks : numBlocks - 1); i++)
            {
                for (j = 0; j < cipher.BlockLength / 32; j++)
                {
                    for (t = 0; t < 4; t++)
                    {
                        block[t, j] ^= input[i * 16 + 4 * j + t];
                    }
                }

                _iRijndaelInternals.EncryptSingleBlock(block, key);
            }

            // Do last block
            byte aflag;
            if ((L[0] & 0x80) == 0x80)
            {
                aflag = 1;
            }
            else
            {
                aflag = 0;
            }

            for (i = 0; i < Cipher._MAX_IV_BYTE_LENGTH - 1; i++)
            {
                L[i] <<= 1;
                if ((L[i + 1] & 0x80) == 0x80)
                {
                    L[i] |= 0x01;
                }
                else
                {
                    L[i] &= 0xfe;
                }
            }

            L[Cipher._MAX_IV_BYTE_LENGTH - 1] <<= 1;
            if (aflag != 0)
            {
                L[Cipher._MAX_IV_BYTE_LENGTH - 1] ^= 0x87;
            }

            if (leftover)
            {
                if ((L[0] & 0x80) == 0x80)
                {
                    aflag = 1;
                }
                else
                {
                    aflag = 0;
                }

                for (i = 0; i < Cipher._MAX_IV_BYTE_LENGTH - 1; i++)
                {
                    L[i] <<= 1;
                    if ((L[i + 1] & 0x80) == 0x80)
                    {
                        L[i] |= 0x01;
                    }
                    else
                    {
                        L[i] &= 0xfe;
                    }
                }
                L[Cipher._MAX_IV_BYTE_LENGTH - 1] <<= 1;
                if (aflag != 0)
                {
                    L[Cipher._MAX_IV_BYTE_LENGTH - 1] ^= 0x87;
                }

                for (j = 0; j < cipher.BlockLength / 32; j++)
                {
                    for (t = 0; t < 4; t++)
                    {
                        block[t, j] ^= L[4 * j + t];
                        if (4 * j + t < ((inputLenBits % cipher.BlockLength) + 7) / 8)
                        {
                            block[t, j] ^= input[numBlocks * 16 + 4 * j + t];
                        }
                        aflag = (byte) (inputLenBits % cipher.BlockLength);
                        if ((4 * j + t) == (aflag / 8))
                        {
                            block[t, j] ^= (byte) (0x80 >> (aflag % 8));
                        }
                    }
                }
            }
            else
            {
                for (j = 0; j < cipher.BlockLength / 32; j++)
                {
                    for (t = 0; t < 4; t++)
                    {
                        block[t, j] ^= (byte) (input[(numBlocks - 1) * 16 + 4 * j + t] ^ L[4 * j + t]);
                    }
                }
            }

            _iRijndaelInternals.EncryptSingleBlock(block, key);

            for (j = 0; j < cipher.BlockLength / 32; j++)
            {
                for (t = 0; t < 4; t++)
                {
                    cipher.IV[4 * j + t] = outBuffer[4 * j + t] = block[t, j];
                }
            }
        }

        private void Decrypt(Cipher cipher, Key key, byte[] input, int numBlocks, byte[,] block, byte[] outBuffer)
        {
            int j, t, i;
            bool leftover;
            int inputLenBits = input.Length * 8;

            //  Set up L
            byte[] L = new byte[Cipher._MAX_IV_BYTE_LENGTH];

            for (j = 0; j < cipher.BlockLength / 32; j++)
            {
                for (t = 0; t < 4; t++)
                {
                    block[t, j] = 0x00;
                }
            }

            _iRijndaelInternals.EncryptSingleBlock(block, key);

            for (j = 0; j < cipher.BlockLength / 32; j++)
            {
                for (t = 0; t < 4; t++)
                {
                    L[4 * j + t] = block[t, j];
                }
            }

            //  Start chaining for MAC
            for (j = 0; j < cipher.BlockLength / 32; j++)
            {
                for (t = 0; t < 4; t++)
                {
                    block[t, j] = 0x00;
                }
            }

            if (inputLenBits % cipher.BlockLength != 0)
            {
                leftover = true;
            }
            else
            {
                leftover = false;
            }

            for (i = 0; i < (leftover ? numBlocks : numBlocks - 1); i++)
            {
                for (j = 0; j < cipher.BlockLength / 32; j++)
                {
                    for (t = 0; t < 4; t++)
                    {
                        block[t, j] ^= input[i * 16 + 4 * j + t];
                    }
                }

                _iRijndaelInternals.EncryptSingleBlock(block, key);
            }

            // Do last block
            byte aflag;
            if (leftover)
            {
                if ((L[Cipher._MAX_IV_BYTE_LENGTH - 1] & 0x01) != 0)
                {
                    aflag = 1;
                }
                else
                {
                    aflag = 0;
                }

                for (i = Cipher._MAX_IV_BYTE_LENGTH - 1; i > 0; i--)
                {
                    L[i] >>= 1;
                    if ((L[i - 1] & 0x01) != 0)
                    {
                        L[i] &= 0x80;
                    }
                    else
                    {
                        L[i] &= 0x7f;
                    }
                }
                L[0] >>= 1;
                if (aflag != 0)
                {
                    L[0] |= 0x80;
                    L[Cipher._MAX_IV_BYTE_LENGTH - 1] ^= 0x43;
                }

                for (j = 0; j < cipher.BlockLength / 32; j++)
                for (t = 0; t < 4; t++)
                {
                    block[t, j] ^= L[4 * j + t];
                    if (4 * j + t < (inputLenBits + 7) / 8)
                    {
                        block[t, j] ^= input[numBlocks * 16 + 4 * j + t];
                    }

                    aflag = (byte) (inputLenBits % cipher.BlockLength);
                    if ((4 * j + t) == (aflag / 8))
                    {
                        block[t, j] ^= (byte) (0x80 >> (aflag % 8));
                    }
                }
            }
            else
            {
                if ((L[0] & 0x80) != 0)
                    aflag = 1;
                else
                    aflag = 0;
                for (i = 0; i < Cipher._MAX_IV_BYTE_LENGTH - 1; i++)
                {
                    L[i] <<= 1;
                    if ((L[i + 1] & 0x80) != 0)
                    {
                        L[i] |= 0x01;
                    }
                    else
                    {
                        L[i] &= 0xfe;
                    }
                }

                L[Cipher._MAX_IV_BYTE_LENGTH - 1] <<= 1;
                if (aflag != 0)
                {
                    L[Cipher._MAX_IV_BYTE_LENGTH - 1] ^= 0x17;
                }

                for (j = 0; j < cipher.BlockLength / 32; j++)
                {
                    for (t = 0; t < 4; t++)
                    {
                        block[t, j] ^= (byte) (input[(numBlocks - 1) * 16 + 4 * j + t] ^ L[4 * j + t]);
                    }
                }
            }

            _iRijndaelInternals.EncryptSingleBlock(block, key);

            for (j = 0; j < cipher.BlockLength / 32; j++)
            {
                for (t = 0; t < 4; t++)
                {
                    cipher.IV[4 * j + t] = outBuffer[4 * j + t] = block[t, j];
                }
            }
        }
    }
}