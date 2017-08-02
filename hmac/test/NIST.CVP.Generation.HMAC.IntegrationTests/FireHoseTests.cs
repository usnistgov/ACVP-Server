using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.HMAC.Parsers;
using NUnit.Framework;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;

namespace NIST.CVP.Generation.HMAC.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class FireHoseTests
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
            LegacyResponseFileParser parser = new LegacyResponseFileParser();
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
            var algoFactory = new HmacFactory(new ShaFactory());

            int count = 0;
            int passes = 0;
            int fails = 0;
            
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

                    var algo = algoFactory
                        .GetHmacInstance(
                            new HashFunction(
                                testGroup.ShaMode,
                                testGroup.ShaDigestSize
                            )
                        );

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

                    Assert.AreEqual(
                        testCase.Mac.ToHex(),
                        result.Mac.ToHex(),
                        $"Failed on count {count} expected CT {testCase.Mac.ToHex()}, got {result.Mac.ToHex()}"
                    );
                }

                if (fails > 0)
                    Assert.Fail("Unexpected failures were encountered.");
            }
            //Assert.Fail($"Passes {passes}, fails {fails}, count {count}.  Failure tests {failureTests}");
        }
       
    }
}
