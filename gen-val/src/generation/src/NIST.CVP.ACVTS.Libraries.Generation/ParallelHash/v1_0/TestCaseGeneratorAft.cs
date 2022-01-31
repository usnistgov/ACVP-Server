using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.ParallelHash.v1_0
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private int _capacity = 0;

        private readonly IOracle _oracle;
        private readonly IRandom800_90 _rand;

        public int NumberOfTestCasesToGenerate => 100;

        private IList<(int outputLength, int messageLength, int customizationLength, int blockSize)> _lengths = new List<(int, int, int, int)>();

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

            inputAllowed.SetRangeOptions(RangeDomainSegmentOptions.Random);

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

            // For every input length, just pick a random block size (min/max always included)
            var blockSizeAllowed = group.BlockSize.GetDeepCopy();
            var blockSizeMinMax = blockSizeAllowed.GetDomainMinMax();
            var blockSizeLengths = new List<int>
            {
                blockSizeMinMax.Minimum,
                blockSizeMinMax.Maximum
            };

            blockSizeLengths.AddRange(blockSizeAllowed.GetValues(x => true, NumberOfTestCasesToGenerate, true));

            var messageLengthQueue = new ShuffleQueue<int>(messageLengths);
            var outputLengthQueue = new ShuffleQueue<int>(outputLengths);
            var blockSizeLengthQueue = new ShuffleQueue<int>(blockSizeLengths);

            // block aligned (for byte pad) tests
            for (var i = 0; i < blockAlignedTestCasesToGenerate; i++)
            {
                // Due to the way byte pad works, there are extra padding bits added that makes the "amount of 
                // customization bits to add" in order to hit the block aligned test... odd.
                // See Sha3DerivedHelpersTests.ShouldGetBlockAlignedDataWithSpecificLengthForCshake tests for some more detail.
                _lengths.Add((outputLengthQueue.Pop(), messageLengthQueue.Pop(), group.DigestSize == 128 ? 149 : 117, blockSizeLengthQueue.Pop()));
            }

            for (var i = 0; i < NumberOfTestCasesToGenerate - blockAlignedTestCasesToGenerate; i++)
            {
                // Customization length will be bits if for hex, or bytes if for ascii
                _lengths.Add((outputLengthQueue.Pop(), messageLengthQueue.Pop(), _rand.GetRandomInt(0, 129), blockSizeLengthQueue.Pop()));
            }

            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            var param = new ParallelHashParameters
            {
                CustomizationLength = _lengths[caseNo].customizationLength,
                HexCustomization = group.HexCustomization,
                MessageLength = _lengths[caseNo].messageLength,
                HashFunction = new HashFunction(_lengths[caseNo].outputLength, _capacity, group.XOF),
                BlockSize = _lengths[caseNo].blockSize
            };

            try
            {
                var oracleResult = await _oracle.GetParallelHashCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Message = oracleResult.Message,
                    Digest = oracleResult.Digest,
                    Customization = oracleResult.Customization,
                    CustomizationHex = oracleResult.CustomizationHex,
                    BlockSize = _lengths[caseNo].blockSize,
                    Deferred = false,
                    DigestLength = _lengths[caseNo].outputLength
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
