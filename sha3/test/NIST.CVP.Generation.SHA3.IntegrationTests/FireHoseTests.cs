using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Generation.SHA3.Parsers;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA3.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        private string _testPath;
        private Crypto.SHA3.SHA3 _sha3;
        private SHA3_MCT _sha3MCT;
        private SHAKE_MCT _shakeMCT;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles");
            _sha3 = new Crypto.SHA3.SHA3();
            _sha3MCT = new SHA3_MCT(_sha3);
            _shakeMCT = new SHAKE_MCT(_sha3);
        }

        [Test]
        [TestCase("SHA3\\BitOriented")]
        [TestCase("SHA3\\ByteOriented")]
        public void ShouldParseAndRunSHA3CAVSFiles(string folder)
        {
            var testPath = Path.Combine(_testPath, folder);

            if (!Directory.Exists(testPath))
            {
                Assert.Fail("Test File Directory does not exist.");
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
            foreach (var iTestGroup in parsedTestVectorSet.ParsedObject.TestGroups)
            {
                var testGroup = (TestGroup)iTestGroup;
                var hashFunction = new HashFunction
                {
                    Capacity = testGroup.DigestSize * 2,
                    DigestSize = testGroup.DigestSize,
                    XOF = false
                };

                foreach (var iTestCase in testGroup.Tests)
                {
                    count++;
                    var testCase = (TestCase)iTestCase;

                    if (testGroup.TestType.ToLower() == "mct")
                    {
                        var result = _sha3MCT.MCTHash(hashFunction, testCase.Message);

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
                        var result = _sha3.HashMessage(hashFunction, testCase.Message);
                        Assert.AreEqual(testCase.Digest, result.Digest, $"Failed on count {count} inside AFT for {testGroup.DigestSize} for message {testCase.Message.ToHex()}.");
                    }
                }
            }
        }

        [Test]
        [TestCase("SHAKE\\BitOriented")]
        [TestCase("SHAKE\\ByteOriented")]
        public void ShouldParseAndRunSHAKECAVSFiles(string folder)
        {
            var testPath = Path.Combine(_testPath, folder);

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
            foreach (var iTestGroup in parsedTestVectorSet.ParsedObject.TestGroups)
            {
                var testGroup = (TestGroup)iTestGroup;
                var shuffledTests = testGroup.Tests.OrderBy(a => Guid.NewGuid()).ToList().GetRange(0, 20);

                foreach (var iTestCase in shuffledTests)
                {
                    count++;
                    var testCase = (TestCase)iTestCase;

                    if (testGroup.TestType.ToLower() == "mct")
                    {
                        var hashFunction = new HashFunction
                        {
                            Capacity = testGroup.DigestSize * 2,
                            DigestSize = testGroup.DigestSize,
                            XOF = true
                        };

                        var domain = new MathDomain();
                        domain.AddSegment(new RangeDomainSegment(null, 16, 65536));
                        var result = _shakeMCT.MCTHash(hashFunction, testCase.Message, domain);

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
                        var hashFunction = new HashFunction
                        {
                            Capacity = testGroup.DigestSize * 2,
                            DigestSize = testCase.Digest.BitLength,     // This is kinda awkward... but makes sense logistically because SHAKE128 at 17 bits is just appending on SHAKE128 at 16 bits
                            XOF = true
                        };

                        var result = _sha3.HashMessage(hashFunction, testCase.Message);
                        Assert.AreEqual(testCase.Digest, result.Digest, $"Failed on count {count} inside AFT/VOT for {testGroup.DigestSize} for message {testCase.Message.ToHex()}.");
                    }
                }
            }
        }
    }
}
