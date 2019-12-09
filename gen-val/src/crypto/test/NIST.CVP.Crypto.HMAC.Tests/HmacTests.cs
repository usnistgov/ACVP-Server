using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Text;

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

        [Test]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, 112, "a", 1000000, "00112233 44556677 8899AABB CCDDEEFF", "63859486E22F8C2E90E5F5BF510E732543414F6FB731B0A8E9807249")]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, 112, "a", 0, "00112233 44556677 8899AABB CCDDEEFF", "63C3DB8305A361388FC52CF4567521303ADA68C31A6FA0638113D594")]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, 112, "12345678901234567890123456789012345678901234567890123456789012345678901234567890", 0, "00112233 44556677 8899AABB CCDDEEFF", "39FC2867E4979919B1EE5A03D1B15571D69BABA4FED9891D9F97FBF1")]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, 112, "message digest", 0, "00112233 44556677 8899AABB CCDDEEFF", "CD676B859E48A06EC59AE4BF8F341997D9E9FDBA4922ABC983D787A1")]

        //[TestCase(ModeValues.SHA3, DigestSizes.d224, 112, "a", 1000000, "00112233 44556677 8899AABB CCDDEEFF", "AFC02C05E75FA91D41A7AADC87B43860BBEF8EB3EE27835BAF298152")]
        //[TestCase(ModeValues.SHA3, DigestSizes.d224, 112, "a", 1000000, "01234567 89ABCDEF FEDCBA98 76543210", "7F36E86CEE6E11B478982AACDB41E2572114C500D8C0C0E766121A91")]
        [TestCase(ModeValues.SHA3, DigestSizes.d224, 112, "a", 0, "01234567 89ABCDEF FEDCBA98 76543210", "1E9F288FAC84AF5D08CB1701FB7C00D08083F33CA37EC47AC06DB9A1")]

        //[TestCase(ModeValues.SHA3, DigestSizes.d256, 128, "a", 1000000, "00112233 44556677 8899AABB CCDDEEFF", "D2A844AD7EDA1A800784481F3405165042E22C494DF73341A970432F67A8399C")]
        //[TestCase(ModeValues.SHA3, DigestSizes.d256, 128, "a", 1000000, "01234567 89ABCDEF FEDCBA98 76543210", "0784168A18EC91375B284A781A1CFD562ACB66A6F5E632E60B027A3AE2D7D7B5")]
        [TestCase(ModeValues.SHA3, DigestSizes.d256, 128, "a", 0, "01234567 89ABCDEF FEDCBA98 76543210", "B6685B21408A6BA1187E6D317F83D437BDAA35F0EDD5A08077D6FBF4610B5B3D")]

        //[TestCase(ModeValues.SHA3, DigestSizes.d384, 192, "a", 1000000, "00112233 44556677 8899AABB CCDDEEFF 01234567 89ABCDEF FEDCBA98 76543210", "33521C62BA708C3010D5421565FBEA3EA241F63E4107F847EC0F9E782AAEE731CA6A98F32BBF8BAB6AA38E1F6E06D9DB")]
        //[TestCase(ModeValues.SHA3, DigestSizes.d384, 192, "a", 1000000, "01234567 89ABCDEF FEDCBA98 76543210 00112233 44556677 8899AABB CCDDEEFF", "44E853FA5777C4ABA1BEE0D81551818824EC39E55DB81DBA1F3C55CD8AD2D293A6B56568749A25B251624EB9C1CA9539")]
        [TestCase(ModeValues.SHA3, DigestSizes.d384, 192, "a", 0, "01234567 89ABCDEF FEDCBA98 76543210 00112233 44556677 8899AABB CCDDEEFF", "1701C53ECCE7E68AE76CBF6E1BE55482D5A47E108F7738175D9B2B2E978326525F80D6C79BFD92EA6C4EFFDD5089A693")]

        //[TestCase(ModeValues.SHA3, DigestSizes.d512, 256, "a", 1000000, "00112233 44556677 8899AABB CCDDEEFF 01234567 89ABCDEF FEDCBA98 76543210", "1CD8AF4273F332626A4737BC2F0BDD7ECF358B4D0A444B9ED824B0642510964CF2D7B5190CAC4F4644E80DCB21A36F6B3C64E422EA01D733063D683B63594767")]
        //[TestCase(ModeValues.SHA3, DigestSizes.d512, 256, "a", 1000000, "01234567 89ABCDEF FEDCBA98 76543210 00112233 44556677 8899AABB CCDDEEFF", "F9F41DEAF916A7386952B81CDEF207AE01AF98A5E879C5C6A373E4B6BCB70BD1CC3BD911583704B1EA7E768099B793C82CBFC2FBD95922FC2C895C19921F1E21")]
        [TestCase(ModeValues.SHA3, DigestSizes.d512, 256, "a", 0, "01234567 89ABCDEF FEDCBA98 76543210 00112233 44556677 8899AABB CCDDEEFF", "A830CAC0CF0391D28B0FD90D754E9158A47EE7844B7E5F05211F4AA8985A62F92851A91A281B9AF04FA583E64D9B2C0F6B97FE4593CCE47A632147AECEC5C0ED")]
        public void IsoIec9797Tests(ModeValues mode, DigestSizes digestSize, int macLength, string ascii, int count, string keyHex, string expectedHex)
        {
            var sha = _shaFactory.GetShaInstance(new HashFunction(mode, digestSize));

            _subject = new Hmac(sha);
            var key = new BitString(keyHex);
            BitString message;

            if (count == 0)
            {
                message = new BitString(Encoding.ASCII.GetBytes(ascii));
            }
            else
            {
                message = new BitString(Encoding.ASCII.GetBytes(ascii), count * 8);
            }

            var result = _subject.Generate(key, message, macLength);

            var expected = new BitString(expectedHex, macLength);

            Assert.AreEqual(expected.ToHex(), result.Mac.ToHex());
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
