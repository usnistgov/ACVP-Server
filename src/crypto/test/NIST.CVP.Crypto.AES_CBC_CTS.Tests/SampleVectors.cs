using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.CTS;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.CTS;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Collections.Generic;

namespace NIST.CVP.Crypto.AES_CBC_CTS.Tests
{
    [TestFixture, FastCryptoTest]
    public class SampleVectors
    {
        private readonly ICiphertextStealingTransformFactory _csTransform = new CiphertextStealingTransformFactory();

        private readonly AesEngine _engine = new AesEngine();
        private readonly CbcCtsBlockCipher _subjectCs1 = new CbcCtsBlockCipher(new AesEngine(), new CiphertextStealingMode1());
        private readonly CbcCtsBlockCipher _subjectCs2 = new CbcCtsBlockCipher(new AesEngine(), new CiphertextStealingMode2());
        private readonly CbcCtsBlockCipher _subjectCs3 = new CbcCtsBlockCipher(new AesEngine(), new CiphertextStealingMode3());

        /// <summary>
        /// Sample vectors from https://www.ietf.org/rfc/rfc3962.txt
        /// </summary>
        private static IEnumerable<object> _testData => new List<object>()
        {
            new object[]
            {
                // label
                "test1",
                // key
                new BitString("63 68 69 63 6b 65 6e 20 74 65 72 69 79 61 6b 69"), 
                // iv
                new BitString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"), 
                // pt
                new BitString("49 20 77 6f 75 6c 64 20 6c 69 6b 65 20 74 68 65 20"), 
                // ct
                new BitString("c6 35 35 68 f2 bf 8c b4 d8 a5 80 36 2d a7 ff 7f 97"),
                // next iv
                new BitString("c6 35 35 68 f2 bf 8c b4 d8 a5 80 36 2d a7 ff 7f")
            },
            new object[]
            {
                // label
                "test2",
                // key
                new BitString("63 68 69 63 6b 65 6e 20 74 65 72 69 79 61 6b 69"), 
                // iv
                new BitString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"), 
                // pt
                new BitString("49 20 77 6f 75 6c 64 20 6c 69 6b 65 20 74 68 65 20 47 65 6e 65 72 61 6c 20 47 61 75 27 73 20"), 
                // ct
                new BitString("fc 00 78 3e 0e fd b2 c1 d4 45 d4 c8 ef f7 ed 22 97 68 72 68 d6 ec cc c0 c0 7b 25 e2 5e cf e5"),
                // next iv
                new BitString("fc 00 78 3e 0e fd b2 c1 d4 45 d4 c8 ef f7 ed 22")
            },
            new object[]
            {
                // label
                "test3",
                // key
                new BitString("63 68 69 63 6b 65 6e 20 74 65 72 69 79 61 6b 69"), 
                // iv
                new BitString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"), 
                // pt
                new BitString("49 20 77 6f 75 6c 64 20 6c 69 6b 65 20 74 68 65 20 47 65 6e 65 72 61 6c 20 47 61 75 27 73 20 43"), 
                // ct
                new BitString("39 31 25 23 a7 86 62 d5 be 7f cb cc 98 eb f5 a8 97 68 72 68 d6 ec cc c0 c0 7b 25 e2 5e cf e5 84"),
                // next iv
                new BitString("39 31 25 23 a7 86 62 d5 be 7f cb cc 98 eb f5 a8")
            },
            new object[]
            {
                // label
                "test4",
                // key
                new BitString("63 68 69 63 6b 65 6e 20 74 65 72 69 79 61 6b 69"), 
                // iv
                new BitString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"), 
                // pt
                new BitString("49 20 77 6f 75 6c 64 20 6c 69 6b 65 20 74 68 65 20 47 65 6e 65 72 61 6c 20 47 61 75 27 73 20 43 68 69 63 6b 65 6e 2c 20 70 6c 65 61 73 65 2c"), 
                // ct
                new BitString("97 68 72 68 d6 ec cc c0 c0 7b 25 e2 5e cf e5 84 b3 ff fd 94 0c 16 a1 8c 1b 55 49 d2 f8 38 02 9e 39 31 25 23 a7 86 62 d5 be 7f cb cc 98 eb f5"),
                // next iv
                new BitString("b3 ff fd 94 0c 16 a1 8c 1b 55 49 d2 f8 38 02 9e")
            },
            new object[]
            {
                // label
                "test5",
                // key
                new BitString("63 68 69 63 6b 65 6e 20 74 65 72 69 79 61 6b 69"), 
                // iv
                new BitString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"), 
                // pt
                new BitString("49 20 77 6f 75 6c 64 20 6c 69 6b 65 20 74 68 65 20 47 65 6e 65 72 61 6c 20 47 61 75 27 73 20 43 68 69 63 6b 65 6e 2c 20 70 6c 65 61 73 65 2c 20"), 
                // ct
                new BitString("97 68 72 68 d6 ec cc c0 c0 7b 25 e2 5e cf e5 84 9d ad 8b bb 96 c4 cd c0 3b c1 03 e1 a1 94 bb d8 39 31 25 23 a7 86 62 d5 be 7f cb cc 98 eb f5 a8"),
                // next iv
                new BitString("9d ad 8b bb 96 c4 cd c0 3b c1 03 e1 a1 94 bb d8")
            },
            new object[]
            {
                // label
                "test6",
                // key
                new BitString("63 68 69 63 6b 65 6e 20 74 65 72 69 79 61 6b 69"), 
                // iv
                new BitString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"), 
                // pt
                new BitString("49 20 77 6f 75 6c 64 20 6c 69 6b 65 20 74 68 65 20 47 65 6e 65 72 61 6c 20 47 61 75 27 73 20 43 68 69 63 6b 65 6e 2c 20 70 6c 65 61 73 65 2c 20 61 6e 64 20 77 6f 6e 74 6f 6e 20 73 6f 75 70 2e"), 
                // ct
                new BitString("97 68 72 68 d6 ec cc c0 c0 7b 25 e2 5e cf e5 84 39 31 25 23 a7 86 62 d5 be 7f cb cc 98 eb f5 a8 48 07 ef e8 36 ee 89 a5 26 73 0d bc 2f 7b c8 40 9d ad 8b bb 96 c4 cd c0 3b c1 03 e1 a1 94 bb d8"),
                // next iv
                new BitString("48 07 ef e8 36 ee 89 a5 26 73 0d bc 2f 7b c8 40")
            }
        };

