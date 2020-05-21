using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.KMAC.v1_0
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly IRandom800_90 _rand;

        private int _capacity = 0;
        private IList<(int macSize, int keySize, int messageSize, int customizationLength)> _lengths { get; } = new List<(int, int, int, int)>();

        public int NumberOfTestCasesToGenerate => 512;

        public TestCaseGeneratorAft(IOracle oracle, IRandom800_90 rand)
        {
            _oracle = oracle;
            _rand = rand;
        }

        public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
        {
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
            
            do
            {
                // Small message lengths (add all less than or equal to capacity)
                messageLengths.AddRange(inputAllowed.GetValues(x => x <= _capacity, _capacity, false));

                // Large message lengths (add a random selection greater than capacity)
                messageLengths.AddRange(inputAllowed.GetValues(x => x > _capacity, _capacity, false));
                
            } while (messageLengths.Count < NumberOfTestCasesToGenerate);
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

            // Keep pulling output lengths until we have enough
            do
            {
                macLengths.AddRange(macAllowed.GetValues(x => true, messageLengths.Count, true));
            } while (macLengths.Count < messageLengths.Count);
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

            // Keep pulling output lengths until we have enough
            do
            {
                keyLengths.AddRange(keyAllowed.GetValues(x => true, messageLengths.Count, true));
            } while (keyLengths.Count < messageLengths.Count);
            #endregion KeyLengths
            
            // Shuffle inputs and outputs
            messageLengths = messageLengths.Shuffle();
            macLengths = macLengths.Shuffle();
            keyLengths = keyLengths.Shuffle();
            
            // Pair up input and output
            if (messageLengths.Count > macLengths.Count || messageLengths.Count > keyLengths.Count)
            {
                return new GenerateResponse("Unable to pair up input and output lengths");
            }
            
            for (var i = 0; i < messageLengths.Count; i++)
            {
                // Customization length will be bits if for hex, or bytes if for ascii
                _lengths.Add((macLengths[i], keyLengths[i], messageLengths[i], _rand.GetRandomInt(0, 129)));
            }
            
            return new GenerateResponse();
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            try
            {
                var oracleResult = await _oracle.GetKmacCaseAsync(new KmacParameters
                {
                    CustomizationLength = _lengths[caseNo].customizationLength,
                    HexCustomization = group.HexCustomization,
                    KeyLength = _lengths[caseNo].keySize,
                    MacLength = _lengths[caseNo].macSize,
                    MessageLength = _lengths[caseNo].messageSize
                });

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Key = oracleResult.Key,
                    Message = oracleResult.Message,
                    Mac = oracleResult.Tag,
                    Customization = oracleResult.Customization,
                    CustomizationHex = oracleResult.CustomizationHex,
                    MacLength = oracleResult.Tag.BitLength
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
