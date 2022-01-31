using System.IO;
using NIST.CVP.ACVTS.Libraries.Crypto.KeyWrap;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0.Parsers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public abstract class FireHoseTestsBase<TTestVectorSet, TTestGroup, TTestCase>
        where TTestVectorSet : TestVectorSetBase<TTestGroup, TTestCase>, new()
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>, new()
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>, new()
    {
        string _testPath;
        protected abstract string FolderName { get; }
        private readonly KeyWrapFactory _subject = new KeyWrapFactory();

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            LoggingHelper.ConfigureLogging("FireHose", "KeyWrap");
        }

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), $@"..\..\LegacyCavsFiles\keyWrap\{FolderName}");
        }

        [Test]
        [Ignore("Virtual test method, not used directly")]
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

            foreach (var testGroup in testVector.TestGroups)
            {
                foreach (var testCase in testGroup.Tests)
                {
                    count++;

                    if (testGroup.Direction.ToLower() == "encrypt")
                    {
                        var expectedCipher = testCase.CipherText.GetDeepCopy();
                        var result = _subject.GetKeyWrapInstance(testGroup.KeyWrapType)
                            .Encrypt(testCase.Key, testCase.PlainText, testGroup.UseInverseCipher);

                        if (!result.Success)
                        {
                            fails++;
                            continue;
                        }

                        Assert.AreEqual(expectedCipher.ToHex(), result.Result.ToHex(), $"Failed on count {count} expected CT {expectedCipher.ToHex()}, got {result.Result.ToHex()}");
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

                        var result = _subject.GetKeyWrapInstance(testGroup.KeyWrapType)
                            .Decrypt(testCase.Key, testCase.CipherText, testGroup.UseInverseCipher);

                        if (!testCase.TestPassed.Value)
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

                        Assert.AreEqual(expectedPlainText.ToHex(), result.Result.ToHex(), $"Failed on count {count} expected PT {testCase.PlainText.ToHex()}, got {result.Result.ToHex()}");
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
    }
}
