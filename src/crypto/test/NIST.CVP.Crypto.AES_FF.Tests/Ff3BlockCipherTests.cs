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