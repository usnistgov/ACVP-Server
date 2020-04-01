using System;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Oracle.Builders;
using NIST.CVP.Crypto.RSA.Keys;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Generation.RSA.v1_0.KeyGen;
using NIST.CVP.Generation.RSA.v1_0.KeyGen.Parsers;

namespace NIST.CVP.Generation.RSA_KeyGen.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class FireHoseTests
    {
        string _testPath;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\LegacyCavsFiles\rsa\");
        }

        [Test]
        [TestCase("3.2", "provable")]
        [TestCase("3.4", "provableWithProvableAux")]
        [TestCase("3.5", "probableWithProvableAux")]
        [TestCase("3.6", "probableWithProbableAux")]
        public void ShouldRunThroughAllTestFilesAndValidate(string keyGenMode, string primeMode)
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

                foreach(var testGroup in testVector.TestGroups)
                {
                    testGroup.PrimeGenMode = RsaKeyGenAttributeConverter.GetSectionFromPrimeGen(EnumHelpers.GetEnumFromEnumDescription<PrimeGenModes>(primeMode));

                    if(testGroup.Tests.Count == 0)
                    {
                        Assert.Fail("No TestCases parsed");
                    }

                    // Let's not run all of them because that'll take too long
                    // Pick 2 from each group and run those 
                    var shuffledTests = testGroup.Tests.OrderBy(a => Guid.NewGuid()).ToList().GetRange(0, 2);
                    //var shuffledTests = testGroup.Tests;
                    foreach (var testCase in shuffledTests)
                    {
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
                            .WithPrimeGenMode(RsaKeyGenAttributeConverter.GetPrimeGenFromSection(testGroup.PrimeGenMode))
                            .WithBitlens(testCase.Bitlens)
                            .WithHashFunction(sha)
                            .WithPrimeTestMode(RsaKeyGenAttributeConverter.GetPrimeTestFromSection(testGroup.PrimeTest))
                            .WithEntropyProvider(entropyProvider)
                            .WithNlen(testGroup.Modulo)
                            .WithPublicExponent(testCase.Key.PubKey.E)
                            .WithSeed(testCase.Seed)
                            .WithKeyComposer(new RsaKeyComposer())
                            .WithStandard(Fips186Standard.Fips186_4)
                            .Build();

                        if (!result.Success)
                        {
                            Assert.Fail($"Could not generate TestCase: {testCase.TestCaseId}, with error: {result.ErrorMessage}");
                        }

                        var compareResult = CompareKeys(testCase.Key, result.Key);
                        if(!compareResult.result)
                        {
                            Assert.Fail($"Failed KeyPair comparison on TestCase: {testCase.TestCaseId}, Reason: {compareResult.reason}");
                        }
                    }
                }
            }
        }

        [Test]
        [TestCase(2048, PrimeTestModes.TwoPow100ErrorBound)]
        [TestCase(2048, PrimeTestModes.TwoPowSecurityStrengthErrorBound)]
        [TestCase(3072, PrimeTestModes.TwoPow100ErrorBound)]
        [TestCase(3072, PrimeTestModes.TwoPowSecurityStrengthErrorBound)]
        public async Task ShouldRunThroughKatsAndValidate(int modulo, PrimeTestModes primeTest)
        {
            var group = new TestGroup
            {
                PrimeGenMode = RsaKeyGenAttributeConverter.GetSectionFromPrimeGen(PrimeGenModes.RandomProbablePrimes),
                Modulo = modulo,
                PrimeTest = RsaKeyGenAttributeConverter.GetSectionFromPrimeTest(primeTest),
                KeyFormat = PrivateKeyModes.Standard
            };

            var count = 1;
            var katGen = new TestCaseGeneratorKat(group, new OracleBuilder().Build().GetAwaiter().GetResult());
            for (int i = 0; i < katGen.NumberOfTestCasesToGenerate; i++)
            {
                var kat = await katGen.GenerateAsync(group, false);

                if (!kat.Success)
                {
                    Assert.Fail("Can't find KATs");
                }

                var katTestCase = kat.TestCase;
                katTestCase.TestCaseId = count++;

                var entropyProvider = new TestableEntropyProvider();
                entropyProvider.AddEntropy(new BitString(katTestCase.Key.PrivKey.P, group.Modulo / 2));
                entropyProvider.AddEntropy(new BitString(katTestCase.Key.PrivKey.Q, group.Modulo / 2));

                var katPrimeGen = new RandomProbablePrimeGenerator(entropyProvider, primeTest);
                var primeResult = katPrimeGen.GeneratePrimesKat(new PrimeGeneratorParameters
                {
                    Modulus = group.Modulo,
                    PublicE = katTestCase.Key.PubKey.E
                });

                if (katTestCase.TestPassed != primeResult.Success)
                {
                    Assert.Fail($"Failed KAT: {katTestCase.TestCaseId}");
                }
            }
        }

        private (bool result, string reason) CompareKeys(KeyPair expected, KeyPair actual)
        {
            var pub1 = expected.PubKey;
            var pub2 = actual.PubKey;

            if (!(expected.PrivKey is PrivateKey priv1) || !(actual.PrivKey is PrivateKey priv2))
            {
                return (false, "Key type mismatch");
            }

            if (pub1.E != pub2.E)
            {
                return (false, "E mismatch");
            }

            if (pub1.N != pub2.N)
            {
                return (false, "N mismatch");
            }

            if (priv1.P != priv2.P)
            {
                return (false, "P mismatch");
            }

            if (priv1.Q != priv2.Q)
            {
                return (false, "Q mismatch");
            }

            if (priv1.D != priv2.D)
            {
                return (false, "D mismatch");
            }

            return (true, "");
        }
    }
}
