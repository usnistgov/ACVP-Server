using System;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.FFC;
using NIST.CVP.Generation.KAS.FFC.Fakes;
using NIST.CVP.Generation.KAS.FFC.Parsers;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class FireHoseTestsFfc
    {
        string _testPath;
        private IShaFactory _shaFactory;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\");
            _shaFactory = new ShaFactory();
        }

        [Test]
        public void ShouldRunThroughAllTestFilesAndValidate()
        {
            LegacyResponseFileParser parser = new LegacyResponseFileParser();
            var parsedFiles = parser.Parse(_testPath);

            if (!parsedFiles.Success)
            {
                Assert.Fail("Failed parsing test files");
            }

            if (parsedFiles.ParsedObject.TestGroups.Count == 0)
            {
                Assert.Fail("No TestGroups were parsed.");
            }
            var testVector = parsedFiles.ParsedObject;
            
            int passes = 0;
            int expectedFails = 0;

            if (testVector.TestGroups.Count == 0)
            {
                Assert.Fail("No TestGroups were parsed.");
            }

            foreach (var iTestGroup in testVector.TestGroups)
            {
                var testGroup = (TestGroup) iTestGroup;

                var sha = _shaFactory.GetShaInstance(testGroup.HashAlg);

                foreach (var iTestCase in testGroup.Tests)
                {
                    var testCase = (TestCase) iTestCase;

                    var schemeBuilder = new SchemeBuilder(
                        new FfcDsa(sha),
                        new KdfFactory(_shaFactory),
                        new KeyConfirmationFactory(),
                        new NoKeyConfirmationFactory(),
                        new OtherInfoFactory(new EntropyProvider(new Random800_90())), 
                        new EntropyProvider(new Random800_90()),
                        new DiffieHellman(),
                        new Mqv()
                    );
                    var kasBuilder = new KasBuilder(schemeBuilder);
                    IDeferredTestCaseResolver<TestGroup, TestCase, KasResult> testCaseResolver;

                    switch (testGroup.KasMode)
                    {
                        case KasMode.NoKdfNoKc:
                            testCaseResolver = new DeferredTestCaseResolverAftNoKdfNoKc(kasBuilder);
                            break;
                        case KasMode.KdfNoKc:
                            testCaseResolver = new DeferredTestCaseResolverAftKdfNoKc(
                                kasBuilder,
                                new MacParametersBuilder(),
                                schemeBuilder,
                                new EntropyProviderFactory()
                            );
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    var result = testCaseResolver.CompleteDeferredCrypto(testGroup, testCase, testCase);

                    if (testCase.FailureTest)
                    {
                        Assert.AreNotEqual(
                            testGroup.KasMode == KasMode.NoKdfNoKc ? testCase.HashZ.ToHex() : testCase.Tag.ToHex(),
                            result.Tag.ToHex()
                        );
                        passes++;
                        expectedFails++;
                    }
                    else
                    {
                        Assert.AreEqual(
                            testGroup.KasMode == KasMode.NoKdfNoKc ? testCase.HashZ.ToHex() : testCase.Tag.ToHex(),
                            result.Tag.ToHex()
                        );
                        passes++;
                    }
                }
            }

            Assert.IsTrue(passes > 0, nameof(passes));
            Assert.IsTrue(expectedFails > 0, nameof(expectedFails));
        }
    }
}