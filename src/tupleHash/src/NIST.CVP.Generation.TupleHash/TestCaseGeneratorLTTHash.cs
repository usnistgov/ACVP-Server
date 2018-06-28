using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
namespace NIST.CVP.Generation.TupleHash
{
    public class TestCaseGeneratorLTTHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _numberOfCases = 512;
        private int _currentEmptyCase = 0;
        private int _currentSemiEmptyCase = 0;
        private int _currentNonEmptyCase = 1;
        private int _customizationLength = 10;
        private int _minBitStringLength = 1;
        private int _maxBitStringLength = 2048;

        private readonly IRandom800_90 _random800_90;
        private readonly ITupleHash _algo;

        public int NumberOfTestCasesToGenerate => _numberOfCases;

        public TestCaseGeneratorLTTHash(IRandom800_90 random800_90, ITupleHash algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var unitSize = group.BitOrientedInput ? 1 : 8;
            var rate = 1600 - group.DigestSize * 2;

            var numEmptyCases = group.IncludeNull ? 10 : -1;

            var numSemiEmptyCases = group.IncludeNull ? 30 : -1;

            var numNonEmptyCases = 25;

            _numberOfCases = numEmptyCases + numSemiEmptyCases + numNonEmptyCases;

            var customization = _random800_90.GetRandomString(_customizationLength);

            var tuple = new List<BitString>();
            var tupleSize = 0;
            if (_currentEmptyCase <= numEmptyCases)
            {
                tupleSize = _currentEmptyCase;
                for (int i = 0; i < tupleSize; i++)
                {
                    tuple.Add(new BitString(""));
                }
                _currentEmptyCase++;
            }
            else if (_currentSemiEmptyCase <= numSemiEmptyCases)
            {
                tupleSize = (_currentSemiEmptyCase + 6) / 3;
                for (int i = 0; i < tupleSize; i++)
                {
                    if (_random800_90.GetRandomInt(0, 2) == 1)  // either 1 or 0
                    {
                        tuple.Add(_random800_90.GetRandomBitString(GetRandomValidLength(group.BitOrientedInput)));
                    }
                    else
                    {
                        tuple.Add(new BitString(""));
                    }
                }
                _currentSemiEmptyCase++;
            }
            else
            {
                if (_currentNonEmptyCase <= 20)
                {
                    tupleSize = _currentNonEmptyCase;
                }
                else
                {
                    tupleSize = _currentNonEmptyCase * 5;
                }
                for (int i = 0; i < tupleSize; i++)
                {
                    tuple.Add(_random800_90.GetRandomBitString(GetRandomValidLength(group.BitOrientedInput)));
                }
                _currentNonEmptyCase++;
            }

            var testCase = new TestCase
            {
                Tuple = tuple,
                Customization = customization,
                Deferred = false
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            HashResult hashResult = null;

            try
            {
                var hashFunction = new HashFunction
                {
                    Capacity = group.DigestSize * 2,
                    DigestSize = group.DigestSize,
                    XOF = group.XOF,
                    Customization = testCase.Customization
                };

                hashResult = _algo.HashMessage(hashFunction, testCase.Tuple);
                if (!hashResult.Success)
                {
                    ThisLogger.Warn(hashResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse<TestGroup, TestCase>(hashResult.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
                }
            }

            testCase.Digest = hashResult.Digest;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private int GetRandomValidLength(bool bitOriented)
        {
            var length = _random800_90.GetRandomInt(_minBitStringLength, _maxBitStringLength + 1);
            if (!bitOriented)
            {
                while (length % 8 != 0)
                {
                    length++;
                }
            }
            return length;
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
