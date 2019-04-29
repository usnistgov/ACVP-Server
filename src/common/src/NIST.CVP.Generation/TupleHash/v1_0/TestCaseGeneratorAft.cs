using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class TestCaseGeneratorAft : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private int _testCasesToGenerate = 512;
        private int _numberOfSmallCases = 0;
        private int _numberOfLargeCases = 0;
        private int _currentSmallCase = 0;
        private int _currentLargeCase = 1;
        private int _currentEmptyCase = 1;
        private int _currentSemiEmptyCase = 1;
        private int _currentLongTupleCase = 1;
        private int _currentTestCase = 0;
        private int _customizationLength = 0;
        private int _digestLength = 0;
        private int _capacity = 0;
        private int _currentMessageLengthCounter = 0;

        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => _testCasesToGenerate;
        public List<int> OutputLengths { get; } = new List<int>() { 0 };                 // Primarily for testing purposes
        public List<int> MessageLengths { get; } = new List<int>();                        // Primarily for testing purposes

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            // Only do this logic once
            if (_capacity == 0)
            {
                OutputLengths.Clear();
                MessageLengths.Clear();
                var smallMessageLengths = DetermineSmallMessageDomain(group.MessageLength, group);
                var largeMessageLengths = DetermineLargeMessageDomain(group.MessageLength, group);
                DetermineLengths(group.OutputLength, smallMessageLengths, largeMessageLengths);
                _capacity = 2 * group.DigestSize;
            }

            _digestLength = OutputLengths[_currentTestCase++ % OutputLengths.Count];

            var includeNull = group.MessageLength.GetDomainMinMax().Minimum == 0;
            var bitOriented = group.MessageLength.DomainSegments.ElementAt(0).RangeMinMax.Increment == 1;
            var param = DetermineParameters(bitOriented, includeNull, group.DigestSize, group.HexCustomization, group.XOF);

            try
            {
                // local variable before await
                var digestLength = _digestLength;

                var oracleResult = await _oracle.GetTupleHashCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Tuple = oracleResult.Tuple,
                    Digest = oracleResult.Digest,
                    Customization = oracleResult.Customization,
                    CustomizationHex = oracleResult.CustomizationHex,
                    Deferred = false,
                    DigestLength = digestLength
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

        private void DetermineLengths(MathDomain outputDomain, MathDomain smallMessageDomain, MathDomain largeMessageDomain)
        {
            outputDomain.SetRangeOptions(RangeDomainSegmentOptions.Random);
            var minMax = outputDomain.GetDomainMinMax();
            smallMessageDomain.SetRangeOptions(RangeDomainSegmentOptions.Random);
            var smallMinMax = smallMessageDomain.GetDomainMinMax();
            largeMessageDomain.SetRangeOptions(RangeDomainSegmentOptions.Random);
            var largeMinMax = largeMessageDomain.GetDomainMinMax();

            var values = outputDomain.GetValues(150).OrderBy(o => Guid.NewGuid()).Take(150);
            int repetitions;
            var smallValues = smallMessageDomain.GetValues(125).OrderBy(o => Guid.NewGuid()).Take(125);
            var largeValues = largeMessageDomain.GetValues(25).OrderBy(o => Guid.NewGuid()).Take(25);

            if (!values.Any())
            {
                repetitions = 149;
            }
            else if (values.Count() > 149)
            {
                repetitions = 1;
            }
            else
            {
                repetitions = 150 / values.Count() + (150 % values.Count() > 0 ? 1 : 0);
            }

            foreach (var value in values)
            {
                for (var i = 0; i < repetitions; i++)
                {
                    OutputLengths.Add(value);
                }
            }

            foreach (var value in smallValues)
            {
                MessageLengths.Add(value);
                _numberOfSmallCases++;
            }

            foreach (var value in largeValues)
            {
                MessageLengths.Add(value);
                _numberOfLargeCases++;
            }

            while (_numberOfLargeCases < 25)
            {
                MessageLengths.Add(largeMessageDomain.GetDomainMinMax().Maximum);
                _numberOfLargeCases++;
            }

            // Make sure min and max appear in the list
            OutputLengths.Add(minMax.Minimum);
            OutputLengths.Add(minMax.Maximum);
            MessageLengths.Add(smallMinMax.Minimum);
            MessageLengths.Add(smallMinMax.Maximum);
            MessageLengths.Add(largeMinMax.Minimum);
            MessageLengths.Add(largeMinMax.Maximum);

            OutputLengths.Sort();
            MessageLengths.Sort();
        }

        private TupleHashParameters DetermineParameters(bool bitOriented, bool includeNull, int digestSize, bool hexCustomization, bool xof)
        {
            var numSmallCases = _numberOfSmallCases;
            var numLargeCases = _numberOfLargeCases;
            var numEmptyCases = includeNull ? 10 : 0;
            var numSemiEmptyCases = includeNull ? 30 : 0;
            var numLongTupleCases = 25;

            _testCasesToGenerate = numSmallCases + numLargeCases + numEmptyCases + numSemiEmptyCases;

            var customizationLen = 0;
            var tupleSize = 0;
            var messageLen = 0;
            var semiEmpty = false;
            var longRandom = false;
            if (_currentEmptyCase <= numEmptyCases)
            {
                tupleSize = _currentEmptyCase - 1;
                messageLen = 0;
                customizationLen = 0;
                _currentEmptyCase++;
            }
            else if (_currentSemiEmptyCase <= numSemiEmptyCases)
            {
                tupleSize = ((_currentSemiEmptyCase - 1) + 6) / 3;
                semiEmpty = true;
                customizationLen = 0;
                _currentSemiEmptyCase++;
            }
            else if (_currentSmallCase <= numSmallCases)
            {
                tupleSize = (_currentSmallCase % 3) + 1;
                messageLen = MessageLengths[_currentMessageLengthCounter++ % MessageLengths.Count];
                if (hexCustomization)
                {
                    customizationLen = _customizationLength * 8;  // always byte oriented... for now?
                }
                else
                {
                    customizationLen = _customizationLength;
                }
                _customizationLength = (_customizationLength + 1) % 100;
                _currentSmallCase++;
            }
            else if (_currentLongTupleCase <= numLongTupleCases)
            {
                if (_currentLongTupleCase <= 20)
                {
                    tupleSize = _currentLongTupleCase;
                }
                else
                {
                    tupleSize = _currentLongTupleCase * 5;
                }
                longRandom = true;
                _currentLongTupleCase++;
            }
            else
            {
                if (_customizationLength * _currentLargeCase < 2000)
                {
                    if (hexCustomization)
                    {
                        customizationLen = _customizationLength++ * _currentLargeCase * 8;  // always byte oriented... for now?
                    }
                    else
                    {
                        customizationLen = _customizationLength++ * _currentLargeCase;
                    }
                }
                customizationLen = 0;
                messageLen = MessageLengths[_currentMessageLengthCounter++ % MessageLengths.Count];
                tupleSize = 1;
                _currentLargeCase++;
            }

            return new TupleHashParameters
            {
                HashFunction = new HashFunction(_digestLength, _capacity, xof),
                BitOrientedInput = bitOriented,
                CustomizationLength = customizationLen,
                HexCustomization = hexCustomization,
                LongRandomCase = longRandom,
                SemiEmptyCase = semiEmpty,
                MessageLength = messageLen,
                TupleSize = tupleSize
            };
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
