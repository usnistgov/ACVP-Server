﻿using System;
using System.IO;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Builders;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.DRBG.v1_0.Parsers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NLog;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.DRBG.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        //private readonly IOracle _subject = new OracleBuilder().Build().GetAwaiter().GetResult();

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            LoggingHelper.ConfigureLogging("FireHose", "DRBG");
        }

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\drbg\");
        }

        [Test]
        [TestCase("aes")]
        [TestCase("tdes")]
        [TestCase("hash")]
        [TestCase("hmac")]
        [Obsolete("Move these down the stack, they don't really do anything here")]
        public void ShouldRunThroughAllTestFilesAndValidate(string folderName)
        {
            var path = Path.Combine(_testPath, folderName);
            if (!Directory.Exists(path))
            {
                Assert.Fail("Test File Directory does not exist");
            }

            LegacyResponseFileParser parser = new LegacyResponseFileParser();
            var parsedFiles = parser.Parse(path);
            if (!parsedFiles.Success)
            {
                Assert.Fail("Failed parsing test files");
            }

            if (parsedFiles.ParsedObject.TestGroups.Count == 0)
            {
                Assert.Fail("No TestGroups were parsed.");
            }

            int count = 0;
            int testPasses = 0;
            int fails = 0;

            var testVector = parsedFiles.ParsedObject;

            if (testVector.TestGroups.Count == 0)
            {
                Assert.Fail("No TestGroups were parsed.");
            }

            foreach (var iTestGroup in testVector.TestGroups)
            {

                var testGroup = iTestGroup;
                foreach (var iTestCase in testGroup.Tests)
                {
                    count++;

                    var testCase = iTestCase;

                    var expectedReturnBits = testCase.ReturnedBits.GetDeepCopy();
                    //var generator = _subject.GetCaseGenerator(testGroup);
                    //var result = generator.Generate(testGroup, testCase);
                    //var resultingTestCase = result.TestCase;

                    //if (!result.Success)
                    //{
                    //    fails++;
                    //    continue;
                    //}

                    //if (expectedReturnBits.ToHex() != resultingTestCase.ReturnedBits.ToHex())
                    //{
                    //    continue;
                    //}

                    //Assert.AreEqual(expectedReturnBits.ToHex(), resultingTestCase.ReturnedBits.ToHex(), $"Failed on count {count} expected CT {expectedReturnBits.ToHex()}, got {resultingTestCase.ReturnedBits.ToHex()}");
                    testPasses++;
                }

            }

            if (fails > 0)
                Assert.Fail("Unexpected failures were encountered.");

            Assert.That(testPasses > 0, Is.True, "No tests were run");
            //Assert.Fail($"Passes {testPasses}, fails {fails}, count {count}.");
        }

        private static Logger ThisLogger => LogManager.GetLogger("FireHose");
    }
}
