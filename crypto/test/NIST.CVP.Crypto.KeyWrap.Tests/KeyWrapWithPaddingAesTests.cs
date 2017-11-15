using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KeyWrap.Tests
{
    [TestFixture, FastCryptoTest]
    public class KeyWrapWithPaddingAesTests
    {
        private Mock<IAES_ECB> _algo;
        private KeyWrapWithPaddingAes _subject;

        [SetUp]
        public void Setup()
        {
            _algo = new Mock<IAES_ECB>();
            _algo
                .Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), false))
                .Returns(new EncryptionResult(new BitString(128)));
            _algo
                .Setup(s => s.BlockDecrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), false))
                .Returns(new DecryptionResult(new BitString(128)));
            _subject = new KeyWrapWithPaddingAes(_algo.Object);
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenKeySizeInvalid()
        {
            Assert.Throws(
                typeof(ArgumentException),
                () => _subject.Encrypt(new BitString(264), new BitString(128), false)
            );
        }

        [Test]
        [TestCase(false, true,
            "000102030405060708090A0B0C0D0E0F",
            "00112233445566778899AABBCCDD",
            "ea0bfde8af063e8918E811A05D2A4C23A367B45315716B5B",
            TestName = "KWP with AES - case 7")]
        [TestCase(false, true,
            "000102030405060708090A0B0C0D0E0F1011121314151617",
            "00112233445566778899AABBCCDD",
            "900484950f84eb6eD74CE81DCDACA26E72BB29D4A6F7AC74",
            TestName = "KWP with AES - case 8")]
        [TestCase(false, true,
            "000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F",
            "00112233445566778899AABBCCDD",
            "c68168173f141e6d5767611574A941259090DA78D7DF9DF7",
            TestName = "KWP with AES - case 9")]
        [TestCase(false, true,
            "000102030405060708090A0B0C0D0E0F1011121314151617",
            "00112233445566778899AABBCCDDEEFF0001020304",
            "a402348f1956db968FDDFD8976420F9DDEB7183CF16B91B0AEB74CAB196C343E",
            TestName = "KWP with AES - case 10")]
        [TestCase(false, true,
            "000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F",
            "00112233445566778899AABBCCDDEEFF0001020304",
            "308d49692b5f8cf638D54BB4B985633504237329964C76EBB3F669870A708DBC",
            TestName = "KWP with AES - case 11")]
        [TestCase(false, true,
            "000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F",
            "00112233445566778899AABBCCDDEEFF000102030405060708090A0B",
            "0942747db07032a3F04CDB2E7DE1CBA038F92BC355393AE9A0E4AE8C901912AC3D3AF0F16D240607",
            TestName = "KWP with AES - case 12")]
        [TestCase(false, true,
            "000102030405060708090A0B0C0D0E0F",
            "0011223344",
            "9E53E571ED4669A51A4B8724788F8C80",
            TestName = "KWP with AES - case 14")]
        [TestCase(false, true,
            "000102030405060708090A0B0C0D0E0F",
            "00112233445566",
            "1B1D4BC2A90B1FA389412B3D40FECB20",
            TestName = "KWP with AES - example 2.1")]
        [TestCase(false, true,
            "d7bf05f8fae98282c196db56d2b7630a",
            "33ec4d883f4bf884a170852e35b4c949cddab3d2efb187dcfadd5a49180cfeb3a661ea26d61ad19fdfd1fd69d646588815a11aa70b3b80e5dc978649fc87c023a0ded9c202563655222b08e20f2357819f0c18e6815452d72a85c5592405e5272d56772b10dc8c82496ce2f7d8ddc6def7ce93e495d1c8107ac40a707d061bdb909a5c346a8694644c9b87e67d7d1a920cdb679430c0dad9e0af6d7772e04796da64e7a795dbce5fa966bcd03a7e1a9c42d10b2b9075319e43b4ac1f3d3a64ed26792d7a4cbbce3dbc5620243146ac625b2b7cec2b376b5136a3b3b933f0ea538c78819c0f10a8a144313a9ac74bb8fa8eaa6faafb92c49d948005eb49f83d463a55872a0f812c6704eabb86cfd8ab1821e1cfe68f68f3e8a1b9977b028d40cec35fc6a2e020ee6b5c7a1dd671f34267b8695fb5c9fb9362d1060edadbfee50897e16b214ddb84a42c2af275a45c7810a4ac96723d70c864114a0d78460c30cc078d68ab66f17e342c4e448ac5aff86eb4d2226b452a3acab01d12409b9e6f80cfe13a50aa67a65c25e5f03cdbeac48e3daa68c2bdd72267ef3a6af9b367d04c2f0506fba3d8e310dbe055ce8394470c4a28e03485bf759440d2f9556e7d383836d0d1e9a3f669c06ceb17b45c6547619647a7f6de2463d3dfcdf1581cfe3c38d98a8b2af95bb3f723f18598c96a187513ebc829088f8f46eae188f93dbf192c",
            "04390fb5b5e5cba73ee6015430db8b669a8d35c25347085cb9dc7ebb8d29b63f8ce87cad637ef5c78ae9a103c52892e340bacdbb57cb8f1ed36de857398161ce930f7d83677c72f879ab602f621b433aeb5417634e9e7239fd8cc418575a64a2c8edc17facdb0cf79843c294ee3f5f3b2897a9da3e28848d11ae3b90c736cef8ccacd1bf12240e133ab4698afcef9ff54739ce5d70d359597f10a879c876c96edc2ee2e26f09a375b18129e4a1be1936384fba917c1fb0b1e04336bc393bdf49e6f793049f09339d8a8810cde0c1071ff567fc817f12f8f30faa4fb218d2624e23075daa0e42b6297e6fc00da3ce646d2cfaf1a6dca10a79635fc4b6761070136397210c5d7784d6fa788c4e2e640d0c51abb3c757d5b653274ca5ddfd5b0b887f4a301bc7ee29f8932325aff7ba7592584ba570568ad0b1106f7988b0d0e9224d468409aae9744019d85daa1a6f57a934258066a09633d89988017d4c9ca0d6a2312bc7d0268dbc50305ea090bc176ca8bb4dd32f3c7662d5fa3aeb28c9890de2f997e8885af68a5ab45993d4702994553a695791d57fad57937ab4bf790fdb6934e427de02d9799cc22d347a0b730f454719b719ebb9934b50fbe6cb8883efafaf396e1224534d3ebf2246769ce7a317c16211758ef1ef097e2b5b512c291d259e247d4dfb70ba2bfc65a59b6b7bc99077bfb57c31a88f00eda8f73098166aa02af29c7a2a83e6",
            TestName = "KWP with AES - long example")]
        [TestCase(true, true,
            "983e4f81b7e92e6b9e43f4c709982c5e",
            "7b",
            "0a8ee455066ed701c05af6264670e33b",
            TestName = "KWP with AES - short example")]
        public void ShouldReturnExpectedValue(bool useInverseCipher, bool successfulAuthenticate, string keyHex, string pHex, string expectedHex)
        {
            var key = new BitString(keyHex);
            var plaintext = new BitString(pHex);
            var expectedCiphertext = new BitString(expectedHex);

            _subject = new KeyWrapWithPaddingAes(new AES_ECB.AES_ECB(new RijndaelFactory(new RijndaelInternals())));
            var resultCiphertext = _subject.Encrypt(key, plaintext, useInverseCipher);

            Assert.IsTrue(resultCiphertext.Success, resultCiphertext.ErrorMessage);

            // Mangle the actualC returned when it should not decrypt successfully
            if (!successfulAuthenticate)
            {
                var rand = new Random800_90();
                resultCiphertext = new KeyWrapResult(rand.GetDifferentBitStringOfSameSize(resultCiphertext.ResultingBitString));
            }

            var decrypt = _subject.Decrypt(key, resultCiphertext.ResultingBitString, useInverseCipher);

            if (!successfulAuthenticate)
            {
                Assert.IsFalse(decrypt.Success);
                return;
            }

            Assert.AreEqual(expectedCiphertext.ToHex(), resultCiphertext.ResultingBitString.ToHex(), "encrypt compare");
            Assert.AreEqual(plaintext.ToHex(), decrypt.ResultingBitString.ToHex(), "decrypt compare");
        }
    }
}
