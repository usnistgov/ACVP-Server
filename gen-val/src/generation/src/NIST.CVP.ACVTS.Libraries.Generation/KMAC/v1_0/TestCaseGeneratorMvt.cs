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

namespace NIST.CVP.ACVTS.Libraries.Generation.KMAC.v1_0
{
    public class TestCaseGeneratorMvt : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly IRandom800_90 _rand;

        private int _capacity = 0;
        private IList<(int macSize, int keySize, int messageSize, int customizationLength)> _lengths = new List<(int, int, int, int)>();

        public int NumberOfTestCasesToGenerate => 100;

        public TestCaseGeneratorMvt(IOracle oracle, IRandom800_90 rand)
        {
            _oracle = oracle;
            _rand = rand;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
            var blockAlignedTestCasesToGenerate = 5;

            _capacity = 2 * group.DigestSize;

            #region MessageLengths
            var inputAllowed = group.MsgLengths.GetDeepCopy();
            var minMax = inputAllowed.GetDomainMinMax();

            var messageLengths = new List<int>
            {
                minMax.Minimum,
                minMax.Maximum
            };

            inputAllowed.SetRangeOptions(RangeDomainSegmentOptions.Random);
            messageLengths.AddRange(inputAllowed.GetValues(x => x <= _capacity, NumberOfTestCasesToGenerate / 2, true));
            messageLengths.AddRange(inputAllowed.GetValues(x => x > _capacity, NumberOfTestCasesToGenerate / 2, true));
            #endregion MessageLengths

            #region MacLengths
            // For every input length, just pick a random output length (min/max always included)
            var macAllowed = group.MacLengths.GetDeepCopy();
            var macMinMax = macAllowed.GetDomainMinMax();
            var macLengths = new List<int>
            {
                macMinMax.Minimum,
                macMinMax.Maximum
            };
            macLengths.AddRange(macAllowed.GetValues(x => true, NumberOfTestCasesToGenerate, true));
            #endregion MacLengths

            #region KeyLengths
            // For every input length, just pick a random key length (min/max always included)
            var keyAllowed = group.KeyLengths.GetDeepCopy();
            var keyMinMax = keyAllowed.GetDomainMinMax();
            var keyLengths = new List<int>
            {
                keyMinMax.Minimum,
                keyMinMax.Maximum
            };

            // Due to the way byte pad works, there are extra padding bits added that makes the "amount of 
            // key bits" in order to hit the block aligned test... odd.
            // See Sha3DerivedHelpersTests.ShouldGetBlockAlignedDataWithSpecificKeyLengthForKmac tests for some more detail.
            keyLengths.AddRangeIfNotNullOrEmpty(@group.DigestSize == 128
                ? keyAllowed.GetValues(x => x == 131 % 136, blockAlignedTestCasesToGenerate, true)
                : keyAllowed.GetValues(x => x == 163 % 168, blockAlignedTestCasesToGenerate, true));

            keyLengths.AddRange(keyAllowed.GetValues(x => true, NumberOfTestCasesToGenerate - blockAlignedTestCasesToGenerate, true));
            #endregion KeyLengths

            var macQueue = new ShuffleQueue<int>(macLengths);
            var keyQueue = new ShuffleQueue<int>(keyLengths);
            var messageQueue = new ShuffleQueue<int>(messageLengths);

            for (var i = 0; i < NumberOfTestCasesToGenerate; i++)
            {
                // Customization length will be bits if for hex, or bytes if for ascii
                _lengths.Add((macQueue.Pop(), keyQueue.Pop(), messageQueue.Pop(), _rand.GetRandomInt(0, 129)));
            }

            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            try
            {
                var oracleResult = await _oracle.GetKmacCaseAsync(new KmacParameters()
                {
                    CouldFail = true,
                    DigestSize = _capacity / 2,
                    CustomizationLength = _lengths[caseNo].customizationLength,
                    HexCustomization = group.HexCustomization,
                    KeyLength = _lengths[caseNo].keySize,
                    MacLength = _lengths[caseNo].macSize,
                    MessageLength = _lengths[caseNo].messageSize,
                    XOF = group.XOF
                });

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Key = oracleResult.Key,
                    Message = oracleResult.Message,
                    Mac = oracleResult.Tag,
                    Customization = oracleResult.Customization,
                    CustomizationHex = oracleResult.CustomizationHex,
                    MacLength = oracleResult.Tag.BitLength,
                    TestPassed = oracleResult.TestPassed
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
