﻿using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.AES_GCM.v1_0
{
    public class TestCaseGeneratorInternalEncrypt : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 15;

        public TestCaseGeneratorInternalEncrypt(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new AeadParameters
            {
                AadLength = group.AadLength,
                KeyLength = group.KeyLength,
                PayloadLength = group.PayloadLength,
                CouldFail = false
            };

            try
            {
                AeadResult oracleResult = null;

                if (isSample)
                {
                    // Get complete test case, we need all the information
                    param.TagLength = group.TagLength;
                    param.IvLength = group.IvLength;

                    oracleResult = await _oracle.GetAesGcmCaseAsync(param);
                }
                else
                {
                    // Get incomplete test case, no need to compute the rest
                    oracleResult = await _oracle.GetDeferredAesGcmCaseAsync(param);
                }

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Deferred = true,
                    AAD = oracleResult.Aad,
                    CipherText = oracleResult.CipherText,
                    IV = oracleResult.Iv,
                    Key = oracleResult.Key,
                    PlainText = oracleResult.PlainText,
                    Tag = oracleResult.Tag,
                    TestPassed = oracleResult.TestPassed
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }
        
        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}