using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.TupleHash.v1_0
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private int _capacity;

        private readonly IOracle _oracle;
        private readonly IRandom800_90 _rand;
        private readonly List<(int[] messageLengths, int outputLengths, int customizationLengths)> _lengths = new List<(int[], int, int)>();

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

            var messageLengths = new List<int[]>
            {
                new [] { minMax.Minimum, minMax.Maximum },
                new [] { minMax.Maximum, minMax.Minimum, minMax.Minimum }
            };

            inputAllowed.SetRangeOptions(RangeDomainSegmentOptions.Random);

            // Pick 1-10 tuples of random lengths mixing large and small
            // Large vs small doesn't matter for TupleHash as the messages are appended together for hashing,
            // but this helps us make sure we don't explode with super-sized messages
            do
            {
                var tupleCount = _rand.GetRandomInt(1, 11);    // [1, 10]
                var smallTuples = _rand.GetRandomInt(1, tupleCount+1);    // [1, tupleCount]
                var largeTuples = tupleCount - smallTuples;
                var tuple = new List<int>();
                
                // Small message lengths (add all less than or equal to capacity)
                tuple.AddRange(inputAllowed.GetValues(x => x <= _capacity, smallTuples, false));

                // Large message lengths (add a random selection greater than capacity)
                tuple.AddRange(inputAllowed.GetValues(x => x > _capacity, largeTuples, false));

                messageLengths.Add(tuple.Shuffle().ToArray());

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
                _lengths.Add((messageLengths[i], outputLengths[i], _rand.GetRandomInt(0, 129)));
            }
            
            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        { 
            try
            {
                var oracleResult = await _oracle.GetTupleHashCaseAsync(new TupleHashParameters
                {
                    MessageLength = _lengths[caseNo].messageLengths,
                    CustomizationLength = _lengths[caseNo].customizationLengths,
                    HashFunction = new HashFunction(_lengths[caseNo].outputLengths, _capacity, group.XOF),
                    HexCustomization = group.HexCustomization,
                    FunctionName = group.Function
                });

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Tuple = oracleResult.Tuple,
                    Digest = oracleResult.Digest,
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
