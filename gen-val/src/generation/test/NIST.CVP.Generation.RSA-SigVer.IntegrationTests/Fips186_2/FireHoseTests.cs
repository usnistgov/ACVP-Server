﻿using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.IO;
using NIST.CVP.Generation.RSA.v1_0.LegacySigVer.Parsers;
using NIST.CVP.Generation.RSA.v1_0.SigVer;

namespace NIST.CVP.Generation.RSA_SigVer.IntegrationTests.Fips186_2
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\rsa\legacy-sigver\");
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
                    testGroup.Mode = EnumHelpers.GetEnumFromEnumDescription<SignatureSchemes>(sigVerMode);

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

                        var paddingScheme = new PaddingFactory(new MaskFactory(new ShaFactory())).GetPaddingScheme(testGroup.Mode, sha, PssMaskTypes.MGF1, entropyProvider, testGroup.SaltLen);

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