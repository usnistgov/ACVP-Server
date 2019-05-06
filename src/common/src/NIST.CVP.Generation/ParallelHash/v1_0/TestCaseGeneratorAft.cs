using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.ParallelHash.v1_0
{
    public class TestCaseGeneratorAft : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private int _testCasesToGenerate = 512;
        private int _numberOfSmallCases = 0;
        private int _numberOfLargeCases = 0;
        private int _currentSmallCase = 0;
        private int _currentLargeCase = 1;
        private int _currentTestCase = 0;
        private int _customizationLength = 0;
        private int _digestLength = 0;
        private int _capacity = 0;
        private int _blockSize = 8;

        private readonly IOracle _oracle;

        public int NumberOfTestCasesToGenerate => _testCasesToGenerate;
        public List<int> OutputLengths { get; } = new List<int>() { 0 };                 // Primarily for testing purposes
        public List<int> MessageLengths { get; } = new List<int>();                        // Primarily for testing purposes

        public TestCaseGeneratorAft(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
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

            _digestLength = OutputLengths[_currentTestCase % OutputLengths.Count];

            var param = DetermineParameters(group.DigestSize, group.HexCustomization, group.XOF);

            _currentTestCase++;

            try
            {
                // local variable before await
                var digestLength = _digestLength;
                var blockSize = _blockSize;

                var oracleResult = await _oracle.GetParallelHashCaseAsync(param);

                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
                {
                    Message = oracleResult.Message,
                    Digest = oracleResult.Digest,
                    Customization = oracleResult.Customization,
                    CustomizationHex = oracleResult.CustomizationHex,
                    BlockSize = blockSize,
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

            var values = outputDomain.GetValues(250).OrderBy(o => Guid.NewGuid()).Take(250);
            int repetitions;
            var smallValues = smallMessageDomain.GetValues(150).OrderBy(o => Guid.NewGuid()).Take(150);
            var largeValues = largeMessageDomain.GetValues(100).OrderBy(o => Guid.NewGuid()).Take(100);

            if (values.Count() == 0)
            {
                repetitions = 249;
            }
            else if (values.Count() > 249)
            {
                repetitions = 1;
            }
            else
            {
                repetitions = 250 / values.Count() + (250 % values.Count() > 0 ? 1 : 0);
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

            while (_numberOfLargeCases < 100)
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

        private ParallelHashParameters DetermineParameters(int digestSize, bool hexCustomization, bool xof)
        {
            _testCasesToGenerate = _numberOfSmallCases + _numberOfLargeCases;

            var messageLength = MessageLengths[_currentTestCase % MessageLengths.Count];

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
                _blockSize = 64;
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
                if (_currentLargeCase < 33)
                {
                    _blockSize = _currentLargeCase * 8;
                }
                else
                {
                    _blockSize = _currentLargeCase < 56 ? 512 : (_currentLargeCase < 79 ? 1024 : 2048);    // 33 to 55 is 64, 56 to 78 is 128, 79 to 100 is 256
                }
                _currentLargeCase++;
            }

            return new ParallelHashParameters
            {
                HashFunction = new HashFunction(_digestLength, digestSize * 2, xof),
                CustomizationLength = customizationLength,
                HexCustomization = hexCustomization,
                BlockSize = _blockSize / 8,         // needs to be in bytes for algo
                MessageLength = messageLength
            };
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
