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

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_FFX.v1_0.Base
{
    public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly IRandom800_90 _random;
        private ShuffleQueue<int> _dataLengths;
        private ShuffleQueue<int> _tweakLengths;
        
        // These cases are called out specifically due to a rounding error possible in certain libraries
        private readonly Dictionary<int, int> _specialCases = new() {{2, 463}, {4, 231}, {16, 115}, {32, 175}};

        public int NumberOfTestCasesToGenerate => 25;

        public TestCaseGenerator(IOracle oracle, IRandom800_90 random)
        {
            _oracle = oracle;
            _random = random;
        }

        public GenerateResponse PrepareGenerator(TestGroup @group, bool isSample)
        {
            // DataLengths
            MathDomain dataLengthDomain = null;
            if (group.Capability.MinLen != group.Capability.MaxLen)
            {
                dataLengthDomain = new MathDomain().AddSegment(new RangeDomainSegment(_random, group.Capability.MinLen, group.Capability.MaxLen));
            }
            else
            {
                dataLengthDomain = new MathDomain().AddSegment(new ValueDomainSegment(group.Capability.MinLen));
            }
            
            // Always test the min/max
            var testableDataLengths = dataLengthDomain.GetDomainMinMaxAsEnumerable().ToList();

            // Add specific combinations if they exist
            if (_specialCases.ContainsKey(group.Radix))
            {
                var lengthToAdd = _specialCases[group.Radix];
                if (dataLengthDomain.IsWithinDomain(lengthToAdd))
                {
                    testableDataLengths.Add(_specialCases[group.Radix]);
                }
            }
            
            // Prefer smaller values for testing
            testableDataLengths.AddRangeIfNotNullOrEmpty(dataLengthDomain.GetRandomValues(_ => _ <= 100, NumberOfTestCasesToGenerate - testableDataLengths.Count));
            testableDataLengths.AddRangeIfNotNullOrEmpty(dataLengthDomain.GetRandomValues(_ => _ <= 1000, NumberOfTestCasesToGenerate - testableDataLengths.Count));
            testableDataLengths.AddRangeIfNotNullOrEmpty(dataLengthDomain.GetRandomValues(NumberOfTestCasesToGenerate - testableDataLengths.Count));

            // Load values into ShuffleQueue
            _dataLengths = new ShuffleQueue<int>(testableDataLengths);
            
            // Tweaks
            var testableTweakLengths = new List<int>();

            // Always test the min/max
            testableTweakLengths.AddRangeIfNotNullOrEmpty(group.TweakLen.GetDomainMinMaxAsEnumerable());
            
            // Prefer smaller values for testing
            testableTweakLengths.AddRangeIfNotNullOrEmpty(group.TweakLen.GetRandomValues(_ => _ <= 100, NumberOfTestCasesToGenerate - testableTweakLengths.Count));
            testableTweakLengths.AddRangeIfNotNullOrEmpty(group.TweakLen.GetRandomValues(_ => _ <= 1000, NumberOfTestCasesToGenerate - testableTweakLengths.Count));
            testableTweakLengths.AddRangeIfNotNullOrEmpty(group.TweakLen.GetRandomValues(NumberOfTestCasesToGenerate - testableTweakLengths.Count));
            
            // Load values into ShuffleQueue
            _tweakLengths = new ShuffleQueue<int>(testableTweakLengths);
            
            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample, int caseNo)
        {
            var param = new AesFfParameters
            {
                AlgoMode = group.AlgoMode,
                Direction = group.Function,
                KeyLength = group.KeyLength,
                DataLength = _dataLengths.Pop(),
                TweakLength = _tweakLengths.Pop(),
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

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
