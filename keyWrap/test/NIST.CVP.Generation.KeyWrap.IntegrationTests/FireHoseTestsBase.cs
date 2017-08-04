using System.IO;
using NIST.CVP.Crypto.KeyWrap;
using NUnit.Framework;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KeyWrap.AES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NLog;
using NIST.CVP.Generation.KeyWrap.Parsers;

namespace NIST.CVP.Generation.KeyWrap.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public abstract class FireHoseTestsBase<TTestVectorSet, TTestGroup, TTestCase>
        where TTestVectorSet : TestVectorSetBase<TTestGroup, TTestCase>, new()
        where TTestGroup : TestGroupBase<TTestCase>, new()
        where TTestCase : TestCaseBase, new()
    {
        string _testPath;
        protected abstract string FolderName { get; }
        private readonly TestCaseGeneratorFactory<TTestGroup, TTestCase> _subject = new TestCaseGeneratorFactory<TTestGroup, TTestCase>(
            new KeyWrapFactory(),
            new Random800_90()
        );

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            LoggingHelper.ConfigureLogging("FireHose", "KeyWrap");
        }

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), $"..\\..\\TestFiles\\LegacyParserFiles\\{FolderName}");
        }
 
        //[Test]
        public virtual void ShouldRunThroughAllTestFilesAndValidate()
        {
            if (!Directory.Exists(_testPath))
            {
                Assert.Fail("Test File Directory does not exist");
            }

            var parser = new LegacyResponseFileParser<TTestVectorSet, TTestGroup, TTestCase>();
            var parsedFiles = parser.Parse(_testPath);
            if (!parsedFiles.Success)
            {
                Assert.Fail("Failed parsing test files");
            }

            if (parsedFiles.ParsedObject.TestGroups.Count == 0)
            {
                Assert.Fail("No TestGroups were parsed.");
            }

            int count = 0;
            int testPasses = 0;
            int fails = 0;
            int failureTests = 0;
            
            var testVector = parsedFiles.ParsedObject;

            if (testVector.TestGroups.Count == 0)
            {
                Assert.Fail("No TestGroups were parsed.");
            }

            foreach (var iTestGroup in testVector.TestGroups)
            {

                var testGroup = (TTestGroup)iTestGroup;
                foreach (var iTestCase in testGroup.Tests)
                {
                    count++;

                    var testCase = (TTestCase)iTestCase;

                    if (testGroup.Direction.ToLower() == "encrypt")
                    {
                        var expectedCipher = testCase.CipherText.GetDeepCopy();
                        var generator = _subject.GetCaseGenerator(testGroup);
                        var result = generator.Generate(testGroup, testCase);
                        var resultingTestCase = (TTestCase)result.TestCase;

                        if (!result.Success)
                        {
                            fails++;
                            continue;
                        }

                        Assert.AreEqual(expectedCipher.ToHex(), resultingTestCase.CipherText.ToHex(), $"Failed on count {count} expected CT {expectedCipher.ToHex()}, got {resultingTestCase.CipherText.ToHex()}");
                        testPasses++;
                        continue;
                    }
                    if (testGroup.Direction.ToLower() == "decrypt")
                    {
                        BitString expectedPlainText = null;
                        if (testCase.PlainText != null)
                        {
                            expectedPlainText = testCase.PlainText.GetDeepCopy();
                        }

                        var result = new KeyWrapFactory().GetKeyWrapInstance(testGroup.KeyWrapType).Decrypt(
                            testCase.Key,
                            testCase.CipherText,
                            testGroup.UseInverseCipher
                        );

                        if (testCase.FailureTest)
                        {
                            failureTests++;
                            if (result.Success)
                            {
                                fails++;
                                continue;
                            }

                            testPasses++;
                            continue;
                        }

                        testPasses++;

                        Assert.AreEqual(expectedPlainText.ToHex(), result.ResultingBitString.ToHex(), $"Failed on count {count} expected PT {testCase.PlainText.ToHex()}, got {result.ResultingBitString.ToHex()}");
                        continue;
                    }
                    
                    Assert.Fail($"unexpected testGroup direction {testGroup.Direction}");
                }
            }

            if (fails > 0)
                Assert.Fail("Unexpected failures were encountered.");

            Assert.IsTrue(testPasses > 0, "No tests were run");
            Assert.IsTrue(failureTests > 0, "No expected failure tests were run");
            //Assert.Fail($"Passes {testPasses}, fails {fails}, count {count}.");
        }

        private static Logger ThisLogger => LogManager.GetLogger("FireHose");
    }
}
