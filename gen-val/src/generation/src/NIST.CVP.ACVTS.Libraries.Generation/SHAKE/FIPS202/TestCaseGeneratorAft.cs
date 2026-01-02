using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHAKE.FIPS202
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        public int NumberOfTestCasesToGenerate { get; set; }
        private ShuffleQueue<int> _messageLengths { get; set; }
        private ShuffleQueue<int> _outputLengths { get; set; }
        
        private int KECCAK_WIDTH = 1600;
        
        private readonly IOracle _oracle;

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            _messageLengths = FillMessageLengths(group);
            _outputLengths = FillOutputLengths(group);
            
            // Generate the max of (messageLengths, outputLengths) tests.
            // Note: number will be >= 120 as both _messageLengths and _outputLengths will have at least 120 values  
            NumberOfTestCasesToGenerate = System.Math.Max(_messageLengths.OriginalList.Count, _outputLengths.OriginalList.Count);
            
            return new GenerateResponse();
        }

        private ShuffleQueue<int> FillMessageLengths(TestGroup group)
        {
            var capacity = group.HashFunction.OutputLen * 2;
            var rate = KECCAK_WIDTH - (capacity);
            
            var messageLengths = group.MessageLengths.GetDeepCopy();
            
            var minMax = messageLengths.GetDomainMinMax();
            
            var lengths = new List<int>
            {
                minMax.Minimum,
                minMax.Maximum
            };

            // Pull all valid values up to 'rate'
            lengths.AddRangeIfNotNullOrEmpty(messageLengths.GetRandomValues(x => x <= rate, rate));

            // Pull 100 values greater than 'rate'
            lengths.AddRangeIfNotNullOrEmpty(messageLengths.GetRandomValues(x => x > rate, 100));

            // Obtain lengths that meet the criteria: x % rate > rate - trailerLen where trailerLen = 4, if possible. Tests the scenario
            // where the bitlength of the trailer (4-bits) appended to the message exceeds the block size. 
            lengths.AddRangeIfNotNullOrEmpty(
                messageLengths.GetRandomValues(x => (x % rate) > (rate - 4), 5));
            
            // Remove duplicated values
            lengths = lengths.Distinct().ToList();

            // Make sure at least 120 values are included
            return new ShuffleQueue<int>(lengths, System.Math.Max(120, lengths.Count));
        }

        private ShuffleQueue<int> FillOutputLengths(TestGroup group)
        {
            var outputLengths = group.OutputLength.GetDeepCopy();
            
            var minMax = outputLengths.GetDomainMinMax();
            
            var lengths = new List<int>
            {
                minMax.Minimum,
                minMax.Maximum
            };

            // The standard output length for SHAKE-128 is 128 bits and the standard output length for
            // SHAKE-256 is 256 bits. Pull 50 values <= the algorithm's standard output length.  
            lengths.AddRangeIfNotNullOrEmpty(outputLengths.GetRandomValues(x => x <= group.HashFunction.OutputLen && x != minMax.Minimum, 50));
            
            // Pull 100 values > the algorithm's standard output length.
            lengths.AddRangeIfNotNullOrEmpty(outputLengths.GetRandomValues(x => x > group.HashFunction.OutputLen  && x != minMax.Maximum, 100));
            
            // Remove duplicated values
            lengths = lengths.Distinct().ToList();

            // Make sure at least 120 values are included
            return new ShuffleQueue<int>(lengths, System.Math.Max(120, lengths.Count));
        }
        
        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
        {
            var param = new ShaParameters
            {
                HashFunction = group.HashFunction,
                MessageLength = _messageLengths.Pop(),
                OutputLength = _outputLengths.Pop()
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

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
