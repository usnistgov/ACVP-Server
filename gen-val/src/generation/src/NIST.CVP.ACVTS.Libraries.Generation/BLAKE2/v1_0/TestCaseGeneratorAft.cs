using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.Blake2;
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
        private const int NumberOfAftTestCases = 25;
        private const int Blake2bBlockSizeBits = 1024;
        private readonly IOracle _oracle;
        private ShuffleQueue<int> _digestLengths;
        private ShuffleQueue<int> _messageLengths;
        private ShuffleQueue<int> _keyLengths;

        public int NumberOfTestCasesToGenerate => NumberOfAftTestCases;

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            var digestLengths = BuildLengths(group.DigestLength);
            var messageLengths = BuildMessageLengths(group);
            var keyLengths = BuildKeyLengths(group);

            _digestLengths = new ShuffleQueue<int>(digestLengths, NumberOfTestCasesToGenerate);
            _messageLengths = new ShuffleQueue<int>(messageLengths, NumberOfTestCasesToGenerate);
            _keyLengths = keyLengths.Count == 0 ? null : new ShuffleQueue<int>(keyLengths, NumberOfTestCasesToGenerate);

            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
        {
            try
            {
                var digestLength = _digestLengths.Pop();
                var keyLength = _keyLengths?.Pop() ?? 0;
                var oracleResult = await _oracle.GetBlake2CaseAsync(new Blake2Parameters
                {
                    HashFunction = new Blake2HashFunction(Blake2Variant.Blake2b, digestLength),
                    MessageLength = _messageLengths.Pop(),
                    KeyLength = keyLength
                });

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Message = oracleResult.Message,
                    Key = oracleResult.Key,
                    DigestLength = digestLength,
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

            AddPatternLength(lengths, domain, x => x % Blake2bBlockSizeBits == Blake2bBlockSizeBits - 8);
            AddPatternLength(lengths, domain, x => x > 0 && x % Blake2bBlockSizeBits == 0);
            AddPatternLength(lengths, domain, x => x > Blake2bBlockSizeBits && x % Blake2bBlockSizeBits == 8);
            lengths.AddRangeIfNotNullOrEmpty(domain.GetRandomValues(x => !lengths.Contains(x), NumberOfAftTestCases - lengths.Count));

            return lengths.Distinct().ToList();
        }

        private static List<int> BuildLengths(MathDomain domain)
        {
            var domainCopy = domain.GetDeepCopy();
            var minMax = domainCopy.GetDomainMinMax();
            var lengths = new List<int>
            {
                minMax.Minimum,
                minMax.Maximum
            };

            lengths.AddRangeIfNotNullOrEmpty(domainCopy.GetRandomValues(x => !lengths.Contains(x), NumberOfAftTestCases - lengths.Count));
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

            lengths.AddRangeIfNotNullOrEmpty(domain.GetRandomValues(x => !lengths.Contains(x), NumberOfAftTestCases - lengths.Count));
            return lengths.Distinct().ToList();
        }

        private static void AddPatternLength(List<int> lengths, MathDomain domain, Func<int, bool> condition)
        {
            lengths.AddRangeIfNotNullOrEmpty(domain.GetRandomValues(x => condition(x) && !lengths.Contains(x), 1));
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
