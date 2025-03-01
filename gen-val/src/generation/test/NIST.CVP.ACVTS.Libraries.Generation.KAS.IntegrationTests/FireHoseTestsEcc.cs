﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Builders;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC.Parsers;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using TestCase = NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC.TestCase;
using TestGroup = NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC.TestGroup;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTestsEcc
    {
        string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\kas\ECC\");
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

            var oracle = await new OracleBuilder().Build();

            var tasks = new Dictionary<Task<KasResult>, (TestGroup testGroup, TestCase testCase)>();

            foreach (var testGroup in testVector.TestGroups)
            {
                SwitchTestGroupIutServerInformation(testGroup);

                foreach (var testCase in testGroup.Tests)
                {
                    var testCaseResolver = new DeferredTestCaseResolver(oracle);

                    SwitchTestCaseIutServerInformation(testCase);

                    tasks.Add(testCaseResolver.CompleteDeferredCryptoAsync(testGroup, testCase, testCase), (testGroup, testCase));
                }
            }

            await Task.WhenAll(tasks.Keys);

            foreach (var keyValuePair in tasks)
            {
                var testGroup = keyValuePair.Value.testGroup;
                var testCase = keyValuePair.Value.testCase;
                var result = keyValuePair.Key.Result;

                if (testCase.TestPassed.Value)
                {
                    Assert.That(
                        result.Tag.ToHex()
, Is.EqualTo(testGroup.KasMode == KasMode.NoKdfNoKc ? testCase.HashZ.ToHex() : testCase.Tag.ToHex()));
                    passes++;
                }
                else
                {
                    Assert.That(
                        result.Tag.ToHex()
, Is.Not.EqualTo(testGroup.KasMode == KasMode.NoKdfNoKc ? testCase.HashZ.ToHex() : testCase.Tag.ToHex()));
                    passes++;
                    expectedFails++;
                }
            }

            Assert.That(passes > 0, Is.True, nameof(passes));
            Assert.That(expectedFails > 0, Is.True, nameof(expectedFails));
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

            var holdEphemeralIut = testCase.EphemeralKeyIut;
            testCase.EphemeralKeyIut = testCase.EphemeralKeyServer;
            testCase.EphemeralKeyServer = holdEphemeralIut;

            var holdStaticIut = testCase.StaticKeyIut;
            testCase.StaticKeyIut = testCase.StaticKeyServer;
            testCase.StaticKeyServer = holdStaticIut;
        }
    }
}
