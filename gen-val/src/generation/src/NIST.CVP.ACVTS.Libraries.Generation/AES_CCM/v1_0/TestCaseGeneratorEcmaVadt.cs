﻿using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CCM.v1_0
{
    public class TestCaseGeneratorEcmaVadt : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        private int _offset = 0;
        private int _maxCasesToGenerate = 1;

        public int NumberOfTestCasesToGenerate => _maxCasesToGenerate;       // 0-18 bytes of additional adata

        public TestCaseGeneratorEcmaVadt(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            _maxCasesToGenerate = group.AADLengths.Length;
            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new AeadParameters
            {
                KeyLength = group.KeyLength,
                AadLength = group.AADLengths[_offset++],
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
