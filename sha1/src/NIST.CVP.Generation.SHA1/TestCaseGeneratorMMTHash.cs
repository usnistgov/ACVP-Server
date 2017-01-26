using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA1
{
    public class TestCaseGeneratorMMTHash :ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ISHA1 _isha1;
        private int _msgLenGenIteration;

        private const int _BITS_IN_BYTE = 8;

        public int NumberOfTestCasesToGenerate { get { return 10; } }

        public TestCaseGeneratorMMTHash(IRandom800_90 random800_90, ISHA1 isha1)
        {
            _random800_90 = random800_90;
            _isha1 = isha1;
            _msgLenGenIteration = 0;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            var bitOriented = group.BitOriented;
            var includeNull = group.IncludeNull;

            if (!includeNull && _msgLenGenIteration == 0)
            {
                _msgLenGenIteration = 1;
            }

            var msgLength = (bitOriented ? _msgLenGenIteration++ : _msgLenGenIteration++ * _BITS_IN_BYTE);
            var message = _random800_90.GetRandomBitString(msgLength);
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
                hashResult = _isha1.HashMessage(testCase.Message);
                if (!hashResult.Success)
                {
                    ThisLogger.Warn(hashResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse(hashResult.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse(ex.Message);
                }
            }

            testCase.Digest = hashResult.Digest;
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
