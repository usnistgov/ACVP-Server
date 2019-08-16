using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Ffx;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.BlockModes.Ffx;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Collections.Generic;

namespace NIST.CVP.Crypto.AES_FF.Tests
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
                "test1",
                // payload
                new NumeralString("8 9 0 1 2 1 2 3 4 5 6 7 8 9 0 0 0 0"),
                // tweak
                new BitString("D8 E7 92 0A FA 33 0A"),
                // key
                new BitString("EF 43 59 D8 D5 80 AA 4F 7F 03 6D 6F 04 FC 6A 94"),
                // radix
                10,
                // expectedResult
                new NumeralString("7 5 0 9 1 8 8 1 4 0 5 8 6 5 4 6 0 7")
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

            Assert.IsNotNull(result.Result);
            Assert.IsNull(result.ErrorMessage);
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

            Assert.IsNotNull(result.Result);
            Assert.IsNull(result.ErrorMessage);
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
            
            Assert.AreEqual(
                NumeralString.ToAlphabetString(alphabet, alphabet.Length, wordNumeralString), 
                NumeralString.ToAlphabetString(alphabet, alphabet.Length, NumeralString.ToNumeralString(decryptResult.Result))
            );
        }
    }
}