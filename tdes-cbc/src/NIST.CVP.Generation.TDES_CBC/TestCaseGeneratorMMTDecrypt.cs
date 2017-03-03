using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.TDES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TDES_CBC
{
    public class TestCaseGeneratorMMTDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
            private const int BLOCK_SIZE_BITS = 64;
            private const int NUMBER_OF_CASES = 10;
            private readonly IRandom800_90 _random800_90;
            private readonly ITDES_CBC _algo;
            private int _currentCase;

            public TestCaseGeneratorMMTDecrypt(IRandom800_90 random800_90, ITDES_CBC algo)
            {
                _random800_90 = random800_90;
                _algo = algo;
            }

            public int NumberOfTestCasesToGenerate
            {
                get { return NUMBER_OF_CASES; }
            }

            public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
            {
            //todo separate out keys?
                var key = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS * @group.NumberOfKeys).ToOddParityBitString();
                var cipherText = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS * (_currentCase + 1));
                var iv = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
                var testCase = new TestCase
                {
                    Key = key,
                    CipherText = cipherText,
                    Iv = iv,
                    Deferred = false
                };
                _currentCase++;
                return Generate(group, testCase);
            }

            public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
            {
                DecryptionResult decryptionResult = null;
                try
                {
                    decryptionResult = _algo.BlockDecrypt(testCase.Key, testCase.CipherText, testCase.Iv);
                    if (!decryptionResult.Success)
                    {
                        ThisLogger.Warn(decryptionResult.ErrorMessage);
                        {
                            return new TestCaseGenerateResponse(decryptionResult.ErrorMessage);
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
                testCase.PlainText = decryptionResult.PlainText;
                return new TestCaseGenerateResponse(testCase);
            }

            private Logger ThisLogger
            {
                get { return LogManager.GetCurrentClassLogger(); }
            }
    }    
}

