using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CBC.v1_0
{
    public class TestCaseGeneratorHact : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private ShuffleQueue<int> _dataLengths;
        private IDictionary<int, string> _jsonFiles = new Dictionary<int, string> { { 128, "AesCbc128.json" }, { 192, "AesCbc192.json" }, { 256, "AesCbc256.json" } };

        public TestCaseGeneratorHact(IOracle oracle)
        {
            _oracle = oracle;
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
        public int NumberOfTestCasesToGenerate { get; set; }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            var hactObject = HactJsonFactory.GetHactParameters(group.KeyLength);

            if (group.Function.ToLower() == "encrypt")
            {
                var lengths = hactObject.EncryptParameters.Select(ep => ep.NumBlocks).Except(new[] { 0 }).ToList();    // Zero blocks is not defined by AES specifications
                _dataLengths = new ShuffleQueue<int>(lengths);
                NumberOfTestCasesToGenerate = _dataLengths.OriginalListCount;
            }
            else
            {
                var lengths = hactObject.DecryptParameters.Select(ep => ep.NumBlocks).Except(new[] { 0 }).ToList();
                _dataLengths = new ShuffleQueue<int>(lengths);
                NumberOfTestCasesToGenerate = _dataLengths.OriginalListCount;
            }

            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
        {
            var param = new AesParameters
            {
                Mode = BlockCipherModesOfOperation.Cbc,
                DataLength = _dataLengths.Pop() * 128,
                Direction = group.Function,
                KeyLength = group.KeyLength
            };

            try
            {
                var oracleResult = await _oracle.GetAesCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Key = oracleResult.Key,
                    IV = oracleResult.Iv,
                    PlainText = oracleResult.PlainText,
                    CipherText = oracleResult.CipherText
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }
    }
}
