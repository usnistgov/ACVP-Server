using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestCaseGeneratorPartialBlock : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        private int _casesPerSize = 5;
        private bool _sizesSet = false;
        private List<int> _validSizes = new List<int>();
        private int _curCasePerSizeIndex;
        private int _curSizeIndex;

        public int NumberOfTestCasesToGenerate { get; private set; } = 1;

        public TestCaseGeneratorPartialBlock(IOracle oracle)
        {
            _oracle = oracle;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                _casesPerSize = 1;
            }

            // Only do this once as a way to make sure nothing changes
            if (!_sizesSet)
            {
                _validSizes = GetValidSizes(group.DataLength);

                // Must be set here because it depends on group information
                NumberOfTestCasesToGenerate = _casesPerSize * _validSizes.Count;
            }

            if (_curCasePerSizeIndex >= _casesPerSize)
            {
                _curCasePerSizeIndex = 0;
                _curSizeIndex++;
            }

            _curCasePerSizeIndex++;

            var ctLen = _validSizes[_curSizeIndex];

            // This is a little hacky... but single block CTR is the same as OFB. So we can get past the awkward factory
            // TODO fix this up
            var param = new AesParameters
            {
                DataLength = ctLen,
                KeyLength = group.KeyLength,
                Direction = group.Direction,
                Mode = BlockCipherModesOfOperation.Ofb
            };

            AesResult result = null;
            try
            {
                result = _oracle.GetAesCase(param);
            }
            catch (Exception ex)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Failed to generate. {ex.Message}");
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                Key = result.Key,
                IV = result.Iv,
                PlainText = result.PlainText,
                CipherText = result.CipherText
            });
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            return null;
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
