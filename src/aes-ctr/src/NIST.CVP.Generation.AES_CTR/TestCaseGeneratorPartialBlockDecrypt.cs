using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NLog;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestCaseGeneratorPartialBlockDecrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IBlockCipherEngine _engine;
        private readonly IModeBlockCipherFactory _modeFactory;
        private readonly ICounterFactory _counterFactory;

        private int _casesPerSize = 5;
        private bool _sizesSet = false;
        private List<int> _validSizes = new List<int>();
        private int _curCasePerSizeIndex;
        private int _curSizeIndex;

        public int NumberOfTestCasesToGenerate { get; private set; } = 1;

        public TestCaseGeneratorPartialBlockDecrypt(IRandom800_90 rand, IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory modeFactory, ICounterFactory counterFactory)
        {
            _rand = rand;
            _engine = engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes);
            _modeFactory = modeFactory;
            _counterFactory = counterFactory;
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

            var ct = _rand.GetRandomBitString(ctLen);
            var key = _rand.GetRandomBitString(group.KeyLength);
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

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            try
            {
                var algo = _modeFactory.GetCounterCipher(
                    _engine,
                    _counterFactory.GetCounter(
                        _engine,
                        CounterTypes.Additive,
                        testCase.IV.GetDeepCopy()
                    ));
                var result = algo.ProcessPayload(new ModeBlockCipherParameters(
                    BlockCipherDirections.Decrypt,
                    testCase.Key.GetDeepCopy(),
                    testCase.CipherText.GetDeepCopy()
                ));
                if (!result.Success)
                {
                    ThisLogger.Warn(result.ErrorMessage);
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(result.ErrorMessage);
                }

                testCase.PlainText = result.Result;
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse<TestGroup, TestCase>(ex.Message);
            }
            
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
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
