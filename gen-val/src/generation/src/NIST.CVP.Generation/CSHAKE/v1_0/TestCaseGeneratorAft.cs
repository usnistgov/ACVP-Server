﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.CSHAKE.v1_0
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private int _capacity;

        private readonly string[] _validFunctionNames = { "KMAC", "TupleHash", "ParallelHash", "" };

        private readonly IOracle _oracle;
        private readonly IRandom800_90 _rand;
        private readonly IList<(int inputLength, int outputLength, string functionName, int customizationLength)> _lengths = new List<(int, int, string, int)>();
        
        public int NumberOfTestCasesToGenerate => 512;

        public TestCaseGeneratorAft(IOracle oracle, IRandom800_90 rand)
        {
            _oracle = oracle;
            _rand = rand;
        }
        
        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            _capacity = 2 * group.DigestSize;
            var inputAllowed = group.MessageLength.GetDeepCopy();
            var minMax = inputAllowed.GetDomainMinMax();

            var messageLengths = new List<int>
            {
                minMax.Minimum,
                minMax.Maximum
            };

            inputAllowed.SetRangeOptions(RangeDomainSegmentOptions.Random);
            
            do
            {
                // Small message lengths (add all less than or equal to capacity)
                messageLengths.AddRange(inputAllowed.GetValues(x => x <= _capacity, _capacity, false));

                // Large message lengths (add a random selection greater than capacity)
                messageLengths.AddRange(inputAllowed.GetValues(x => x > _capacity, _capacity, false));
                
            } while (messageLengths.Count < NumberOfTestCasesToGenerate);
            
            // For every input length, just pick a random output length (min/max always included)
            var outputAllowed = group.OutputLength.GetDeepCopy();
            var outputMinMax = outputAllowed.GetDomainMinMax();
            var outputLengths = new List<int>
            {
                outputMinMax.Minimum,
                outputMinMax.Maximum
            };

            // Keep pulling output lengths until we have enough
            do
            {
                outputLengths.AddRange(outputAllowed.GetValues(x => true, 1, false));

            } while (outputLengths.Count < messageLengths.Count);
            
            // Shuffle inputs and outputs
            messageLengths = messageLengths.Shuffle();
            outputLengths = outputLengths.Shuffle();
            
            // Pair up input and output
            if (messageLengths.Count != outputLengths.Count)
            {
                return new GenerateResponse("Unable to pair up input and output lengths");
            }
            
            for (var i = 0; i < messageLengths.Count; i++)
            {
                // Customization length will be bits if for hex, or bytes if for ascii
                _lengths.Add((messageLengths[i], outputLengths[i], _validFunctionNames[_rand.GetRandomInt(0, 4)], _rand.GetRandomInt(0, 129)));
            }
            
            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            try
            {
                var oracleResult = await _oracle.GetCShakeCaseAsync(new CShakeParameters
                {
                    HashFunction = new HashFunction(_lengths[caseNo].outputLength, _capacity),
                    CustomizationLength = _lengths[caseNo].customizationLength,
                    HexCustomization = group.HexCustomization,
                    FunctionName = _lengths[caseNo].functionName,
                    MessageLength = _lengths[caseNo].inputLength
                });

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    FunctionName = oracleResult.FunctionName,
                    Message = oracleResult.Message,
                    Digest = oracleResult.Digest,
                    DigestLength = oracleResult.Digest.BitLength,
                    Customization = oracleResult.Customization,
                    CustomizationHex = oracleResult.CustomizationHex
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