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
    public class TestCaseGeneratorMCTHash : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly ISHA1_MCT _sha1_mct;
        private readonly IRandom800_90 _random800_90;

        public int NumberOfTestCasesToGenerate { get { return 1; } }

        public TestCaseGeneratorMCTHash(IRandom800_90 random800_90, ISHA1_MCT sha1_mct)
        {
            _random800_90 = random800_90;
            _sha1_mct = sha1_mct;
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

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            MCTResult hashResult = null;
            try
            {
                hashResult = _sha1_mct.MCTHash(testCase.Message);
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

            testCase.ResultsArray = hashResult.Response;
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
