using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using NIST.CVP.Generation.RSA_KeyGen.Parsers;
using System.Text;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Crypto.RSA;
using System.Linq;
using NIST.CVP.Math.Entropy;
using System.Numerics;
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
            var genFactory = new PrimeGeneratorFactory();

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

                    if(testGroup.Tests.Count == 0)
                    {
                        Assert.Fail("No TestCases parsed");
                    }

                    // Let's not run all of them because that'll take too long
                    // Pick 2 from each group and run those 
                    var shuffledTests = testGroup.Tests.OrderBy(a => Guid.NewGuid()).ToList().GetRange(0, 2);
                    //var shuffledTests = testGroup.Tests;
                    foreach (var iTestCase in shuffledTests)
                    {
                        var testCase = (TestCase)iTestCase;

                        var algo = genFactory.GetPrimeGenerator(keyGenMode);

                        // Set properties
                        algo.SetBitlens(testCase.Bitlens);
                        algo.SetHashFunction(testGroup.HashAlg);
                        algo.SetPrimeTestMode(testGroup.PrimeTest);
                        algo.SetEntropyProviderType(EntropyProviderTypes.Testable);
                        algo.AddEntropy(testCase.XP1);
                        algo.AddEntropy(testCase.XP2);
                        algo.AddEntropy(testCase.XQ1);
                        algo.AddEntropy(testCase.XQ2);
                        if(testCase.XP != null)
                        {
                            algo.AddEntropy(testCase.XP.ToPositiveBigInteger());
                            algo.AddEntropy(testCase.XQ.ToPositiveBigInteger());
                        }

                        var result = algo.GeneratePrimes(testGroup.Modulo, testCase.Key.PubKey.E, testCase.Seed);
                        if (!result.Success)
                        {
                            Assert.Fail($"Could not generate TestCase: {testCase.TestCaseId}");
                        }

                        var resultKey = new KeyPair(result.P, result.Q, testCase.Key.PubKey.E);
                        if(!CompareKeys(testCase.Key, resultKey))
                        {
                            Assert.Fail($"Failed KeyPair comparison on TestCase: {testCase.TestCaseId}");
                        }
                    }
                }
            }
        }

        [Test]
        [TestCase(2048, "C.2")]
        [TestCase(2048, "C.3")]
        [TestCase(3072, "C.2")]
        [TestCase(3072, "C.3")]
        public void ShouldRunThroughKATsAndValidate(int modulo, string primeTest)
        {
            var group = new TestGroup
            {
                Mode = KeyGenModes.B33,
                Modulo = modulo,
                PrimeTest = RSAEnumHelpers.StringToPrimeTestMode(primeTest)
            };

            var count = 1;
            var katGen = new KnownAnswerTestCaseGeneratorB33(group);
            for (int i = 0; i < katGen.NumberOfTestCasesToGenerate; i++)
            {
                var kat = katGen.Generate(group, false);

                if (!kat.Success)
                {
                    Assert.Fail("Can't find KATs");
                }

                var katTestCase = (TestCase) kat.TestCase;
                katTestCase.TestCaseId = count++;

                var algo = new RandomProbablePrimeGenerator(EntropyProviderTypes.Testable);
                algo.AddEntropy(new BitString(katTestCase.Key.PrivKey.P, group.Modulo / 2));
                algo.AddEntropy(new BitString(katTestCase.Key.PrivKey.Q, group.Modulo / 2));
                var result = algo.GeneratePrimes(group.Modulo, katTestCase.Key.PubKey.E, null);
                
                if (katTestCase.FailureTest == result.Success)
                {
                    Assert.Fail($"Failed KAT: {katTestCase.TestCaseId}");
                }
            }
        }

        private bool CompareKeys(KeyPair expected, KeyPair actual)
        {
            return
                expected.PubKey.E == actual.PubKey.E &&
                expected.PubKey.N == actual.PubKey.N &&
                expected.PrivKey.D == actual.PrivKey.D &&
                expected.PrivKey.P == actual.PrivKey.P &&
                expected.PrivKey.Q == actual.PrivKey.Q;
        }
    }
}
