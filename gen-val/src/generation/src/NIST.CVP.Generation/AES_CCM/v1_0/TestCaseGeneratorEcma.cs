﻿using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.AES_CCM.v1_0
{
    public class TestCaseGeneratorEcma : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 25;

        public TestCaseGeneratorEcma(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new AeadParameters
            {
                KeyLength = group.KeyLength,
                AadLength = group.AADLength,
                IvLength = group.IVLength,
                PayloadLength = group.PayloadLength,
                TagLength = group.TagLength
            };

            try
            {
                var oracleResult = await _oracle.GetEcmaCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    AAD = oracleResult.Aad,
                    CipherText = oracleResult.CipherText,
                    IV = oracleResult.Iv,
                    Key = oracleResult.Key,
                    PlainText = oracleResult.PlainText,
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}