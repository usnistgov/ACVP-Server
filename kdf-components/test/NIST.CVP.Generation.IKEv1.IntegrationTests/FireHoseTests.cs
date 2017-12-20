using System.IO;
using NIST.CVP.Crypto.IKEv1;
using NIST.CVP.Generation.IKEv1.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.IKEv1.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\IKEv1\");
        }

        [Test]
        public void ShouldRunThroughAllTestFilesAndValidate()
        {
            if (!Directory.Exists(_testPath))

            {
                Assert.Fail("Test File Directory does not exist");
            }

            var testDir = new DirectoryInfo(_testPath);
            var parser = new LegacyResponseFileParser();
            var algoFactory = new IkeV1Factory();

            var count = 0;
            foreach (var testFilePath in testDir.EnumerateFiles())
            {
                var parseResult = parser.Parse(testFilePath.FullName);
                if (!parseResult.Success)
                {
                    Assert.Fail($"Could not parse: {testFilePath.FullName}");
                }
                var testVector = parseResult.ParsedObject;

                if (testVector.TestGroups.Count == 0)
                {
                    Assert.Fail("No TestGroups were parsed.");
                }

                foreach (var iTestGroup in testVector.TestGroups)
                {
                    var testGroup = (TestGroup)iTestGroup;
                    var algo = algoFactory.GetIkeV1Instance(testGroup.AuthenticationMethod, testGroup.HashAlg);
                    
                    foreach (var iTestCase in testGroup.Tests)
                    {
                        count++;

                        var testCase = (TestCase)iTestCase;
                        var result = algo.GenerateIke(
                            testCase.NInit,
                            testCase.NResp,
                            testCase.Gxy,
                            testCase.CkyInit,
                            testCase.CkyResp,
                            testCase.PreSharedKey
                        );

                        Assert.IsTrue(result.Success, result.ErrorMessage);
                        Assert.AreEqual(testCase.SKeyId.ToHex(), result.SKeyId.ToHex(), $"Failed on SKeyId {testCase.SKeyId.ToHex()}, got {result.SKeyId.ToHex()}");
                        Assert.AreEqual(testCase.SKeyIdA.ToHex(), result.SKeyIdA.ToHex(), $"Failed on SKeyIdA {testCase.SKeyIdA.ToHex()}, got {result.SKeyIdA.ToHex()}");
                        Assert.AreEqual(testCase.SKeyIdD.ToHex(), result.SKeyIdD.ToHex(), $"Failed on SKeyIdD {testCase.SKeyIdD.ToHex()}, got {result.SKeyIdD.ToHex()}");
                        Assert.AreEqual(testCase.SKeyIdE.ToHex(), result.SKeyIdE.ToHex(), $"Failed on SKeyIdE {testCase.SKeyIdE.ToHex()}, got {result.SKeyIdE.ToHex()}");
                    }
                }
            }
        }
    }
}
