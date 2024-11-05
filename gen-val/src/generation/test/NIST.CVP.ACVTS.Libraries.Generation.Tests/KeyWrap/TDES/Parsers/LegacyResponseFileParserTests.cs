using System;
using System.IO;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0.Parsers;
using NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0.TDES;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KeyWrap.TDES.Parsers
{
    [TestFixture, UnitTest]
    public class LegacyResponseFileParserTests
    {

        private string _unitTestPath;
        private int expectedNumberOfGroups = 10;
        private int expectedNumberOfTests = 50;
        #region File
        private string _testFileContents = @"
# CAVS 21.1
# 'NIST SP 800-38F TKW-AE with TDEA inverse cipher function' information for abc
# Seed = ff19e547183f437c97f430b4a36af3c9f4522ff2935fd6beed37d17a850e742ed668b383bfd3a0bd642e95fd86d74a5b01e2e590ccd5d1b4348b252cf7de3096
# Generated on Fri Jul 21 10:47:35 2017


[PLAINTEXT LENGTH = 64]

COUNT = 0
K = 962b6a3373fa0facdfc758c20581f1cb47c9ee8418ed45d0
P = df50d8af70c35bf2
C = ff9ff5979bc1523c793037ad

COUNT = 1
K = 231bde71e6e52a13084040870446dccf21b9362005642d4e
P = 6eb4d745dac49031
C = 2b90090a47d7ec7a8c8ad993

COUNT = 2
K = 486499c6a4d1354813886307a0330c8d1dd884cb5484d781
P = 3c8972f062c9ac92
C = 0f69a2a35f55bb9240d7e5e0

COUNT = 3
K = 268f2e935b431f4fc4d645ead0121a32430601bb2630a59a
P = 0f808442d8387ef5
C = 5e489fd60fb7e62ff884ae26

COUNT = 4
K = 7a0098e3b46190c50189909411a3909c004541934535b2d6
P = 020aa213828ea5ac
C = 951c38cd485449252af7c98f

[PLAINTEXT LENGTH = 128]

COUNT = 0
K = 749b624916c782efd8253fdcedadf1dad840757864ff479c
P = 02a73d4a371832495add14d8afe961f6
C = 663399f6e91beb75c6a2a92515a36f08b9f89970

COUNT = 1
K = 5e98b437a96b05de10afa3fa9d0cda3e8580ac39d4e63778
P = a4fadda0be2dc8eae04ff92f0b4ee22e
C = 48762d08de161932f2deb30bb1b7636e60eac4a3

COUNT = 2
K = 5b7a35a773dfed16619eefdfbf8fff028937d4638e2f341e
P = 240d5db52fbf74a6891876253f0e7664
C = e8be8e85d4702b8fde71ca3d3c1eb761c597b097

COUNT = 3
K = f0f1a6e51d5c5f21a13c676de201b17d35942e4c8a87532a
P = 30a16475e1c1ddc1c0b1511d002d7187
C = 4be3a8563db950fe07e8e59463779691ba361ba3

COUNT = 4
K = b1730e61d1f2e1e1b3a5c713a3245f5c5377ef4dc2684470
P = 645e37ee71731b8e4c6d6bd209feca79
C = 56cfcc1f6ec399e66af4b266a92976d7247572ce

[PLAINTEXT LENGTH = 96]

COUNT = 0
K = 1a147d236328a4b619a19f8fcc58e1ad259cd65f469851d3
P = 753545e0e41361b2c46fa738
C = c456ca215ba631cc56404aa2252c20ec

COUNT = 1
K = 42eb1c40ab34bdd6de8bd2e63a20159ada911076594bfe8b
P = 7773c02892bba2f3d729cb12
C = 90cba6458702e0b88640a957fef0bd76

COUNT = 2
K = ea2ae55b85ab803c9448843100752962eca1f4174de2a132
P = 3bbc7f07bf1d4ea7952f2e03
C = b48f858da4c904ee039d07f5692c3825

COUNT = 3
K = 232f384e176ee8a01314578cd55c6679fbfdce56d5b1622b
P = e33da5ed66ad84fa4cac9600
C = 936d0dad0bdca0406e1eed7c06c62c5c

COUNT = 4
K = b33dd44e99e2cc730bd0ad84280120323f7159d4023694b0
P = fdede4f65cf917390cf12fa7
C = bcf8ae0dbc933044f2276f82f4144f56

[PLAINTEXT LENGTH = 160]

COUNT = 0
K = f78d0feeaeb1db9bbdb433a7d42505caab1be0c23ddfb42a
P = f33344f56edd03131626e0c3d80eb364db5602de
C = 2a4649093326576d6ca979da07b26e8d4db5b6127efc7045

COUNT = 1
K = 28dbb7d64def524c7c4bdae09c5931df2ea84129b1e31ae0
P = cb2a5941a85c0f0ad8cf7bf96f55b2e4ef65b624
C = 31c7fe332c552122a609272616545baffad6d41699999147

COUNT = 2
K = f0334622fac4e5702839f29710f4ecd8e2a82a139183d78a
P = 5f69c06080e1ded57f3be8d9d5de51d7c80f5cab
C = c26345cf11c44290aff562fc3100fbcee6ebf4b7c886d974

COUNT = 3
K = ae2969f0b3161ce7a6a20c550ac0403c9dd5efb524929b36
P = 979d39cc4594ab5bfd151eb588cfcf9edf11916d
C = 701795c3dd08ff84dee9dd481e30166228713a007a1302f8

COUNT = 4
K = 28b00a95a9f18bc4b53e2f4af3f5cd10bfcb336520f760e9
P = 2e9370f5a07a6fe99613cfe54a6df2566603cafa
C = 439f1fc84b1a668156ef20f19de7c7e0660ce924c15c7e64

[PLAINTEXT LENGTH = 2048]

COUNT = 0
K = c7b6970c7e5de4b6fbca68e50e6bcf3c5892246a96577176
P = dfaad4bbe1dfea61469f523685557ff9778b6d6d5a8f21f01d6af94047f627da7ea10c4407e0aa197a7c725b5a1aebf523b0210eebe76ffcb6c273d66d6cc9eeaeaf8cb1b4e9d2be6734241d0700acb6aa5fd0bd12cd440c50cab6d11b2b4ce9cb913a21af98222edbcb2021b5be4f8e3b75f69ce9817276ee4a32a6d50b08153695a0a6f0f73bc59a16389d375a67ebf0bad1f2a69363494191d993bb4071e4657f28bfedfc9476e8f03c2d2f3b6c4031fc3ad837d674d5b3f096c0cb31f2421377fd2f6620e1ae0408613cea5163f26bcc6385b30f128634a0bc929fdb491daa2b7b98624fcd363ab72533302ac65b09666adf6088762c74afb1eb28e494d0
C = 7f667b7f2a29c143ae63d61378f7a9a768ca7e25cb96fdade3b586dea86e5eb4014e99374aa64dee03d6a783b0f887c0508db564397a437f56d49c581e27b33e18d90307d5a65c6e9e67acbd4b55fa02bf5f1f2437a0cf2c293ebde81fa6775a057750aa454116a9dcde21fd1a2a8dc19f8f92ab8e0ad81caec3e7766430d7196fda4850d3ac8433af9bf947af792fb7ad06063cfa349f35bcc1dce24f80322763e8089266e7a447ceb38be29af7670a85752e3b75c9d050a409fdce21f8204130a48fa493c6f97c192444ccba3f565a0219c4b93f0580b43e6acee21c82051f0bb49c4530454270e83c18b219e5f9ecd3f7b9f46d366b1dc51aa6983154724dbf5a9def

COUNT = 1
K = 700ac432e903348cff94e2203e0d26e40723762624547575
P = 78040afed498ee3d421c16b08c5b1b5c239d404ac257c6b2fe9c6781a3b6c50c59a496bfd9fe4ace38f3078739f77d4dd30d1482800f39284481db33b87d5463481d3ec4f61898fee243ec088b386727723dc99379555e4a4dd89c4c819a724ab3f10891107b3f31b993aa6cf40899ad63135a0d4e430ad751ae28eac870ac3a4d4729af4121a708c76d1fb4afa097843823d1bb325a263873dcb244f050f7439f51d744f2ed869a5698cca2b4e00155f3d2c0b4d54588c082b1360d578b17ed1a8876993661ada8146b4451f4ca70e95788c33f5a66e00b0de399a5b96ccdc5dc37c021cd646ae9e090fab276310be1d77c673231f09097cd5538c0bf31916d
C = b60a051cbc1d70c9ce7c0eaca98348390f692bb47d1cc5967919217cd98e14ea607c8efcb3094da9ca26eda7cabdfc3c2d8e1af1e63a7aeaf2eb7d106baff6d613f644e671f419e12b9f18dcfd5edcc04806d2d1a6389e6bd029ea85ef600dcd9f3b3ef2804eaa9c1276cfee0fbc0fa5c3e9b5f239c480bf8108308ee20570e0096e203515a4a793157900bd3ad70b26200113edf0d1398e15e5c3b372293843fe036ad6077d687037643df32b5049f09b65ceb21ad85740d0e574a15308e7b8839cbbf4a1c8bc56e2bbae31d045c6bd5452204453bc3c44cfa3d064fb3ddd36cdd4efd7f1483d4143b1fbcd34bba3b2140c2f4d3e2ca0fd204520615bea06a03e65d0b7

COUNT = 2
K = 57fa1d73c9458413b34677a0fcccaf8554c875ad0bffb81d
P = 3e631774f81b62722213044502c7cedfb437dedbc506bdf857a3952fa3f4bdca0b22e6e766426c391a3109c1bf281961a46857461598cc5c8a00caf9a4b646bd1513548e7efc410d47505399b68edf44493657e2378c044ff9148e49dd5d96dcbf4aca35444d70998ad59ec632531d190a245436e1fd61c1cb3ffbfbc999d9ec423f06f0322b108c6b2af4615f328a01b8367cbb38da797d3d15c2b773dae770a2c2bfbc37c0489684813aeea4031035029193df87808b721c596766bdcf50b8fdc3c20420312b5e087410048452f6aa4f275c35fa9b4977cfeec60d074f6e9dbc5d1b484281207d7673d7a12b8eed265ae2570e70dec5c5d121abaa02c0d80c
C = e1abc968efaf60ed56d7a67a0a3b3d8b4d3530e1892a1091899462a1c53bcf1f746f5c184e6cc7fb105bc0ac264d7e091c13b14865f5ffc73b84edd9e7336eab04c2f8699b0ba863029c1da99955f1f8f2b193958055b8495437358e678b8f55a91d3d320bda8977ae413175cb3626fb0e7176855e60e0e4540dbc69f7025551646540d3bb03876e88e357b5ec050e2957737e5f3738532eac00ff5782839d2d52b03f26658479e75cc7a63962ac580b9a3ce1917e27fc2ea3cdcbce738a850ff5c178a4189d44ea43c4b360d4b3696922db26c77fbf00616fc36bb9fab7fb31d1ab59520a8f8d0a0c4e5267eea7514482fb9d217122dc9e5c510863797ab46c248877da

COUNT = 3
K = daa1fd1c73f80c4ab9d0bbc6ff812c703c18f65b70d2105b
P = 418e3721384c9190a96df0fcb9ac9387e31149b93953dbd6d94c6370dfd48135ec914a9801476c9966517e9aacc994e6b7acb84c9c3f5c9ef5e395864df6b857b6591d259c5786a9792fc255ba7cbb87f22daa4e42cda007127c6227feb01b48d0a00610d6077756987448f45dc1b570bcc74e328db56482a0dd252af36da614459fdcd32af7591c9469cf1d9f39af4d914e0622064a016e888c3e1801c4990cef95795d2c7fc2cfbbbb6630fae1ea7910b2d3d301a7e748a811c112c223fbdc4d3454832eaa71a10e5cc7c9ba962b2e10ce7d2bf89ab94dcfb27f37461faf0321a3dd6c00984d8a7d355b65588a4b486e8b401f0e2efba281a865c45bdd65cb
C = 29c1fbd47a4516087e1ce712315df301d34874503927e5e3f1f9751f1574b30307bb9df119227bfce0c5d93b7e600e47d6e897181cc1c6a1c2ec628af1424b5ad1221dd22b0daacf7d9e4c09a360b2983662ad3addba0760bdd50e4e0bedeba3aaadb329fe1293ba9f56b74e97a5e8c6f7316a345c0cf2e1420b328ebdf473994132b7bdfc50e8e808011167ca1e155435a3a39be276a49d6eeb5b421e6a13c2308acb2a3e94d9d765a4513ec4113a97311f4dc94bf908971d4d15a88934c5ad72214a600e3baf4f21a7195a1a1e11b2a318f7f1ff7bafdbc245026cdf031eecf800c231da7e33972b8bd334a9b195bd19e55cf0883d5d8cc9dfd6cf34e9d7892041c89c

COUNT = 4
K = 3616fc52be58652d0d8d772115e2ce71407fb3385458e78d
P = 7d11ec6242af73f9de9805f02db84eeef7cad793b634e78422cee2b646a95a86373b1e597e4de2c6d97afe6e2200e1ea00811836753f1065e51a8a490f4b64f62f5a4455e73270b2ad79f903311d9330259ba2ec0e2ea39e5eb83de5016d642036e81eb7c8871688ea6ec41f7427108ab56179fe71f004084159183447b9ced88a7f10288051caa94b8c18296fd17ba808e4301400675040be572c0f3ee36618ddf1472a229f08cbbe9b152814505fbe2f877c1e3b4694b865c4ff1bb4c0d855109a65475fa621019108f8ede25a132d20d6ec0545863cd3df4637720b489040674d072fd04005b832c291aca06d0929efabd0800295ae4f111b0caae8e65eab
C = 28e75c91dc23981ee5d313517f960043d6d634495003d7f732bd60e9d74eb2371060061b21fa871b6ace5afc9b415f1b76787da362807018a559a6ea2a948fd18c5cff77d4999353f4214e4ec8d58919d5246ad594a44aefe487f4d5ce4845e8146316a7f409cfcd9f16c21fed77f61a775819f8a6dcf6b61c3b7da451b5eab165d85120290de710425255badb9f37aada96e75783c486e758a285507d07c5273042bda06d00c279bd6aacf1f59237d254522bfb8239dc4728a9bfe2ee1b6047f4b55a3f60e0bcbc15a668a6168c650fabe19ca2769c8495763da2f0a7ca37c14742729d4a69c7e2ce322eec4a6057d03435de524a6dde169db02f40a7fb1eba12315c39
";
        #endregion File

        [OneTimeSetUp]
        public void Setup()
        {
            _unitTestPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\temp_LegacyFileParser\tdes");
            if (Directory.Exists(_unitTestPath))
            {
                Directory.Delete(_unitTestPath, true);
            }
            Directory.CreateDirectory(_unitTestPath);
            File.WriteAllText(Path.Combine(_unitTestPath, "KW_AD_128_inv.fax"), _testFileContents); // 5 groups, 5 tests per group, 25 tests total
            File.WriteAllText(Path.Combine(_unitTestPath, "KW_AE_256.fax"), _testFileContents); // + 5 groups, 5 tests per group, 25 tests total
            File.WriteAllText(Path.Combine(_unitTestPath, "fileThatShouldntBeParsed.dat"), _testFileContents); // + 0 (shouldn't be included)
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
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(Path.Combine(_unitTestPath, $"{Guid.NewGuid()}.rsp"));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void ShouldReturnErrorForNullOrEmptyPath(string path)
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(path);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public void ShouldParseValidFile()
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(_unitTestPath);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void ShouldHaveProperNumberOfGroups()
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(_unitTestPath);
            Assert.That(result != null);
            var vectorSet = result.ParsedObject;
            Assert.That(vectorSet.TestGroups.Count, Is.EqualTo(expectedNumberOfGroups));
        }

        [Test]
        public void ShouldHaveProperNumberOfTests()
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(_unitTestPath);
            Assert.That(result != null);
            var vectorSet = result.ParsedObject;
            Assert.That(vectorSet.TestGroups.Sum(c => c.Tests.Count), Is.EqualTo(expectedNumberOfTests));
        }

        [Test]
        public void ShouldParseEncryptDecryptProperlyForGroups()
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(_unitTestPath);
            Assert.That(result != null);
            var vectorSet = result.ParsedObject;
            var testGroups = vectorSet.TestGroups.Select(s => (TestGroup)s);
            Assert.That(testGroups.Count(c => c.Direction == "encrypt"), Is.EqualTo(expectedNumberOfGroups / 2), "encrypt");
            Assert.That(testGroups.Count(c => c.Direction == "decrypt"), Is.EqualTo(expectedNumberOfGroups / 2), "decrypt");
        }

        [Test]
        public void ShouldParseKeyLenProperlyForGroups()
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(_unitTestPath);
            Assert.That(result != null);
            var vectorSet = result.ParsedObject;
            var testGroups = vectorSet.TestGroups.Select(s => (TestGroup)s);
            Assert.That(testGroups.Count(c => c.KeyLength == 192), Is.EqualTo(expectedNumberOfGroups), "keyLen 192");
        }

        [Test]
        public void ShouldParseKwCipherProperlyForGroups()
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(_unitTestPath);
            Assert.That(result != null);
            var vectorSet = result.ParsedObject;
            var testGroups = vectorSet.TestGroups.Select(s => (TestGroup)s);
            Assert.That(testGroups.Count(c => c.UseInverseCipher), Is.EqualTo(expectedNumberOfGroups / 2), "UseInverseCipher");
            Assert.That(testGroups.Count(c => !c.UseInverseCipher), Is.EqualTo(expectedNumberOfGroups / 2), "!UseInverseCipher");
        }

        [Test]
        public void ShouldHaveTestsWithKeyFilled()
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(_unitTestPath);
            Assert.That(result != null);
            var vectorSet = result.ParsedObject;
            var cases = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).Key != null));
            Assert.That(cases, Is.Not.Empty);
        }

        [Test]
        public void ShouldHaveTestsWithPlainTextFilled()
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(_unitTestPath);
            Assert.That(result != null);
            var vectorSet = result.ParsedObject;
            var cases = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).PlainText != null));
            Assert.That(cases, Is.Not.Empty);
        }

        [Test]
        public void ShouldHaveTestsWithCipherTextFilled()
        {
            var subject = new LegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>();
            var result = subject.Parse(_unitTestPath);
            Assert.That(result != null);
            var vectorSet = result.ParsedObject;
            var cases = vectorSet.TestGroups.SelectMany(g => g.Tests.Where(t => ((TestCase)t).CipherText != null));
            Assert.That(cases, Is.Not.Empty);
        }
    }
}
