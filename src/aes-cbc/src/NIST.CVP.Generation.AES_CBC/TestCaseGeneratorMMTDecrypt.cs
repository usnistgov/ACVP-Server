﻿using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_CBC
{
    public class TestCaseGeneratorMMTDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IModeBlockCipherFactory _modeFactory;

        private const int _CT_LENGTH_MULTIPLIER = 16;
        private const int _BITS_IN_BYTE = 8;

        private int _ctLenGenIteration = 1;

        public int NumberOfTestCasesToGenerate => 10;

        public TestCaseGeneratorMMTDecrypt(
            IRandom800_90 random800_90, 
            IBlockCipherEngineFactory engineFactory, 
            IModeBlockCipherFactory modeFactory
        )
        {
            _random800_90 = random800_90;
            _engineFactory = engineFactory;
            _modeFactory = modeFactory;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, bool isSample)
        {
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
            SymmetricCipherResult result = null;
            try
            {
                var algo = _modeFactory.GetStandardCipher(
                    _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes),
                    BlockCipherModesOfOperation.Cbc
                );
                var p = new ModeBlockCipherParameters(
                    BlockCipherDirections.Decrypt,
                    testCase.IV,
                    testCase.Key,
                    testCase.CipherText
                );

                result = algo.ProcessPayload(p);
                if (!result.Success)
                {
                    ThisLogger.Warn(result.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse<TestGroup, TestCase>(result.ErrorMessage);
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

            testCase.PlainText = result.Result;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}