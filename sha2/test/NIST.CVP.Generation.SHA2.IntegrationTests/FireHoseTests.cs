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
        private SHA_MCT _shaMCT;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles");
            _sha = new SHA();
            _shaMCT = new SHA_MCT(_sha);
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
                        var result = _shaMCT.MCTHash(hashFunction, testCase.Message);

                        Assert.IsTrue(result.Success, "result.Success must be successful");
                        Assert.IsTrue(testCase.ResultsArray.Count > 0, $"{nameof(testCase)} MCT hash count should be greater than 0");
                        Assert.IsTrue(result.Response.Count > 0, $"{nameof(result)} MCT hash count should be greater than 0");

                        for (var i = 0; i < testCase.ResultsArray.Count; i++)
                        {
                            Assert.AreEqual(testCase.ResultsArray[i].Digest, result.Response[i].Digest, $"Digest mismatch on index {i}");
                        }
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
