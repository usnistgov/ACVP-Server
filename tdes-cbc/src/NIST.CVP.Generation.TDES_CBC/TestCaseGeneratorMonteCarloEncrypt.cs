using System;
using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.TDES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TDES_CBC
{
    public class TestCaseGeneratorMonteCarloEncrypt : ITestCaseGenerator<TestGroup, TestCase>
    {
        private const int BLOCK_SIZE_BITS = 64;
        private const int NUMBER_OF_CASES = 400;
        private const int NUMBER_OF_SAMPLE_CASES = 3;
        private const int NUMBER_OF_ITERATIONS = 10000;
        private const int NUMBER_OF_CIPHER_TEXTS_TO_SAVE = 3;

        private readonly IRandom800_90 _random800_90;
        private readonly ITDES_CBC _algo;
        private readonly IMonteCarloKeyMaker _keyMaker;

        private int _testCaseId;
        private TestCase _previousCase = null;
        public bool IsSample { get; } = false;
        private readonly List<BitString> _lastCipherTexts = new List<BitString>();
        private TestCase _seedCaseForTest = null;

        public TestCaseGeneratorMonteCarloEncrypt(IRandom800_90 random800_90, ITDES_CBC algo, IMonteCarloKeyMaker keyMaker, bool isSample)
        {
            _random800_90 = random800_90;
            _algo = algo;
            _keyMaker = keyMaker;
            IsSample = isSample;
        }

        public TestCaseGeneratorMonteCarloEncrypt(IRandom800_90 random800_90, ITDES_CBC algo, IMonteCarloKeyMaker keyMaker, TestCase seedCaseForTest)
        {
            _random800_90 = random800_90;
            _algo = algo;
            _seedCaseForTest = seedCaseForTest;
            _keyMaker = keyMaker;
        }

        public int NumberOfTestCasesToGenerate
        {
            get
            {
                if (IsSample)
                {
                    return NUMBER_OF_SAMPLE_CASES;
                }
                return NUMBER_OF_CASES;
            }
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            var seedCase = GetSeedCase(@group);

            var result = Generate(@group, seedCase);
            if (result.Success)
            {
                _previousCase = (TestCase)result.TestCase;
            }
            return result;

        }

        private TestCase GetSeedCase(TestGroup @group)
        {
            //if this is the first call, get the initial seed case
            if (_previousCase == null)
            {
                return GetInitialSeedCase(@group);
            }

            //use the previous case to make start the new one
            //mix up the keys and use the cipherText as the new plainText
            var newKey = _keyMaker.MixKeys(new TDESKeys(_previousCase.Key), _lastCipherTexts);
            return new TestCase {PlainText = _previousCase.CipherText, Key = newKey};
           
        }

        private TestCase GetInitialSeedCase(TestGroup @group)
        {
            //is seeded for testing -- use it
            if (_seedCaseForTest != null)
            {
                return _seedCaseForTest;
            }
 
            //make a new random case
            var key = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS * @group.NumberOfKeys);
            var plainText = _random800_90.GetRandomBitString(BLOCK_SIZE_BITS);
            return new TestCase {Key = key, PlainText = plainText};
        }

        private void SaveCipherTextForKeyMixing(BitString cipherText)
        {
            _lastCipherTexts.Insert(0, cipherText);
            if (_lastCipherTexts.Count > NUMBER_OF_CIPHER_TEXTS_TO_SAVE)
            {
                _lastCipherTexts.RemoveRange(NUMBER_OF_CIPHER_TEXTS_TO_SAVE, _lastCipherTexts.Count - NUMBER_OF_CIPHER_TEXTS_TO_SAVE);
            }

        }
       
        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase seedCase)
        {
            TestCase tempTestCase = seedCase;
            BitString key = seedCase.Key;
            
            try
            {
                EncryptionResult encryptionResult = null;
                for (int iteration = 0; iteration < NUMBER_OF_ITERATIONS; iteration++)
                {
                    //COMMENTED THE FOLLOWING LINE OF CODE - Alex
                   // encryptionResult = _algo.BlockEncrypt(tempTestCase.Key, tempTestCase.PlainText);
                    if (!encryptionResult.Success)
                    {
                        ThisLogger.Warn(encryptionResult.ErrorMessage);
                        {
                            return new TestCaseGenerateResponse(encryptionResult.ErrorMessage);
                        }
                    }
                    // ThisLogger.Debug($"{iteration:00000} K: {tempTestCase.Key.ToHex()}, PT: {tempTestCase.PlainText.ToHex()}, CT: {encryptionResult.CipherText.ToHex()}");
                    SaveCipherTextForKeyMixing(encryptionResult.CipherText);
                    tempTestCase.PlainText = encryptionResult.CipherText;
                }
                var testCase = new TestCase { Key = key, PlainText = seedCase.PlainText, CipherText = encryptionResult.CipherText, TestCaseId = _testCaseId };
                return new TestCaseGenerateResponse(testCase);

            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                {
                    return new TestCaseGenerateResponse(ex.Message);
                }
            }
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }

       
    }
}
