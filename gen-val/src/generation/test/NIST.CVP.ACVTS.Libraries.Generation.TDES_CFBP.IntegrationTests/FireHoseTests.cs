using System;
using System.IO;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CFBP.v1_0.Parsers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_CFBP.IntegrationTests
{
    public class FireHoseTests
    {

        private TdesMonteCarloFactory _mctFactory;
        private readonly BlockCipherEngineFactory _engineFactory = new BlockCipherEngineFactory();
        private readonly ModeBlockCipherFactory _modeFactory = new ModeBlockCipherFactory();

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mctFactory = new TdesMonteCarloFactory(_engineFactory, _modeFactory);
        }

        [Test]
        [TestCase(BlockCipherModesOfOperation.CfbpBit)]
        [TestCase(BlockCipherModesOfOperation.CfbpByte)]
        [TestCase(BlockCipherModesOfOperation.CfbpBlock)]
        public void ShouldParseAndRunCAVSFiles(BlockCipherModesOfOperation mode)
        {
            var mct = _mctFactory.GetInstance(mode);
            var engine = _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes);
            var algo = _modeFactory.GetStandardCipher(engine, mode);

            var testPath = string.Empty;
            switch (mode)
            {
                case BlockCipherModesOfOperation.CfbpBit:
                    testPath = Utilities.GetConsistentTestingStartPath(
                        GetType(),
                        $@"..\..\LegacyCavsFiles\tdes-cfb\acvp-tdes-cfbp1-1.0"
                    );
                    break;
                case BlockCipherModesOfOperation.CfbpByte:
                    testPath = Utilities.GetConsistentTestingStartPath(
                        GetType(),
                        $@"..\..\LegacyCavsFiles\tdes-cfb\acvp-tdes-cfbp8-1.0"
                    );
                    break;
                case BlockCipherModesOfOperation.CfbpBlock:
                    testPath = Utilities.GetConsistentTestingStartPath(
                        GetType(),
                        $@"..\..\LegacyCavsFiles\tdes-cfb\acvp-tdes-cfbp64-1.0"
                    );
                    break;
                default:
                    throw new ArgumentException(nameof(mode));
            }

            if (!Directory.Exists(testPath))
            {
                Assert.Fail("Test File Directory does not exist");
            }

            LegacyResponseFileParser parser = new LegacyResponseFileParser();
            var parsedTestVectorSet = parser.Parse(testPath);

            if (!parsedTestVectorSet.Success)
            {
                Assert.Fail("Failed parsing test files");
            }

            if (parsedTestVectorSet.ParsedObject.TestGroups.Count == 0)
            {
                Assert.Fail("No TestGroups were parsed.");
            }

            int count = 0;
            int passes = 0;
            int fails = 0;
            bool mctTestHit = false;
            bool nonMctTestHit = false;
            foreach (var testGroup in parsedTestVectorSet.ParsedObject.TestGroups)
            {
                foreach (var testCase in testGroup.Tests)
                {
                    count++;

                    if (testGroup.TestType.ToLower() == "mct")
                    {
                        mctTestHit = true;
                        var firstResult = testCase.ResultsArray.First();
                        if (testGroup.Function.ToLower() == "encrypt")
                        {
                            var param = new ModeBlockCipherParameters(
                                BlockCipherDirections.Encrypt,
                                firstResult.IV.GetDeepCopy(),
                                firstResult.Keys.GetDeepCopy(),
                                firstResult.PlainText.GetDeepCopy()
                            );

                            var result = mct.ProcessMonteCarloTest(param);

                            Assert.IsTrue(testCase.ResultsArray.Count > 0, $"{nameof(testCase)} MCT encrypt count should be gt 0");
                            for (int i = 0; i < testCase.ResultsArray.Count; i++)
                            {
                                Assert.AreEqual(testCase.ResultsArray[i].IV.ToHex(), result.Response[i].IV.ToHex(), $"IV mismatch on index {i}");
                                Assert.AreEqual(testCase.ResultsArray[i].Keys.ToHex(), result.Response[i].Keys.ToHex(), $"Key mismatch on index {i}");
                                Assert.AreEqual(testCase.ResultsArray[i].PlainText.ToHex(), result.Response[i].PlainText.ToHex(), $"PlainText mismatch on index {i}");
                                Assert.AreEqual(testCase.ResultsArray[i].CipherText.ToHex(), result.Response[i].CipherText.ToHex(), $"CipherText mismatch on index {i}");
                            }
                            continue;
                        }
                        if (testGroup.Function.ToLower() == "decrypt")
                        {
                            var param = new ModeBlockCipherParameters(
                                BlockCipherDirections.Decrypt,
                                firstResult.IV.GetDeepCopy(),
                                firstResult.Keys.GetDeepCopy(),
                                firstResult.CipherText.GetDeepCopy()
                            );

                            var result = mct.ProcessMonteCarloTest(param);

                            Assert.IsTrue(testCase.ResultsArray.Count > 0, $"{nameof(testCase)} MCT decrypt count should be gt 0");
                            Assert.IsTrue(testCase.ResultsArray.Count == result.Response.Count, "Result and response arrays must be of the same size.");
                            for (int i = 0; i < testCase.ResultsArray.Count; i++)
                            {
                                Assert.AreEqual(testCase.ResultsArray[i].IV.ToHex(), result.Response[i].IV.ToHex(), $"IV mismatch on index {i}");
                                Assert.AreEqual(testCase.ResultsArray[i].Keys.ToHex(), result.Response[i].Keys.ToHex(), $"Key mismatch on index {i}");
                                Assert.AreEqual(testCase.ResultsArray[i].PlainText.ToHex(), result.Response[i].PlainText.ToHex(), $"PlainText mismatch on index {i}");
                                Assert.AreEqual(testCase.ResultsArray[i].CipherText.ToHex(), result.Response[i].CipherText.ToHex(), $"CipherText mismatch on index {i}");
                            }
                            continue;
                        }
                    }

                    else
                    {
                        nonMctTestHit = true;

                        if (testGroup.Function.ToLower() == "encrypt")
                        {
                            if (testGroup.TestType.ToLower() == "inversepermutation")
                            {
                                var param = new ModeBlockCipherParameters(
                                    BlockCipherDirections.Encrypt,
                                    testCase.IV.GetDeepCopy(),
                                    testCase.Keys.GetDeepCopy(),
                                    testCase.PlainText1
                                        .ConcatenateBits(testCase.PlainText2)
                                        .ConcatenateBits(testCase.PlainText3)
                                );

                                var result = algo.ProcessPayload(param);
                                var ct = testCase.CipherText1
                                    .ConcatenateBits(testCase.CipherText2)
                                    .ConcatenateBits(testCase.CipherText3);

                                if (ct.ToHex() == result.Result.ToHex())
                                {
                                    passes++;
                                }
                                else
                                {
                                    fails++;
                                }

                                Assert.AreEqual(ct.ToHex(), result.Result.ToHex(), $"Failed on count {count} expected CT {ct}, got { result.Result}");
                                continue;
                            }
                            else if (testGroup.TestType.ToLower() == "multiblockmessage")
                            {
                                var param = new ModeBlockCipherParameters(
                                    BlockCipherDirections.Encrypt,
                                    testCase.IV.GetDeepCopy(),
                                    testCase.Keys.GetDeepCopy(),
                                    testCase.PlainText
                                );

                                var result = algo.ProcessPayload(param);

                                if (testCase.CipherText.ToString() == result.Result.ToString())
                                {
                                    passes++;
                                }
                                else
                                {
                                    fails++;
                                }

                                Assert.AreEqual(testCase.CipherText.ToHex(), result.Result.ToHex(), $"Failed on count {count} expected CT {testCase.CipherText}, got { result.Result}");
                                continue;
                            }
                            else if (testGroup.TestType.ToLower() == "permutation" ||
                                     testGroup.TestType.ToLower() == "substitutiontable" ||
                                     testGroup.TestType.ToLower() == "variablekey" ||
                                     testGroup.TestType.ToLower() == "variabletext")
                            {
                                var param = new ModeBlockCipherParameters(
                                    BlockCipherDirections.Encrypt,
                                    testCase.IV.GetDeepCopy(),
                                    testCase.Keys.GetDeepCopy(),
                                    testCase.PlainText
                                        // include "aux values" for these tests
                                        .ConcatenateBits(BitString.Zeroes(testCase.PlainText.BitLength * 2))
                                );

                                var result = algo.ProcessPayload(param);
                                var ct = testCase.CipherText1
                                    .ConcatenateBits(testCase.CipherText2)
                                    .ConcatenateBits(testCase.CipherText3);

                                if (ct.ToHex() == result.Result.ToHex())
                                {
                                    passes++;
                                }
                                else
                                {
                                    fails++;
                                }

                                Assert.AreEqual(ct.ToHex(), result.Result.ToHex(), $"Failed on count {count} expected CT {ct}, got { result.Result}");
                                continue;
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }

                        if (testGroup.Function.ToLower() == "decrypt")
                        {
                            if (testGroup.TestType.ToLower() == "inversepermutation")
                            {
                                var param = new ModeBlockCipherParameters(
                                    BlockCipherDirections.Decrypt,
                                    testCase.IV.GetDeepCopy(),
                                    testCase.Keys.GetDeepCopy(),
                                    testCase.CipherText1
                                        .ConcatenateBits(testCase.CipherText2)
                                        .ConcatenateBits(testCase.CipherText3)
                                );

                                var result = algo.ProcessPayload(param);
                                var pt = testCase.PlainText1
                                    .ConcatenateBits(testCase.PlainText2)
                                    .ConcatenateBits(testCase.PlainText3);

                                if (pt.ToHex() == result.Result.ToHex())
                                {
                                    passes++;
                                }
                                else
                                {
                                    fails++;
                                }

                                Assert.AreEqual(pt.ToHex(), result.Result.ToHex(), $"Failed on count {count} expected PT {pt}, got { result.Result}");
                                continue;
                            }
                            else if (testGroup.TestType.ToLower() == "multiblockmessage")
                            {
                                var param = new ModeBlockCipherParameters(
                                    BlockCipherDirections.Decrypt,
                                    testCase.IV.GetDeepCopy(),
                                    testCase.Keys.GetDeepCopy(),
                                    testCase.CipherText
                                );

                                var result = algo.ProcessPayload(param);

                                if (testCase.PlainText.ToString() == result.Result.ToString())
                                {
                                    passes++;
                                }
                                else
                                {
                                    fails++;
                                }

                                Assert.AreEqual(testCase.PlainText.ToHex(), result.Result.ToHex(), $"Failed on count {count} expected CT {testCase.PlainText}, got {result.Result}");
                                continue;
                            }
                            else if (testGroup.TestType.ToLower() == "permutation" ||
                                     testGroup.TestType.ToLower() == "substitutiontable" ||
                                     testGroup.TestType.ToLower() == "variablekey" ||
                                     testGroup.TestType.ToLower() == "variabletext")
                            {
                                var param = new ModeBlockCipherParameters(
                                    BlockCipherDirections.Decrypt,
                                    testCase.IV.GetDeepCopy(),
                                    testCase.Keys.GetDeepCopy(),
                                    testCase.CipherText
                                        // include "aux values" for these tests
                                        .ConcatenateBits(BitString.Zeroes(testCase.CipherText.BitLength * 2))
                                );

                                var result = algo.ProcessPayload(param);
                                var pt = testCase.PlainText1
                                    .ConcatenateBits(testCase.PlainText2)
                                    .ConcatenateBits(testCase.PlainText3);

                                if (pt.ToHex() == result.Result.ToHex())
                                {
                                    passes++;
                                }
                                else
                                {
                                    fails++;
                                }

                                Assert.AreEqual(pt.ToHex(), result.Result.ToHex(), $"Failed on count {count} expected PT {pt}, got { result.Result}");
                                continue;
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                    }

                    Assert.Fail($"{testGroup.Function} did not meet expected function values");
                }
            }

            Assert.IsTrue(mctTestHit, "No MCT tests were run");
            Assert.IsTrue(nonMctTestHit, "No normal (non MCT) tests were run");
            // Assert.Fail($"Passes {passes}, fails {fails}, count {count}");
        }
    }
}
