using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.SHA3
{
    public class TestCaseGeneratorSHAKEVOTHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _capacity = 0;
        private int _currentCase = 0;

        private List<int> _testCaseSizes = new List<int>();

        private int _digestSize = 0;

        private readonly IRandom800_90 _random800_90;
        private readonly ISHA3 _algo;

        public int NumberOfTestCasesToGenerate { get { return _testCaseSizes.Count; } }
        public List<int> TestCaseSizes { get { return _testCaseSizes; } }                   // Primarily for testing purposes

        public TestCaseGeneratorSHAKEVOTHash(IRandom800_90 random800_90, ISHA3 algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
            _testCaseSizes.Add(-1);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            // Only do this logic once
            if (_capacity == 0)
            {
                _testCaseSizes.Clear();
                DetermineLengths(group.MinOutputLength, group.MaxOutputLength, group.BitOrientedOutput);
                _capacity = 2 * group.DigestSize;
            }

            _digestSize = _testCaseSizes[_currentCase];
            _currentCase++;

            var message = _random800_90.GetRandomBitString(_capacity / 2);
            var testCase = new TestCase
            {
                Message = message,
                Deferred = false
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            HashResult hashResult = null;

            try
            {
                var hashFunction = new HashFunction
                {
                    Capacity = _capacity,
                    DigestSize = _digestSize,
                    XOF = true
                };

                hashResult = _algo.HashMessage(hashFunction, testCase.Message);
                if (!hashResult.Success)
                {
                    ThisLogger.Warn(hashResult.ErrorMessage);
                    return new TestCaseGenerateResponse(hashResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse(ex.Message);
            }

            testCase.Digest = hashResult.Digest;
            return new TestCaseGenerateResponse(testCase);
        }

        private void DetermineLengths(int min, int max, bool bitOriented)
        {
            var validValues = (max - min) / (bitOriented ? 1 : 8);
            int step, repetitions;

            if (validValues == 0)
            {
                repetitions = 999;
                step = 0;
            }
            else if (validValues > 999)
            {
                repetitions = 1;
                step = (int) System.Math.Ceiling(validValues / 999.0) * (bitOriented ? 1 : 8);
            }
            else
            {
                if (validValues == 0)
                {
                    validValues = 1;
                }
                repetitions = 1000 / validValues;
                step = bitOriented ? 1 : 8;
            }

            // Shift step a bit to make sure it has the desired properties
            if (bitOriented)
            {
                if (step % 2 == 0)
                {
                    step++;
                }
            }
            else
            {
                if (step % 8 != 0)
                {
                    step += 8 - step % 8;
                }
            }

            if (validValues == 0)
            {
                for (var i = 0; i < 1000; i++)
                {
                    _testCaseSizes.Add(max);
                }
            }
            else
            {
                for (var i = min; i < max; i += step)
                {
                    for (var j = 0; j < repetitions; j++)
                    {
                        _testCaseSizes.Add(i);
                    }
                }

                _testCaseSizes.Add(max);
            }
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}



