using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.CSHAKE.v1_0
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private int _capacity;

        private readonly string[] _validFunctionNames = { "KMAC", "TupleHash", "ParallelHash", "" };

        private readonly IOracle _oracle;
        private readonly IRandom800_90 _rand;
        private readonly IList<(int inputLength, int outputLength, string functionName, int customizationLength)> _lengths = new List<(int, int, string, int)>();

        public int NumberOfTestCasesToGenerate => 100;

        public TestCaseGeneratorAft(IOracle oracle, IRandom800_90 rand)
        {
            _oracle = oracle;
            _rand = rand;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            var blockAlignedTestCasesToGenerate = 5;

            _capacity = 2 * group.DigestSize;
            var inputAllowed = group.MessageLength.GetDeepCopy();
            var minMax = inputAllowed.GetDomainMinMax();

            var messageLengths = new List<int>
            {
                minMax.Minimum,
                minMax.Maximum
            };
            messageLengths.AddRange(inputAllowed.GetValues(x => x <= _capacity, NumberOfTestCasesToGenerate / 2, true));
            messageLengths.AddRange(inputAllowed.GetValues(x => x > _capacity, NumberOfTestCasesToGenerate / 2, true));

            // For every input length, just pick a random output length (min/max always included)
            var outputAllowed = group.OutputLength.GetDeepCopy();
            var outputMinMax = outputAllowed.GetDomainMinMax();
            var outputLengths = new List<int>
            {
                outputMinMax.Minimum,
                outputMinMax.Maximum
            };

            outputLengths.AddRange(outputAllowed.GetValues(x => true, NumberOfTestCasesToGenerate, true));

            var messageLengthQueue = new ShuffleQueue<int>(messageLengths);
            var outputLengthQueue = new ShuffleQueue<int>(outputLengths);

            // block aligned (for byte pad) tests
            for (var i = 0; i < blockAlignedTestCasesToGenerate; i++)
            {
                // Due to the way byte pad works, there are extra padding bits added that makes the "amount of 
                // customization bits to add" in order to hit the block aligned test... odd.
                // See Sha3DerivedHelpersTests.ShouldGetBlockAlignedDataWithSpecificLengthForCshake tests for some more detail.
                _lengths.Add((messageLengthQueue.Pop(), outputLengthQueue.Pop(), "", group.DigestSize == 128 ? 161 : 129));
            }

            for (var i = 0; i < NumberOfTestCasesToGenerate - blockAlignedTestCasesToGenerate; i++)
            {
                // Customization length will be bits if for hex, or bytes if for ascii
                _lengths.Add((messageLengthQueue.Pop(), outputLengthQueue.Pop(), _validFunctionNames[_rand.GetRandomInt(0, 4)], _rand.GetRandomInt(0, 129)));
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