        [Test]
        [TestCaseSource(nameof(_testData))]
        public void EncryptTestsCs1(string label, BitString key, BitString iv, BitString pt, BitString ct, BitString nextIv)
        {
            // transform the cs3 ct to cs3 again (to cancel the sample vector CS3)
            var ctBytes = BitString.PadToModulus(ct, _engine.BlockSizeBits).ToBytes();
            var numberOfBlocks = GetNumberOfBlocks(pt.BitLength);
            var cs3Transformer = _csTransform.Get(CiphertextStealingMode.CS3);
            cs3Transformer.Transform(ctBytes, _engine, numberOfBlocks, pt.BitLength);

            // apply a cs1 transform
            var cs1Transformer = _csTransform.Get(CiphertextStealingMode.CS1);
            cs1Transformer.Transform(ctBytes, _engine, numberOfBlocks, pt.BitLength);
            ct = new BitString(ctBytes).GetMostSignificantBits(pt.BitLength);

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                iv,
                key,
                pt
            );
            var result = _subjectCs1.ProcessPayload(param);

            Assert.AreEqual(ct.ToHex(), result.Result.ToHex(), nameof(ct));
            Assert.AreEqual(nextIv.ToHex(), param.Iv.ToHex(), nameof(nextIv));
        }

        [Test]
        [TestCaseSource(nameof(_testData))]
        public void DecryptTestsCs1(string label, BitString key, BitString iv, BitString pt, BitString ct, BitString nextIv)
        {
            // transform the cs3 ct to cs3 again (to cancel the sample vector CS3)
            var ctBytes = BitString.PadToModulus(ct, _engine.BlockSizeBits).ToBytes();
            var numberOfBlocks = GetNumberOfBlocks(pt.BitLength);
            var cs3Transformer = _csTransform.Get(CiphertextStealingMode.CS3);
            cs3Transformer.Transform(ctBytes, _engine, numberOfBlocks, pt.BitLength);

            // apply a cs1 transform
            var cs1Transformer = _csTransform.Get(CiphertextStealingMode.CS1);
            cs1Transformer.Transform(ctBytes, _engine, numberOfBlocks, pt.BitLength);
            ct = new BitString(ctBytes).GetMostSignificantBits(pt.BitLength);

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                iv,
                key,
                ct
            );
            var result = _subjectCs1.ProcessPayload(param);

