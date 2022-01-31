using System.IO;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.RSA;
using NIST.CVP.ACVTS.Libraries.Crypto.RSA.Signatures;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SigVer.Parsers;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA_SigVer.IntegrationTests.Fips186_4
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\rsa\sigver\");
        }

        [Test]
        [TestCase("ansx9.31")]
        [TestCase("pkcs1v1.5")]
        [TestCase("pss")]
        public void ShouldRunThroughAllTestFilesAndValidate(string sigVerMode)
        {
            if (!Directory.Exists(_testPath))
            {
                Assert.Fail("Test File Directory does not exist");
            }

            var folderPath = new DirectoryInfo(Path.Combine(_testPath, sigVerMode));
            var parser = new LegacyResponseFileParser();
            var signerBuilder = new SignatureBuilder();

            foreach (var testFilePath in folderPath.EnumerateFiles())
            {
                var parseResult = parser.Parse(testFilePath.FullName);
                if (!parseResult.Success)
                {
                    Assert.Fail($"Could not parse: {testFilePath.FullName}");
                }

                var testVector = parseResult.ParsedObject;
                if (testVector.TestGroups.Count == 0)
                {
                    Assert.Fail("No TestGroups parsed");
                }

                foreach (var testGroup in testVector.TestGroups)
                {
                    testGroup.Mode = EnumHelpers.GetEnumFromEnumDescription<SignatureSchemes>(sigVerMode);

                    if (testGroup.Tests.Count == 0)
                    {
                        Assert.Fail("No TestCases parsed");
                    }

                    foreach (var testCase in testGroup.Tests)
                    {
                        var entropyProvider = new TestableEntropyProvider();
                        entropyProvider.AddEntropy(testCase.Salt);

                        var sha = new NativeShaFactory().GetShaInstance(testGroup.HashAlg);

                        var paddingScheme = new PaddingFactory(new MaskFactory(new NativeShaFactory())).GetPaddingScheme(testGroup.Mode, sha, PssMaskTypes.MGF1, entropyProvider, testGroup.SaltLen);

                        var result = signerBuilder
                            .WithMessage(testCase.Message)
                            .WithSignature(testCase.Signature)
                            .WithKey(testGroup.Key)
                            .WithPaddingScheme(paddingScheme)
                            .WithDecryptionScheme(new Rsa(new RsaVisitor()))
                            .BuildVerify();

                        if (result.Success != testCase.TestPassed)
                        {
                            Assert.Fail($"Could not generate TestCase: {testCase.TestCaseId}.\nTestCase expected to {(testCase.TestPassed.Value ? "pass" : "fail")}.\nTestCase actually {(result.Success ? "pass" : "fail")}\nTestCase actual fail reason: {(result.Success ? "none" : result.ErrorMessage)}");
                        }
                    }
                }
            }
        }
    }
}
