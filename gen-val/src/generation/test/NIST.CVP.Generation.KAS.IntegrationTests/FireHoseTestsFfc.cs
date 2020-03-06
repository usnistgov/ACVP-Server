using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Oracle.Builders;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.v1_0.FFC;
using NIST.CVP.Generation.KAS.v1_0.FFC.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NLog;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTestsFfc
    {
        string _testPath;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        [SetUp]
        public void Setup()
        {
            LoggingHelper.ConfigureLogging($"{this.GetType()}_{DateTime.Now:yyyy-MM-dd}.log", "firehose", LogLevel.Info);
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\kas\FFC\");
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

            var oracle = new OracleBuilder().Build();
            
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

            var holdEphemeralIut = testCase.EphemeralKeyIut;
            testCase.EphemeralKeyIut = testCase.EphemeralKeyServer;
            testCase.EphemeralKeyServer = holdEphemeralIut;
            
            var holdStaticIut = testCase.StaticKeyIut;
            testCase.StaticKeyIut = testCase.StaticKeyServer;
            testCase.StaticKeyServer = holdStaticIut;
        }
    }
}