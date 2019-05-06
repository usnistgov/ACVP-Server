using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.KMAC.v1_0
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        private int _capacity = 0;
        private int _macLength = 0;
        private int _keyLength = 0;
        private int _numberOfCases = 512;
        private int _numberOfSmallCases = 0;
        private int _numberOfLargeCases = 0;
        private int _currentTestCase = 0;
        private int _currentSmallCase = 0;
        private int _currentLargeCase = 1;
        private int _customizationLength = 0;

        public int NumberOfTestCasesToGenerate => _numberOfCases;
        public List<int> MacSizes { get; } = new List<int>();                 // Primarily for testing purposes
        public List<int> KeySizes { get; } = new List<int>();                 // Primarily for testing purposes
        public List<int> MessageSizes { get; } = new List<int>();             // Primarily for testing purposes

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            // Only do this logic once
            if (_capacity == 0)
            {
                MacSizes.Clear();
                KeySizes.Clear();
                MessageSizes.Clear();
                var smallMessageLengths = DetermineSmallMessageDomain(group.MsgLengths, group);
                var largeMessageLengths = DetermineLargeMessageDomain(group.MsgLengths, group);
                DetermineLengths(group.MacLengths, group.KeyLengths, smallMessageLengths, largeMessageLengths);
                _capacity = 2 * group.DigestSize;
            }

            var param = DetermineParameters(group.DigestSize, group.HexCustomization, group.XOF);

            try
            {
                var oracleResult = await _oracle.GetKmacCaseAsync(param);

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

        private MathDomain DetermineSmallMessageDomain(MathDomain msgLengths, TestGroup group)
        {
            if (msgLengths.DomainSegments.ElementAt(0).RangeMinMax.Maximum - msgLengths.DomainSegments.ElementAt(0).RangeMinMax.Minimum < (group.DigestSize == 128 ? 1344 : 1088))
            {
                return msgLengths.GetDeepCopy();
            }
            return new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), msgLengths.DomainSegments.ElementAt(0).RangeMinMax.Minimum, msgLengths.DomainSegments.ElementAt(0).RangeMinMax.Minimum + (group.DigestSize == 128 ? 1344 : 1088), msgLengths.DomainSegments.ElementAt(0).RangeMinMax.Increment));
        }

        private MathDomain DetermineLargeMessageDomain(MathDomain msgLengths, TestGroup group)
        {
            if (msgLengths.DomainSegments.ElementAt(0).RangeMinMax.Maximum < (group.DigestSize == 128 ? 1344 : 1088))
            {
                return msgLengths.GetDeepCopy();
            }
            return new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), group.DigestSize == 128 ? 1344 : 1088, msgLengths.DomainSegments.ElementAt(0).RangeMinMax.Maximum, msgLengths.DomainSegments.ElementAt(0).RangeMinMax.Increment));
        }

        private void DetermineLengths(MathDomain macDomain, MathDomain keyDomain, MathDomain smallMessageDomain, MathDomain largeMessageDomain)
        {
            macDomain.SetRangeOptions(RangeDomainSegmentOptions.Random);
            var macMinMax = macDomain.GetDomainMinMax();
            keyDomain.SetRangeOptions(RangeDomainSegmentOptions.Random);
            var keyMinMax = keyDomain.GetDomainMinMax();
            smallMessageDomain.SetRangeOptions(RangeDomainSegmentOptions.Random);
            var smallMinMax = smallMessageDomain.GetDomainMinMax();
            largeMessageDomain.SetRangeOptions(RangeDomainSegmentOptions.Random);
            var largeMinMax = largeMessageDomain.GetDomainMinMax();

            var macValues = macDomain.GetValues(250).OrderBy(o => Guid.NewGuid()).Take(250);
            int macRepetitions;
            var keyValues = keyDomain.GetValues(250).OrderBy(o => Guid.NewGuid()).Take(250);
            int keyRepetitions;
            var smallValues = smallMessageDomain.GetValues(225).OrderBy(o => Guid.NewGuid()).Take(225);
            var largeValues = largeMessageDomain.GetValues(25).OrderBy(o => Guid.NewGuid()).Take(25);

            if (macValues.Count() == 0)
            {
                macRepetitions = 25;
            }
            else if (macValues.Count() > 499)
            {
                macRepetitions = 1;
            }
            else
            {
                macRepetitions = 25 / (macValues.Count() / 10) + (250 % macValues.Count() > 0 ? 1 : 0);
            }

            if (keyValues.Count() == 0)
            {
                keyRepetitions = 50;
            }
            else if (keyValues.Count() > 499)
            {
                keyRepetitions = 1;
            }
            else
            {
                keyRepetitions = 25 / (keyValues.Count() / 10) + (250 % keyValues.Count() > 0 ? 1 : 0);
            }

            foreach (var value in macValues)
            {
                for (var i = 0; i < macRepetitions; i++)
                {
                    MacSizes.Add(value);
                }
            }

            foreach (var value in keyValues)
            {
                for (var i = 0; i < keyRepetitions; i++)
                {
                    KeySizes.Add(value);
                }
            }

            foreach (var value in smallValues)
            {
                MessageSizes.Add(value);
                _numberOfSmallCases++;
            }

            foreach (var value in largeValues)
            {
                MessageSizes.Add(value);
                _numberOfLargeCases++;
            }

            while (_numberOfLargeCases < 23)
            {
                MessageSizes.Add(largeMessageDomain.GetDomainMinMax().Maximum);
                _numberOfLargeCases++;
            }

            // Make sure min and max appear in the list
            MacSizes.Add(macMinMax.Minimum);
            MacSizes.Add(macMinMax.Maximum);
            KeySizes.Add(keyMinMax.Minimum);
            KeySizes.Add(keyMinMax.Maximum);
            MessageSizes.Add(smallMinMax.Minimum);
            MessageSizes.Add(smallMinMax.Maximum);
            MessageSizes.Add(largeMinMax.Minimum);
            MessageSizes.Add(largeMinMax.Maximum);

            MacSizes.Sort();
            KeySizes.Sort();
            MessageSizes.Sort();
        }

        private KmacParameters DetermineParameters(int digestSize, bool hexCustomization, bool xof)
        {
            _numberOfCases = _numberOfSmallCases + _numberOfLargeCases;

            _macLength = MacSizes[_currentTestCase % MacSizes.Count];

            _keyLength = KeySizes[_currentTestCase % KeySizes.Count];

            var messageLength = MessageSizes[_currentTestCase++ % MessageSizes.Count];
            
            var customizationLength = 0;
            if (_currentSmallCase <= _numberOfSmallCases)
            {
                if (hexCustomization)
                {
                    customizationLength = _customizationLength * 8;  // always byte oriented... for now?
                }
                else
                {
                    customizationLength = _customizationLength;
                }
                _customizationLength = (_customizationLength + 1) % 100;
                _currentSmallCase++;
            }
            else
            {
                if (_customizationLength * _currentLargeCase < 2000)
                {
                    if (hexCustomization)
                    {
                        customizationLength = _customizationLength++ * _currentLargeCase * 8;  // always byte oriented... for now?
                    }
                    else
                    {
                        customizationLength = _customizationLength++ * _currentLargeCase;
                    }
                }
                _currentLargeCase++;
            }

            return new KmacParameters()
            {
                CouldFail = false,
                CustomizationLength = customizationLength,
                DigestSize = digestSize,
                HexCustomization = hexCustomization,
                KeyLength = _keyLength,
                MacLength = _macLength,
                MessageLength = messageLength,
                XOF = xof
            };
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
