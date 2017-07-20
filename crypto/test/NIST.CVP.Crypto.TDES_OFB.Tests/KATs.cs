using NUnit.Framework;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_OFB.Tests
{
    [TestFixture]
    public class KATs
    {
        [Test]
        public void ShouldPassStartupTest()
        {
            var keyBytes = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef };
            var plainTextBytes = new byte[] { 0x4e, 0x6f, 0x77, 0x20, 0x69, 0x73, 0x20, 0x74 };
            var expectedBytes = new byte[] { 0x3f, 0xa4, 0x0e, 0x8a, 0x98, 0x4d, 0x48, 0x15 };
            var iv = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var subject = new TdesOfb();
            var result = subject.BlockEncrypt(new BitString(keyBytes), new BitString(iv), new BitString(plainTextBytes));
            Assert.IsTrue((bool)result.Success);
            var actual = result.CipherText.ToBytes();
            Assert.AreEqual(expectedBytes, actual);
        }


        [Test]
        [TestCase("0101010101010101", "95f8a5e5dd31d900", "8000000000000000")]
        [TestCase("0101010101010101", "dd7f121ca5015619", "4000000000000000")]
        [TestCase("0101010101010101", "2e8653104f3834ea", "2000000000000000")]
        [TestCase("0101010101010101", "4bd388ff6cd81d4f", "1000000000000000")]
        [TestCase("0101010101010101", "20b9e767b2fb1456", "0800000000000000")]
        [TestCase("0101010101010101", "55579380d77138ef", "0400000000000000")]
        [TestCase("0101010101010101", "6cc5defaaf04512f", "0200000000000000")]
        [TestCase("0101010101010101", "0d9f279ba5d87260", "0100000000000000")]
        [TestCase("0101010101010101", "d9031b0271bd5a0a", "0080000000000000")]
        [TestCase("0101010101010101", "424250b37c3dd951", "0040000000000000")]
        [TestCase("0101010101010101", "b8061b7ecd9a21e5", "0020000000000000")]
        [TestCase("0101010101010101", "f15d0f286b65bd28", "0010000000000000")]
        [TestCase("0101010101010101", "add0cc8d6e5deba1", "0008000000000000")]
        [TestCase("0101010101010101", "e6d5f82752ad63d1", "0004000000000000")]
        [TestCase("0101010101010101", "ecbfe3bd3f591a5e", "0002000000000000")]
        [TestCase("0101010101010101", "f356834379d165cd", "0001000000000000")]
        [TestCase("0101010101010101", "2b9f982f20037fa9", "0000800000000000")]
        [TestCase("0101010101010101", "889de068a16f0be6", "0000400000000000")]
        [TestCase("0101010101010101", "e19e275d846a1298", "0000200000000000")]
        [TestCase("0101010101010101", "329a8ed523d71aec", "0000100000000000")]
        [TestCase("0101010101010101", "e7fce22557d23c97", "0000080000000000")]
        [TestCase("0101010101010101", "12a9f5817ff2d65d", "0000040000000000")]
        [TestCase("0101010101010101", "a484c3ad38dc9c19", "0000020000000000")]
        [TestCase("0101010101010101", "fbe00a8a1ef8ad72", "0000010000000000")]
        [TestCase("0101010101010101", "750d079407521363", "0000008000000000")]
        [TestCase("0101010101010101", "64feed9c724c2faf", "0000004000000000")]
        [TestCase("0101010101010101", "f02b263b328e2b60", "0000002000000000")]
        [TestCase("0101010101010101", "9d64555a9a10b852", "0000001000000000")]
        [TestCase("0101010101010101", "d106ff0bed5255d7", "0000000800000000")]
        [TestCase("0101010101010101", "e1652c6b138c64a5", "0000000400000000")]
        [TestCase("0101010101010101", "e428581186ec8f46", "0000000200000000")]
        [TestCase("0101010101010101", "aeb5f5ede22d1a36", "0000000100000000")]
        [TestCase("0101010101010101", "e943d7568aec0c5c", "0000000080000000")]
        [TestCase("0101010101010101", "df98c8276f54b04b", "0000000040000000")]
        [TestCase("0101010101010101", "b160e4680f6c696f", "0000000020000000")]
        [TestCase("0101010101010101", "fa0752b07d9c4ab8", "0000000010000000")]
        [TestCase("0101010101010101", "ca3a2b036dbc8502", "0000000008000000")]
        [TestCase("0101010101010101", "5e0905517bb59bcf", "0000000004000000")]
        [TestCase("0101010101010101", "814eeb3b91d90726", "0000000002000000")]
        [TestCase("0101010101010101", "4d49db1532919c9f", "0000000001000000")]
        [TestCase("0101010101010101", "25eb5fc3f8cf0621", "0000000000800000")]
        [TestCase("0101010101010101", "ab6a20c0620d1c6f", "0000000000400000")]
        [TestCase("0101010101010101", "79e90dbc98f92cca", "0000000000200000")]
        [TestCase("0101010101010101", "866ecedd8072bb0e", "0000000000100000")]
        [TestCase("0101010101010101", "8b54536f2f3e64a8", "0000000000080000")]
        [TestCase("0101010101010101", "ea51d3975595b86b", "0000000000040000")]
        [TestCase("0101010101010101", "caffc6ac4542de31", "0000000000020000")]
        [TestCase("0101010101010101", "8dd45a2ddf90796c", "0000000000010000")]
        [TestCase("0101010101010101", "1029d55e880ec2d0", "0000000000008000")]
        [TestCase("0101010101010101", "5d86cb23639dbea9", "0000000000004000")]
        [TestCase("0101010101010101", "1d1ca853ae7c0c5f", "0000000000002000")]
        [TestCase("0101010101010101", "ce332329248f3228", "0000000000001000")]
        [TestCase("0101010101010101", "8405d1abe24fb942", "0000000000000800")]
        [TestCase("0101010101010101", "e643d78090ca4207", "0000000000000400")]
        [TestCase("0101010101010101", "48221b9937748a23", "0000000000000200")]
        [TestCase("0101010101010101", "dd7c0bbd61fafd54", "0000000000000100")]
        [TestCase("0101010101010101", "2fbc291a570db5c4", "0000000000000080")]
        [TestCase("0101010101010101", "e07c30d7e4e26e12", "0000000000000040")]
        [TestCase("0101010101010101", "0953e2258e8e90a1", "0000000000000020")]
        [TestCase("0101010101010101", "5b711bc4ceebf2ee", "0000000000000010")]
        [TestCase("0101010101010101", "cc083f1e6d9e85f6", "0000000000000008")]
        [TestCase("0101010101010101", "d2fd8867d50d2dfe", "0000000000000004")]
        [TestCase("0101010101010101", "06e7ea22ce92708f", "0000000000000002")]
        [TestCase("0101010101010101", "166b40b44aba4bd6", "0000000000000001")]
        public void ShouldPassInversePermuatationEncryptKnownAnswerTest(string key, string iv, string cipherText)
        {
            var subject = new TdesOfb();
            var plaintext = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var result = subject.BlockEncrypt(new BitString(key), new BitString(plaintext), new BitString(iv));
            Assert.IsTrue((bool)result.Success);

            Assert.AreEqual(new BitString(cipherText).ToHex(), result.CipherText.ToHex());
        }

        [Test]
        [TestCase("1046913489980131", "0000000000000000", "88d55e54f54c97b4")]
        [TestCase("1007103489988020", "0000000000000000", "0c0cc00c83ea48fd")]
        [TestCase("10071034c8980120", "0000000000000000", "83bc8ef3a6570183")]
        [TestCase("1046103489988020", "0000000000000000", "df725dcad94ea2e9")]
        [TestCase("1086911519190101", "0000000000000000", "e652b53b550be8b0")]
        [TestCase("1086911519580101", "0000000000000000", "af527120c485cbb0")]
        [TestCase("5107b01519580101", "0000000000000000", "0f04ce393db926d5")]
        [TestCase("1007b01519190101", "0000000000000000", "c9f00ffc74079067")]
        [TestCase("3107915498080101", "0000000000000000", "7cfd82a593252b4e")]
        [TestCase("3107919498080101", "0000000000000000", "cb49a2f9e91363e3")]
        [TestCase("10079115b9080140", "0000000000000000", "00b588be70d23f56")]
        [TestCase("3107911598080140", "0000000000000000", "406a9a6ab43399ae")]
        [TestCase("1007d01589980101", "0000000000000000", "6cb773611dca9ada")]
        [TestCase("9107911589980101", "0000000000000000", "67fd21c17dbb5d70")]
        [TestCase("9107d01589190101", "0000000000000000", "9592cb4110430787")]
        [TestCase("1007d01598980120", "0000000000000000", "a6b7ff68a318ddd3")]
        [TestCase("1007940498190101", "0000000000000000", "4d102196c914ca16")]
        [TestCase("0107910491190401", "0000000000000000", "2dfa9f4573594965")]
        [TestCase("0107910491190101", "0000000000000000", "b46604816c0e0774")]
        [TestCase("0107940491190401", "0000000000000000", "6e7e6221a4f34e87")]
        [TestCase("19079210981a0101", "0000000000000000", "aa85e74643233199")]
        [TestCase("1007911998190801", "0000000000000000", "2e5a19db4d1962d6")]
        [TestCase("10079119981a0801", "0000000000000000", "23a866a809d30894")]
        [TestCase("1007921098190101", "0000000000000000", "d812d961f017d320")]
        [TestCase("100791159819010b", "0000000000000000", "055605816e58608f")]
        [TestCase("1004801598190101", "0000000000000000", "abd88e8b1b7716f1")]
        [TestCase("1004801598190102", "0000000000000000", "537ac95be69da1e1")]
        [TestCase("1004801598190108", "0000000000000000", "aed0f6ae3c25cdd8")]
        [TestCase("1002911598100104", "0000000000000000", "b3e35a5ee53e7b8d")]
        [TestCase("1002911598190104", "0000000000000000", "61c79c71921a2ef8")]
        [TestCase("1002911598100201", "0000000000000000", "e2f5728f0995013c")]
        [TestCase("1002911698100101", "0000000000000000", "1aeac39a61f0a464")]
        public void ShouldPassPermuatationEncryptKnownAnswerTest(string key, string iv, string cipherText)
        {
            var subject = new TdesOfb();
            var plaintext = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var result = subject.BlockEncrypt(new BitString(key), new BitString(plaintext), new BitString(iv));
            Assert.IsTrue((bool)result.Success);

            Assert.AreEqual(new BitString(cipherText).ToHex(), result.CipherText.ToHex());
        }

        [Test]
        [TestCase("7ca110454a1a6e57", "01a1d6d039776742", "690f5b0d9a26939b")]
        [TestCase("0131d9619dc1376e", "5cd54ca83def57da", "7a389d10354bd271")]
        [TestCase("07a1133e4a0b2686", "0248d43806f67172", "868ebb51cab4599a")]
        [TestCase("3849674c2602319e", "51454b582ddf440a", "7178876e01f19b2a")]
        [TestCase("04b915ba43feb5b6", "42fd443059577fa2", "af37fb421f8c4095")]
        [TestCase("0113b970fd34f2ce", "059b5e0851cf143a", "86a560f10ec6d85b")]
        [TestCase("0170f175468fb5e6", "0756d8e0774761d2", "0cd3da020021dc09")]
        [TestCase("43297fad38e373fe", "762514b829bf486a", "ea676b2cb7db2b7a")]
        [TestCase("07a7137045da2a16", "3bdd119049372802", "dfd64a815caf1a0f")]
        [TestCase("04689104c2fd3b2f", "26955f6835af609a", "5c513c9c4886c088")]
        [TestCase("37d06bb516cb7546", "164d5e404f275232", "0a2aeeae3ff4ab77")]
        [TestCase("1f08260d1ac2465e", "6b056e18759f5cca", "ef1bf03e5dfa575a")]
        [TestCase("584023641aba6176", "004bd6ef09176062", "88bf0db6d70dee56")]
        [TestCase("025816164629b007", "480d39006ee762f2", "a1f9915541020b56")]
        [TestCase("49793ebc79b3258f", "437540c8698f3cfa", "6fbf1cafcffd0556")]
        [TestCase("4fb05e1515ab73a7", "072d43a077075292", "2f22e49bab7ca1ac")]
        [TestCase("49e95d6d4ca229bf", "02fe55778117f12a", "5a6b612cc26cce4a")]
        [TestCase("018310dc409b26d6", "1d9d5c5018f728c2", "5f4c038ed12b2e41")]
        [TestCase("1c587f1c13924fef", "305532286d6f295a", "63fac0d034d9f793")]
        public void ShouldPassSubstitutionTableEncryptKnownAnswerTest(string key, string iv, string cipherText)
        {
            var subject = new TdesOfb();
            var plaintext = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var result = subject.BlockEncrypt(new BitString(key), new BitString(plaintext), new BitString(iv));
            Assert.IsTrue((bool)result.Success);

            Assert.AreEqual(new BitString(cipherText).ToHex(), result.CipherText.ToHex());
        }

        [Test]
        [TestCase("8001010101010101", "95a8d72813daa94d")]
        [TestCase("4001010101010101", "0eec1487dd8c26d5")]
        [TestCase("2001010101010101", "7ad16ffb79c45926")]
        [TestCase("1001010101010101", "d3746294ca6a6cf3")]
        [TestCase("0801010101010101", "809f5f873c1fd761")]
        [TestCase("0401010101010101", "c02faffec989d1fc")]
        [TestCase("0201010101010101", "4615aa1d33e72f10")]
        [TestCase("0180010101010101", "2055123350c00858")]
        [TestCase("0140010101010101", "df3b99d6577397c8")]
        [TestCase("0120010101010101", "31fe17369b5288c9")]
        [TestCase("0110010101010101", "dfdd3cc64dae1642")]
        [TestCase("0108010101010101", "178c83ce2b399d94")]
        [TestCase("0104010101010101", "50f636324a9b7f80")]
        [TestCase("0102010101010101", "a8468ee3bc18f06d")]
        [TestCase("0101800101010101", "a2dc9e92fd3cde92")]
        [TestCase("0101400101010101", "cac09f797d031287")]
        [TestCase("0101200101010101", "90ba680b22aeb525")]
        [TestCase("0101100101010101", "ce7a24f350e280b6")]
        [TestCase("0101080101010101", "882bff0aa01a0b87")]
        [TestCase("0101040101010101", "25610288924511c2")]
        [TestCase("0101020101010101", "c71516c29c75d170")]
        [TestCase("0101018001010101", "5199c29a52c9f059")]
        [TestCase("0101014001010101", "c22f0a294a71f29f")]
        [TestCase("0101012001010101", "ee371483714c02ea")]
        [TestCase("0101011001010101", "a81fbd448f9e522f")]
        [TestCase("0101010801010101", "4f644c92e192dfed")]
        [TestCase("0101010401010101", "1afa9a66a6df92ae")]
        [TestCase("0101010201010101", "b3c1cc715cb879d8")]
        [TestCase("0101010180010101", "19d032e64ab0bd8b")]
        [TestCase("0101010140010101", "3cfaa7a7dc8720dc")]
        [TestCase("0101010120010101", "b7265f7f447ac6f3")]
        [TestCase("0101010110010101", "9db73b3c0d163f54")]
        [TestCase("0101010108010101", "8181b65babf4a975")]
        [TestCase("0101010104010101", "93c9b64042eaa240")]
        [TestCase("0101010102010101", "5570530829705592")]
        [TestCase("0101010101800101", "8638809e878787a0")]
        [TestCase("0101010101400101", "41b9a79af79ac208")]
        [TestCase("0101010101200101", "7a9be42f2009a892")]
        [TestCase("0101010101100101", "29038d56ba6d2745")]
        [TestCase("0101010101080101", "5495c6abf1e5df51")]
        [TestCase("0101010101040101", "ae13dbd561488933")]
        [TestCase("0101010101020101", "024d1ffa8904e389")]
        [TestCase("0101010101018001", "d1399712f99bf02e")]
        [TestCase("0101010101014001", "14c1d7c1cffec79e")]
        [TestCase("0101010101012001", "1de5279dae3bed6f")]
        [TestCase("0101010101011001", "e941a33f85501303")]
        [TestCase("0101010101010801", "da99dbbc9a03f379")]
        [TestCase("0101010101010401", "b7fc92f91d8e92e9")]
        [TestCase("0101010101010201", "ae8e5caa3ca04e85")]
        [TestCase("0101010101010180", "9cc62df43b6eed74")]
        [TestCase("0101010101010140", "d863dbb5c59a91a0")]
        [TestCase("0101010101010120", "a1ab2190545b91d7")]
        [TestCase("0101010101010110", "0875041e64c570f7")]
        [TestCase("0101010101010108", "5a594528bebef1cc")]
        [TestCase("0101010101010104", "fcdb3291de21f0c0")]
        [TestCase("0101010101010102", "869efd7f9f265a09")]
        public void ShouldPassVariableKeyEncryptKnownAnswerTest(string key, string cipherText)
        {
            string plaintext = "0000000000000000";
            var subject = new TdesOfb();
            var iv = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var result = subject.BlockEncrypt(new BitString(key), new BitString(plaintext), new BitString(iv));
            Assert.IsTrue((bool)result.Success);

            Assert.AreEqual((new BitString(cipherText)).ToHex(), result.CipherText.ToHex());
        }

        [Test]
        [TestCase("8000000000000000", "95f8a5e5dd31d900")]
        [TestCase("4000000000000000", "dd7f121ca5015619")]
        [TestCase("2000000000000000", "2e8653104f3834ea")]
        [TestCase("1000000000000000", "4bd388ff6cd81d4f")]
        [TestCase("0800000000000000", "20b9e767b2fb1456")]
        [TestCase("0400000000000000", "55579380d77138ef")]
        [TestCase("0200000000000000", "6cc5defaaf04512f")]
        [TestCase("0100000000000000", "0d9f279ba5d87260")]
        [TestCase("0080000000000000", "d9031b0271bd5a0a")]
        [TestCase("0040000000000000", "424250b37c3dd951")]
        [TestCase("0020000000000000", "b8061b7ecd9a21e5")]
        [TestCase("0010000000000000", "f15d0f286b65bd28")]
        [TestCase("0008000000000000", "add0cc8d6e5deba1")]
        [TestCase("0004000000000000", "e6d5f82752ad63d1")]
        [TestCase("0002000000000000", "ecbfe3bd3f591a5e")]
        [TestCase("0001000000000000", "f356834379d165cd")]
        [TestCase("0000800000000000", "2b9f982f20037fa9")]
        [TestCase("0000400000000000", "889de068a16f0be6")]
        [TestCase("0000200000000000", "e19e275d846a1298")]
        [TestCase("0000100000000000", "329a8ed523d71aec")]
        [TestCase("0000080000000000", "e7fce22557d23c97")]
        [TestCase("0000040000000000", "12a9f5817ff2d65d")]
        [TestCase("0000020000000000", "a484c3ad38dc9c19")]
        [TestCase("0000010000000000", "fbe00a8a1ef8ad72")]
        [TestCase("0000008000000000", "750d079407521363")]
        [TestCase("0000004000000000", "64feed9c724c2faf")]
        [TestCase("0000002000000000", "f02b263b328e2b60")]
        [TestCase("0000001000000000", "9d64555a9a10b852")]
        [TestCase("0000000800000000", "d106ff0bed5255d7")]
        [TestCase("0000000400000000", "e1652c6b138c64a5")]
        [TestCase("0000000200000000", "e428581186ec8f46")]
        [TestCase("0000000100000000", "aeb5f5ede22d1a36")]
        [TestCase("0000000080000000", "e943d7568aec0c5c")]
        [TestCase("0000000040000000", "df98c8276f54b04b")]
        [TestCase("0000000020000000", "b160e4680f6c696f")]
        [TestCase("0000000010000000", "fa0752b07d9c4ab8")]
        [TestCase("0000000008000000", "ca3a2b036dbc8502")]
        [TestCase("0000000004000000", "5e0905517bb59bcf")]
        [TestCase("0000000002000000", "814eeb3b91d90726")]
        [TestCase("0000000001000000", "4d49db1532919c9f")]
        [TestCase("0000000000800000", "25eb5fc3f8cf0621")]
        [TestCase("0000000000400000", "ab6a20c0620d1c6f")]
        [TestCase("0000000000200000", "79e90dbc98f92cca")]
        [TestCase("0000000000100000", "866ecedd8072bb0e")]
        [TestCase("0000000000080000", "8b54536f2f3e64a8")]
        [TestCase("0000000000040000", "ea51d3975595b86b")]
        [TestCase("0000000000020000", "caffc6ac4542de31")]
        [TestCase("0000000000010000", "8dd45a2ddf90796c")]
        [TestCase("0000000000008000", "1029d55e880ec2d0")]
        [TestCase("0000000000004000", "5d86cb23639dbea9")]
        [TestCase("0000000000002000", "1d1ca853ae7c0c5f")]
        [TestCase("0000000000001000", "ce332329248f3228")]
        [TestCase("0000000000000800", "8405d1abe24fb942")]
        [TestCase("0000000000000400", "e643d78090ca4207")]
        [TestCase("0000000000000200", "48221b9937748a23")]
        [TestCase("0000000000000100", "dd7c0bbd61fafd54")]
        [TestCase("0000000000000080", "2fbc291a570db5c4")]
        [TestCase("0000000000000040", "e07c30d7e4e26e12")]
        [TestCase("0000000000000020", "0953e2258e8e90a1")]
        [TestCase("0000000000000010", "5b711bc4ceebf2ee")]
        [TestCase("0000000000000008", "cc083f1e6d9e85f6")]
        [TestCase("0000000000000004", "d2fd8867d50d2dfe")]
        [TestCase("0000000000000002", "06e7ea22ce92708f")]
        [TestCase("0000000000000001", "166b40b44aba4bd6")]
        public void ShouldPassVariablePlainTextEncryptKnownAnswerTest(string iv, string cipherText)
        {
            string key = "0101010101010101";
            var plaintext = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var subject = new TdesOfb();
            var result = subject.BlockEncrypt(new BitString(key), new BitString(plaintext), new BitString(iv));
            Assert.IsTrue((bool)result.Success);

            Assert.AreEqual(new BitString(cipherText).ToHex(), result.CipherText.ToHex());
        }

        [Test]
        [TestCase("1046913489980131", "0000000000000000", "88d55e54f54c97b4")]
        [TestCase("1007103489988020", "0000000000000000", "0c0cc00c83ea48fd")]
        [TestCase("10071034c8980120", "0000000000000000", "83bc8ef3a6570183")]
        [TestCase("1046103489988020", "0000000000000000", "df725dcad94ea2e9")]
        [TestCase("1086911519190101", "0000000000000000", "e652b53b550be8b0")]
        [TestCase("1086911519580101", "0000000000000000", "af527120c485cbb0")]
        [TestCase("5107b01519580101", "0000000000000000", "0f04ce393db926d5")]
        [TestCase("1007b01519190101", "0000000000000000", "c9f00ffc74079067")]
        [TestCase("3107915498080101", "0000000000000000", "7cfd82a593252b4e")]
        [TestCase("3107919498080101", "0000000000000000", "cb49a2f9e91363e3")]
        [TestCase("10079115b9080140", "0000000000000000", "00b588be70d23f56")]
        [TestCase("3107911598080140", "0000000000000000", "406a9a6ab43399ae")]
        [TestCase("1007d01589980101", "0000000000000000", "6cb773611dca9ada")]
        [TestCase("9107911589980101", "0000000000000000", "67fd21c17dbb5d70")]
        [TestCase("9107d01589190101", "0000000000000000", "9592cb4110430787")]
        [TestCase("1007d01598980120", "0000000000000000", "a6b7ff68a318ddd3")]
        [TestCase("1007940498190101", "0000000000000000", "4d102196c914ca16")]
        [TestCase("0107910491190401", "0000000000000000", "2dfa9f4573594965")]
        [TestCase("0107910491190101", "0000000000000000", "b46604816c0e0774")]
        [TestCase("0107940491190401", "0000000000000000", "6e7e6221a4f34e87")]
        [TestCase("19079210981a0101", "0000000000000000", "aa85e74643233199")]
        [TestCase("1007911998190801", "0000000000000000", "2e5a19db4d1962d6")]
        [TestCase("10079119981a0801", "0000000000000000", "23a866a809d30894")]
        [TestCase("1007921098190101", "0000000000000000", "d812d961f017d320")]
        [TestCase("100791159819010b", "0000000000000000", "055605816e58608f")]
        [TestCase("1004801598190101", "0000000000000000", "abd88e8b1b7716f1")]
        [TestCase("1004801598190102", "0000000000000000", "537ac95be69da1e1")]
        [TestCase("1004801598190108", "0000000000000000", "aed0f6ae3c25cdd8")]
        [TestCase("1002911598100104", "0000000000000000", "b3e35a5ee53e7b8d")]
        [TestCase("1002911598190104", "0000000000000000", "61c79c71921a2ef8")]
        [TestCase("1002911598100201", "0000000000000000", "e2f5728f0995013c")]
        [TestCase("1002911698100101", "0000000000000000", "1aeac39a61f0a464")]
        public void ShouldPassPermuatationDecryptKnownAnswerTest(string key, string plaintext, string cipherText)
        {
            var subject = new TdesOfb();
            var iv = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var result = subject.BlockDecrypt(new BitString(key), new BitString(cipherText), new BitString(iv));
            Assert.IsTrue((bool)result.Success);

            Assert.AreEqual(new BitString(plaintext).ToHex(), result.PlainText.ToHex());
        }

        [Test]
        [TestCase("0101010101010101", "95f8a5e5dd31d900", "8000000000000000")]
        [TestCase("0101010101010101", "dd7f121ca5015619", "4000000000000000")]
        [TestCase("0101010101010101", "2e8653104f3834ea", "2000000000000000")]
        [TestCase("0101010101010101", "4bd388ff6cd81d4f", "1000000000000000")]
        [TestCase("0101010101010101", "20b9e767b2fb1456", "0800000000000000")]
        [TestCase("0101010101010101", "55579380d77138ef", "0400000000000000")]
        [TestCase("0101010101010101", "6cc5defaaf04512f", "0200000000000000")]
        [TestCase("0101010101010101", "0d9f279ba5d87260", "0100000000000000")]
        [TestCase("0101010101010101", "d9031b0271bd5a0a", "0080000000000000")]
        [TestCase("0101010101010101", "424250b37c3dd951", "0040000000000000")]
        [TestCase("0101010101010101", "b8061b7ecd9a21e5", "0020000000000000")]
        [TestCase("0101010101010101", "f15d0f286b65bd28", "0010000000000000")]
        [TestCase("0101010101010101", "add0cc8d6e5deba1", "0008000000000000")]
        [TestCase("0101010101010101", "e6d5f82752ad63d1", "0004000000000000")]
        [TestCase("0101010101010101", "ecbfe3bd3f591a5e", "0002000000000000")]
        [TestCase("0101010101010101", "f356834379d165cd", "0001000000000000")]
        [TestCase("0101010101010101", "2b9f982f20037fa9", "0000800000000000")]
        [TestCase("0101010101010101", "889de068a16f0be6", "0000400000000000")]
        [TestCase("0101010101010101", "e19e275d846a1298", "0000200000000000")]
        [TestCase("0101010101010101", "329a8ed523d71aec", "0000100000000000")]
        [TestCase("0101010101010101", "e7fce22557d23c97", "0000080000000000")]
        [TestCase("0101010101010101", "12a9f5817ff2d65d", "0000040000000000")]
        [TestCase("0101010101010101", "a484c3ad38dc9c19", "0000020000000000")]
        [TestCase("0101010101010101", "fbe00a8a1ef8ad72", "0000010000000000")]
        [TestCase("0101010101010101", "750d079407521363", "0000008000000000")]
        [TestCase("0101010101010101", "64feed9c724c2faf", "0000004000000000")]
        [TestCase("0101010101010101", "f02b263b328e2b60", "0000002000000000")]
        [TestCase("0101010101010101", "9d64555a9a10b852", "0000001000000000")]
        [TestCase("0101010101010101", "d106ff0bed5255d7", "0000000800000000")]
        [TestCase("0101010101010101", "e1652c6b138c64a5", "0000000400000000")]
        [TestCase("0101010101010101", "e428581186ec8f46", "0000000200000000")]
        [TestCase("0101010101010101", "aeb5f5ede22d1a36", "0000000100000000")]
        [TestCase("0101010101010101", "e943d7568aec0c5c", "0000000080000000")]
        [TestCase("0101010101010101", "df98c8276f54b04b", "0000000040000000")]
        [TestCase("0101010101010101", "b160e4680f6c696f", "0000000020000000")]
        [TestCase("0101010101010101", "fa0752b07d9c4ab8", "0000000010000000")]
        [TestCase("0101010101010101", "ca3a2b036dbc8502", "0000000008000000")]
        [TestCase("0101010101010101", "5e0905517bb59bcf", "0000000004000000")]
        [TestCase("0101010101010101", "814eeb3b91d90726", "0000000002000000")]
        [TestCase("0101010101010101", "4d49db1532919c9f", "0000000001000000")]
        [TestCase("0101010101010101", "25eb5fc3f8cf0621", "0000000000800000")]
        [TestCase("0101010101010101", "ab6a20c0620d1c6f", "0000000000400000")]
        [TestCase("0101010101010101", "79e90dbc98f92cca", "0000000000200000")]
        [TestCase("0101010101010101", "866ecedd8072bb0e", "0000000000100000")]
        [TestCase("0101010101010101", "8b54536f2f3e64a8", "0000000000080000")]
        [TestCase("0101010101010101", "ea51d3975595b86b", "0000000000040000")]
        [TestCase("0101010101010101", "caffc6ac4542de31", "0000000000020000")]
        [TestCase("0101010101010101", "8dd45a2ddf90796c", "0000000000010000")]
        [TestCase("0101010101010101", "1029d55e880ec2d0", "0000000000008000")]
        [TestCase("0101010101010101", "5d86cb23639dbea9", "0000000000004000")]
        [TestCase("0101010101010101", "1d1ca853ae7c0c5f", "0000000000002000")]
        [TestCase("0101010101010101", "ce332329248f3228", "0000000000001000")]
        [TestCase("0101010101010101", "8405d1abe24fb942", "0000000000000800")]
        [TestCase("0101010101010101", "e643d78090ca4207", "0000000000000400")]
        [TestCase("0101010101010101", "48221b9937748a23", "0000000000000200")]
        [TestCase("0101010101010101", "dd7c0bbd61fafd54", "0000000000000100")]
        [TestCase("0101010101010101", "2fbc291a570db5c4", "0000000000000080")]
        [TestCase("0101010101010101", "e07c30d7e4e26e12", "0000000000000040")]
        [TestCase("0101010101010101", "0953e2258e8e90a1", "0000000000000020")]
        [TestCase("0101010101010101", "5b711bc4ceebf2ee", "0000000000000010")]
        [TestCase("0101010101010101", "cc083f1e6d9e85f6", "0000000000000008")]
        [TestCase("0101010101010101", "d2fd8867d50d2dfe", "0000000000000004")]
        [TestCase("0101010101010101", "06e7ea22ce92708f", "0000000000000002")]
        [TestCase("0101010101010101", "166b40b44aba4bd6", "0000000000000001")]
        public void ShouldPassInversePermuatationDecryptKnownAnswerTest(string key, string plaintext, string iv)
        {
            var subject = new TdesOfb();
            var cipherText = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var result = subject.BlockDecrypt(new BitString(key), new BitString(cipherText), new BitString(iv));
            Assert.IsTrue((bool)result.Success);

            Assert.AreEqual(new BitString(plaintext).ToHex(), result.PlainText.ToHex());
        }

        [Test]
        [TestCase("7ca110454a1a6e57", "01a1d6d039776742", "690f5b0d9a26939b")]
        [TestCase("0131d9619dc1376e", "5cd54ca83def57da", "7a389d10354bd271")]
        [TestCase("07a1133e4a0b2686", "0248d43806f67172", "868ebb51cab4599a")]
        [TestCase("3849674c2602319e", "51454b582ddf440a", "7178876e01f19b2a")]
        [TestCase("04b915ba43feb5b6", "42fd443059577fa2", "af37fb421f8c4095")]
        [TestCase("0113b970fd34f2ce", "059b5e0851cf143a", "86a560f10ec6d85b")]
        [TestCase("0170f175468fb5e6", "0756d8e0774761d2", "0cd3da020021dc09")]
        [TestCase("43297fad38e373fe", "762514b829bf486a", "ea676b2cb7db2b7a")]
        [TestCase("07a7137045da2a16", "3bdd119049372802", "dfd64a815caf1a0f")]
        [TestCase("04689104c2fd3b2f", "26955f6835af609a", "5c513c9c4886c088")]
        [TestCase("37d06bb516cb7546", "164d5e404f275232", "0a2aeeae3ff4ab77")]
        [TestCase("1f08260d1ac2465e", "6b056e18759f5cca", "ef1bf03e5dfa575a")]
        [TestCase("584023641aba6176", "004bd6ef09176062", "88bf0db6d70dee56")]
        [TestCase("025816164629b007", "480d39006ee762f2", "a1f9915541020b56")]
        [TestCase("49793ebc79b3258f", "437540c8698f3cfa", "6fbf1cafcffd0556")]
        [TestCase("4fb05e1515ab73a7", "072d43a077075292", "2f22e49bab7ca1ac")]
        [TestCase("49e95d6d4ca229bf", "02fe55778117f12a", "5a6b612cc26cce4a")]
        [TestCase("018310dc409b26d6", "1d9d5c5018f728c2", "5f4c038ed12b2e41")]
        [TestCase("1c587f1c13924fef", "305532286d6f295a", "63fac0d034d9f793")]
        public void ShouldPassSubstitutionTableDecryptKnownAnswerTest(string key, string iv, string cipherText)
        {
            var subject = new TdesOfb();
            var plaintext = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var result = subject.BlockDecrypt(new BitString(key), new BitString(cipherText), new BitString(iv));
            Assert.IsTrue((bool)result.Success);


            Assert.AreEqual(new BitString(plaintext).ToHex(), result.PlainText.ToHex());
        }


        [Test]
        [TestCase("8001010101010101", "95a8d72813daa94d")]
        [TestCase("4001010101010101", "0eec1487dd8c26d5")]
        [TestCase("2001010101010101", "7ad16ffb79c45926")]
        [TestCase("1001010101010101", "d3746294ca6a6cf3")]
        [TestCase("0801010101010101", "809f5f873c1fd761")]
        [TestCase("0401010101010101", "c02faffec989d1fc")]
        [TestCase("0201010101010101", "4615aa1d33e72f10")]
        [TestCase("0180010101010101", "2055123350c00858")]
        [TestCase("0140010101010101", "df3b99d6577397c8")]
        [TestCase("0120010101010101", "31fe17369b5288c9")]
        [TestCase("0110010101010101", "dfdd3cc64dae1642")]
        [TestCase("0108010101010101", "178c83ce2b399d94")]
        [TestCase("0104010101010101", "50f636324a9b7f80")]
        [TestCase("0102010101010101", "a8468ee3bc18f06d")]
        [TestCase("0101800101010101", "a2dc9e92fd3cde92")]
        [TestCase("0101400101010101", "cac09f797d031287")]
        [TestCase("0101200101010101", "90ba680b22aeb525")]
        [TestCase("0101100101010101", "ce7a24f350e280b6")]
        [TestCase("0101080101010101", "882bff0aa01a0b87")]
        [TestCase("0101040101010101", "25610288924511c2")]
        [TestCase("0101020101010101", "c71516c29c75d170")]
        [TestCase("0101018001010101", "5199c29a52c9f059")]
        [TestCase("0101014001010101", "c22f0a294a71f29f")]
        [TestCase("0101012001010101", "ee371483714c02ea")]
        [TestCase("0101011001010101", "a81fbd448f9e522f")]
        [TestCase("0101010801010101", "4f644c92e192dfed")]
        [TestCase("0101010401010101", "1afa9a66a6df92ae")]
        [TestCase("0101010201010101", "b3c1cc715cb879d8")]
        [TestCase("0101010180010101", "19d032e64ab0bd8b")]
        [TestCase("0101010140010101", "3cfaa7a7dc8720dc")]
        [TestCase("0101010120010101", "b7265f7f447ac6f3")]
        [TestCase("0101010110010101", "9db73b3c0d163f54")]
        [TestCase("0101010108010101", "8181b65babf4a975")]
        [TestCase("0101010104010101", "93c9b64042eaa240")]
        [TestCase("0101010102010101", "5570530829705592")]
        [TestCase("0101010101800101", "8638809e878787a0")]
        [TestCase("0101010101400101", "41b9a79af79ac208")]
        [TestCase("0101010101200101", "7a9be42f2009a892")]
        [TestCase("0101010101100101", "29038d56ba6d2745")]
        [TestCase("0101010101080101", "5495c6abf1e5df51")]
        [TestCase("0101010101040101", "ae13dbd561488933")]
        [TestCase("0101010101020101", "024d1ffa8904e389")]
        [TestCase("0101010101018001", "d1399712f99bf02e")]
        [TestCase("0101010101014001", "14c1d7c1cffec79e")]
        [TestCase("0101010101012001", "1de5279dae3bed6f")]
        [TestCase("0101010101011001", "e941a33f85501303")]
        [TestCase("0101010101010801", "da99dbbc9a03f379")]
        [TestCase("0101010101010401", "b7fc92f91d8e92e9")]
        [TestCase("0101010101010201", "ae8e5caa3ca04e85")]
        [TestCase("0101010101010180", "9cc62df43b6eed74")]
        [TestCase("0101010101010140", "d863dbb5c59a91a0")]
        [TestCase("0101010101010120", "a1ab2190545b91d7")]
        [TestCase("0101010101010110", "0875041e64c570f7")]
        [TestCase("0101010101010108", "5a594528bebef1cc")]
        [TestCase("0101010101010104", "fcdb3291de21f0c0")]
        [TestCase("0101010101010102", "869efd7f9f265a09")]
        public void ShouldPassVariableKeyDecryptKnownAnswerTest(string key, string cipherText)
        {
            string plaintext = "0000000000000000";
            var iv = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var subject = new TdesOfb();
            var result = subject.BlockDecrypt(new BitString(key), new BitString(cipherText), new BitString(iv));
            Assert.IsTrue((bool)result.Success);

            Assert.AreEqual(new BitString(plaintext).ToHex(), result.PlainText.ToHex());
        }

        [Test]
        [TestCase("8000000000000000", "95f8a5e5dd31d900")]
        [TestCase("4000000000000000", "dd7f121ca5015619")]
        [TestCase("2000000000000000", "2e8653104f3834ea")]
        [TestCase("1000000000000000", "4bd388ff6cd81d4f")]
        [TestCase("0800000000000000", "20b9e767b2fb1456")]
        [TestCase("0400000000000000", "55579380d77138ef")]
        [TestCase("0200000000000000", "6cc5defaaf04512f")]
        [TestCase("0100000000000000", "0d9f279ba5d87260")]
        [TestCase("0080000000000000", "d9031b0271bd5a0a")]
        [TestCase("0040000000000000", "424250b37c3dd951")]
        [TestCase("0020000000000000", "b8061b7ecd9a21e5")]
        [TestCase("0010000000000000", "f15d0f286b65bd28")]
        [TestCase("0008000000000000", "add0cc8d6e5deba1")]
        [TestCase("0004000000000000", "e6d5f82752ad63d1")]
        [TestCase("0002000000000000", "ecbfe3bd3f591a5e")]
        [TestCase("0001000000000000", "f356834379d165cd")]
        [TestCase("0000800000000000", "2b9f982f20037fa9")]
        [TestCase("0000400000000000", "889de068a16f0be6")]
        [TestCase("0000200000000000", "e19e275d846a1298")]
        [TestCase("0000100000000000", "329a8ed523d71aec")]
        [TestCase("0000080000000000", "e7fce22557d23c97")]
        [TestCase("0000040000000000", "12a9f5817ff2d65d")]
        [TestCase("0000020000000000", "a484c3ad38dc9c19")]
        [TestCase("0000010000000000", "fbe00a8a1ef8ad72")]
        [TestCase("0000008000000000", "750d079407521363")]
        [TestCase("0000004000000000", "64feed9c724c2faf")]
        [TestCase("0000002000000000", "f02b263b328e2b60")]
        [TestCase("0000001000000000", "9d64555a9a10b852")]
        [TestCase("0000000800000000", "d106ff0bed5255d7")]
        [TestCase("0000000400000000", "e1652c6b138c64a5")]
        [TestCase("0000000200000000", "e428581186ec8f46")]
        [TestCase("0000000100000000", "aeb5f5ede22d1a36")]
        [TestCase("0000000080000000", "e943d7568aec0c5c")]
        [TestCase("0000000040000000", "df98c8276f54b04b")]
        [TestCase("0000000020000000", "b160e4680f6c696f")]
        [TestCase("0000000010000000", "fa0752b07d9c4ab8")]
        [TestCase("0000000008000000", "ca3a2b036dbc8502")]
        [TestCase("0000000004000000", "5e0905517bb59bcf")]
        [TestCase("0000000002000000", "814eeb3b91d90726")]
        [TestCase("0000000001000000", "4d49db1532919c9f")]
        [TestCase("0000000000800000", "25eb5fc3f8cf0621")]
        [TestCase("0000000000400000", "ab6a20c0620d1c6f")]
        [TestCase("0000000000200000", "79e90dbc98f92cca")]
        [TestCase("0000000000100000", "866ecedd8072bb0e")]
        [TestCase("0000000000080000", "8b54536f2f3e64a8")]
        [TestCase("0000000000040000", "ea51d3975595b86b")]
        [TestCase("0000000000020000", "caffc6ac4542de31")]
        [TestCase("0000000000010000", "8dd45a2ddf90796c")]
        [TestCase("0000000000008000", "1029d55e880ec2d0")]
        [TestCase("0000000000004000", "5d86cb23639dbea9")]
        [TestCase("0000000000002000", "1d1ca853ae7c0c5f")]
        [TestCase("0000000000001000", "ce332329248f3228")]
        [TestCase("0000000000000800", "8405d1abe24fb942")]
        [TestCase("0000000000000400", "e643d78090ca4207")]
        [TestCase("0000000000000200", "48221b9937748a23")]
        [TestCase("0000000000000100", "dd7c0bbd61fafd54")]
        [TestCase("0000000000000080", "2fbc291a570db5c4")]
        [TestCase("0000000000000040", "e07c30d7e4e26e12")]
        [TestCase("0000000000000020", "0953e2258e8e90a1")]
        [TestCase("0000000000000010", "5b711bc4ceebf2ee")]
        [TestCase("0000000000000008", "cc083f1e6d9e85f6")]
        [TestCase("0000000000000004", "d2fd8867d50d2dfe")]
        [TestCase("0000000000000002", "06e7ea22ce92708f")]
        [TestCase("0000000000000001", "166b40b44aba4bd6")]
        public void ShouldPassVariablePlainTextDecryptKnownAnswerTest(string plaintext, string iv)
        {
            string key = "0101010101010101";
            var cipherText = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var subject = new TdesOfb();
            var result = subject.BlockDecrypt(new BitString(key), new BitString(cipherText), new BitString(iv));
            Assert.IsTrue((bool)result.Success);

            Assert.AreEqual(new BitString(plaintext).ToHex(), result.PlainText.ToHex());
        }

        [Test]
        [TestCase("2fd5fb2aea3462a120e6cd5b20f2bc8ff48a9e58f1cb89d5", "1018c71607c83787", "bb68f8f6a0a2c749", "95a87d4e54522958")]
        [TestCase("b9e0e032375ebae04cda7cab1c3d2f5119d346f208ea83f7", "b8eab3ef61c7d1b4", "b78e05dc12ecdfb59291c3cd3bff7dd2", "070dbff0d1333afe81f84ca3510f0ce2")]
        public void ShouldPassMultiBlockEncryptTests(string key, string iv, string plaintext, string cipherText)
        {
            var subject = new TdesOfb();
            var result = subject.BlockEncrypt(new BitString(key), new BitString(plaintext), new BitString(iv));
            Assert.IsTrue((bool)result.Success);

            Assert.AreEqual(new BitString(cipherText).ToHex(), result.CipherText.ToHex());
        }

        [Test]
        [TestCase("198f973b5462df64946245e0b6ec0d98a4e5136ba75294b3", "60dd7b8f00554bb6003161ba62fd66adc2757a40dbf5d6d84d30b0e49e255f6d7e4fc0ee4f1cd867e4a1bbad898cac04445a85ef5bca5a471691598bc64ff47706c243d84139a39a", "b141cecac08330b54c182e9115881265693cd5a84e505c903dd81757fc351dfcc6ecdd23672d24761c7cb05978abc45dd4a5dc34ae11d65eaa02606aac3bffc76f5d6c4a7d5d17b0")]
        [TestCase("c1da626de38389f11cc28fe0f8fdb623374ccb495d20c81f", "5fe074a3c30b281ec7db62b76ddbcb7d51c784242ffbc410a42ac2b03953d50d9df1d9a33273d66fcbebdc49b50a3174f44caf74ce70671f8e2b8af7821d8ab746047c2c4430c1467c37e56f81e9c71c", "d7f5edf35f207dfcdae580d4d903a3c289da22a9ec1c0e06ffd333f47773d3193570cc048571b923d21a1871eb228f4155455535da42e71289764000356acf089605a5f57aba25533bda531db14f6553")]
        public void ShouldFailMultiBlockEncryptTests(string key, string plaintext, string cipherText)
        {
            var subject = new TdesOfb();
            var iv = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var result = subject.BlockEncrypt(new BitString(key), new BitString(iv), new BitString(plaintext));
            Assert.IsTrue((bool)result.Success);

            Assert.AreNotEqual(new BitString(cipherText), result.CipherText);
        }

        [Test]
        [TestCase("3e37e6fd7f15a7eff7f2ce83759bae97296e4fbfabe91025", "136aa27207e95411", "54a3be1f0886d8e1", "4b3581305873de1b")]
        [TestCase("4c9229576b04ad837fa1513804c215ae9efd9e38e3a8765b", "47302567b055db1d", "e513b748ea166b1328533e2879838002e060f7fc0bfc18e2", "548f67b8e6ea5ef313085d30f1f2546f0e515a38618d44de")]
        public void ShouldPassMultiBlockDecryptTests(string key, string iv, string plaintext, string cipherText)
        {
            var subject = new TdesOfb();
            var result = subject.BlockDecrypt(new BitString(key), new BitString(cipherText), new BitString(iv));
            Assert.IsTrue((bool)result.Success);

            Assert.AreEqual(new BitString(plaintext).ToHex(), result.PlainText.ToHex());
        }




        [Test]
        [TestCase("198f973b5462df64946245e0b6ec0d98a4e5136ba75294b3", "60dd7b8f00554bb6003161ba62fd66adc2757a40dbf5d6d84d30b0e49e255f6d7e4fc0ee4f1cd867e4a1bbad898cac04445a85ef5bca5a471691598bc64ff47706c243d84139a39a", "b141cecac08330b54c182e9115881265693cd5a84e505c903dd81757fc351dfcc6ecdd23672d24761c7cb05978abc45dd4a5dc34ae11d65eaa02606aac3bffc76f5d6c4a7d5d17b0")]
        [TestCase("c1da626de38389f11cc28fe0f8fdb623374ccb495d20c81f", "5fe074a3c30b281ec7db62b76ddbcb7d51c784242ffbc410a42ac2b03953d50d9df1d9a33273d66fcbebdc49b50a3174f44caf74ce70671f8e2b8af7821d8ab746047c2c4430c1467c37e56f81e9c71c", "d7f5edf35f207dfcdae580d4d903a3c289da22a9ec1c0e06ffd333f47773d3193570cc048571b923d21a1871eb228f4155455535da42e71289764000356acf089605a5f57aba25533bda531db14f6553")]
        public void ShouldFailMultiBlockDecryptTests(string key, string plaintext, string cipherText)
        {
            var subject = new TdesOfb();
            var iv = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var result = subject.BlockDecrypt(new BitString(key), new BitString(iv), new BitString(cipherText));
            Assert.IsTrue((bool)result.Success);

            Assert.AreNotEqual(new BitString(plaintext), result.PlainText);
        }

    }
}

