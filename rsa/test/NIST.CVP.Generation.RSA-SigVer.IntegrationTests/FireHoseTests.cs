using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Generation.RSA_SigVer.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NIST.CVP.Generation.RSA_SigVer.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\sigver\");
        }

        [Test]
        [TestCase("ansx9.31")]
        [TestCase("pkcs1v15")]
        [TestCase("pss")]
        public void ShouldRunThroughAllTestFilesAndValidate(string sigVerMode)
        {
            var vals = new List<int>();

            if (!Directory.Exists(_testPath))
            {
                Assert.Fail("Test File Directory does not exist");
            }

            var folderPath = new DirectoryInfo(Path.Combine(_testPath, sigVerMode));
            var parser = new LegacyResponseFileParser();
            var signerFactory = new SignerFactory();

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
                        var algo = signerFactory.GetSigner(sigVerMode);

                        algo.SetHashFunction(testGroup.HashAlg);
                        algo.SetSaltLen(testGroup.SaltLen);

                        var result = algo.Verify(testGroup.Modulo, testCase.Signature, testGroup.Key, testCase.Message);
                        if (result.Success != testCase.Result)
                        {
                            //vals.Add(testCase.TestCaseId);
                            Assert.Fail($"Could not generate TestCase: {testCase.TestCaseId}");
                        }
                    }
                }
            }

            // Assert.Fail(string.Join(", ", vals));
        }
    }
}
