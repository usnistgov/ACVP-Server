using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.v1_0
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private List<int> _caseSizes = new List<int>();
        public int NumberOfTestCasesToGenerate => _caseSizes.Count;

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            if (group.IncludeNull)
            {
                _caseSizes.Add(0);
            }

            _caseSizes = DetermineMessageLength(group.BitOrientedInput, group.CommonHashFunction.OutputLen);
            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new ShaParameters
            {
                HashFunction = group.CommonHashFunction,
                MessageLength = _caseSizes[caseNo],
                OutputLength = group.CommonHashFunction.OutputLen
            };

            try
            {
                var oracleResult = await _oracle.GetSha3CaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Message = oracleResult.Message,
                    Digest = oracleResult.Digest,
                    DigestLength = param.OutputLength
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private List<int> DetermineMessageLength(bool bitOriented, int digestSize)
        {
            var list = new List<int>();
            var unitSize = bitOriented ? 1 : 8;
            var rate = 1600 - digestSize * 2;

            for (var i = 1; i < rate / unitSize; i++)
            {
                list.Add(unitSize * i);
            }

            var largeMessageSize = rate;
            do
            {
                largeMessageSize += (rate + unitSize);
                list.Add(largeMessageSize);
            } while (largeMessageSize <= 65536);

            return list;
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

