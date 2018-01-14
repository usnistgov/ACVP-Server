using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.CTR;
using NIST.CVP.Crypto.CTR.Enums;
using NIST.CVP.Crypto.CTR.Helpers;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Crypto.TDES_ECB;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CTR
{
    public class TdesCtr : ITdesCtr
    {
        private readonly ITDES_ECB _tdesEcb = new TDES_ECB.TDES_ECB();
        private readonly int _blockSize = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(Cipher.TDES).blockSize;

        /// <summary>
        /// Only operates on a single block at a time.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="plainText"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public SymmetricCipherResult EncryptBlock(BitString key, BitString plainText, BitString iv)
        {
            var encryption = _tdesEcb.BlockEncrypt(key, iv);
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

                var result = EncryptBlock(key.GetDeepCopy(), blockPt, iv);
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

                var result = EncryptBlock(key.GetDeepCopy(), lastBlockPt, iv);
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
            var encryptionResult = Encrypt(key, cipherText, counter);
            if (encryptionResult.Success)
            {
                return new SymmetricCounterResult(encryptionResult.Result, encryptionResult.IVs);
            }
            else
            {
                return new SymmetricCounterResult(encryptionResult.ErrorMessage);
            }
        }
    }
}
