using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Helpers;
using NIST.CVP.Math;
using Cipher = NIST.CVP.Crypto.Common.Symmetric.CTR.Enums.Cipher;

namespace NIST.CVP.Crypto.AES_CTR
{
    public class AesCtr : IAesCtr
    {
        private readonly IAES_ECB _aesEcb = new AES_ECB.AES_ECB(new RijndaelFactory(new RijndaelInternals()));
        private readonly int _blockSize = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(Cipher.AES).blockSize;

        /// <summary>
        /// Only operates on a single block at a time.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="plainText"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public SymmetricCipherResult EncryptBlock(BitString key, BitString plainText, BitString iv)
        {
            var encryption = _aesEcb.BlockEncrypt(key, iv);
            if (encryption.Success)
            {
                var completeBlockPlainText = plainText.ConcatenateBits(BitString.Zeroes(_blockSize - plainText.BitLength));
                var cipherText = BitString.XOR(completeBlockPlainText, encryption.Result);

                var partialCipherText = cipherText.MSBSubstring(0, plainText.BitLength);

                return new SymmetricCipherResult(partialCipherText);
            }

            return new SymmetricCipherResult(encryption.ErrorMessage);
        }

        /// <summary>
        /// Only operates on a single block at a time.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cipherText"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public SymmetricCipherResult DecryptBlock(BitString key, BitString cipherText, BitString iv)
        {
            // Use the same encryption process and wrap the result in a DecryptionResult
            var encryptionResult = EncryptBlock(key, cipherText, iv);
            if (encryptionResult.Success)
            {
                return new SymmetricCipherResult(encryptionResult.Result);
            }
            else
            {
                return new SymmetricCipherResult(encryptionResult.ErrorMessage);
            }
        }

        /// <summary>
        /// Encrypts multiple blocks at a time
        /// </summary>
        /// <param name="key"></param>
        /// <param name="plainText"></param>
        /// <param name="counter"></param>
        /// <returns></returns>
        public SymmetricCounterResult Encrypt(BitString key, BitString plainText, ICounter counter)
        {
            var numCompleteBlocks = plainText.BitLength / _blockSize;
            var cipherText = new BitString(0);
            var ivs = new List<BitString>();

            for (var i = 0; i < numCompleteBlocks; i++)
            {
                var blockPt = plainText.MSBSubstring(i * _blockSize, _blockSize);
                var iv = counter.GetNextIV();
                ivs.Add(iv);

                var result = EncryptBlock(key, blockPt, iv);
                if (!result.Success)
                {
                    return new SymmetricCounterResult(result.ErrorMessage);
                }

                cipherText = cipherText.ConcatenateBits(result.Result);
            }

            var numIncompleteBlock = plainText.BitLength % _blockSize == 0 ? 0 : 1;
            for (var i = 0; i < numIncompleteBlock; i++)
            {
                var lastBlockPt = plainText.Substring(0, plainText.BitLength % _blockSize);
                var iv = counter.GetNextIV();
                ivs.Add(iv);

                var result = EncryptBlock(key, lastBlockPt, iv);
                if (!result.Success)
                {
                    return new SymmetricCounterResult(result.ErrorMessage);
                }

                cipherText = cipherText.ConcatenateBits(result.Result);
            }

            return new SymmetricCounterResult(cipherText, ivs);
        }

        /// <summary>
        /// Decrypts multiple blocks at a time
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cipherText"></param>
        /// <param name="counter"></param>
        /// <returns></returns>
        public SymmetricCounterResult Decrypt(BitString key, BitString cipherText, ICounter counter)
        {
            var numCompleteBlocks = cipherText.BitLength / _blockSize;
            var plainText = new BitString(0);
            var ivs = new List<BitString>();

            for (var i = 0; i < numCompleteBlocks; i++)
            {
                var blockCt = cipherText.MSBSubstring(i * _blockSize, _blockSize);
                var iv = counter.GetNextIV();
                ivs.Add(iv);

                var result = DecryptBlock(key, blockCt, iv);
                if (!result.Success)
                {
                    return new SymmetricCounterResult(result.ErrorMessage);
                }

                plainText = plainText.ConcatenateBits(result.Result);
            }

            var numIncompleteBlock = cipherText.BitLength % _blockSize == 0 ? 0 : 1;
            for (var i = 0; i < numIncompleteBlock; i++)
            {
                var lastBlockCt = cipherText.Substring(0, cipherText.BitLength % _blockSize);
                var iv = counter.GetNextIV();
                ivs.Add(iv);

                var result = DecryptBlock(key, lastBlockCt, iv);
                if (!result.Success)
                {
                    return new SymmetricCounterResult(result.ErrorMessage);
                }

                plainText = plainText.ConcatenateBits(result.Result);
            }

            return new SymmetricCounterResult(plainText, ivs);
        }
    }
}

