using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NLog;

namespace NIST.CVP.Generation.SHA3.v1_0
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
            
            _caseSizes = DetermineMessageLength(group.BitOrientedInput, group.DigestSize);
            return new GenerateResponse();
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new Sha3Parameters
            {
                HashFunction = new HashFunction(group.DigestSize, group.DigestSize * 2, group.Function.ToLower().Equals("shake", StringComparison.OrdinalIgnoreCase)),
                MessageLength = _caseSizes[caseNo]
            };

            try
            {
                var oracleResult = await _oracle.GetSha3CaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Message = oracleResult.Message,
                    Digest = oracleResult.Digest
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

            for (var i = 0; i < rate / unitSize; i++)
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

