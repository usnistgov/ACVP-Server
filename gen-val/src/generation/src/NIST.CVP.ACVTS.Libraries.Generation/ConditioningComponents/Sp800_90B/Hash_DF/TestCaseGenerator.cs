using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.ConditioningComponents.Sp800_90B.Hash_DF
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        public int NumberOfTestCasesToGenerate { get; set; } = 101;    // Odd number because of the breakdown. 3 are pre-set, then the rest needs to be divisible by 2
        private ShuffleQueue<int> _inputLen;
        private readonly IOracle _oracle;

        private static readonly ILogger ThisLogger = LogManager.GetCurrentClassLogger();

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 5;
            }

            var inputLength = group.PayloadLength.GetDeepCopy();

            var minMax = inputLength.GetDomainMinMaxAsEnumerable();
            var blockSize = inputLength.GetSequentialValues(x => x == group.HashAlg.BlockSize, 1).ToList();
            var underBlockSize = inputLength.GetRandomValues(x => x < group.HashAlg.BlockSize, NumberOfTestCasesToGenerate).ToList();
            var overBlockSize = inputLength.GetRandomValues(x => x > group.HashAlg.BlockSize, NumberOfTestCasesToGenerate).ToList();

            // Always add min/max/blocksize
            var lengths = new List<int>();
            lengths.AddRange(minMax);
            lengths.AddRangeIfNotNullOrEmpty(blockSize);

            var halfAvailable = (NumberOfTestCasesToGenerate - 3) / 2;
            lengths.AddRange(underBlockSize.Count <= halfAvailable
                ? underBlockSize
                : underBlockSize.Take(halfAvailable + System.Math.Max(halfAvailable - overBlockSize.Count, 0)));

            lengths.AddRange(overBlockSize.Count <= halfAvailable
                ? overBlockSize
                : overBlockSize.Take(halfAvailable + System.Math.Max(halfAvailable - underBlockSize.Count, 0)));

            _inputLen = new ShuffleQueue<int>(lengths.ToHashSet().ToList(), NumberOfTestCasesToGenerate);

            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample, int caseNo = -1)
        {
            var param = new ShaWrapperParameters
            {
                HashFunction = group.HashAlg,
                MessageLength = _inputLen.Pop()
            };

            try
            {
                var oracleResult = await _oracle.GetHashDfCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Payload = oracleResult.Message,
                    RequestedBits = oracleResult.Digest
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
