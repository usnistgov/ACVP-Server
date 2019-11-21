using NIST.CVP.Generation.TDES_CBC.v1_0;
using NIST.CVP.Generation.TDES_CBC.v1_0.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace NIST.CVP.Generation.TDES_CBC.Tests.ParserTests
{
    [TestFixture, UnitTest]
    public class LegacyResponseFileParserTests
    {

        private string _unitTestPath;
        #region File
        private string _testFIleContents = @"
# CAVS 20.2
# Config Info for : Certicom FIPS 810
# TDES Multi block Message Test for CBC
# State : Encrypt and Decrypt

[ENCRYPT]

COUNT = 0
KEY1 = e39ef443925e76a8
KEY2 = 529b3483f85b10d0
KEY3 = d9203eb6d03757ad
IV = 99a274eab5fd9d01
PLAINTEXT = 8f8f334f17d7fd49
CIPHERTEXT = 3b7c7b5a1ba6f9d5

COUNT = 1
KEY1 = bc452310e502864c
KEY2 = 767f4f983eb6452a
KEY3 = 1349cd86709bd3b5
IV = 6fd14af3eb968fa8
PLAINTEXT = 65dbf7d69380b6cf49bd2746c6e12d23
CIPHERTEXT = d1c75db6da58df87683ef8323cf08eac

COUNT = 2
KEY1 = 8cc1c1529bc18cd9
KEY2 = ae5115bf07ce4952
KEY3 = 6eef387031a75786
IV = c4444ec413c1d573
PLAINTEXT = 2acdb10d8f3dc3edee60db658bb97faf46fba3c3ca707bd5
CIPHERTEXT = 5f3d74abbe17135b1f3d4eca6075d13169d460f9eb8e12c6

COUNT = 3
KEY1 = 6b07d30d3137fb9e
KEY2 = b91ff15ba49702f2
KEY3 = 2fc492a462047361
IV = e9121506f8c0bd36
PLAINTEXT = 436999cecf20a26b0f584fa2f44fb9b7008f149caabf346704be50049bf30727
CIPHERTEXT = bcba4c9efc137610b639ac7f594046c477ad30670f844a9c180116d886c5b91b

COUNT = 4
KEY1 = 20239e979d380898
KEY2 = d6e0cde532f26bc2
KEY3 = daa7577f1326a475
IV = a0aa7f154354014f
PLAINTEXT = e61459cd98f516b7b356197662c78921ac4732e951a28407f4fedff37fb00ed7c4541143601c1f22
CIPHERTEXT = c94fc4e460737dfc9d6cc6b90a800e0349420544da60aa47cdcbbedec0e32815d63a95072e5b1be1

COUNT = 5
KEY1 = 2c4af132462a19a8
KEY2 = c2642a9132fdf7a8
KEY3 = 70f4da0752a7f464
IV = 9ded75a5281ac27f
PLAINTEXT = adedc6224a4e3b35f4e2019ecd5ad6ad35a1c6d033b3ab0716b2f5985c37a704d2e6a239fd452609b9ce96db20427ba1
CIPHERTEXT = ca0a7a8260075bead13dccce60874657cc1d66da38a78f5574b0585fccfe53f36ff030e2021d607e329b153e552613b8

COUNT = 6
KEY1 = 230b52b552d520f8
KEY2 = f11a1951fe2980ab
KEY3 = 2c2a156726eff775
IV = 7d3e01966e94c630
PLAINTEXT = 00d8b2300936759704ef85f38ad0d08a986de6bfd75b5be3209f6d4d3f67e7adf7f8469d47e81979ec8dae7127b5eadcc09779cf4b0a28ef
CIPHERTEXT = 85dfa603c91c08c3bb9bc7252f23fb87a2ca0d0e7ff7550e92cf1fb5848da41d6921d87f370c62e1f8b077413d08f9e86238da19fa665be4

COUNT = 7
KEY1 = 450e9b2ff785d5da
KEY2 = ec3e320234ab5d89
KEY3 = dfd957023eb01c0d
IV = b24fca1204fe0d9a
PLAINTEXT = 98d9a48a34940bce40257675d31cc337cda6fd46b00ecfeab2843f68101e9929a41ca818ba702c4c207e89e77c20bd3ee2ed943aa6bb1ffe6f6262c98dc34d1e
CIPHERTEXT = 3d9fe76f41f421f5948aa184a85b27b3f1e915881981afda7a2e3da693283f5e88d60b7ef4126ee0f98e4ec83663f2563aa214add36205838c21c9c14d092b66

COUNT = 8
KEY1 = 7031ad43ceea4070
KEY2 = 7f43a2316b40a46e
KEY3 = 768049b3dc1962f2
IV = 0b866c285a85ef61
PLAINTEXT = 98186fd498e2f4e2d7285756e310b91ffbaf6b9820cda6686ee97da7ad7d0f42248401a0ad382c850f209237874a97e0a8d66e3466b4d1536dadf5f1b7a8bdb45d262eddad10232b
CIPHERTEXT = 25f99643f05c8fb3cb6be1838f0abe2bd24bf299d1356115fa0691da9e7b509959555bede15dc8e0316fdacc38fb4e4f13ed1dd3900b04660029a0a801946d3e3cb8c9036dfca18b

COUNT = 9
KEY1 = 4934cde334f15b34
KEY2 = 38f7b6f75db057e5
KEY3 = a449544079491fbf
IV = f27b6fd9f8f2b061
PLAINTEXT = d41061cf1777dc8b9718d93ccfd1c763c8da14838c6046e0de7048872817018eb5c82b9d20fd63ae0d217c1016ae19b209937b9e50d897c4b0846cf937bf933f6f341847d8b9883495324ba266b234e7
CIPHERTEXT = f2b8f0607de8b31df49075ffab6f01c0c526fbf33c3c49ebfcf561c1d60a8173dd2207bd98fc52ef05a45ba961cafaf765ea190f576634036c05eadb1d8a59ef5395c5789092e703905d5dea7bbeab78

[DECRYPT]

COUNT = 0
KEY1 = 6129c11013a7c8b5
KEY2 = 4f86684c68703b58
KEY3 = 2a37da4cd9c2c819
IV = a3687cc54fb0ff6c
CIPHERTEXT = 49b450d666bf4bd0
PLAINTEXT = 8bd66b27a6f0271e

COUNT = 1
KEY1 = 3bd68a92fe5bd65e
KEY2 = e051ba866b987aba
KEY3 = d66ebc434c8a2fd9
IV = 1d1e42cdf41a069b
CIPHERTEXT = 69888d05f25b050ae0ded6c950c7adc8
PLAINTEXT = bc6299ccffe51a4dcccf39cb02a17842

COUNT = 2
KEY1 = a168c197a43edaad
KEY2 = 5104a87af80bd373
KEY3 = 5bcda289ce85706e
IV = 086ac8513c9722dd
CIPHERTEXT = 7abbdc01666bb5f73dfcfc721cac49ba010d311ad2fe6c7e
PLAINTEXT = 1b3b1ad567527cdd6aa038aa601efabefbda6a0e45869eee

COUNT = 3
KEY1 = 37e0a215c8f746b3
KEY2 = 1f4f4f190b2f0143
KEY3 = efb3e546f16bb90e
IV = ad0ca8d9a5d48002
CIPHERTEXT = 56b3eb056470419f067f7cfff5c1c33e3932f64e4a7af03176bf096882bedfaf
PLAINTEXT = 986d8c3cd88c4f830a6de5f7ec77cbc84de036a78b4847fd84e40cca6f3025e8

COUNT = 4
KEY1 = 5837167af489f2b0
KEY2 = f20e4c80130b8c6e
KEY3 = 643e855e43f4bcf4
IV = f4ae6d594acfe798
CIPHERTEXT = 4bb8f752ea5bcae73f5b0e8f30157ea8ec0967d9d031af048c9216758110bc606e86e48a4c16f746
PLAINTEXT = ac043d7796ef7a1b4ec4c71f7322ef8581608038fcdb1b5ab0be39beaca06e88e0f3537e960b2604

COUNT = 5
KEY1 = 5762bf1601e30823
KEY2 = 02200483ba6df767
KEY3 = 5dea4073b02a1c5b
IV = 84035423e5c4a5b4
CIPHERTEXT = 300ce0178d4d545640243b9db818904ae917195de7bb9adab3cc486f540580572a1bb97e80fbb8e660530349ab559700
PLAINTEXT = 038276fbb0b07ac66ab982bde4836ec1621a422b4b2ec73255576e22f2304440d0958fb04defd38da2544a65eadd0941

COUNT = 6
KEY1 = f1f8456468dafe4f
KEY2 = 52c797528ad58a61
KEY3 = 7ce0528fd31357f1
IV = d85b5ca902a66130
CIPHERTEXT = 861283a2dc9d7d2739ff2ae5ed5af5304cc176cd544a39a99064c1cb3b6bcc88a97ad9f6e381e8a3929781861e91f73516d3ee59d3661b5f
PLAINTEXT = 3fd1caf7401a5bdac7cb7928782f5e2a00de357144d022bc6722f4352813058ffa19f2162a1d5e436fae8edad56f4af0c284230fa235c229

COUNT = 7
KEY1 = 51a70152e679040d
KEY2 = 9e0e07bf6bd9689b
KEY3 = 7f04dcbcd989da75
IV = 0aac354252ed6ced
CIPHERTEXT = cbf63fb1655ea4eeadc9da2d927ddd098d19a03acb2851c58ef7df70d1a4ba792b78da87f57602e1af3446dea0eacb9dbdc9cef5cc8ee7afc9568ff389b4e101
PLAINTEXT = 9f05a8d336212d9badeaab96bb1a6e79b8021cb59e226b3b2b1808779bcb5ab73bd6fbf2619f955aaecf81ef3cff8ef16acc1759ad24f958aa0441021eda59c2

COUNT = 8
KEY1 = 434cab51890eda8c
KEY2 = 3420b03280d30b07
KEY3 = cd2c25a4e325d602
IV = b793eb4a63289a32
CIPHERTEXT = d90450388a258fce092074e3c7348ed180be1607bdaf89149c3e9701d3da3ce4f3632db2ea997555ccd560fed3f798bda49f8e4fd3a01b37aab1681026660b21435f723279c0859e
PLAINTEXT = fce25601d6d547d8cc88572b1d9f84ce8be3d3ce26524b96aed472f28604e39e732fdee4588a29bc18987708ae052f3325252091aa5b6e6a009c8167f921f1da406da5282992b3b6

COUNT = 9
KEY1 = ab7a0819a2025d9d
KEY2 = 29155119f8a10234
KEY3 = d616fd8a8a371af1
IV = e1579429f69d72c8
CIPHERTEXT = 47e2ecac878ede30276f3f448ba3c39440d57cc3ec6162b085b933f8c2077dbccf14f8a445681a2359b3e6fd546b050743b7172eef8c4bbb0f152dd6bd59c0482718cd8587f57ae955db1e42ba99548f
PLAINTEXT = a3eba1a25aa7d632cd02b2ba760d3564fca21fcefb1f1d12235b353b03feae82f3dd12a59308efe7094bf4e2e794d48f2f0ec1c150656c5e0ae647d49c3f7f2dcf94e050b30051a18d234fe5daf8c2bc
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
            File.WriteAllText($@"{_unitTestPath}\file1.rsp", _testFIleContents); // 2 groups, 18 tests 
            File.WriteAllText($@"{_unitTestPath}\file2.rsp", _testFIleContents); // + 2 groups, 18 tests
            File.WriteAllText($@"{_unitTestPath}\fileThatShouldntBeParsed.dat", _testFIleContents); // + 0 (shouldn't be included)
            //File.WriteAllText($@"{_unitTestPath}\MCT.rsp", _testFIleContents); // + 2 groups, 18 tests
            // Total groups = 6, total tests = 54
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Directory.Delete(_unitTestPath, true);
        }

        [Test]
        public void ShouldReturnErrorForNonExistentPath()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse($@"{_unitTestPath}\{Guid.NewGuid()}.rsp");
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void ShouldReturnErrorForNullOrEmptyPath(string path)
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(path);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldParseValidFile()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldHaveProperNumberOfGroups()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            Assert.AreEqual(4, vectorSet.TestGroups.Count); //todo: WE NEED TO CHANGE TO 6 WHEN WE IMPLEMENT MCT. 
        }

        [Test]
        public void ShouldHaveTestsWithIVFilled()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var casesWithIV = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).Iv != null));
            Assert.IsNotEmpty(casesWithIV);
        }

        [Test]
        public void ShouldHaveTestsWithPlainTextFilled()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var casesWithPlainText = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).PlainText != null));
            Assert.IsNotEmpty(casesWithPlainText);
        }

        [Test]
        public void ShouldHaveTestsWithCipherTextFilled()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var casesWithCipherText = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).CipherText != null));
            Assert.IsNotEmpty(casesWithCipherText);
        }

        [Test]
        public void ShouldParseValidDecryptFile()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldParseDecryptFromFileContents()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            Assume.That(vectorSet.TestGroups.Count() == 4); //todo: WE NEED TO CHANGE TO 6 WHEN WE IMPLEMENT MCT. 
            var decryptCount = vectorSet.TestGroups.Count(g => ((TestGroup)g).Function.ToLower() == "decrypt");
            Assert.AreEqual(2, decryptCount, decryptCount.ToString()); //todo: WE NEED TO CHANGE TO 3 WHEN WE IMPLEMENT MCT. 
        }

        [Test]
        public void ShouldParseEncryptFromFileContents()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            Assume.That(vectorSet.TestGroups.Count() == 4); //todo: WE NEED TO CHANGE TO 6 WHEN WE IMPLEMENT MCT. 
            var encryptCount = vectorSet.TestGroups.Count(g => ((TestGroup)g).Function.ToLower() == "encrypt");
            Assert.AreEqual(2, encryptCount, encryptCount.ToString()); //todo: WE NEED TO CHANGE TO 3 WHEN WE IMPLEMENT MCT. 

        }
    }
}
