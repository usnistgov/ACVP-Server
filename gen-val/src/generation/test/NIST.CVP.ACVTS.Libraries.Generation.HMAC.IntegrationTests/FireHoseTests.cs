using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Generation.HMAC.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.HMAC.v1_0.Parsers;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.HMAC.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\hmac\");
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
            var algoFactory = new HmacFactory(new NativeShaFactory());

            int count = 0;
            int passes = 0;
            int fails = 0;

            if (testVector.TestGroups.Count == 0)
            {
                Assert.Fail("No TestGroups were parsed.");
            }

            foreach (var iTestGroup in testVector.TestGroups)
            {

                var testGroup = (TestGroup)iTestGroup;
                foreach (var iTestCase in testGroup.Tests)
                {
                    count++;

                    var testCase = (TestCase)iTestCase;

                    var algo = algoFactory
                        .GetHmacInstance(
                            new HashFunction(
                                testGroup.ShaMode,
                                testGroup.ShaDigestSize
                            )
                        );

                    var result = algo.Generate(
                        testCase.Key,
                        testCase.Message,
                        testGroup.MacLength
                    );

                    if (!result.Success)
                    {
                        fails++;
                        continue;
                    }

                    Assert.That(
                        result.Mac.ToHex(), Is.EqualTo(testCase.Mac.ToHex()),
                        $"Failed on count {count}, hmac: {testGroup.ShaMode}-{testGroup.ShaDigestSize}, message: {testCase.Message.ToHex()}, key: {testCase.Key.ToHex()} expected CT {testCase.Mac.ToHex()}, got {result.Mac.ToHex()}"
                    );

                    passes++;
                }

            }

            Assert.That(passes > 0, Is.True, nameof(passes));
            Assert.That(fails == 0, Is.True, "fails");
        }
    }
}
