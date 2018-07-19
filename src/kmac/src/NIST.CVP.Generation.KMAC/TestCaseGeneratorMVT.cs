using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.KMAC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.KMAC
{
    public class TestCaseGeneratorMVT : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IKmac _kmac;
        private readonly IRandom800_90 _random800_90;

        private int _capacity = 0;

        public int NumberOfTestCasesToGenerate => 15;

        public TestCaseGeneratorMVT(IRandom800_90 random800_90, IKmac kmac)
        {
            _random800_90 = random800_90;
            _kmac = kmac;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (_capacity == 0)
            {
                SetDomainRandomness(group.MacLengths);
                _capacity = group.DigestSize * 2;
            }

            //known answer - need to do an encryption operation to get the tag
            var key = _random800_90.GetRandomBitString(group.KeyLengths.GetDomainMinMax().Minimum);
            var msg = _random800_90.GetRandomBitString(group.MessageLength);
            var customization = _random800_90.GetRandomAlphaCharacters(_random800_90.GetRandomInt(0, 11));
            var macLen = group.MacLengths.GetValues(1).ElementAt(0);    // assuming there is only one segment
            var testCase = new TestCase
            {
                Key = key,
                Message = msg,
                MacLength = macLen,
                Customization = customization,
                MacVerified = true
            };
            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            MacResult macResult = null;
            try
            {
                macResult = _kmac.Generate(testCase.Key, testCase.Message, testCase.Customization, testCase.MacLength);
                if (!macResult.Success)
                {
                    ThisLogger.Warn(macResult.ErrorMessage);
                    {
                        return new TestCaseGenerateResponse<TestGroup, TestCase>(macResult.ErrorMessage);
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
            testCase.Mac = macResult.Mac;

            SometimesMangleTestCaseTag(testCase);

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private void SometimesMangleTestCaseTag(TestCase testCase)
        {
            // Alter the mac 50% of the time for a "failure" test
            int option = _random800_90.GetRandomInt(0, 2);
            if (option == 0)
            {
                testCase.Mac = _random800_90.GetDifferentBitStringOfSameSize(testCase.Mac);
                testCase.MacVerified = false;
            }
        }

        // can only be called once
        private void SetDomainRandomness(MathDomain domain)
        {
            domain.SetRangeOptions(RangeDomainSegmentOptions.Random);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
