using System;
using System.IO;
using System.Linq;
using NIST.CVP.Generation.AES_CBC.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CBC.Tests.Parsers
{
    [TestFixture, UnitTest]
    public class LegacyResponseFileParserTests
    {

        private string _unitTestPath;
        private int _groupsPerFile = 2;
        private int _TestsPerGroup = 9;
        #region File
        private string _testFIleContents = @"
# CAVS 20.2
# State : Encrypt and Decrypt
# Key Length : 128

[ENCRYPT]

COUNT = 0
KEY = bfe8cd7528f8fc074e3388ef38d3d1d7
IV = 64cd6b75579c3f703f9f457148a03127
PLAINTEXT = 524ae9a41d6274ce7fdb6bffe5fd4ad7
CIPHERTEXT = a8c8a7f3e1cc6f8db297280775dab0e9

COUNT = 1
KEY = d390ccb358c90a7bbcae6b6af627c357
IV = 40e6564b4f8b2eacaf1e9c36da81028d
PLAINTEXT = bee038a5f183e13ac58e68e7312e1c568a768f5aa4f6e2253a9042bb8f87b3a7
CIPHERTEXT = 6601d80a07b0a796f25bec1a2e443985f5e72e336338b365ef2af987dc6ff7c3

COUNT = 2
KEY = a2657763ac5ba00a8d6c86464b1eebe4
IV = ffd8a001a1a852c40f7fa77432b37129
PLAINTEXT = aaf58d8ab9698eaf0c67f4a5f87d5f98c26fd87d62e5b75e8662c3f2da759c71873356a6f9e46746e6fdc3808c4f45d5
CIPHERTEXT = cc6a553915f1f7328f67a7ee3406995daebfe4c8360c7a29b680163faad490a9021c03cc58edfe8b577bdf4229fa6e39

COUNT = 3
KEY = d7e942ba3da0a24cb2fe8eb42fac5b0a
IV = 6a9a97a45ad1150005d72e71c2b49e50
PLAINTEXT = 8354680358d51b844be18db080562f080757ab560ea9bf37555d4cd7e2b0aafcf52f3f463e1f50b54e1b166b4cff15b3e905a943952ee2bef4e4917d8bdda38f
CIPHERTEXT = 9f6e8710667aa6d8e24b3522f86ee7f49f07318c48da263ee03df3ba1e1bc9e954a78c1c433d0152c890bbe29f9f5a1cd294eb907e55d35bf664c6495ebf9d90

COUNT = 4
KEY = c4f9aa56070935cab0422c3e885d374b
IV = 15a710e7dc8520f82766764f567b8b54
PLAINTEXT = f1fdcdf2740b1be0673dadf9ed73edd2db86649d24e6d0f3958177d1c2e7bce040acf0f9140e6ae86cd8da708d87da900daac605428fce10fa1b49796cfa4cd46ff97c03eaaf20fc8c637bcdbd2a58dd
CIPHERTEXT = 686bb34f8caaf3bb5a0998ab05c4d58abd85b2a4f5464cdf01e9281ea8a8fe1c55c5ee7cc3c3f2fbb1e8638c5ec2e8a247628f1fac91c57f2fad2d259ba10ba7bffe168ba35aab339e3a4265ae68029a

COUNT = 5
KEY = cc8ea7eb6554deecaddc692aa03fc8ab
IV = 35d701fe8a90e94bcf785002292f1ef9
PLAINTEXT = 266f37a9e1be0d19f981e2843f59a1b2af5dd2a298b5d4c4fd6566693b666d77edfbf1610d4268565c0937b903e476d27a7cf3c3a92a7abd4dc554739eb80ecf2f577ce41573fc254b2bffd45506c6bd243f6a233a601ffb0e12b958b2347027
CIPHERTEXT = 7d145732cfb146770ac8588428f12c5cd087171e0c0c02a00893db8da706978ab126c59a391785db46c5bdef0994db9abceb7bf809b5d300ecaa8863974d05895e428817ecc756b66bb9a5f425ded2ffa21d648eef9d957b2c510fb340f14484

COUNT = 6
KEY = 4c9aacf93aeca67c9d4076e60f14b245
IV = e7abc7e720d9f18ef72abc3819a7d962
PLAINTEXT = 2bd04180fd0ac1b7797624d67d83efe641a0a2f1f25082ba9d819bab4f1a060f7bd368819b49bba41a525115728d159e923e15ad82593d0449dd65b10f638f417ff769316f261aa5c3583a5c8e6122e47b52670a6c95a195603971349a8ac2b400e54e76c1ca64213a33e9941b249e1f
CIPHERTEXT = a130e76df6216c27ef4c080ac94ce3860072b629971d577306bdcbbffc8df0e094fd2fbb8b40acbe4b1878a0a3c8ceb0502526a39d2c2368676c9631561c07cc9ea5c6b7bce7ce4d84ef91e63b985a1103d8baf982787bc36b449f41e7629587199c90edbad149e5f8ce76703a5c4878

COUNT = 7
KEY = a8537f63a879d92ffba0477f7effb0ee
IV = 924a2b9c7fde9c05508b70310fc4a33f
PLAINTEXT = f1cd83b49256e5d2afddef1406bfdcdb4871987166b825e30b1c3d0f699a39fd39993173fd1f65c38945dbe8c1e84ad1a4a237fd8af85e48a9d55305de38694cab29d3957cf109862e8a6e0ad1552fa3ad0b6e0184f6a0d3176133ddeb4d744b4d65b14594349ac3a696eda3be6c53f96c9485a6dd19b22c5ca625c0194123a1
CIPHERTEXT = a8b9a30535c9d0da9bc6ef599115b73798dcee649600552851b663fe58ab8e01afd534d662030d4e96d0e33d58837265b35f2e0caf01fcb3260e1f70d61c1a0d26aff86b62e86eb055b77dc74aacd242d0a74f18d5439a477cf85f625fe81420f09182100b94b48fc06b50e8236d8dc44d28e538122dad588d412e0308d56811

COUNT = 8
KEY = 4a45dd68ff034c63789fa5137d8f1737
IV = 5073a5f250912ee9f3561ea26f2909ec
PLAINTEXT = 32cacb98301c30a0b9e3f34925a8d073aa535b6d25e3e7531762a79aee1813e987008d2af07845e2723c5abdcf95426ec8ce6bb5e2e822c16a8fb5b9b74cd496affb3d338d8099a7e1b6a0d2d7a88436bd048d08c0cc0087f64f468da2584a64b3a523da8d9af3980c370fafc648607491db098e46f7494dc48ba33e37a53dfc9ae487487a301c5cfda8ed7d24dc9cd0
CIPHERTEXT = b859cce929c076c13cc34eaadc3bcc332b50865dce1d7b3679863f9768652cecbd0619ea6c74357367d98ac92cb413ca044006c85e9395978845aff4a91fe5832ee1cae8a62ef58b80166d9576a78d3da2b52edc7a8007c7346a6d2a8eff188f800e132997999f7335b23124c7ca51d732e84623c4efec4cd70e60c3fc663318ed21e0c0432854fea6913f1e1326f8ef

COUNT = 9
KEY = 79f1f498cf32a277bf00a181c9a79814
IV = 6aa519c26b718c2452db31cbddfad61b
PLAINTEXT = 8c3c40ea02e75f72f79bc4f97cf923b40672b742c24e33d628f3fc97d364954790085a23b5ad720bb4ccffe3ef0e9739ce10f8280f63f77c5ed8ba1626fa2793a6db9c284714e1d02d075baa0c67829c3dfb953d119e8cf2658b4e5776d724e0f73f6dc0612494098bd440157a0bca267abaf4496d06d88064941e1f6c047277abbb35b2abe673fef5bb16eee202591caed57f14cea4c46d837c9439b088fba0
CIPHERTEXT = 82b475cace7a62e190bab7d8e8cd0e7fd3335939cc8a0a3098bc0c3391f686e2943cd246b1d56783c073452cd43921517b24fffa80aac802d582dc650071ceb221d4c906e123bc274462a04ab2729eb88a25b7ba8121b37a9718c9fc8d2ff0ad4b774ecd893aa540bf189a016ffa3278964478efa3eebd0cb0df76fbfcc5a6bbf6442b47c400d3ce8f295a563853ec46c9be2090bcbf9e604bc77ece46953940

[DECRYPT]

COUNT = 0
KEY = 10a4ad32f71dfa625f3ae1378614a633
IV = aa3a32815f16f18e98897b8824baaaef
CIPHERTEXT = 8c2dd17e181b8cf8e638de4d4d126a9f
PLAINTEXT = 6019cd89ff0630fc102669f3144038dd

COUNT = 1
KEY = e89186cbc1018bff985d0eebb69fb6e8
IV = 6b259cb59c3f7dc0b1a1b63d048a960f
CIPHERTEXT = de832b9d7417a6c573f7aef25c306eb54afc85407a38ab790a18984d011a2247
PLAINTEXT = 60198524a54fcbcf2d2eb30338326fd67b9cd38a3f17ea40b7286201c869bceb

COUNT = 2
KEY = ad843a1c5a21b7e57e76b8262d58898d
IV = 7d03cc934ba7f90162843cebcb117fa0
CIPHERTEXT = 898eb1a6e05633ce99d1147ae7343d3f157171261b71800d6f9ebd491da5b1901a7a1117d59ceef7109a2cd5eed635c2
PLAINTEXT = 542ceb51db2b82119eb7e079093d54d347ae5d609b85620ae87439b6bc081097433ca2336ed8c4bc2daedda54ce4fba7

COUNT = 3
KEY = ed392fac47b07fe46b1fd91c2ce4b542
IV = dd683533f2bcbc4ed1d0b12769e41937
CIPHERTEXT = 1e3240a67205957fa18c0a403d59fba6a0b698f2958c38ec5483f128b46b65889cdbde02ba179ea3783770d42c8683dd8f2978a77101bac6abd20eb654a46b65
PLAINTEXT = 67dd7c2633fbac3ca68ecbf42941c789fb0be98a362520ec76963336c822db7e14b6e539951b6aaac2d25c2d2b1ebd79e7a51641f44b5da2c8e5ac06ad3eebb8

COUNT = 4
KEY = aa185c95c8c87032b018ad34ba0dd3f4
IV = bea0791c9c1338d9b70bb0917275eeac
CIPHERTEXT = 89102dadd4db01d3c0409ae81aa36203b263c4316ca7e15f945b857a727559d41e7d5aca964c89a40e14e4bc87a096e3a3dd6b00b89a86fa238c56d8df95c6275645b49c76a2a5fb0824b7373d04d7db
PLAINTEXT = c771eeb229e55befbb8d8c8b3ad9c5233563b128921e5ef7bf3ed74949835294032388e73c32b8192b08f9cca3f19d668de7c1def76882dc3995edbfb7528e1ad35a32f85da864dcd2ddd3aa3016e3e3

COUNT = 5
KEY = e1ae4a9ab200451634d2018e0fc1f7a3
IV = b2e83726b235a7128055e8aece43d8f2
CIPHERTEXT = 80216f5fb43a44c70fb18a31587e6bf41d46087ba8800f912fd68bdb4bce01a6aba9b8e40a8c58705eddb79a53f0efdff7e3b4f8b7c6e4178aff606d6ced1bef0ecc705accd9b974dfb61d95d40fa9673c7ea2eaa6ac1abbcabac29507dfff18
PLAINTEXT = ba73596f0734b510c1210f2f708c2b728596c8e86052278c4129a16cd01aea5f3342c0c9edfe79d6e6fffbc14da75446781863f38cd98536ed2a5965c8638e098df3385220d6bac48c8a5fa48073ea144934cb4f77d06118f9590ba784518d3c

COUNT = 6
KEY = 1321f81e9895d230c431ddb68244fa98
IV = f2f6ed70601983fb21f3dc7076116956
CIPHERTEXT = 77c883f79ed423168e5faa410a358cb2b57c25e0778943f722f8e58656fb4278a2f20d46eb3cf86b124d7717d101c627e6302d55748383aca686f14095fef81b6c7cb57aca32643a6706248dfbe5f9f15b5a1763e55f5ecf1f7321cc9bec2b861e8f04fb005e706bd6b139094f882ef7
PLAINTEXT = c8a9a600bd80bfe8ee71fe8101829073dbbd77bb5b6a1fa042cbe8e9843b65bd89c63ff7aa55a05e31c1dbe5eedaca0a281b6c67661236cc870208954b5f562245e875284a961371dba86a442fa3a2223247a65f1cb9a2f2227b86f9b62a8c09d077f339c5dd4a66f5fcd0f5e2c9208e

COUNT = 7
KEY = 9b310e48137a61040091c52402a47b71
IV = fbb07432c4d8afa256c44cf03ce0ce24
CIPHERTEXT = a788b6e989ac2935fc93f397d45227e75b970b6898df769ed1e97c5f4e6a4d2d60025bc3a26fbbee7f01ce9ef44d203d9726903fd9c9b2fb885871b9988af90c22a0d462ad9b52d9adac2f5449ec70cf614d8eefd0d0d6ddeef9105559815ffaf64a68ed3298b69d8e7d5de15a95de44c0f44b9f055dacee0cb4a05c1cb648a0
PLAINTEXT = 10dfe8b688bca0168a8982bce3820cfa83c39b20a7920114f6a2568359b26d5c580ebcb7155d470099ef1e2338c2d77c64c20c0cb0dabe454e4260b10f700fe32304da2b6257401976c41076d8ab6b2221c4f6e5c45ce94eb32b2c45450af4b18ae24eb7c484cb8cb90b65737e838a13e04e2d0fbddb979ac2ccfb46e6765f8e

COUNT = 8
KEY = eede36e71b5611cd4490873eb141c8bc
IV = 6121c985e9e4b0821eff6e636e7ee845
CIPHERTEXT = cf0705892ea05b634f73815688ce9bc641c97228aa2ac708a985e75117a87fc92b36b2db3691699991fdea01d403ad7f49146ab4a16f7776e5455118af1a79eb996e8104f69d6640c0042086d265784c578c1a5d8333ebd981249c7dac9d802401f89a8ed48bfb61c3ce4b6de9bba534547ba8aab83eca39646bf00876f93c7d4b1c26013a25cf0382a3913f80cfdb40
PLAINTEXT = 8a143b7c91e1f7b9265fddca4801421f5a98592a6d21c8fa23da08f221795c1f00eef7d32d3e2d4798bfda75c1543d3165947241e8b813f7577aeacedd2b37f4685894eb357a070d7f9450e273353bad5af9c1dd5fa82c0ccca3d757fa1a2fcef40c0f9c041da270f05c3ce4aedbe5b8bcada00079185fa3e6d4073ff7718f3a5940d142b59db14fd86a11c838a5c7f1

COUNT = 9
KEY = 70912e89521a485ac35a10ad06187eac
IV = 603336e51e55003d3bcc69d050616bc9
CIPHERTEXT = 37771cf060b9623c5e5410f140edcb72ec9ed152b73f2fbb33e4341048a625d187a732bfba50a941c0dd6a07981c9bedbb719c22af1e3d89d9c12f5f931eb91f7e1faca21919998b935141e2debf4dff051240aabff4a980448e9d995f4f6467c5e973e1a59d9ae180b97fcb3b5f4b707387a3321047da681d536504d7c08f73030d6ec48563150d2f1c0b0ad783fc08acdaaf01caa238090b166fe820f9a009
PLAINTEXT = 09424ff1ed3fb8ad593f82619a7e1794bbddc1fc1013ae78c3d3f34632215100ba95d395d6ecea98c3013efdf6194cb71680fff50c8882c14bd21f6be5e380c61e51c89ecce34a540a2c474ad17de2a6285fc677917777d93b60c466bab4dd26b2241d033ae3488d803e17db4963ec94fdd88ef84056a19f6647ca1bd08f3c2f92ea193b116db92c1ec3b3b0fe9a70b7ee15b0c88f902f44a9770c9720019b53
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
            File.WriteAllText($@"{_unitTestPath}\MCT.rsp", _testFIleContents); // + 2 groups, 18 tests
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
            Assert.AreEqual(6, vectorSet.TestGroups.Count);
        }
        
        [Test]
        public void ShouldHaveTestsWithKeyFilled()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var casesWithKey = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).Key != null));
            Assert.IsNotEmpty(casesWithKey);
        }

        [Test]
        public void ShouldHaveTestsWithIVFilled()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            var casesWithIV = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).IV != null));
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
            Assume.That(vectorSet.TestGroups.Count() == 6);
            var decryptCount = vectorSet.TestGroups.Count(g => ((TestGroup) g).Function.ToLower() == "decrypt");
            Assert.AreEqual(3, decryptCount, decryptCount.ToString());
        }

        [Test]
        public void ShouldParseEncryptFromFileContents()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(_unitTestPath);
            Assume.That(result != null);
            var vectorSet = result.ParsedObject;
            Assume.That(vectorSet.TestGroups.Count() == 6);
            var encryptCount = vectorSet.TestGroups.Count(g => ((TestGroup)g).Function.ToLower() == "encrypt");
            Assert.AreEqual(3, encryptCount, encryptCount.ToString());

        }
    }
}
