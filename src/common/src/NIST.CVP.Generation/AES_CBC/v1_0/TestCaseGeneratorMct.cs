﻿using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.AES_CBC.v1_0
{
    public class TestCaseGeneratorMct : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMct(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            var param = new AesParameters
            {
                Mode = BlockCipherModesOfOperation.Cbc,
                DataLength = 128,
                Direction = group.Function,
                KeyLength = group.KeyLength
            };

            try
            {
                var oracleResult = await _oracle.GetAesMctCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    PlainText = oracleResult.Seed.PlainText,
                    CipherText = oracleResult.Seed.CipherText,
                    IV = oracleResult.Seed.Iv,
                    Key = oracleResult.Seed.Key,
                    ResultsArray = Array.ConvertAll(oracleResult.Results.ToArray(), element => new AlgoArrayResponse
                    {
                        PlainText = element.PlainText,
                        CipherText = element.CipherText,
                        IV = element.Iv,
                        Key = element.Key
                    }).ToList()
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