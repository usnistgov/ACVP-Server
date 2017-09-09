using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Generation.CMAC.AES;
using NIST.CVP.Generation.CMAC.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class AesFireHoseTests
    {
        string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\");
        }
 
        [Test]
        public void ShouldRunThroughAllTestFilesAndValidate()
        {
            var parser = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
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
            var algo = new CmacAes(new RijndaelFactory(new RijndaelInternals()));

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

                var testGroup = (TestGroup) iTestGroup;
                foreach (var iTestCase in testGroup.Tests)
                {
                    count++;

                    var testCase = (TestCase) iTestCase;

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

                        if (testCase.Mac.ToHex() == result.ResultingMac.ToHex())
                        {
                            passes++;
                        }
                        else
                        {
                            fails++;
                        }

                        Assert.AreEqual(
                            testCase.Mac.ToHex(),
                            result.ResultingMac.ToHex(),
                            $"Failed on count {count} expected CT {testCase.Mac.ToHex()}, got {result.ResultingMac.ToHex()}"
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

                        if (testCase.FailureTest)
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

                    if (fails > 0)
                        Assert.Fail("Unexpected failures were encountered.");

                    Assert.Fail($"{testGroup.Function} did not meet expected function values");
                }
            }
            //Assert.Fail($"Passes {passes}, fails {fails}, count {count}.  Failure tests {failureTests}");
        }
       
    }
}
