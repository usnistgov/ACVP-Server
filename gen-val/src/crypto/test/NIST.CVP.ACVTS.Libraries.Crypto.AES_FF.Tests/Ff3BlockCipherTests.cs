using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.Ffx;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes.Ffx;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_FF.Tests
{
    [TestFixture, FastCryptoTest]
    public class Ff3BlockCipherTests
    {
        private Ff3BlockCipher _subject;

        [OneTimeSetUp]
        public void Setup()
        {
            var engineFactory = new BlockCipherEngineFactory();
            var modeFactory = new ModeBlockCipherFactory();

            _subject = new Ff3BlockCipher(
                engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes),
                modeFactory,
                new AesFfInternals(engineFactory, modeFactory));
        }

        /// <summary>
        /// Samples from https://csrc.nist.gov/CSRC/media/Projects/Cryptographic-Standards-and-Guidelines/documents/examples/FF3samples.pdf
        /// </summary>
        private static IEnumerable<object> _testData = new List<object>()
        {
            new object[]
            {
                // label
                "test1",
                // payload
                new NumeralString("8 9 0 1 2 1 2 3 4 5 6 7 8 9 0 0 0 0"), 
                // tweak
                new BitString("D8 E7 92 0A FA 33 0A 73"),
                // key
                new BitString("EF 43 59 D8 D5 80 AA 4F 7F 03 6D 6F 04 FC 6A 94"),
                // radix
                10,
                // expectedResult
                new NumeralString("7 5 0 9 1 8 8 1 4 0 5 8 6 5 4 6 0 7")
            },
            new object[]
            {
                // label
                "test2",
                // payload
                new NumeralString("8 9 0 1 2 1 2 3 4 5 6 7 8 9 0 0 0 0"), 
                // tweak
                new BitString("9A 76 8A 92 F6 0E 12 D8"),
                // key
                new BitString("EF 43 59 D8 D5 80 AA 4F 7F 03 6D 6F 04 FC 6A 94"),
                // radix
                10,
                // expectedResult
                new NumeralString("0 1 8 9 8 9 8 3 9 1 8 9 3 9 5 3 8 4")
            },
            new object[]
            {
                // label
                "test3",
                // payload
                new NumeralString("8 9 0 1 2 1 2 3 4 5 6 7 8 9 0 0 0 0 0 0 7 8 9 0 0 0 0 0 0"), 
                // tweak
                new BitString("D8 E7 92 0A FA 33 0A 73"),
                // key
                new BitString("EF 43 59 D8 D5 80 AA 4F 7F 03 6D 6F 04 FC 6A 94"),
                // radix
                10,
                // expectedResult
                new NumeralString("4 8 5 9 8 3 6 7 1 6 2 2 5 2 5 6 9 6 2 9 3 9 7 4 1 6 2 2 6")
            },
            new object[]
            {
                // label
                "test4",
                // payload
                new NumeralString("8 9 0 1 2 1 2 3 4 5 6 7 8 9 0 0 0 0 0 0 7 8 9 0 0 0 0 0 0"), 
                // tweak
                new BitString("00 00 00 00 00 00 00 00"),
                // key
                new BitString("EF 43 59 D8 D5 80 AA 4F 7F 03 6D 6F 04 FC 6A 94"),
                // radix
                10,
                // expectedResult
                new NumeralString("3 4 6 9 5 2 2 4 8 2 1 7 3 4 5 3 5 1 2 2 6 1 3 7 0 1 4 3 4")
            },
            new object[]
            {
                // label
                "test5",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18"), 
                // tweak
                new BitString("9A 76 8A 92 F6 0E 12 D8"),
                // key
                new BitString("EF 43 59 D8 D5 80 AA 4F 7F 03 6D 6F 04 FC 6A 94"),
                // radix
                26,
                // expectedResult
                new NumeralString("16 2 25 20 4 0 18 9 9 2 15 23 2 0 12 19 10 20 11")
            },
            new object[]
            {
                // label
                "test6",
                // payload
                new NumeralString("8 9 0 1 2 1 2 3 4 5 6 7 8 9 0 0 0 0"), 
                // tweak
                new BitString("D8 E7 92 0A FA 33 0A 73"),
                // key
                new BitString("EF 43 59 D8 D5 80 AA 4F 7F 03 6D 6F 04 FC 6A 94 2B 7E 15 16 28 AE D2 A6"),
                // radix
                10,
                // expectedResult
                new NumeralString("6 4 6 9 6 5 3 9 3 8 7 5 0 2 8 7 5 5")
            },
            new object[]
            {
                // label
                "test7",
                // payload
                new NumeralString("8 9 0 1 2 1 2 3 4 5 6 7 8 9 0 0 0 0"), 
                // tweak
                new BitString("9A 76 8A 92 F6 0E 12 D8"),
                // key
                new BitString("EF 43 59 D8 D5 80 AA 4F 7F 03 6D 6F 04 FC 6A 94 2B 7E 15 16 28 AE D2 A6"),
                // radix
                10,
                // expectedResult
                new NumeralString("9 6 1 6 1 0 5 1 4 4 9 1 4 2 4 4 4 6")
            },
            new object[]
            {
                // label
                "test8",
                // payload
                new NumeralString("8 9 0 1 2 1 2 3 4 5 6 7 8 9 0 0 0 0 0 0 7 8 9 0 0 0 0 0 0"), 
                // tweak
                new BitString("D8 E7 92 0A FA 33 0A 73"),
                // key
                new BitString("EF 43 59 D8 D5 80 AA 4F 7F 03 6D 6F 04 FC 6A 94 2B 7E 15 16 28 AE D2 A6"),
                // radix
                10,
                // expectedResult
                new NumeralString("5 3 0 4 8 8 8 4 0 6 5 3 5 0 2 0 4 5 4 1 7 8 6 3 8 0 8 0 7")
            },
            new object[]
            {
                // label
                "test9",
                // payload
                new NumeralString("8 9 0 1 2 1 2 3 4 5 6 7 8 9 0 0 0 0 0 0 7 8 9 0 0 0 0 0 0"), 
                // tweak
                new BitString("00 00 00 00 00 00 00 00"),
                // key
                new BitString("EF 43 59 D8 D5 80 AA 4F 7F 03 6D 6F 04 FC 6A 94 2B 7E 15 16 28 AE D2 A6"),
                // radix
                10,
                // expectedResult
                new NumeralString("9 8 0 8 3 8 0 2 6 7 8 8 2 0 3 8 9 2 9 5 0 4 1 4 8 3 5 1 2")
            },
            new object[]
            {
                // label
                "test10",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18"), 
                // tweak
                new BitString("9A 76 8A 92 F6 0E 12 D8"),
                // key
                new BitString(" EF 43 59 D8 D5 80 AA 4F 7F 03 6D 6F 04 FC 6A 94 2B 7E 15 16 28 AE D2 A6"),
                // radix
                26,
                // expectedResult
                new NumeralString("18 0 18 17 14 2 19 15 19 7 10 9 24 25 15 9 25 8 8")
            },
            new object[]
            {
                // label
                "test10",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18"), 
                // tweak
                new BitString("00 00 00 00 00 00 00 00"),
                // key
                new BitString("2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C EF 43 59 D8 D5 80 AA 4F 7F 03 6D 6F 04 FC 6A 94"),
                // radix
                36,
                // expectedResult
                new NumeralString("8 27 24 31 21 27 35 25 12 19 26 23 10 8 24 2 0 33 9")
            },
            new object[]
            {
                // label
                "test11 - should be compatible with ff3-1",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18"), 
                // tweak
                new BitString("FF FF FF F0 00 00 00 00"),
                // key
                new BitString("2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C EF 43 59 D8 D5 80 AA 4F 7F 03 6D 6F 04 FC 6A 94"),
                // radix
                36,
                // expectedResult
                new NumeralString("9 31 11 25 5 29 28 16 1 0 19 7 14 4 27 13 6 17 14")
            },
            new object[]
            {
                // label
                "test12 - should be compatible with ff3-1",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 0 0 0 0 1 5 8 2 10 1 1 1 1 1"), 
                // tweak
                new BitString("FA F5 7F A0 00 00 00 00"),
                // key
                new BitString("2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C EF 43 59 D8 D5 80 AA 4F 7F 03 6D 6F 04 FC 6A 94"),
                // radix
                36,
                // expectedResult
                new NumeralString("5 16 25 5 13 12 9 13 28 10 11 14 23 6 12 16 10 2 13 9 3 1 30 29 17 28 30 15 33 11 2 32 31")
            }
        };

        [Test]
        [TestCaseSource(nameof(_testData))]
        public void ShouldEncryptCorrectly(string testLabel, NumeralString payload, BitString tweak, BitString key, int radix, NumeralString cipherText)
        {
            var result = _subject.ProcessPayload(new FfxModeBlockCipherParameters()
            {
                Direction = BlockCipherDirections.Encrypt,
                Iv = tweak,
                Key = key,
                Payload = NumeralString.ToBitString(payload),
                Radix = radix
            });

            Assert.That(NumeralString.ToNumeralString(result.Result).ToString(), Is.EqualTo(cipherText.ToString()));
        }

        [Test]
        [TestCaseSource(nameof(_testData))]
        public void ShouldDecryptCorrectly(string testLabel, NumeralString payload, BitString tweak, BitString key, int radix, NumeralString cipherText)
        {
            var result = _subject.ProcessPayload(new FfxModeBlockCipherParameters()
            {
                Direction = BlockCipherDirections.Decrypt,
                Iv = tweak,
                Key = key,
                Payload = NumeralString.ToBitString(cipherText),
                Radix = radix
            });

            Assert.That(NumeralString.ToNumeralString(result.Result).ToString(), Is.EqualTo(payload.ToString()));
        }

        public static IEnumerable<object> _encryptDecryptTest = new List<object>()
        {
            new object[]
            {
                // label
                "test 1",
                // alphabet
                "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
                // word / payload
                "lUnLs5KrrKXwJu6axnE2obK6",
                // tweak
                new BitString("C82E75AE46F6648FE5BF5B83C195"), 
                // key
                new BitString("01B6932FF610BC056CCF223F5BB10C92"),
            }
        };

        [Test]
        [TestCaseSource(nameof(_encryptDecryptTest))]
        public void ShouldEncryptDecryptBackToSameValue(string label, string alphabet, string word, BitString tweak, BitString key)
        {
            var wordNumeralString = NumeralString.ToNumeralString(word, alphabet);

            var encryptResult = _subject.ProcessPayload(new FfxModeBlockCipherParameters()
            {
                Direction = BlockCipherDirections.Encrypt,
                Iv = tweak,
                Key = key,
                Payload = NumeralString.ToBitString(wordNumeralString),
                Radix = alphabet.Length
            });

            var decryptResult = _subject.ProcessPayload(new FfxModeBlockCipherParameters()
            {
                Direction = BlockCipherDirections.Decrypt,
                Iv = tweak,
                Key = key,
                Payload = encryptResult.Result,
                Radix = alphabet.Length
            });

            Assert.That(
                NumeralString.ToAlphabetString(alphabet, alphabet.Length, NumeralString.ToNumeralString(decryptResult.Result))
, Is.EqualTo(NumeralString.ToAlphabetString(alphabet, alphabet.Length, wordNumeralString)));
        }
    }
}
