using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.AES_CTR;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestCaseGeneratorPartialBlockDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IAesCtr _algo;

        private int _casesPerSize = 5;
        private bool _sizesSet = false;
        private List<int> _validSizes = new List<int>();
        private int _curCasePerSizeIndex;
        private int _curSizeIndex;

        public int NumberOfTestCasesToGenerate { get; private set; } = 1;

        public TestCaseGeneratorPartialBlockDecrypt(IRandom800_90 rand, IAesCtr algo)
        {
            _rand = rand;
            _algo = algo;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                _casesPerSize = 1;
            }

            // Only do this once as a way to make sure nothing changes
            if (!_sizesSet)
            {
                _validSizes = GetValidSizes(group.DataLength);
            }

            // Must be set here because it depends on group information
            NumberOfTestCasesToGenerate = _casesPerSize * _validSizes.Count;

            if (_curCasePerSizeIndex < _casesPerSize)
            {
                _curCasePerSizeIndex++;
            }
            else
            {
                _curCasePerSizeIndex = 0;
                _curSizeIndex++;
            }

            var ctLen = _validSizes[_curSizeIndex];

            var ct = _rand.GetRandomBitString(ctLen);
            var key = _rand.GetRandomBitString(128);
            var iv = _rand.GetRandomBitString(128);

            var testCase = new TestCase
            {
                CipherText = ct,
                Key = key,
                IV = iv,
                Length = ctLen
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            DecryptionResult decryptionResult = null;
            try
            {
                decryptionResult = _algo.DecryptBlock(testCase.Key, testCase.CipherText, testCase.IV);
                if (!decryptionResult.Success)
                {
                    ThisLogger.Warn(decryptionResult.ErrorMessage);
                    return new TestCaseGenerateResponse(decryptionResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse(ex.Message);
            }

            testCase.PlainText = decryptionResult.PlainText;
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();

        private List<int> GetValidSizes(MathDomain dataLength)
        {
            _sizesSet = true;

            // Can ask for 128 values because the valid domain only has this many elements
            return dataLength.GetValues(ParameterValidator.MAXIMUM_DATA_LEN).ToList();
        }
    }
}
