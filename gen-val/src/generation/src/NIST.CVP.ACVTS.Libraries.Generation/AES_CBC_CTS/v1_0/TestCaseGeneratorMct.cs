using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CBC_CTS.v1_0
{
    public class TestCaseGeneratorMct : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMct(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new AesParameters
            {
                Mode = group.BlockCipherModeOfOperation,
                DataLength = ChoosePayloadLen(group.PayloadLen),
                Direction = group.Function,
                KeyLength = group.KeyLength
            };

            try
            {
                var oracleResult = await _oracle.GetAesMctCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    PayloadLen = param.DataLength,
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

        private int ChoosePayloadLen(MathDomain groupPayloadLen)
        {
            List<int> values = new List<int>();

            // We're using specific values for pool generation, just due to the large range of possible values.
            // If any if those values are within the domain, use those as the data len, as the results are precomputed.
            if (groupPayloadLen.IsWithinDomain(521))
            {
                return 521;
            }
            if (groupPayloadLen.IsWithinDomain(1337))
            {
                return 1337;
            }
            if (groupPayloadLen.IsWithinDomain(320))
            {
                return 320;
            }

            // Use larger numbers only when the "smaller" values don't exist.
            values.AddRangeIfNotNullOrEmpty(groupPayloadLen.GetValues(a => a > 128 && a < 1280 && a % 128 != 0, 128, true));
            values.AddRangeIfNotNullOrEmpty(groupPayloadLen.GetValues(a => a % 128 != 0, 128, true));

            return values.First();
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
