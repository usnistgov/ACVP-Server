using NIST.CVP.Crypto.AES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES_GCM.Tests
{
    [TestFixture, FastCryptoTest]
    public class CAVS_HealthCheckTests
    {
        private Crypto.AES_GCM.AES_GCM _subject;
        
        [SetUp]
        public void Setup()
        {
            _subject = new Crypto.AES_GCM.AES_GCM(
                new AES_GCMInternals(
                    new RijndaelFactory(
                        new RijndaelInternals()
                    )
                ), 
                new RijndaelFactory(
                    new RijndaelInternals()
                )
            );
        }

        static object[] aesGcmTestDataGroup = new object[]
        {
            new object[] // Example 1
            {
                $"{nameof(aesGcmTestDataGroup)}_1",
                "FEFFE992 8665731C 6D6A8F94 67308308", // key
                "CAFEBABE FACEDBAD DECAF888", // IV,
                string.Empty, // aad
                string.Empty, // plainText
                string.Empty, // cipherText
                "3247184B 3C4F69A4 4DBCD228 87BBB418", // tag
                128 // tag length
            },
            new object[] // Example 2
            {
                $"{nameof(aesGcmTestDataGroup)}_2",
                "FEFFE992 8665731C 6D6A8F94 67308308", // key
                "CAFEBABE FACEDBAD DECAF888", // IV,
                string.Empty, // aad
                "D9313225 F88406E5 A55909C5 AFF5269A 86A7A953 1534F7DA 2E4C303D 8A318A72 1C3C0C95 95680953 2FCF0E24 49A6B525 B16AEDF5 AA0DE657 BA637B39 1AAFD255", // plainText
                string.Empty, // cipherText
                "4D5C2AF3 27CD64A6 2CF35ABD 2BA6FAB4", // tag
                128 // tag length
            },
            new object[] // Example 3
            {
                $"{nameof(aesGcmTestDataGroup)}_3",
                "FEFFE992 8665731C 6D6A8F94 67308308", // key
                "CAFEBABE FACEDBAD DECAF888", // IV,
                "3AD77BB4 0D7A3660 A89ECAF3 2466EF97 F5D3D585 03B9699D E785895A 96FDBAAF 43B1CD7F 598ECE23 881B00E3 ED030688 7B0C785E 27E8AD3F 82232071 04725DD4", // aad
                string.Empty, // plainText
                string.Empty, // cipherText
                "5F91D771 23EF5EB9 99791384 9B8DC1E9", // tag
                128 // tag length
            },
            new object[] // Example 4
            {
                $"{nameof(aesGcmTestDataGroup)}_4",
                "FEFFE992 8665731C 6D6A8F94 67308308", // key
                "CAFEBABE FACEDBAD DECAF888", // IV,
                "3AD77BB4 0D7A3660 A89ECAF3 2466EF97 F5D3D585 03B9699D E785895A 96FDBAAF 43B1CD7F 598ECE23 881B00E3 ED030688 7B0C785E 27E8AD3F 82232071 04725DD4", // aad
                "D9313225 F88406E5 A55909C5 AFF5269A 86A7A953 1534F7DA 2E4C303D 8A318A72 1C3C0C95 95680953 2FCF0E24 49A6B525 B16AEDF5 AA0DE657 BA637B39 1AAFD255", // plainText
                string.Empty, // cipherText
                "64C02329 04AF398A 5B67C10B 53A5024D", // tag
                128 // tag length
            },
            new object[] // Example 5
            {
                $"{nameof(aesGcmTestDataGroup)}_5",
                "FEFFE992 8665731C 6D6A8F94 67308308", // key
                "CAFEBABE FACEDBAD DECAF888", // IV,
                "3AD77BB4 0D7A3660 A89ECAF3 2466EF97 F5D3D585", // aad
                "D9313225 F88406E5 A55909C5 AFF5269A 86A7A953 1534F7DA 2E4C303D 8A318A72 1C3C0C95 95680953 2FCF0E24 49A6B525 B16AEDF5 AA0DE657 BA637B39", // plainText
                string.Empty, // cipherText
                "F07C2528 EEA2FCA1 211F905E 1B6A881B", // tag
                128 // tag length
            },
            new object[] // Example 6
            {
                $"{nameof(aesGcmTestDataGroup)}_6",
                "FEFFE992 8665731C 6D6A8F94 67308308", // key
                "CAFEBABE FACEDBAD DECAF888", // IV,
                "3AD77BB4 0D7A3660 A89ECAF3 2466EF97 F5D3D585", // aad
                "D9313225 F88406E5 A55909C5 AFF5269A 86A7A953 1534F7DA 2E4C303D 8A318A72 1C3C0C95 95680953 2FCF0E24 49A6B525 B16AEDF5 AA0DE657 BA637B39", // plainText
                string.Empty, // cipherText
                "F07C2528 EEA2FCA1 211F905E", // tag
                96 // tag length
            },
        };

        static object[] aesGcmTestDataGroup192 = new object[]
        {
            new object[] // Example 1
            {
                $"{nameof(aesGcmTestDataGroup192)}_1",
                "FEFFE992 8665731C 6D6A8F94 67308308 FEFFE992 8665731C", // key
                "CAFEBABE FACEDBAD DECAF888", // IV,
                string.Empty, // aad
                string.Empty, // plainText
                string.Empty, // cipherText
                "C835AA88 AEBBC94F 5A02E179 FDCFC3E4", // tag
                128 // tag length
            },
            new object[] // Example 2
            {
                $"{nameof(aesGcmTestDataGroup192)}_2",
                "FEFFE992 8665731C 6D6A8F94 67308308 FEFFE992 8665731C", // key
                "CAFEBABE FACEDBAD DECAF888", // IV,
                string.Empty, // aad
                "D9313225 F88406E5 A55909C5 AFF5269A 86A7A953 1534F7DA 2E4C303D 8A318A72 1C3C0C95 95680953 2FCF0E24 49A6B525 B16AEDF5 AA0DE657 BA637B39 1AAFD255", // plainText
                string.Empty, // cipherText
                "9924A7C8 587336BF B118024D B8674A14", // tag
                128 // tag length
            },
            new object[] // Example 3
            {
                $"{nameof(aesGcmTestDataGroup192)}_3",
                "FEFFE992 8665731C 6D6A8F94 67308308 FEFFE992 8665731C", // key
                "CAFEBABE FACEDBAD DECAF888", // IV,
                "3AD77BB4 0D7A3660 A89ECAF3 2466EF97 F5D3D585 03B9699D E785895A 96FDBAAF 43B1CD7F 598ECE23 881B00E3 ED030688 7B0C785E 27E8AD3F 82232071 04725DD4", // aad
                string.Empty, // plainText
                string.Empty, // cipherText
                "02CC773B C919F4E1 C5E9C543 13BFACE0", // tag
                128 // tag length
            },
            new object[] // Example 4
            {
                $"{nameof(aesGcmTestDataGroup192)}_4",
                "FEFFE992 8665731C 6D6A8F94 67308308 FEFFE992 8665731C", // key
                "CAFEBABE FACEDBAD DECAF888", // IV,
                "3AD77BB4 0D7A3660 A89ECAF3 2466EF97 F5D3D585 03B9699D E785895A 96FDBAAF 43B1CD7F 598ECE23 881B00E3 ED030688 7B0C785E 27E8AD3F 82232071 04725DD4", // aad
                "D9313225 F88406E5 A55909C5 AFF5269A 86A7A953 1534F7DA 2E4C303D 8A318A72 1C3C0C95 95680953 2FCF0E24 49A6B525 B16AEDF5 AA0DE657 BA637B39 1AAFD255", // plainText
                string.Empty, // cipherText
                "3B9153B4 E7318A5F 3BBEAC10 8F8A8EDB", // tag
                128 // tag length
            },
            new object[] // Example 5
            {
                $"{nameof(aesGcmTestDataGroup192)}_5",
                "FEFFE992 8665731C 6D6A8F94 67308308 FEFFE992 8665731C", // key
                "CAFEBABE FACEDBAD DECAF888", // IV,
                "3AD77BB4 0D7A3660 A89ECAF3 2466EF97 F5D3D585", // aad
                "D9313225 F88406E5 A55909C5 AFF5269A 86A7A953 1534F7DA 2E4C303D 8A318A72 1C3C0C95 95680953 2FCF0E24 49A6B525 B16AEDF5 AA0DE657 BA637B39", // plainText
                string.Empty, // cipherText
                "93EA28C6 59E26990 2A80ACD2 08E7FC80", // tag
                128 // tag length
            },
            new object[] // Example 6
            {
                $"{nameof(aesGcmTestDataGroup192)}_6",
                "FEFFE992 8665731C 6D6A8F94 67308308 FEFFE992 8665731C", // key
                "CAFEBABE FACEDBAD DECAF888", // IV,
                "3AD77BB4 0D7A3660 A89ECAF3 2466EF97 F5D3D585", // aad
                "D9313225 F88406E5 A55909C5 AFF5269A 86A7A953 1534F7DA 2E4C303D 8A318A72 1C3C0C95 95680953 2FCF0E24 49A6B525 B16AEDF5 AA0DE657 BA637B39", // plainText
                string.Empty, // cipherText
                "93EA28C6 59E26990 2A80ACD2", // tag
                96 // tag length
            },
        };

        static object[] aesGcmTestDataGroup256 = new object[]
        {
            new object[] // Example 1
            {
                $"{nameof(aesGcmTestDataGroup256)}_1",
                "FEFFE992 8665731C 6D6A8F94 67308308 FEFFE992 8665731C 6D6A8F94 67308308", // key
                "CAFEBABE FACEDBAD DECAF888", // IV,
                string.Empty, // aad
                string.Empty, // plainText
                string.Empty, // cipherText
                "FD2CAA16 A5832E76 AA132C14 53EEDA7E", // tag
                128 // tag length
            },
            new object[] // Example 2
            {
                $"{nameof(aesGcmTestDataGroup256)}_2",
                "FEFFE992 8665731C 6D6A8F94 67308308 FEFFE992 8665731C 6D6A8F94 67308308", // key
                "CAFEBABE FACEDBAD DECAF888", // IV,
                string.Empty, // aad
                "D9313225 F88406E5 A55909C5 AFF5269A 86A7A953 1534F7DA 2E4C303D 8A318A72 1C3C0C95 95680953 2FCF0E24 49A6B525 B16AEDF5 AA0DE657 BA637B39 1AAFD255", // plainText
                string.Empty, // cipherText
                "B094DAC5 D93471BD EC1A5022 70E3CC6C", // tag
                128 // tag length
            },
            new object[] // Example 3
            {
                $"{nameof(aesGcmTestDataGroup256)}_3",
                "FEFFE992 8665731C 6D6A8F94 67308308 FEFFE992 8665731C 6D6A8F94 67308308", // key
                "CAFEBABE FACEDBAD DECAF888", // IV,
                "3AD77BB4 0D7A3660 A89ECAF3 2466EF97 F5D3D585 03B9699D E785895A 96FDBAAF 43B1CD7F 598ECE23 881B00E3 ED030688 7B0C785E 27E8AD3F 82232071 04725DD4", // aad
                string.Empty, // plainText
                string.Empty, // cipherText
                "DE34B6DC D4CEE2FD BEC3CEA0 1AF1EE44", // tag
                128 // tag length
            },
            new object[] // Example 4
            {
                $"{nameof(aesGcmTestDataGroup256)}_4",
                "FEFFE992 8665731C 6D6A8F94 67308308 FEFFE992 8665731C 6D6A8F94 67308308", // key
                "CAFEBABE FACEDBAD DECAF888", // IV,
                "3AD77BB4 0D7A3660 A89ECAF3 2466EF97 F5D3D585 03B9699D E785895A 96FDBAAF 43B1CD7F 598ECE23 881B00E3 ED030688 7B0C785E 27E8AD3F 82232071 04725DD4", // aad
                "D9313225 F88406E5 A55909C5 AFF5269A 86A7A953 1534F7DA 2E4C303D 8A318A72 1C3C0C95 95680953 2FCF0E24 49A6B525 B16AEDF5 AA0DE657 BA637B39 1AAFD255", // plainText
                string.Empty, // cipherText
                "C06D76F3 1930FEF3 7ACAE23E D465AE62", // tag
                128 // tag length
            },
            new object[] // Example 5
            {
                $"{nameof(aesGcmTestDataGroup256)}_5",
                "FEFFE992 8665731C 6D6A8F94 67308308 FEFFE992 8665731C 6D6A8F94 67308308", // key
                "CAFEBABE FACEDBAD DECAF888", // IV,
                "3AD77BB4 0D7A3660 A89ECAF3 2466EF97 F5D3D585", // aad
                "D9313225 F88406E5 A55909C5 AFF5269A 86A7A953 1534F7DA 2E4C303D 8A318A72 1C3C0C95 95680953 2FCF0E24 49A6B525 B16AEDF5 AA0DE657 BA637B39", // plainText
                string.Empty, // cipherText
                "E097195F 4532DA89 5FB917A5 A55C6AA0", // tag
                128 // tag length
            },
            new object[] // Example 6
            {
                $"{nameof(aesGcmTestDataGroup256)}_6",
                "FEFFE992 8665731C 6D6A8F94 67308308 FEFFE992 8665731C 6D6A8F94 67308308", // key
                "CAFEBABE FACEDBAD DECAF888", // IV,
                "3AD77BB4 0D7A3660 A89ECAF3 2466EF97 F5D3D585", // aad
                "D9313225 F88406E5 A55909C5 AFF5269A 86A7A953 1534F7DA 2E4C303D 8A318A72 1C3C0C95 95680953 2FCF0E24 49A6B525 B16AEDF5 AA0DE657 BA637B39", // plainText
                string.Empty, // cipherText
                "E097195F 4532DA89 5FB917A5", // tag
                96 // tag length
            },
        };
        
        [Test]
        public void AESGCM_XPN()
        {
            BitString aad = new BitString("56d1dc66b2ae1c5c972aa1c22025c74b");
            BitString plainText = new BitString("f15e9ceb86dd8309767c3f675eb5503c");
            BitString key = new BitString("b9eac5c6 50daeab9 c15aec8d 362cfd1f ccef9d20 fe3c0e54 fd321554 8d203f0d");
            BitString iv = new BitString("04527842 a98b3336 2a09067c");
            BitString salt = new BitString("12345678 87654388 44888844");
            BitString newIV;
            newIV = iv.XOR(salt);
            var encryptResult = _subject.BlockEncrypt(key, plainText, newIV, aad, 128);

            Assert.IsTrue(encryptResult.Success);
            Assert.AreEqual(encryptResult.Tag, new BitString("e47971b2 c83ed28a d66fb896 2478d01f"), nameof(encryptResult.Tag));

            var decryptResult = _subject.BlockDecrypt(key, encryptResult.CipherText, newIV, aad, encryptResult.Tag);

            Assert.IsTrue(decryptResult.Success);
        }

        [Test]
        [TestCaseSource(nameof(aesGcmTestDataGroup))]
        [TestCaseSource(nameof(aesGcmTestDataGroup192))]
        [TestCaseSource(nameof(aesGcmTestDataGroup256))]
        public void ShouldEncryptAndDecryptSuccessfully(
            string testLabel, 
            string keyString, 
            string ivString, 
            string aadString, 
            string plainTextString, 
            string cipherTextString, 
            string tagString, 
            int tagLength
        )
        {
            BitString key = new BitString(keyString);
            BitString iv = new BitString(ivString);
            BitString aad = new BitString(aadString);
            BitString plainText = new BitString(plainTextString);
            BitString cipherText = new BitString(cipherTextString);
            BitString tag = new BitString(tagString);

            var encryptResult = _subject.BlockEncrypt(key, plainText, iv, aad, tagLength);

            Assert.IsTrue(encryptResult.Success, nameof(_subject.BlockEncrypt));
            Assert.AreEqual(tag, encryptResult.Tag, nameof(encryptResult.Tag));

            var decryptResult = _subject.BlockDecrypt(key, encryptResult.CipherText, iv, aad, encryptResult.Tag);
            Assert.IsTrue(decryptResult.Success, nameof(_subject.BlockDecrypt));

            Assert.AreEqual(plainText, decryptResult.Result, nameof(plainText));
        }

        [Test]
        [TestCaseSource(nameof(aesGcmTestDataGroup))]
        [TestCaseSource(nameof(aesGcmTestDataGroup192))]
        [TestCaseSource(nameof(aesGcmTestDataGroup256))]
        public void ShouldReportErrorOnInvalidDecryptionTag(
            string testLabel,
            string keyString,
            string ivString,
            string aadString,
            string plainTextString,
            string cipherTextString,
            string tagString,
            int tagLength
        )
        {
            BitString key = new BitString(keyString);
            BitString iv = new BitString(ivString);
            BitString aad = new BitString(aadString);
            BitString plainText = new BitString(plainTextString);
            BitString cipherText = new BitString(cipherTextString);
            BitString tag = new BitString(tagString);

            var encryptResult = _subject.BlockEncrypt(key, plainText, iv, aad, tagLength);

            Assume.That(encryptResult.Success, nameof(_subject.BlockEncrypt));
            Assume.That(tag.Equals(encryptResult.Tag), nameof(encryptResult.Tag));

            var xoredTag = encryptResult.Tag.XOR(GetBitStringOfLengthWithAll1s(encryptResult.Tag.BitLength));

            var decryptResult = _subject.BlockDecrypt(key, encryptResult.CipherText, iv, aad, xoredTag);
            Assert.IsFalse(decryptResult.Success, nameof(_subject.BlockDecrypt));
            Assert.AreEqual("Tags do not match", decryptResult.ErrorMessage, nameof(decryptResult.ErrorMessage));
        }

        [Test]
        [TestCaseSource(nameof(aesGcmTestDataGroup))]
        [TestCaseSource(nameof(aesGcmTestDataGroup192))]
        [TestCaseSource(nameof(aesGcmTestDataGroup256))]
        public void ShouldGetDifferingPlainTextAfterDecryptionWithModifiedCipherText(
                    string testLabel,
                    string keyString,
                    string ivString,
                    string aadString,
                    string plainTextString,
                    string cipherTextString,
                    string tagString,
                    int tagLength
                )
        {
            BitString key = new BitString(keyString);
            BitString iv = new BitString(ivString);
            BitString aad = new BitString(aadString);
            BitString plainText = new BitString(plainTextString);
            BitString cipherText = new BitString(cipherTextString);
            BitString tag = new BitString(tagString);

            var encryptResult = _subject.BlockEncrypt(key, plainText, iv, aad, tagLength);

            Assume.That(encryptResult.Success, nameof(_subject.BlockEncrypt));
            Assume.That(tag.Equals(encryptResult.Tag), nameof(encryptResult.Tag));

            var xoredTag = encryptResult.Tag.XOR(GetBitStringOfLengthWithAll1s(encryptResult.Tag.BitLength));

            var decryptResult = _subject.BlockDecrypt(key, encryptResult.CipherText, iv, aad, xoredTag);

            Assert.AreNotEqual(plainText, decryptResult.Result, nameof(plainText));
        }

        private static object[] _testDataNon96BitIvs = new object[]
        {
            new object[]
            {
                $"{nameof(_testDataNon96BitIvs)}_1",
                // key
                new BitString("7c7a7a0bc1bc3f051cfdaeff51483c14165cc7a7eaa33dcf"),
                // iv
                new BitString("3252c144a15fb2c3c0c2b2e7f879f2"),
                // pt
                new BitString("0fd15eed3fd2226051234fdc6334cb90"),
                // aad
                new BitString("8216f8d68932987cd7c8bef4c8c8b424"),
                // expectedCt
                new BitString("6eba9f926e261f304809507f55a3feed"),
                // expectedTag
                new BitString("be538d8ad94a387855e17991b2335e06")
            },
            new object[]
            {
                $"{nameof(_testDataNon96BitIvs)}_2",
                // key
                new BitString("3b96842bbccdd5219b65fa0febaac3ad6b0aaa5b5e6d5ef7"),
                // iv
                new BitString("6ce9ce453c0bc6e849162d15244149"),
                // pt
                new BitString(""),
                // aad
                new BitString("0d2c403e33844e4f2ba5aed57713723b"),
                // expectedCt
                new BitString(""),
                // expectedTag
                new BitString("6cbb25c4379b415f0d435d4fd7abc7cc")
            },
            new object[]
            {
                $"{nameof(_testDataNon96BitIvs)}_3",
                // key
                new BitString("6b504be7374c95abe3714099490c463bbf02bbcbe8c63fdd"),
                // iv
                new BitString("ae2d3c4ddd7ebb91341899c0bc32fa54d0"),
                // pt
                new BitString(""),
                // aad
                new BitString("8fa3c89482c345226a102af0f3bd5043"),
                // expectedCt
                new BitString(""),
                // expectedTag
                new BitString("872d99f52264fb0f49230e8fd5f23584")
            }
        };

        [Test]
        [TestCaseSource(nameof(_testDataNon96BitIvs))]
        public void ShouldEncryptNon96BitIvsCorrectly(string testLabel, BitString key, BitString iv, BitString pt, BitString aad, BitString expectedCt, BitString expectedTag)
        {
            var result = _subject.BlockEncrypt(key, pt, iv, aad, expectedTag.BitLength);

            Assert.AreEqual(result.CipherText.ToHex(), expectedCt.ToHex(), nameof(expectedCt));
            Assert.AreEqual(result.Tag.ToHex(), expectedTag.ToHex(), nameof(expectedTag));
        }

        private BitString GetBitStringOfLengthWithAll1s(int length)
        {
            BitString bs = new BitString(length);

            for (int i = 0; i < length; i++)
            {
                bs.Bits[i] = true;
            }

            return bs;
        }
    }
}

