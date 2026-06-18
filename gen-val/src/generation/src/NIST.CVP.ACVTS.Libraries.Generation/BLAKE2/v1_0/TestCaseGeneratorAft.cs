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

namespace NIST.CVP.ACVTS.Libraries.Generation.BLAKE2.v1_0
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private const int Blake2bBlockSizeBits = 1024;
        private readonly IOracle _oracle;
        private ShuffleQueue<int> _messageLengths;
        private ShuffleQueue<int> _keyLengths;

        public int NumberOfTestCasesToGenerate { get; private set; }

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            var messageLengths = BuildMessageLengths(group);
            var keyLengths = BuildKeyLengths(group);

            NumberOfTestCasesToGenerate = System.Math.Max(25, messageLengths.Count);
            _messageLengths = new ShuffleQueue<int>(messageLengths, NumberOfTestCasesToGenerate);
            _keyLengths = keyLengths.Count == 0 ? null : new ShuffleQueue<int>(keyLengths, NumberOfTestCasesToGenerate);

            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
        {
            try
            {
                var keyLength = _keyLengths?.Pop() ?? 0;
                var oracleResult = await _oracle.GetBlake2CaseAsync(new Blake2Parameters
                {
                    HashFunction = group.HashFunction,
                    MessageLength = _messageLengths.Pop(),
                    KeyLength = keyLength
                });

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Message = oracleResult.Message,
                    Key = oracleResult.Key,
                    Digest = oracleResult.Digest
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private static List<int> BuildMessageLengths(TestGroup group)
        {
            var domain = group.MessageLength.GetDeepCopy();
            var minMax = domain.GetDomainMinMax();
            var lengths = new List<int>
            {
                minMax.Minimum,
                minMax.Maximum
            };

            lengths.AddRangeIfNotNullOrEmpty(domain.GetRandomValues(x => x % 8 == 0, 20));
            AddBoundaryLength(lengths, domain, Blake2bBlockSizeBits - 8);
            AddBoundaryLength(lengths, domain, Blake2bBlockSizeBits);
            AddBoundaryLength(lengths, domain, Blake2bBlockSizeBits + 8);

            return lengths.Distinct().ToList();
        }

        private static List<int> BuildKeyLengths(TestGroup group)
        {
            if (group.KeyLength == null)
            {
                return new List<int>();
            }

            var domain = group.KeyLength.GetDeepCopy();
            var minMax = domain.GetDomainMinMax();
            var lengths = new List<int>
            {
                minMax.Minimum,
                minMax.Maximum
            };

            lengths.AddRangeIfNotNullOrEmpty(domain.GetRandomValues(x => x % 8 == 0, 10));
            return lengths.Distinct().ToList();
        }

        private static void AddBoundaryLength(List<int> lengths, MathDomain domain, int length)
        {
            if (domain.IsWithinDomain(length))
            {
                lengths.Add(length);
            }
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
