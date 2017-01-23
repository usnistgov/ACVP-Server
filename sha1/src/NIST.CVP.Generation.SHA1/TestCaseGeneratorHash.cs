using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.SHA;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.SHA1
{
    public class TestCaseGeneratorHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly ISHA1 _sha1;
        private readonly IRandom800_90 _random800_90;

        public TestCaseGeneratorHash(IRandom800_90 random800_90, ISHA1 sha1)
        {
            _random800_90 = random800_90;
            _sha1 = sha1;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            HashResult hashResult = null;
            try
            {
                hashResult = _sha1.HashMessage(testCase.Message);
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

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            var message = _random800_90.GetRandomBitString(group.MessageLength);
            var testCase = new TestCase
            {
                Message = message,
                Deferred = false
            };
            return Generate(group, testCase);
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
