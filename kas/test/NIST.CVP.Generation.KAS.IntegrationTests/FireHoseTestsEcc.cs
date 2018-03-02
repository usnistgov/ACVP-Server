using System;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Builders.Ecc;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.ECC;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.Generation.KAS.ECC.Parsers;

namespace NIST.CVP.Generation.KAS.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class FireHoseTestsEcc
    {
        string _testPath;
        private readonly IShaFactory _shaFactory = new ShaFactory();
        private readonly IEccCurveFactory _curveFactory = new EccCurveFactory();

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\");
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
                var testGroup = (TestGroup)iTestGroup;

                SwitchTestGroupIutServerInformation(testGroup);

                foreach (var iTestCase in testGroup.Tests)
                {
                    var testCase = (TestCase)iTestCase;

                    var schemeBuilder = new SchemeBuilderEcc(
                        new DsaEccFactory(_shaFactory),
                        _curveFactory,
                        new KdfFactory(_shaFactory),
                        new KeyConfirmationFactory(),
                        new NoKeyConfirmationFactory(),
                        new OtherInfoFactory(new EntropyProvider(new Random800_90())),
                        new EntropyProvider(new Random800_90()),
                        new DiffieHellmanEcc(),
                        new MqvEcc()
                    );
                    var kasBuilder = new KasBuilderEcc(schemeBuilder);
                    IDeferredTestCaseResolver<TestGroup, TestCase, KasResult> testCaseResolver;

                    switch (testGroup.KasMode)
                    {
                        case KasMode.NoKdfNoKc:
                            testCaseResolver = new DeferredTestCaseResolverAftNoKdfNoKc(
                                _curveFactory,
                                kasBuilder,
                                schemeBuilder,
                                new MacParametersBuilder(),
                                new EntropyProviderFactory()
                            );
                            break;
                        case KasMode.KdfNoKc:
                            testCaseResolver = new DeferredTestCaseResolverAftKdfNoKc(
                                _curveFactory,
                                kasBuilder,
                                schemeBuilder,
                                new MacParametersBuilder(),
                                new EntropyProviderFactory()
                            );
                            break;
                        case KasMode.KdfKc:
                            testCaseResolver = new DeferredTestCaseResolverAftKdfKc(
                                _curveFactory,
                                kasBuilder,
                                schemeBuilder,
                                new MacParametersBuilder(),
                                new EntropyProviderFactory()
                            );
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    SwitchTestCaseIutServerInformation(testCase);

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

        /// <summary>
        /// note that deferred test case resolver performs KAS from the server perspective,
        /// but in order to detect IUT errors from the CAVS files, the test cases must be
        /// performed from the IUT perspective.
        /// 
        /// Switch around all server and iut related test information
        /// </summary>
        private static void SwitchTestGroupIutServerInformation(TestGroup testGroup)
        {
            var holdIdServer = testGroup.IdServer?.GetDeepCopy();
            testGroup.IdServer = testGroup.IdIut?.GetDeepCopy();
            testGroup.IdIut = holdIdServer?.GetDeepCopy();

            var holdIdServerLen = testGroup.IdServerLen;
            testGroup.IdServerLen = testGroup.IdIutLen;
            testGroup.IdIutLen = holdIdServerLen;

            testGroup.KasRole =
                KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(testGroup.KasRole);
            testGroup.KcRole =
                KeyGenerationRequirementsHelper.GetOtherPartyKeyConfirmationRole(testGroup.KcRole);
        }

        /// <summary>
        /// note that deferred test case resolver performs KAS from the server perspective,
        /// but in order to detect IUT errors from the CAVS files, the test cases must be
        /// performed from the IUT perspective.
        /// 
        /// Switch around all server and iut related test information
        /// </summary>
        /// <param name="testCase"></param>
        private static void SwitchTestCaseIutServerInformation(TestCase testCase)
        {
            testCase.IdIut = null;

            var holdEphemNonceIut = testCase.EphemeralNonceIut?.GetDeepCopy();
            testCase.EphemeralNonceIut = testCase.EphemeralNonceServer?.GetDeepCopy();
            testCase.EphemeralNonceServer = holdEphemNonceIut?.GetDeepCopy();

            var holdDkmNonceIut = testCase.DkmNonceIut?.GetDeepCopy();
            testCase.DkmNonceIut = testCase.DkmNonceServer?.GetDeepCopy();
            testCase.DkmNonceServer = holdDkmNonceIut?.GetDeepCopy();

            var holdEphemeralPrivateKeyIut = testCase.EphemeralPrivateKeyIut;
            testCase.EphemeralPrivateKeyIut = testCase.EphemeralPrivateKeyServer;
            testCase.EphemeralPrivateKeyServer = holdEphemeralPrivateKeyIut;

            var holdEphemeralPublicKeyIutX = testCase.EphemeralPublicKeyIutX;
            testCase.EphemeralPublicKeyIutX = testCase.EphemeralPublicKeyServerX;
            testCase.EphemeralPublicKeyServerX = holdEphemeralPublicKeyIutX;

            var holdEphemeralPublicKeyIutY = testCase.EphemeralPublicKeyIutY;
            testCase.EphemeralPublicKeyIutY = testCase.EphemeralPublicKeyServerY;
            testCase.EphemeralPublicKeyServerY = holdEphemeralPublicKeyIutY;

            var holdStaticPrivateKeyIut = testCase.StaticPrivateKeyIut;
            testCase.StaticPrivateKeyIut = testCase.StaticPrivateKeyServer;
            testCase.StaticPrivateKeyServer = holdStaticPrivateKeyIut;

            var holdStaticPublicKeyIutX = testCase.StaticPublicKeyIutX;
            testCase.StaticPublicKeyIutX = testCase.StaticPublicKeyServerX;
            testCase.StaticPublicKeyServerX = holdStaticPublicKeyIutX;

            var holdStaticPublicKeyIutY = testCase.StaticPublicKeyIutY;
            testCase.StaticPublicKeyIutY = testCase.StaticPublicKeyServerY;
            testCase.StaticPublicKeyServerY = holdStaticPublicKeyIutY;
        }
    }
}