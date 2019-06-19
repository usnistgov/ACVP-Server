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
    [TestFixture, FastCryptoTest]
    public class Ff1BlockCipherTests
    {
        private Ff1BlockCipher _subject;

        [OneTimeSetUp]
        public void Setup()
        {
            var engineFactory = new BlockCipherEngineFactory();
            var modeFactory = new ModeBlockCipherFactory();

            _subject = new Ff1BlockCipher(
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
                new NumeralString("0 1 2 3 4 5 6 7 8 9"), 
                // tweak
                new BitString(0),
                // key
                new BitString("2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C"),
                // radix
                10,
                // expectedResult
                new NumeralString("2 4 3 3 4 7 7 4 8 4")
            },
            new object[]
            {
                // label
                "test2",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9"), 
                // tweak
                new BitString("39 38 37 36 35 34 33 32 31 30 "),
                // key
                new BitString("2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C"),
                // radix
                10,
                // expectedResult
                new NumeralString("6 1 2 4 2 0 0 7 7 3")
            },
            new object[]
            {
                // label
                "test3",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18"), 
                // tweak
                new BitString("37 37 37 37 70 71 72 73 37 37 37"),
                // key
                new BitString("2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C"),
                // radix
                36,
                // expectedResult
                new NumeralString("10 9 29 31 4 0 22 21 21 9 20 13 30 5 0 9 14 30 22")
            },
            new object[]
            {
                // label
                "test4",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9"), 
                // tweak
                new BitString(0),
                // key
                new BitString("2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C EF 43 59 D8 D5 80 AA 4F"),
                // radix
                10,
                // expectedResult
                new NumeralString("2 8 3 0 6 6 8 1 3 2")
            },
            new object[]
            {
                // label
                "test5",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9"), 
                // tweak
                new BitString("39 38 37 36 35 34 33 32 31 30"),
                // key
                new BitString(" 2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C EF 43 59 D8 D5 80 AA 4F"),
                // radix
                10,
                // expectedResult
                new NumeralString("2 4 9 6 6 5 5 5 4 9")
            },
            new object[]
            {
                // label
                "test6",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18"), 
                // tweak
                new BitString("37 37 37 37 70 71 72 73 37 37 37"),
                // key
                new BitString("2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C EF 43 59 D8 D5 80 AA 4F"),
                // radix
                36,
                // expectedResult
                new NumeralString("33 11 19 3 20 31 3 5 19 27 10 32 33 31 3 2 34 28 27")
            },
            new object[]
            {
                // label
                "test7",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9"), 
                // tweak
                new BitString(0),
                // key
                new BitString("2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C EF 43 59 D8 D5 80 AA 4F 7F 03 6D 6F 04 FC 6A 94"),
                // radix
                10,
                // expectedResult
                new NumeralString("6 6 5 7 6 6 7 0 0 9")
            },
            new object[]
            {
                // label
                "test8",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9"), 
                // tweak
                new BitString("39 38 37 36 35 34 33 32 31 30"),
                // key
                new BitString("2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C EF 43 59 D8 D5 80 AA 4F 7F 03 6D 6F 04 FC 6A 94"),
                // radix
                10,
                // expectedResult
                new NumeralString("1 0 0 1 6 2 3 4 6 3")
            },
            new object[]
            {
                // label
                "test9",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18"), 
                // tweak
                new BitString("37 37 37 37 70 71 72 73 37 37 37"),
                // key
                new BitString("2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C EF 43 59 D8 D5 80 AA 4F 7F 03 6D 6F 04 FC 6A 94"),
                // radix
                36,
                // expectedResult
                new NumeralString("33 28 8 10 0 10 35 17 2 10 31 34 10 21 34 35 30 32 13")
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
                Radix = (short)radix
            });

            Assert.AreEqual(cipherText.ToString(), NumeralString.ToNumeralString(result.Result).ToString());
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
                Radix = (short)radix
            });

            Assert.AreEqual(payload.ToString(), NumeralString.ToNumeralString(result.Result).ToString());
        }
    }
}