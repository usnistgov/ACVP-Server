﻿using System;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.KeyWrap;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestCaseGeneratorEncrypt<TTestGroup, TTestCase> : ITestCaseGenerator<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>, new()
    {
        private readonly IKeyWrapFactory _iKeyWrapFactory;
        private readonly IRandom800_90 _iRandom800_90;

        public TestCaseGeneratorEncrypt(IKeyWrapFactory iKeyWrapFactory, IRandom800_90 iRandom800_90)
        {
            _iKeyWrapFactory = iKeyWrapFactory;
            _iRandom800_90 = iRandom800_90;
        }

        public int NumberOfTestCasesToGenerate => 100;

        public TestCaseGenerateResponse<TTestGroup, TTestCase> Generate(TTestGroup group, bool isSample)
        {
            var key = _iRandom800_90.GetRandomBitString(group.KeyLength);
            var plainText = _iRandom800_90.GetRandomBitString(group.PtLen);
            var testCase = new TTestCase
            {
                Key = key,
                PlainText = plainText
            };
            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TTestGroup, TTestCase> Generate(TTestGroup group, TTestCase testCase)
        {
            SymmetricCipherResult wrapResult = null;
            try
            {
                var keyWrap = _iKeyWrapFactory.GetKeyWrapInstance(group.KeyWrapType);

                wrapResult = keyWrap.Encrypt(testCase.Key, testCase.PlainText, group.UseInverseCipher);
                if (!wrapResult.Success)
                {
                    ThisLogger.Warn(wrapResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse<TTestGroup, TTestCase>(wrapResult.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse<TTestGroup, TTestCase>(ex.Message);
                }
            }
            testCase.CipherText = wrapResult.Result;
            return new TestCaseGenerateResponse<TTestGroup, TTestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}