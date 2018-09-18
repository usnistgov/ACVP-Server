using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Builders.Ffc;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.Oracle;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.FFC;
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
        public async Task ShouldRunThroughAllTestFilesAndValidate()
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

            foreach (var testGroup in testVector.TestGroups)
            {
                SwitchTestGroupIutServerInformation(testGroup);

                foreach (var testCase in testGroup.Tests)
                {
                    var testCaseResolver = new DeferredTestCaseResolver(new Oracle(null));

                    SwitchTestCaseIutServerInformation(testCase);

                    var result = await testCaseResolver.CompleteDeferredCryptoAsync(testGroup, testCase, testCase);

                    Debug.Assert(testCase.TestPassed != null, "testCase.TestPassed != null");
                    if (testCase.TestPassed.Value)
                    {
                        Assert.AreEqual(
                            testGroup.KasMode == KasMode.NoKdfNoKc ? testCase.HashZ.ToHex() : testCase.Tag.ToHex(),
                            result.Tag.ToHex()
                        );
                        passes++;
                    }
                    else
                    {
                        Assert.AreNotEqual(
                            testGroup.KasMode == KasMode.NoKdfNoKc ? testCase.HashZ.ToHex() : testCase.Tag.ToHex(),
                            result.Tag.ToHex()
                        );
                        passes++;
                        expectedFails++;
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

            var holdEphemeralPublicKeyIut = testCase.EphemeralPublicKeyIut;
            testCase.EphemeralPublicKeyIut = testCase.EphemeralPublicKeyServer;
            testCase.EphemeralPublicKeyServer = holdEphemeralPublicKeyIut;

            var holdStaticPrivateKeyIut = testCase.StaticPrivateKeyIut;
            testCase.StaticPrivateKeyIut = testCase.StaticPrivateKeyServer;
            testCase.StaticPrivateKeyServer = holdStaticPrivateKeyIut;

            var holdStaticPublicKeyIut = testCase.StaticPublicKeyIut;
            testCase.StaticPublicKeyIut = testCase.StaticPublicKeyServer;
            testCase.StaticPublicKeyServer = holdStaticPublicKeyIut;
        }
    }
}