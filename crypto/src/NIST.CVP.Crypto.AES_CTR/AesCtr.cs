using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_CTR
{
    public class AesCtr : IAesCtr
    {
        private readonly IAES_ECB _aesEcb = new AES_ECB.AES_ECB(new RijndaelFactory(new RijndaelInternals()));

        /// <summary>
        /// Only operates on a single block at a time.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="plainText"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public EncryptionResult EncryptBlock(BitString key, BitString plainText, BitString iv)
        {
            var encryption = _aesEcb.BlockEncrypt(key, iv);
            if (encryption.Success)
            {
                var completeBlockPlainText = plainText.ConcatenateBits(BitString.Zeroes(128 - plainText.BitLength));
                var cipherText = BitString.XOR(completeBlockPlainText, encryption.CipherText);

                var partialCipherText = cipherText.MSBSubstring(0, plainText.BitLength);

                return new EncryptionResult(partialCipherText);
            }

            return new EncryptionResult(encryption.ErrorMessage);
        }

        /// <summary>
        /// Only operates on a single block at a time.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cipherText"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public DecryptionResult DecryptBlock(BitString key, BitString cipherText, BitString iv)
        {
            // Use the same encryption process and wrap the result in a DecryptionResult
            var encryptionResult = EncryptBlock(key, cipherText, iv);
            if (encryptionResult.Success)
            {
                return new DecryptionResult(encryptionResult.CipherText);
            }
            else
            {
                return new DecryptionResult(encryptionResult.ErrorMessage);
            }
        }

        /// <summary>
        /// Encrypts multiple blocks at a time
        /// </summary>
        /// <param name="key"></param>
        /// <param name="plainText"></param>
        /// <param name="counter"></param>
        /// <returns></returns>
        public CounterEncryptionResult Encrypt(BitString key, BitString plainText, ICounter counter)
        {
            var numCompleteBlocks = plainText.BitLength / 128;
            var cipherText = new BitString(0);
            var ivs = new List<BitString>();

            for (var i = 0; i < numCompleteBlocks; i++)
            {
                var blockPt = plainText.MSBSubstring(i * 128, 128);
                var iv = counter.GetNextIV();
                ivs.Add(iv);

                var result = EncryptBlock(key, blockPt, iv);
                if (!result.Success)
                {
                    return new CounterEncryptionResult(result.ErrorMessage);
                }

                cipherText = cipherText.ConcatenateBits(result.CipherText);
            }

            var numIncompleteBlock = plainText.BitLength % 128 == 0 ? 0 : 1;
            for (var i = 0; i < numIncompleteBlock; i++)
            {
                var lastBlockPt = plainText.Substring(0, plainText.BitLength % 128);
                var iv = counter.GetNextIV();
                ivs.Add(iv);

                var result = EncryptBlock(key, lastBlockPt, iv);
                if (!result.Success)
                {
                    return new CounterEncryptionResult(result.ErrorMessage);
                }

                cipherText = cipherText.ConcatenateBits(result.CipherText);
            }

            return new CounterEncryptionResult(cipherText, ivs);
        }

        /// <summary>
        /// Decrypts multiple blocks at a time
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cipherText"></param>
        /// <param name="counter"></param>
        /// <returns></returns>
        public CounterDecryptionResult Decrypt(BitString key, BitString cipherText, ICounter counter)
        {
            var numCompleteBlocks = cipherText.BitLength / 128;
            var plainText = new BitString(0);
            var ivs = new List<BitString>();

            for (var i = 0; i < numCompleteBlocks; i++)
            {
                var blockCt = cipherText.MSBSubstring(i * 128, 128);
                var iv = counter.GetNextIV();
                ivs.Add(iv);

                var result = DecryptBlock(key, blockCt, iv);
                if (!result.Success)
                {
                    return new CounterDecryptionResult(result.ErrorMessage);
                }

                plainText = plainText.ConcatenateBits(result.PlainText);
            }

            var numIncompleteBlock = cipherText.BitLength % 128 == 0 ? 0 : 1;
            for (var i = 0; i < numIncompleteBlock; i++)
            {
                var lastBlockCt = cipherText.Substring(0, cipherText.BitLength % 128);
                var iv = counter.GetNextIV();
                ivs.Add(iv);

                var result = DecryptBlock(key, lastBlockCt, iv);
                if (!result.Success)
                {
                    return new CounterDecryptionResult(result.ErrorMessage);
                }

                plainText = plainText.ConcatenateBits(result.PlainText);
            }

            return new CounterDecryptionResult(plainText, ivs);
        }
    }
}

