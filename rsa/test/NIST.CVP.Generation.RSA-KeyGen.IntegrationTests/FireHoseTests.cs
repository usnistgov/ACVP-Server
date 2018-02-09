using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.IO;
using NIST.CVP.Generation.RSA_KeyGen.Parsers;
using System.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Crypto.RSA2.Enums;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Crypto.RSA2.PrimeGenerators;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_KeyGen.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LegacyParserFiles\");
        }

        [Test]
        [TestCase("3.2")]
        [TestCase("3.4")]
        [TestCase("3.5")]
        [TestCase("3.6")]
        public void ShouldRunThroughAllTestFilesAndValidate(string keyGenMode)
        {
            if (!Directory.Exists(_testPath))
            {
                Assert.Fail("Test File Directory does not exist");
            }

            var folderPath = new DirectoryInfo(Path.Combine(_testPath, keyGenMode));
            var parser = new LegacyResponseFileParser();
            var shaFactory = new ShaFactory();

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
                    testGroup.PrimeGenMode = EnumHelpers.GetEnumFromEnumDescription<PrimeGenModes>("b." + keyGenMode);

                    if(testGroup.Tests.Count == 0)
                    {
                        Assert.Fail("No TestCases parsed");
                    }

                    // Let's not run all of them because that'll take too long
                    // Pick 2 from each group and run those 
                    //var shuffledTests = testGroup.Tests.OrderBy(a => Guid.NewGuid()).ToList().GetRange(0, 2);
                    var shuffledTests = testGroup.Tests;
                    foreach (var iTestCase in shuffledTests)
                    {
                        var testCase = (TestCase)iTestCase;

                        var algo = new KeyBuilder(new PrimeGeneratorFactory());
                        
                        var entropyProvider = new TestableEntropyProvider();
                        entropyProvider.AddEntropy(testCase.XP1);
                        entropyProvider.AddEntropy(testCase.XP2);
                        entropyProvider.AddEntropy(testCase.XQ1);
                        entropyProvider.AddEntropy(testCase.XQ2);
                        if (testCase.XP != null)
                        {
                            entropyProvider.AddEntropy(testCase.XP.ToPositiveBigInteger());
                            entropyProvider.AddEntropy(testCase.XQ.ToPositiveBigInteger());
                        }

                        // TODO not all groups need a SHA but the factory wants it to be there
                        ISha sha = null;
                        if (testGroup.HashAlg != null)
                        {
                            sha = shaFactory.GetShaInstance(testGroup.HashAlg);
                        }

                        var result = algo
                            .WithPrimeGenMode(testGroup.PrimeGenMode)
                            .WithBitlens(testCase.Bitlens)
                            .WithHashFunction(sha)
                            .WithPrimeTestMode(testGroup.PrimeTest)
                            .WithEntropyProvider(entropyProvider)
                            .WithNlen(testGroup.Modulo)
                            .WithPublicExponent(testCase.Key.PubKey.E)
                            .WithSeed(testCase.Seed)
                            .WithKeyComposer(new RsaKeyComposer())
                            .Build();

                        if (!result.Success)
                        {
                            Assert.Fail($"Could not generate TestCase: {testCase.TestCaseId}, with error: {result.ErrorMessage}");
                        }

                        if(!CompareKeys(testCase.Key, result.Key))
                        {
                            Assert.Fail($"Failed KeyPair comparison on TestCase: {testCase.TestCaseId}");
                        }
                    }
                }
            }
        }

        [Test]
        [TestCase(2048, PrimeTestModes.C2)]
        [TestCase(2048, PrimeTestModes.C3)]
        [TestCase(3072, PrimeTestModes.C2)]
        [TestCase(3072, PrimeTestModes.C3)]
        public void ShouldRunThroughKatsAndValidate(int modulo, PrimeTestModes primeTest)
        {
            var group = new TestGroup
            {
                PrimeGenMode = PrimeGenModes.B33,
                Modulo = modulo,
                PrimeTest = primeTest,
                KeyFormat = PrivateKeyModes.Standard
            };

            var count = 1;
            var katGen = new TestCaseGeneratorKat(group, new KeyComposerFactory());
            for (int i = 0; i < katGen.NumberOfTestCasesToGenerate; i++)
            {
                var kat = katGen.Generate(group, false);

                if (!kat.Success)
                {
                    Assert.Fail("Can't find KATs");
                }

                var katTestCase = (TestCase) kat.TestCase;
                katTestCase.TestCaseId = count++;

                var algo = new KeyBuilder(new PrimeGeneratorFactory());
                var entropyProvider = new TestableEntropyProvider();
                entropyProvider.AddEntropy(new BitString(katTestCase.Key.PrivKey.P, group.Modulo / 2));
                entropyProvider.AddEntropy(new BitString(katTestCase.Key.PrivKey.Q, group.Modulo / 2));

                var result = algo
                    .WithPrimeGenMode(group.PrimeGenMode)
                    .WithPrimeTestMode(primeTest)
                    .WithPublicExponent(katTestCase.Key.PubKey.E)
                    .WithEntropyProvider(entropyProvider)
                    .WithNlen(group.Modulo)
                    .WithKeyComposer(new RsaKeyComposer())
                    .Build();
                
                if (katTestCase.FailureTest == result.Success)
                {
                    Assert.Fail($"Failed KAT: {katTestCase.TestCaseId}");
                }
            }
        }

        private bool CompareKeys(KeyPair expected, KeyPair actual)
        {
            var pub1 = expected.PubKey;
            var pub2 = actual.PubKey;

            if (!(expected.PrivKey is PrivateKey priv1) || !(actual.PrivKey is PrivateKey priv2)) return false;

            if (pub1.E != pub2.E || pub1.N != pub2.N) return false;
            if (priv1.P != priv2.P || priv1.Q != priv2.Q || priv1.D != priv2.D) return false;
            
            return true;
        }
    }
}
