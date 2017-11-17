using System.Collections.Generic;
using System.Linq;
using System.Text;
using AES_CFB1;
using Autofac;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_CFB1;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CFB1.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsBase
    {
        public override string Algorithm { get; } = "AES";
        public override string Mode { get; } = "CFB1";

        public override Executable Generator => Program.Main;
        public override Executable Validator => AES_CFB1_Val.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            AutofacConfig.OverrideRegistrations = null;
            AES_CFB1_Val.AutofacConfig.OverrideRegistrations = null;
        }

        protected override void OverrideRegistrationGenFakeFailure()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeFailure()
        {
            AES_CFB1_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            AES_CFB1_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC is intended to be a failure test, change it
            if (testCase.decryptFail != null)
            {
                testCase.decryptFail = false;
            }

            // If TC has a cipherText, change it
            if (testCase.cipherText != null)
            {
                string text = testCase.cipherText.ToString();
                BitOrientedBitString bs = ModifyText(ref text);
                testCase.cipherText = new string(bs.ToString().Replace(" ", string.Empty).ToArray());
            }

            // If TC has a plainText, change it
            if (testCase.plainText != null)
            {
                string text = testCase.plainText.ToString();

                BitOrientedBitString bs = ModifyText(ref text);
                testCase.plainText = new string(bs.ToString().Replace(" ", string.Empty).ToArray());
            }

            // If TC has a resultsArray, change some of the elements
            if (testCase.resultsArray != null)
            {
                BitString bsIV = new BitString(testCase.resultsArray[0].iv.ToString());
                bsIV = rand.GetDifferentBitStringOfSameSize(bsIV);
                testCase.resultsArray[0].iv = bsIV.ToHex();

                BitString bsKey = new BitString(testCase.resultsArray[0].key.ToString());
                bsKey = rand.GetDifferentBitStringOfSameSize(bsKey);
                testCase.resultsArray[0].key = bsKey.ToHex();

                string plainText = testCase.resultsArray[0].plainText.ToString();
                BitOrientedBitString bsPlainText = ModifyText(ref plainText);
                testCase.resultsArray[0].plainText = new string(bsPlainText.ToString().Replace(" ", string.Empty).ToArray());

                string cipherText = testCase.resultsArray[0].cipherText.ToString();
                BitOrientedBitString bsCipherText = ModifyText(ref cipherText);
                testCase.resultsArray[0].cipherText = new string(bsCipherText.ToString().Replace(" ", string.Empty).ToArray());
            }
        }

        #region MMT issue test
        #region test classes
        /// <summary>
        /// An issue was found with MMT where client/server disagreed on answer.  
        /// Issue only applied to MMT over a single segment size.
        /// The issue came down to <see cref="BitOrientedBitString"/> was written in the reverse order to the json files.
        /// This test class is used to mock up some of the things the generator does in order to test the failing/passing behavior.
        /// </summary>
        private class TestGenerator : BitOrientedGenerator<Parameters, TestVectorSet>
        {
            private readonly TestVectorSet _injectedTestVectorSet;
            
            public TestGenerator(
                TestVectorSet injectedTestVectorSet,
                ITestVectorFactory<Parameters> testVectorFactory, IParameterParser<Parameters> parameterParser, IParameterValidator<Parameters> parameterValidator, ITestCaseGeneratorFactoryFactory<TestVectorSet> iTestCaseGeneratorFactoryFactory) : base(testVectorFactory, parameterParser, parameterValidator, iTestCaseGeneratorFactoryFactory)
            {
                _injectedTestVectorSet = injectedTestVectorSet;
            }

            public override GenerateResponse Generate(string requestFilePath)
            {
                return SaveOutputs(requestFilePath, _injectedTestVectorSet);
            }
        }

        /// <summary>
        /// An issue was found with MMT where client/server disagreed on answer.  
        /// Issue only applied to MMT over a single segment size.
        /// The issue came down to <see cref="BitOrientedBitString"/> was written in the reverse order to the json files.
        /// This test class is used to publicly expose the parsed <see cref="TestVectorSet"/> to compare to expactations.
        /// </summary>
        private class TestValidator : Validator<TestVectorSet, TestCase>
        {
            
            public TestVectorSet ParsedTestVectorSet { get; private set; }
            
            public TestValidator(IDynamicParser dynamicParser, IResultValidator<TestCase> resultValidator, ITestCaseValidatorFactory<TestVectorSet, TestCase> testCaseValidatorFactory, ITestReconstitutor<TestVectorSet, TestCase> testReconstitutor) : base(dynamicParser, resultValidator, testCaseValidatorFactory, testReconstitutor)
            {
            }

            public override TestVectorValidation ValidateWorker(ParseResponse<dynamic> answerParseResponse, ParseResponse<dynamic> promptParseResponse,
                ParseResponse<dynamic> testResultParseResponse)
            {
                var testVectorSet = _testReconstitutor
                    .GetTestVectorSetExpectationFromResponse(answerParseResponse.ParsedObject, promptParseResponse.ParsedObject);

                ParsedTestVectorSet = testVectorSet;

                var results = testResultParseResponse.ParsedObject;
                var suppliedResults = _testReconstitutor.GetTestCasesFromResultResponse(results.testResults);
                var testCases = _testCaseValidatorFactory.GetValidators(testVectorSet, suppliedResults);
                var response = _resultValidator.ValidateResults(testCases, suppliedResults);
                return response;
            }
        }
        #endregion test classes

        [Test]
        [TestCase("5D48014888D0D2817EB6DE6C9A531350", "DC003497D79D92E1CB780DDCE437EEAD", "1010101101", "0110000101")]
        public void ShouldEnsureBitOrientBitStringsWrittenInCorrectOrderToJsonFile(string keyString, string ivString, string plaintextString, string expectedCiphertextString)
        {
            var targetFolder = GetTestFolder("BitOriented");

            BitString key = new BitString(keyString);
            BitString iv = new BitString(ivString);
            var plainText = BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit(plaintextString);
            var expectedCipherText = BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit(expectedCiphertextString);

            Crypto.AES_CFB1.AES_CFB1 algo = new Crypto.AES_CFB1.AES_CFB1(new RijndaelFactory(new RijndaelInternals()));
            var actualCipherText = algo.BlockEncrypt(iv, key, plainText);

            Assert.AreEqual(expectedCipherText, actualCipherText.CipherText, "Algo check pre serialization");

            var tv = SetupVectorSet(key, iv, plainText, actualCipherText);
            
            TestGenerator testGenerator = new TestGenerator(tv, null, null, null, null);
            testGenerator.Generate($@"{targetFolder}\test.test");

            var result = $"{targetFolder}{TestVectorFileNames[0]}";
            var prompt = $"{targetFolder}{TestVectorFileNames[1]}";
            var answer = $"{targetFolder}{TestVectorFileNames[2]}";

            TestValidator testValidator = new TestValidator(new DynamicParser(), new ResultValidator<TestCase>(), new TestCaseValidatorFactory(), new TestReconstitutor());
            testValidator.Validate(result, answer, prompt);

            var parsedTestVectorSet = testValidator.ParsedTestVectorSet;
            
            Assert.AreEqual(((TestCase)tv.TestGroups[0].Tests[0]).Key, ((TestCase)parsedTestVectorSet.TestGroups[0].Tests[0]).Key, "Key");
            Assert.AreEqual(((TestCase)tv.TestGroups[0].Tests[0]).IV, ((TestCase)parsedTestVectorSet.TestGroups[0].Tests[0]).IV, "IV");
            Assert.AreEqual(((TestCase)tv.TestGroups[0].Tests[0]).PlainText, ((TestCase)parsedTestVectorSet.TestGroups[0].Tests[0]).PlainText, "Plaintext");
            Assert.AreEqual(((TestCase)tv.TestGroups[0].Tests[0]).CipherText, ((TestCase)parsedTestVectorSet.TestGroups[0].Tests[0]).CipherText, "Ciphertext");
        }

        private TestVectorSet SetupVectorSet(BitString key, BitString iv, BitOrientedBitString plainText, EncryptionResult actualCipherText)
        {
            TestCase tc = new TestCase()
            {
                Key = key,
                IV = iv.GetDeepCopy(),
                PlainText = plainText,
                CipherText = actualCipherText.CipherText
            };

            TestGroup tg = new TestGroup()
            {
                Function = "encrypt",
                KeyLength = 128,
                StaticGroupOfTests = false,
                TestType = "MMT",
                Tests = new List<ITestCase>()
            };

            tg.Tests.Add(tc);

            TestVectorSet tv = new TestVectorSet()
            {
                Algorithm = "AES-CFB1",
                TestGroups = new List<ITestGroup>()
            };

            tv.TestGroups.Add(tg);

            return tv;
        }
        #endregion

        private static BitOrientedBitString ModifyText(ref string text)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var ch in text)
            {
                if (ch == '0')
                {
                    sb.Append('1');
                }
                else
                {
                    sb.Append('0');
                }
            }
            text = sb.ToString();
            var bs = BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit(text);
            return bs;
        }

        protected override string GetTestFileMinimalTestCases(string targetFolder)
        {
            RemoveMCTAndKATTestGroupFactories();
            return GetTestFileFewTestCases(targetFolder);
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            RemoveMCTAndKATTestGroupFactories();

            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = new string[] { "encrypt", "decrypt" },
                KeyLen = new int[] { ParameterValidator.VALID_KEY_SIZES.First() },
                IsSample = true
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = ParameterValidator.VALID_KEY_SIZES,
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }

        /// <summary>
        /// Can be used to only generate MMT groups for the genval tests
        /// </summary>
        public class FakeTestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters>
        {
            public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
            {
                return new List<ITestGroupGenerator<Parameters>>()
                {
                    new TestGroupGeneratorMultiBlockMessage()
                };
            }
        }

        private void RemoveMCTAndKATTestGroupFactories()
        {
            AutofacConfig.OverrideRegistrations += builder =>
            {
                builder.RegisterType<FakeTestGroupGeneratorFactory>().AsImplementedInterfaces();
            };
        }
    }
}
