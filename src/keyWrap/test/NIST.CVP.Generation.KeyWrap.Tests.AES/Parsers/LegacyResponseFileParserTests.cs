using System;
using System.IO;
using System.Linq;
using NIST.CVP.Generation.KeyWrap.AES;
using NIST.CVP.Generation.KeyWrap.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KeyWrap.Tests.AES.Parsers
{
    [TestFixture, UnitTest]
    public class LegacyResponseFileParserTests
    {

        private string _unitTestPath;
        private const string _FILE_EXTENSION = ".fax";
        private const string _FOLDER_NAME = "AES";
        private int expectedNumberOfGroups = 10;
        private int expectedNumberOfTests = 50;
        #region File
        private string _testFIleContents = @"
# CAVS 20.2
# Generated on Tue May 16 07:36:07 2017

[PLAINTEXT LENGTH = 128]

COUNT = 0
K = 7dbe3ae26f2939458b0eb34379630766
C = a6262ef73fcb5dc8c1a0191b20ab33e14d7e5f3538073c2a
P = 277ce89929cbbef9e4bfb645eba1b377

COUNT = 1
K = 281854d8ca28b3bf3d0a5c706b3cd050
C = 7d15bbb29df7a5f7fc7d30024e683a56c11453171a97a205
P = cdd860589fbe313cf8bcac483e02faf4

COUNT = 2
K = 651aa921b0ff7a79dc2cb8d73e2fb093
C = 8f90ede456d99294dbbb18fdb7b6cdaa5affd3a2d223fc80
P = 985743d02cca6650668fa0d4819c0cd3

COUNT = 3
K = 2659347489234586452d550332039cea
C = 306e680a978f918834a11d82152cc0d89d048acb4c4fec9a
P = 8fd053e79e1bdede4bc3040607886e1f

COUNT = 4
K = 49abd77133d71563b9ecfb43f916a167
C = a13ad9fd1c11259185f7ebde17294920aa2d9338e2ba635d
FAIL

[PLAINTEXT LENGTH = 256]

COUNT = 0
K = 1b7ee707afdf86d0866e0c1a6d2f25c2
C = f94c270cc624370d075218ee5896f813a7deefd42ea4f1f6203206decca77e5c7a0a4bf2239dde6f
FAIL

COUNT = 1
K = 10ea3f83d4aeaee21ad656e319d19a8a
C = f5e33c6dc93283e4d39099c1b803d9fecead7e11b5936733f86ee803959b0f82ac409ed675a86f3a
P = 9aa49a746e3e799ea9869f3d85c539b582273d052b55a9216170a0ca06dda9eb

COUNT = 2
K = a5558fbc29f240651abf14255cd1847b
C = e44846fd728b8788d33112442eb7848e417604bf509daa7f9c653bf52ded9928591bd5d5c6d3cbd5
P = 56e3435cfabdf5706a3ea17d39c6d0dde2a11cbc1b9af698f558d7d2693449e9

COUNT = 3
K = b4ddb83c4d5402d528a381c6e02070d5
C = 958266cd16fd5679f8370b2c8f6e2357a3c4ee921391fbe6d910742c1233b6f01ba32c6b33f8ff93
P = 5226255a869136a585e8c1dd1f6b965d2de7b9ffa3936b9ab7a3d398f885acca

COUNT = 4
K = 12b358d3ca07343c365584357b493ef6
C = a72c1650670b8cd78c483e026669149302f476b20c68bbb845c6a9134c75a2a5c098b384f65b5fd9
P = fe0d328ad3477c6db5c6ca1e0a05718526703ad878a950fcdf3b7d3325d241e8

[PLAINTEXT LENGTH = 192]

COUNT = 0
K = 9aee3378ebaaa7f3d41fcd0c56fc1ae4
C = 0b849fecace13df0d97309895bf0a706bbb6215ca902937461d90deea2eaa988
P = 69109e6df20aa1ddb3fb03d901370993c11732d0faba2cb9

COUNT = 1
K = 20074014475587b28d95fa81a86abb60
C = 42d60ca81ea5b5fb401bb3f025932f82e235b85f58a8685ae2092a3062bb56da
FAIL

COUNT = 2
K = 9a61ba7e92f9d0cca4b4b39af1dcf4c4
C = da664fddd79a6a5d5a40a60c05f776bcd140fdd55061b1c9761637ea74ac4241
P = d20a264d2076f560a7e06025a577910e767c4cb89ca100cc

COUNT = 3
K = a57910c670f73129e9f8e9903db30089
C = 47ab54442d5bec6942baa8cc723b0620c07ca8b54b89d529e3d9a9c646a6d6f2
P = 1c8d26771c44a971ef3c834616fe9295bbd32961b6fe860b

COUNT = 4
K = f4a8ac5ebc1f4585acb885fc1002d3ac
C = c4fd38c96c4b1ea17861223dccdf4c4234c2aa569e919b9e2a94bb8f0c66ae78
P = 1adbcf01ce9b3c6f9eb6e5d97a05002d7c383954976ee767

[PLAINTEXT LENGTH = 320]

COUNT = 0
K = 06cd59fc01fd5c8f2a51727d91014e0f
C = c6a2abd0b0ce4dc6656219de03f34bf2495e2ff148762b581f651bacdeab1aaa0e21ee9c4f9998d696832f54d04345fc
FAIL

COUNT = 1
K = 5133fe885f8f32481a8256f6e850c9aa
C = c5e1b41b324f208a3190d486fc4df2fefa874a3cf2a97c884555eafedf751d3bf18ec4bc5845436759636239eb069560
P = 064f01de6686f5f4d3203a697053978b6232e50fad47df72913a567360d3b9afdec7b45a767eaf2c

COUNT = 2
K = 0ddcc137f02ef623070e473b1f5a378b
C = a81e1ef062cb67f9d8be7d434a21d0bbae244b8d8b9a5f14e65e0e07bba70a9fa8e54e42915d5c35bea7aa94846b8cb7
P = 58001ab23e23cacefd18614101a878a5419859264f6006b41baf769abc1cdc955a6bc641e8d4bbfe

COUNT = 3
K = b9c7675a59735f55ddfcd9b21c24f8e4
C = 8f7f2b600be45069ed54f1cf50f71482f60691df6bfe3c1a3e964216d41d69472a5b5261de0630dcc50b714fe5b60816
P = 07359944c43776199324ee2888ff4aae90da457657891aa592bea8135b19b929a9186e15708f43ef

COUNT = 4
K = 3f0ca08c1657eecb3ea1f56cf1d293ef
C = fb18fac15170dcd663181c916ac33fbcdafb9e0fd991b220c0417e4009e43f942785ea8c897afef28c5cc745b9e59f3b
P = bc912d33faa184c4a3950796b183046d1a8cdd87c519124ae6986d552cc2b6450ae8bfb2553c2b58

[PLAINTEXT LENGTH = 1024]

COUNT = 0
K = 3462cf99402f31cc6a6ae44e73e4ef3a
C = dc2644784bd9e22765048705d40e2b81f16be7995afb96a2ff61193fd09926a3e7ef85b119aaed4bbf5e278bc4535b8fd905d463adad541ab2d2c12aba9ff33e75014cba261109c7fc72fb80a32f621a9341d1881efda6e802660763caefde5b9e84b94f77eddf741823fe4340f1fc41d4cd6e3ea82bd319d5bde49506854f937dd1178ffbc0fa3f
P = 39a224049f9eb7181a61cbefbdc56c8161c2e6c423a41df1adb71fa8d16ef36f8014efc98c9859aa06ec0b98ec4a2773378c02b1db5497347b9355dc6427c4902f277888ae2997435babdef840d077ca793b49cfe3a14e609fb114c0a45184bae7bdb576bbbdcf90582b82c36ab2bfb0794076bae6eeb4dda6bb70546142fbea

COUNT = 1
K = ed6b3ecc5f82ab2bd4667d78b1ee8cec
C = 1d001de9312c0e083e56f5b44fa8fa18f62273cc9b656d14865e8d7074fbb23b4af70b54e0695d45e8e27cd0b2945bee3a01a9f1d642d07a5bbe8865d0b111b18816071c94ecc5b29f0cb0eecb10ec067bc1a89fe18d5b67a80b5692de642176708d85b743e376d39d361cee1f50e376d0939fd7594b06a78c3337f0e9b2f2f83e2cc5ba919f8185
FAIL

COUNT = 2
K = da723efdd075ebd4a57983fadccafdc9
C = faafc2c077714e4acd416bb840808a0cf870f33bc4b2eead91aff01c0edb542282ba5f7fbe96ffd731cc89ff81284cbfca95c1127ee9b44eee45e0a1295a4995c49eab251efd16272a64e84f0dd5163aa139b3de48bc795a3079073ddb768a20534c7ad2e5c996a9c01d112cec8417fabb1030da3aa3f33c1869f6e38f1d927aaf56455b0d4503cb
P = 9ae2095bf345fc59db26be33b7413117db2d296b1d265bdf8d630d1382ae36f2b61e7d5d40b0d3e2527c7bbae100bbad2676e60e1a80a8fcce91e386f2258922fdd88c6c40eb5e75784c224fa5fc5688dc4b0b2d3a6ccc9c4cfe6c812d3593c4039243f38e58a1e00b55702787d9b463c67902f213f3405f51a8c8186bba357a

COUNT = 3
K = f1fa748383342d82bf35a51a3463ed89
C = 47a8740b99dff596daab3266aac5b70fd8cd4828f478cacd47d8570ed186a32e8a56b97ad50f9481b08a64aa945a1471006d4dd75f29c945b1d8501addbdcd9676516c94e51c7073dbd82bb8c4d97abdc0c5c591f15d79f173458f3057577ec003db52ba1c5f834b36a28c0c3f70cddc6034a88f2cd978967837a7c0ba18d8253aab350a33232862
P = 32898c9e533b389e61057124e99f9bfabe54279278392f1ae28f4d34d847630bd9286e414147c83440a26cf41ee5545945b119280d5c16f9b4346ed27227c66d54a3bb6cb3ba75df423aeabce8bdc02215edfecd1a35498bf72b702e5e0008e90ebb9a83ccf8e19947e4db6a806ce021cb7ddb0d3ad3e5d75f425345074305d9

COUNT = 4
K = 93ce8af59d405e85109b037caf365ae9
C = f82427a97146edd5fd60f3d10d8913f1c4bde0e88423996f94487492ba82e760f7aad110a5dc5170c1b1646464482189ccb4d9cd4071432a007f3515ef49a4ce5f20a094e515c7e9c20022909ee49a9238cec1a86bcf2a2a75a55e3d1bfcb0acc4e162158c0f9247dce8a9ad6c65f63ff7797f8c1bf91a6d31301497e6cfdb0b4eb480f01ac1dbe7
P = 182b244d51ddcb004cbd8db2c20e9f6de3b61069b32e0b977ff19ef94183c24b68ef627e3e6ffafca61a0ce77c6f931456d564e421ea73eccd997bd75d7bedf7f66cbda56ff9398976bc802efea821b3ffb159fb99d943810f42ea1ec1647da8a2cfe425b459db1b78b22a87de1008d7892db37f48df3001f478eb259e64d19c
";
        #endregion File

        [OneTimeSetUp]
        public void Setup()
        {
            _unitTestPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\temp_LegacyFileParser");
            if (Directory.Exists(_unitTestPath))
            {
                Directory.Delete(_unitTestPath, true);
            }
            Directory.CreateDirectory(_unitTestPath);
            File.WriteAllText($@"{_unitTestPath}\KW_AD_128_inv{_FILE_EXTENSION}", _testFIleContents); // 5 groups, 5 tests per group, 25 tests total
            File.WriteAllText($@"{_unitTestPath}\KW_AE_256{_FILE_EXTENSION}", _testFIleContents); // + 5 groups, 5 tests per group, 25 tests total
            File.WriteAllText($@"{_unitTestPath}\fileThatShouldntBeParsed.dat", _testFIleContents); // + 0 (shouldn't be included)
            // Total groups = 10, total tests = 50
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Directory.Delete(_unitTestPath, true);
        }

        [Test]
        public void ShouldReturnErrorForNonExistentPath()
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup,TestCase>();
            var result = subject.Parse($@"{_unitTestPath}\{Guid.NewGuid()}\{_FOLDER_NAME}\{_FILE_EXTENSION}");
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void ShouldReturnErrorForNullOrEmptyPath(string path)
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(path);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldParseValidFile()
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(_unitTestPath);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldHaveProperNumberOfGroups()
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            Assert.AreEqual(expectedNumberOfGroups, vectorSet.TestGroups.Count);
        }

        [Test]
        public void ShouldHaveProperNumberOfTests()
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            Assert.AreEqual(expectedNumberOfTests, vectorSet.TestGroups.Sum(c => c.Tests.Count));
        }

        [Test]
        public void ShouldParseEncryptDecryptProperlyForGroups()
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var testGroups = vectorSet.TestGroups.Select(s => (TestGroup)s);
            Assert.AreEqual(expectedNumberOfGroups/2, testGroups.Count(c => c.Direction == "encrypt"), "encrypt");
            Assert.AreEqual(expectedNumberOfGroups / 2, testGroups.Count(c => c.Direction == "decrypt"), "decrypt");
        }

        [Test]
        public void ShouldParseKeyLenProperlyForGroups()
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var testGroups = vectorSet.TestGroups.Select(s => (TestGroup)s);
            Assert.AreEqual(expectedNumberOfGroups / 2, testGroups.Count(c => c.KeyLength == 128), "keyLen 128");
            Assert.AreEqual(expectedNumberOfGroups / 2, testGroups.Count(c => c.KeyLength == 256), "keyLen 256");
        }

        [Test]
        public void ShouldParseKwCipherProperlyForGroups()
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var testGroups = vectorSet.TestGroups.Select(s => (TestGroup)s);
            Assert.AreEqual(expectedNumberOfGroups / 2, testGroups.Count(c => c.UseInverseCipher), "UseInverseCipher");
            Assert.AreEqual(expectedNumberOfGroups / 2, testGroups.Count(c => !c.UseInverseCipher), "!UseInverseCipher");
        }

        [Test]
        public void ShouldHaveTestsWithKeyFilled()
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var cases = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).Key != null));
            Assert.IsNotEmpty(cases);
        }

        [Test]
        public void ShouldHaveTestsWithPlainTextFilled()
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var cases = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).PlainText != null));
            Assert.IsNotEmpty(cases);
        }

        [Test]
        public void ShouldHaveTestsWithCipherTextFilled()
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var cases = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).CipherText != null));
            Assert.IsNotEmpty(cases);
        }
    }
}
