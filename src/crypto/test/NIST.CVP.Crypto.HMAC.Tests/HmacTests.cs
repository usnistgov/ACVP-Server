using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.HMAC.Tests
{
    [TestFixture, FastCryptoTest]
    public class HmacTests
    {
        private Hmac _subject;
        private ShaFactory _shaFactory;

        [SetUp]
        public void Setup()
        {
            _shaFactory = new ShaFactory();
        }

        private static object[] _getTestData = new object[]
        {
            new object[]
            {
                "Sample #1",
                ModeValues.SHA1,
                DigestSizes.d160,
                64, 0x00,
                new BitString("4f4ca3d5d68ba7cc0a1208c9c61e9c5da0403c0a"),
                160
            },
            new object[]
            {
                "Sample #3",
                ModeValues.SHA1,
                DigestSizes.d160,
                100, 0x50,
                new BitString("bcf41eab8bb2d802f3d05caf7cb092ecf8d1a3aa"),
                160
            },
            new object[]
            {
                "Sample #2", // 2 comes after 3...
                ModeValues.SHA2,
                DigestSizes.d224,
                64, 0x00,
                new BitString("bb29 0be6 ac0a 23ba bd01 2cfd 5d1f"),
                112
            },
            new object[]
            {
                "Sample #4",
                ModeValues.SHA2,
                DigestSizes.d224,
                100, 0x50,
                new BitString("4f60 6161 bd98 9693 cdb2 bb01 6ed5"),
                112
            },
            new object[]
            {
                "Sample #5",
                ModeValues.SHA2,
                DigestSizes.d256,
                64, 0x00,
                new BitString("c47a ac2d 5444 1f5b 270c dbab 2a78 6e1e"),
                128
            },
            new object[]
            {
                "Sample #6",
                ModeValues.SHA2,
                DigestSizes.d256,
                100, 0x50,
                new BitString("585a 46be 7bb9 98cc 54f3 0e1c 8521 53ac"),
                128
            },
            new object[]
            {
                "Sample #7",
                ModeValues.SHA2,
                DigestSizes.d384,
                64, 0x00,
                new BitString("803a 3dff 7b75 7e6e 068e 12c2 7467 1ad3 66fc 5b77 4a8e 2b10"),
                192
            },
            new object[]
            {
                "Sample #8",
                ModeValues.SHA2,
                DigestSizes.d384,
                100, 0x50,
                new BitString("447d bebe 46da c15e 24d1 1c95 9728 cf20 0ced 27ea 7846 0520"),
                192
            },
            new object[]
            {
                "Sample #9",
                ModeValues.SHA2,
                DigestSizes.d512,
                64, 0x00,
                new BitString("c717 b6f7 7442 4963 077c 9750 7192 cf1e d632 d394 c645 eca1 983b 3bac 9912 8331"),
                256
            },
            new object[]
            {
                "Sample #10",
                ModeValues.SHA2,
                DigestSizes.d512,
                100, 0x50,
                new BitString("5aa7 b876 65f1 0dcd 2048 1f4b f628 b7d6 d041 dde5 bd3b 0d88 c8f7 5e86 9479 4c55"),
                256
            },
            new object[]
            {
                "Sample message for keylen=blocklen",
                ModeValues.SHA3,
                DigestSizes.d224,
                144, 0x00,
                new BitString("d8b7 33bc f66c 644a 1232 3d56 4e24 dcf3 fc75 f231 f3b6 7968 3591 00c7"),
                224
            }
        };

        [Test]
        [TestCaseSource(nameof(_getTestData))]
        public void ShouldHmacCorrectly(string label, ModeValues mode, DigestSizes digestSize, int keyByteSize, int additionToIndexInKey, BitString expectedHmac, int macLength)
        {
            var sha = _shaFactory.GetShaInstance(new HashFunction(mode, digestSize));
            
            _subject = new Hmac(sha);

            var key = GenKey(keyByteSize, additionToIndexInKey);
            var message = GetBitStringFromString(label);
            
            var result = _subject.Generate(key, message, macLength);
            
            Assert.AreEqual(expectedHmac.ToHex(), result.Mac.ToHex());
        }

        private static BitString GenKey(int numberOfBytes, int additionToIndex)
        {
            byte[] keyBytes = new byte[numberOfBytes];

            for (int i = 0; i < numberOfBytes; i++)
            {
                keyBytes[i] = (byte)(i + additionToIndex);
            }

            return new BitString(keyBytes);
        }

        private static BitString GetBitStringFromString(string message)
        {
            return new BitString(Encoding.ASCII.GetBytes(message));
        }
    }
}
