using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES_FFX.v1_0.Base
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly IRandom800_90 _random;
        private readonly List<int> _dataLengths = new List<int>();
        private readonly List<int> _tweakLengths = new List<int>();

        public int NumberOfTestCasesToGenerate => 25;

        public TestCaseGenerator(IOracle oracle, IRandom800_90 random)
        {
            _oracle = oracle;
            _random = random;
        }

        public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
        {
            MathDomain dataLengthDomain = null;
            if (group.Capability.MinLength != group.Capability.MaxLength)
            {
                dataLengthDomain = new MathDomain().AddSegment(new RangeDomainSegment(_random, group.Capability.MinLength, group.Capability.MaxLength));
            }
            else
            {
                dataLengthDomain = new MathDomain().AddSegment(new ValueDomainSegment(group.Capability.MinLength));
            }

            dataLengthDomain.SetRangeOptions(RangeDomainSegmentOptions.Random);

            _dataLengths.AddRangeIfNotNullOrEmpty(GetLengthsForTesting(dataLengthDomain));
            _tweakLengths.AddRangeIfNotNullOrEmpty(GetLengthsForTesting(group.TweakLen));

            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample, int caseNo)
        {
            var param = new AesFfParameters
            {
                AlgoMode = group.AlgoMode,
                Direction = group.Function,
                KeyLength = group.KeyLength,
                DataLength = _dataLengths[caseNo],
                TweakLength = _tweakLengths[caseNo],
                Radix = group.Capability.Radix
            };

            try
            {
                var oracleResult = await _oracle.GetAesFfCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Key = oracleResult.Key,
                    Tweak = oracleResult.Iv,
                    PlainText = NumeralString.ToAlphabetString(group.Alphabet, group.Radix, NumeralString.ToNumeralString(oracleResult.PlainText)),
                    CipherText = NumeralString.ToAlphabetString(group.Alphabet, group.Radix, NumeralString.ToNumeralString(oracleResult.CipherText))
                });
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }
        }

        private List<int> GetLengthsForTesting(MathDomain lengthDomain)
        {
            List<int> potentialLengths = new List<int>();

            // Always test the min/max
            potentialLengths.AddRangeIfNotNullOrEmpty(lengthDomain.GetDomainMinMaxAsEnumerable());
            // Prefer smaller values for testing
            potentialLengths.AddRangeIfNotNullOrEmpty(
                lengthDomain.GetValues(_ => _ <= 100, NumberOfTestCasesToGenerate - potentialLengths.Count, true)
            );
            potentialLengths.AddRangeIfNotNullOrEmpty(
                lengthDomain.GetValues(_ => _ <= 1000, NumberOfTestCasesToGenerate - potentialLengths.Count, true)
            );
            potentialLengths.AddRangeIfNotNullOrEmpty(
                lengthDomain.GetValues(NumberOfTestCasesToGenerate - potentialLengths.Count)
            );

            var count = 0;
            List<int> testLengths = new List<int>();
            while (testLengths.Count < NumberOfTestCasesToGenerate)
            {
                testLengths.Add(potentialLengths[count]);

                count++;
                if (count >= potentialLengths.Count)
                {
                    count = 0;
                }
            }

            return testLengths;
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
