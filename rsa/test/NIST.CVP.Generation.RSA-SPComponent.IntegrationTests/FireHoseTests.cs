using System.IO;
using NIST.CVP.Crypto.RSA2;
using NIST.CVP.Generation.RSA_SPComponent.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_SPComponent.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
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

                foreach(var iTestGroup in testVector.TestGroups)
                {
                    var testGroup = (TestGroup)iTestGroup;

                    if(testGroup.Tests.Count == 0)
                    {
                        Assert.Fail("No TestCases parsed");
                    }

                    foreach(var iTestCase in testGroup.Tests)
                    {
                        var testCase = (TestCase)iTestCase;

                        var result = rsa.Decrypt(testCase.Message.ToPositiveBigInteger(), testCase.Key.PrivKey, testCase.Key.PubKey);
                        if (result.Success == testCase.FailureTest)
                        {
                            Assert.Fail($"TestCase {testCase.TestCaseId} was incorrect. Expected {testCase.FailureTest}");
                        }
                    }
                }
            }
        }
    }
}