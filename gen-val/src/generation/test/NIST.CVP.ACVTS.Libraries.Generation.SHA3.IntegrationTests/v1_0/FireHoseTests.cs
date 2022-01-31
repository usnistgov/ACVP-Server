using System.IO;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Generation.SHA3.v1_0.Parsers;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.IntegrationTests.v1_0
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        private string _sha3TestPath;
        private string _shakeTestPath;

        [SetUp]
        public void SetUp()
        {
            _sha3TestPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\sha3\SHA3\BitOriented");
            _shakeTestPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\sha3\SHAKE\BitOriented");
        }

        [Test]
        public void ShouldParseAndRunSHA3CAVSFiles()
        {
            var testPath = _sha3TestPath;

            if (!Directory.Exists(testPath))
            {
                Assert.Fail($"Test File Directory does not exist: {testPath}.");
            }

            var parser = new SHA3LegacyResponseFileParser();
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
                foreach (var testCase in testGroup.Tests)
                {
                    count++;

                    if (testGroup.TestType.ToLower() == "mct")
                    {
                        var sha3Mct = GetShaMctInstance(testGroup.CommonHashFunction);
                        var result = sha3Mct.MctHash(testCase.Message, null, false);

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
                        var sha3 = GetShaInstance(testGroup.CommonHashFunction);
                        var result = sha3.HashMessage(testCase.Message);
                        Assert.AreEqual(testCase.Digest.ToHex(), result.Digest.ToHex(), $"Failed on count {count} inside AFT for {testGroup.DigestSize} for message {testCase.Message.ToHex()} with length {testCase.Message.BitLength}.");
                    }
                }
            }
        }

        [Test]
        public void ShouldParseAndRunSHAKECAVSFiles()
        {
            var testPath = _shakeTestPath;

            if (!Directory.Exists(testPath))
            {
                Assert.Fail("Test File Directory does not exist.");
            }

            var parser = new SHAKELegacyResponseFileParser();
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
                foreach (var testCase in testGroup.Tests)
                {
                    count++;

                    if (testGroup.TestType.ToLower() == "mct")
                    {
                        var domain = new MathDomain();
                        domain.AddSegment(new RangeDomainSegment(null, 16, 65536));

                        var shakeMct = GetShaMctInstance(testGroup.CommonHashFunction);
                        var result = shakeMct.MctHash(testCase.Message, domain, false);

                        Assert.IsTrue(result.Success, "result.Success must be successful");
                        Assert.IsTrue(testCase.ResultsArray.Count > 0, $"{nameof(testCase)} MCT hash count should be greater than 0");
                        Assert.IsTrue(result.Response.Count > 0, $"{nameof(result)} MCT hash count should be greater than 0");

                        for (var i = 0; i < testCase.ResultsArray.Count; i++)
                        {
                            Assert.AreEqual(testCase.ResultsArray[i].Digest.ToLittleEndianHex(), result.Response[i].Digest.ToLittleEndianHex(), $"Digest mismatch on index {i}");
                        }
                    }
                    else
                    {
                        var shake = GetShaInstance(testGroup.CommonHashFunction);
                        var result = shake.HashMessage(testCase.Message, testCase.DigestLength);
                        Assert.AreEqual(testCase.Digest.ToLittleEndianHex(), result.Digest.ToLittleEndianHex(), $"Failed on count {count} inside AFT/VOT for {testGroup.DigestSize} for message {testCase.Message.ToLittleEndianHex()} with length {testCase.Message.BitLength}.");
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
