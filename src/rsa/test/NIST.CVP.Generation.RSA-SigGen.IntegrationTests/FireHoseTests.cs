using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.IO;
using NIST.CVP.Generation.RSA.v1_0.SigGen;
using NIST.CVP.Generation.RSA.v1_0.SigGen.Parsers;

namespace NIST.CVP.Generation.RSA_SigGen.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\siggen\");
        }

        [Test]
        [TestCase("ansx9.31")]
        [TestCase("pkcs1v1.5")]
        [TestCase("pss")]
        public void ShouldRunThroughAllTestFilesAndValidate(string sigGenMode)
        {
            if (!Directory.Exists(_testPath))
            {
                Assert.Fail("Test File Directory does not exist");
            }

            var folderPath = new DirectoryInfo(Path.Combine(_testPath, sigGenMode));
            var parser = new LegacyResponseFileParser();
            var signer = new SignatureBuilder();

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
                    testGroup.Mode = EnumHelpers.GetEnumFromEnumDescription<SignatureSchemes>(sigGenMode);

                    if(testGroup.Tests.Count == 0)
                    {
                        Assert.Fail("No TestCases parsed");
                    }

                    foreach(var iTestCase in testGroup.Tests)
                    {
                        var testCase = (TestCase)iTestCase;

                        var entropyProvider = new TestableEntropyProvider();
                        entropyProvider.AddEntropy(testCase.Salt);

                        var sha = new ShaFactory().GetShaInstance(testGroup.HashAlg);

                        var paddingScheme = new PaddingFactory().GetPaddingScheme(testGroup.Mode, sha, entropyProvider, testGroup.SaltLen);

                        var result = signer
                            .WithMessage(testCase.Message)
                            .WithKey(testGroup.Key)
                            .WithDecryptionScheme(new Rsa(new RsaVisitor()))
                            .WithPaddingScheme(paddingScheme)
                            .BuildSign();

                        if (!result.Success)
                        {
                            Assert.Fail($"Could not generate TestCase: {testCase.TestCaseId}");
                        }

                        if (!testCase.Signature.ToPositiveBigInteger().Equals(result.Signature))
                        {
                            Assert.Fail($"Failed SigGen comparison on TestCase: {testCase.TestCaseId}");
                        }
                    }
                }
            }
        }
    }
}
