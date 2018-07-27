using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Generation.Core;
using NLog;
using System;
using HashFunction = NIST.CVP.Crypto.Common.Hash.SHA3.HashFunction;

namespace NIST.CVP.Generation.SHA3
{
    public class TestCaseGeneratorMct : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public bool IsSample { get; set; } = false;

        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGeneratorMct(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            IsSample = isSample;
            var param = new Sha3Parameters
            {
                HashFunction = new HashFunction(group.DigestSize, group.DigestSize * 2, group.Function.ToLower().Equals("shake", StringComparison.OrdinalIgnoreCase)),
                MessageLength = group.DigestSize
            };

            MctResult<Common.Oracle.ResultTypes.HashResult> oracleResult = null;
            try
            {
                oracleResult = _oracle.GetSha3MctCase(param);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                Message = oracleResult.Results[0].Message,
                Digest = oracleResult.Results[0].Digest,
                ResultsArray = oracleResult.Results.ConvertAll(element => new AlgoArrayResponse { Message = element.Message, Digest = element.Digest })
            });
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
