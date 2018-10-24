using NIST.CVP.Crypto.RSA;
using NIST.CVP.Generation.RSA_SPComponent.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.IO;

namespace NIST.CVP.Generation.RSA_SPComponent.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        private string _testPath;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\sp-component\");
        }

        [Test]
        public void ShouldRunThroughAllTestFilesAndValidate()
        {
            if (!Directory.Exists(_testPath))
            {
                Assert.Fail("Test File Directory does not exist");
            }

            var folderPath = new DirectoryInfo(Path.Combine(_testPath));
            var parser = new LegacyResponseFileParser();
            var rsa = new Rsa(new RsaVisitor());

            foreach(var testFilePath in folderPath.EnumerateFiles())
            {
                var parseResult = parser.Parse(testFilePath.FullName);
                if (!parseResult.Success)
                {
                    Assert.Fail($"Could not parse: {testFilePath.FullName}");
                }

                var testVector = parseResult.ParsedObject;
                if(testVector.TestGroups.Count == 0)
                {
                    Assert.Fail("No TestGroups parsed");
                }

                foreach(var testGroup in testVector.TestGroups)
                {
                    if(testGroup.Tests.Count == 0)
                    {
                        Assert.Fail("No TestCases parsed");
                    }

                    foreach(var testCase in testGroup.Tests)
                    {
                        var result = rsa.Decrypt(testCase.Message.ToPositiveBigInteger(), testCase.Key.PrivKey, testCase.Key.PubKey);
                        if (result.Success != testCase.TestPassed)
                        {
                            Assert.Fail($"TestCase {testCase.TestCaseId} was incorrect. Expected {testCase.TestPassed}");
                        }
                    }
                }
            }
        }
    }
}