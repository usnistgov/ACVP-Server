using System.IO;
using NIST.CVP.Generation.SHA2.Parsers;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.IntegrationTests
{
    [TestFixture]
    public class FireHoseTests
    {
        private string _testPath;
        private SHA _sha;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles");
            _sha = new SHA();
        }

        [Test]
        [TestCase("BitOriented")]
        [TestCase("ByteOriented")]
        public void ShouldParseAndRunCAVSFiles(string folder)
        {
            var testPath = Path.Combine(_testPath, folder);

            if (!Directory.Exists(testPath))
            {
                Assert.Fail("Test File Directory does not exist.");
            }

            var parser = new LegacyResponseFileParser();
            var parsedTestVectorSet = parser.Parse(testPath);

            if (!parsedTestVectorSet.Success)
            {
                Assert.Fail("Failed parsing test files");
            }

            if (parsedTestVectorSet.ParsedObject.TestGroups.Count == 0)
            {
                Assert.Fail("No TestGroups were parsed.");
            }

            var count = 0;
            foreach (var iTestGroup in parsedTestVectorSet.ParsedObject.TestGroups)
            {
                var testGroup = (TestGroup) iTestGroup;
                var hashFunction = new HashFunction
                {
                    Mode = testGroup.Function,
                    DigestSize = testGroup.DigestSize
                };

                foreach (var iTestCase in testGroup.Tests)
                {
                    count++;
                    var testCase = (TestCase) iTestCase;
                    
                    if (testGroup.TestType.ToLower() == "montecarlo")
                    {
                        var testCaseGenerator = new TestCaseGeneratorMonteCarloHash(new Random800_90(), _sha, false);
                        var generateResponse = testCaseGenerator.Generate(testGroup, testCase);
                        Assert.AreEqual(((TestCase)generateResponse.TestCase).Digest.ToHex(), testCase.Digest.ToHex(), $"Failed in montecarlo on count {count}"); 
                    }
                    else
                    {
                        var result = _sha.HashMessage(hashFunction, testCase.Message);
                        Assert.AreEqual(testCase.Digest.ToHex(), result.Digest.ToHex(), $"Failed on count {count}.");
                    }
                }
            }
        }
    }
}
