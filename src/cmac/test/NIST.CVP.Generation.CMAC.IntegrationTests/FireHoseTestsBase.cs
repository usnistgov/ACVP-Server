using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Generation.CMAC.Parsers;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public abstract class FireHoseTestsBase<TLegacyResponseFileParser, TTestCaseGeneratorGen, TTestVectorSet, TTestGroup, TTestCase>
        where TLegacyResponseFileParser : LegacyResponseFileParserBase<TTestVectorSet, TTestGroup, TTestCase>, new()
        where TTestCaseGeneratorGen : TestCaseGeneratorGenBase<TTestGroup, TTestCase>
        where TTestVectorSet : TestVectorSetBase<TTestGroup, TTestCase>, new()
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>, new()
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>, new()
    {
        string _testPath;
        protected abstract string FolderName { get; }

        private readonly TestCaseGeneratorFactory<TTestCaseGeneratorGen, TTestGroup, TTestCase> _subject = 
            new TestCaseGeneratorFactory<TTestCaseGeneratorGen, TTestGroup, TTestCase>(
                new Random800_90(), new CmacFactory());
        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), $@"..\..\TestFiles\LegacyParserFiles\{FolderName}");
        }
 
        [Test]
        protected void ShouldRunThroughAllTestFilesAndValidate()
        {
            var parser = new TLegacyResponseFileParser();
            var parsedFiles = parser.Parse(_testPath);
            
            if (!parsedFiles.Success)
            {
                Assert.Fail("Failed parsing test files");
            }

            if (parsedFiles.ParsedObject.TestGroups.Count == 0)
            {
                Assert.Fail("No TestGroups were parsed.");
            }
            var testVector = parsedFiles.ParsedObject;
            

            int count = 0;
            int passes = 0;
            int fails = 0;
            int failureTests = 0;
            
            if (testVector.TestGroups.Count == 0)
            {
                Assert.Fail("No TestGroups were parsed.");
            }

            foreach (var iTestGroup in testVector.TestGroups)
            {

                var testGroup = iTestGroup;
                var algo = _subject.GetCmac(testGroup);
                foreach (var iTestCase in testGroup.Tests)
                {
                    count++;

                    var testCase = iTestCase;

                    if (testGroup.Function.ToLower() == "gen")
                    {
                        var result = algo.Generate(
                            testCase.Key,
                            testCase.Message,
                            testGroup.MacLength
                        );

                        if (!result.Success)
                        {
                            fails++;
                            continue;
                        }

                        if (testCase.Mac.ToHex() == result.Mac.ToHex())
                        {
                            passes++;
                        }
                        else
                        {
                            fails++;
                        }

                        Assert.AreEqual(
                            testCase.Mac.ToHex(),
                            result.Mac.ToHex(),
                            $"Failed on count {count} expected CT {testCase.Mac.ToHex()}, got {result.Mac.ToHex()}"
                        );
                        continue;
                    }

                    if (testGroup.Function.ToLower() == "ver")
                    {
                        var result = algo.Verify(
                            testCase.Key,
                            testCase.Message,
                            testCase.Mac
                        );

                        if (testCase.TestPassed != null && !testCase.TestPassed.Value)
                        {
                            failureTests++;
                            if (result.Success)
                            {
                                fails++;
                            }
                            else
                            {
                                passes++;
                            }
                        }
                        else
                        {
                            if (result.Success)
                            {
                                passes++;
                            }
                            else
                            {
                                fails++;
                            }
                        }
                        continue;
                    }

                    Assert.Fail($"{testGroup.Function} did not meet expected function values");
                }
            }

            if (fails > 0)
                Assert.Fail("Unexpected failures were encountered.");

            //Assert.Fail($"Passes {passes}, fails {fails}, count {count}.  Failure tests {failureTests}");
        }
       
    }
}