            Assert.AreEqual(pt.ToHex(), result.Result.ToHex(), nameof(pt));
            Assert.AreEqual(nextIv.ToHex(), param.Iv.ToHex(), nameof(nextIv));
        }

        [Test]
        [TestCaseSource(nameof(_testData))]
        public void EncryptTestsCs2(string label, BitString key, BitString iv, BitString pt, BitString ct, BitString nextIv)
        {
            // transform the cs3 ct to cs3 again (to cancel the sample vector CS3)
            var ctBytes = BitString.PadToModulus(ct, _engine.BlockSizeBits).ToBytes();
            var numberOfBlocks = GetNumberOfBlocks(pt.BitLength);
            var cs3Transformer = _csTransform.Get(CiphertextStealingMode.CS3);
            cs3Transformer.Transform(ctBytes, _engine, numberOfBlocks, pt.BitLength);

            // apply a cs1 transform
            var cs2Transformer = _csTransform.Get(CiphertextStealingMode.CS2);
            cs2Transformer.Transform(ctBytes, _engine, numberOfBlocks, pt.BitLength);
            ct = new BitString(ctBytes).GetMostSignificantBits(pt.BitLength);

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                iv,
                key,
                pt
            );
            var result = _subjectCs2.ProcessPayload(param);

            Assert.AreEqual(ct.ToHex(), result.Result.ToHex(), nameof(ct));
            Assert.AreEqual(nextIv.ToHex(), param.Iv.ToHex(), nameof(nextIv));
        }

        [Test]
        [TestCaseSource(nameof(_testData))]
        public void DecryptTestsCs2(string label, BitString key, BitString iv, BitString pt, BitString ct, BitString nextIv)
        {
            // transform the cs3 ct to cs3 again (to cancel the sample vector CS3)
            var ctBytes = BitString.PadToModulus(ct, _engine.BlockSizeBits).ToBytes();
            var numberOfBlocks = GetNumberOfBlocks(pt.BitLength);
            var cs3Transformer = _csTransform.Get(CiphertextStealingMode.CS3);
            cs3Transformer.Transform(ctBytes, _engine, numberOfBlocks, pt.BitLength);

            // apply a cs1 transform
            var cs2Transformer = _csTransform.Get(CiphertextStealingMode.CS2);
            cs2Transformer.Transform(ctBytes, _engine, numberOfBlocks, pt.BitLength);
            ct = new BitString(ctBytes).GetMostSignificantBits(pt.BitLength);

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                iv,
                key,
                ct
            );
            var result = _subjectCs2.ProcessPayload(param);

            Assert.AreEqual(pt.ToHex(), result.Result.ToHex(), nameof(pt));
            Assert.AreEqual(nextIv.ToHex(), param.Iv.ToHex(), nameof(nextIv));
        }

        [Test]
        [TestCaseSource(nameof(_testData))]
        public void EncryptTestsCs3(string label, BitString key, BitString iv, BitString pt, BitString ct, BitString nextIv)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                iv,
                key,
                pt
            );
            var result = _subjectCs3.ProcessPayload(param);

            Assert.AreEqual(ct.ToHex(), result.Result.ToHex(), nameof(ct));
            Assert.AreEqual(nextIv.ToHex(), param.Iv.ToHex(), nameof(nextIv));
        }

        [Test]
        [TestCaseSource(nameof(_testData))]
        public void DecryptTestsCs3(string label, BitString key, BitString iv, BitString pt, BitString ct, BitString nextIv)
        {
            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                iv,
                key,
                ct
            );
            var result = _subjectCs3.ProcessPayload(param);

            Assert.AreEqual(pt.ToHex(), result.Result.ToHex(), nameof(pt));
            Assert.AreEqual(nextIv.ToHex(), param.Iv.ToHex(), nameof(nextIv));
        }

        protected int GetNumberOfBlocks(int outputLengthBits)
        {
            var numberOfBlocks = outputLengthBits / 128;

            // In cases of partial block, add an additional block for processing.
            if (outputLengthBits % 128 != 0)
            {
                numberOfBlocks++;
            }

            return numberOfBlocks;
        }
    }
}
