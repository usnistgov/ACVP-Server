﻿using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_OFB
{
    public class TestCaseGeneratorMMTDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IModeBlockCipher<SymmetricCipherResult> _algo;

        private const int _CT_LENGTH_MULTIPLIER = 16;
        private const int _BITS_IN_BYTE = 8;

        private int _ctLenGenIteration = 1;

        public int NumberOfTestCasesToGenerate => 10;

        public TestCaseGeneratorMMTDecrypt(IRandom800_90 random800_90, IModeBlockCipher<SymmetricCipherResult> algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, bool isSample)
        {
            //known answer - need to do an encryption operation to get the tag
            var key = _random800_90.GetRandomBitString(@group.KeyLength);
            var cipherText = _random800_90.GetRandomBitString(_ctLenGenIteration++ * _CT_LENGTH_MULTIPLIER * _BITS_IN_BYTE);
            var iv = _random800_90.GetRandomBitString((Cipher._MAX_IV_BYTE_LENGTH * _BITS_IN_BYTE));
            var testCase = new TestCase
            {
                IV = iv,
                Key = key,
                CipherText = cipherText,
                Deferred = false
            };
            return Generate(@group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, TestCase testCase)
        {
            SymmetricCipherResult decryptionResult = null;
            try
            {
                var param = new ModeBlockCipherParameters(BlockCipherDirections.Decrypt, testCase.IV, testCase.Key, testCase.CipherText);
                decryptionResult = _algo.ProcessPayload(param);
                if (!decryptionResult.Success)
                {
                    ThisLogger.Warn(decryptionResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse<TestGroup, TestCase>(decryptionResult.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
                }
            }

            testCase.PlainText = decryptionResult.Result;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}