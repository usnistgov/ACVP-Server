using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.FFX;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes.Ffx;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_FF.Tests
{
    /// <summary>
    /// Note this algo (at the time of writing this comment) does not have vectors to test against.
    /// These tests are to just make sure the algorithm can get through a run of encrypt/decrypt without hitting an exception
    /// like substringing bits that aren't there, invalid lengths passed into block cipher, etc.
    /// </summary>
    [TestFixture, FastCryptoTest]
    public class Ff3_1BlockCipherTests
    {
        private Ff3_1BlockCipher _subject;

        [OneTimeSetUp]
        public void Setup()
        {
            var engineFactory = new BlockCipherEngineFactory();
            var modeFactory = new ModeBlockCipherFactory();

            _subject = new Ff3_1BlockCipher(
                engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes),
                modeFactory,
                new AesFfInternals(engineFactory, modeFactory));
        }

        private static IEnumerable<object> _testData = new List<object>()
        {
            new object[]
            {
                // label
                "test1 all zero tweak",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18"), 
                // tweak
                new BitString("00 00 00 00 00 00 00"),
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
                "test2 tweak with first 28 bits as non 0, ",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18"), 
                // tweak
                new BitString("FF FF FF F0 00 00 00"),
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
                "test3 - should be compatible with ff3",
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
            },
            new object[]
            {
                // label
                "test4 - testing from another implementation",
                // payload
                new NumeralString("3 1 9 1 5 5"), 
                // tweak
                new BitString("3F 09 6D E3 5B FA 31"),
                // key
                new BitString("33 9B B5 B1 F2 D4 4B AA BF 87 CA 1B 73 80 CD C8"),
                // radix
                10,
                // expectedResult
                new NumeralString("4 5 5 4 3 2")
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
                new BitString("C8 2E 75 AE 46 F6 64"), 
                // key
                new BitString("01B6932FF610BC056CCF223F5BB10C92"),
            },
            new object[]
            {
                // label
                "test 2 all zero tweak",
                // alphabet
                "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
                // word / payload
                "asd87f68as7df687asd6f8a7s6f",
                // tweak
                new BitString("00 00 00 00 00 00 00"), 
                // key
                new BitString("01B6932FF610BC056CCF223F5BB10C92"),
            },
            new object[]
            {
                // label
                "test 3",
                // alphabet
                "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
                // word / payload
                "s5d76fa5s7fd65a",
                // tweak
                new BitString("FF FF FF F0 00 00 00"), 
                // key
                new BitString("01B6932FF610BC056CCF223F5BB10C92"),
            },
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
