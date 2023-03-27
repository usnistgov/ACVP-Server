using System.IO;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Generation.SHA2.v1_0.Parsers;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA2.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        private string _testPath;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\sha2");
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
            foreach (var testGroup in parsedTestVectorSet.ParsedObject.TestGroups)
            {
                var hashFunction = testGroup.CommonHashFunction;
                var sha = GetShaInstance(hashFunction);
                var shaMct = GetShaMctInstance(hashFunction);

                foreach (var testCase in testGroup.Tests)
                {
                    count++;

                    if (testGroup.TestType.ToLower() == "montecarlo")
                    {
                        var result = shaMct.MctHash(testCase.Message);

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
                        var result = sha.HashMessage(testCase.Message);
                        Assert.AreEqual(testCase.Digest.ToHex(), result.Digest.ToHex(), $"Failed on count {count}.");
                    }
                }
            }
        }

        private ISha GetShaInstance(HashFunction hashFunction)
        {
            return new NativeShaFactory().GetShaInstance(hashFunction);
        }

        private IShaMct GetShaMctInstance(HashFunction hashFunction)
        {
            return new NativeShaFactory().GetShaMctInstance(hashFunction);
        }
    }
}
