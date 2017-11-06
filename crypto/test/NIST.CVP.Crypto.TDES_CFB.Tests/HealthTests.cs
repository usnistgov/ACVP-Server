using System;
using System.Collections;
using System.Linq;
using NIST.CVP.Crypto.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.TDES_CFB.Tests
{
    [TestFixture, UnitTest, FastIntegrationTest]
    public class HealthTests
    {
        [Test]

        [TestCase("TDES-CFB1", "57d0d5a8fd40a804fd70c8d6d9f72915b6aee0b5e68385a8", "d538d850d0c5fcc6", "1", "0", false)]
        [TestCase("TDES-CFB1", "928615e6ea9eadbc7986ea49fe1901d3e92a5d08b93404fe", "bc6f93a5600683ef", "11", "10", false)]
        [TestCase("TDES-CFB1", "54d01f5bd9bf251c6e831038c2d092bae6b3b9979d5bad46", "94c0ed07c97fda00", "011", "010", false)]
        [TestCase("TDES-CFB1", "c11934a18319949b61194ccb7a9b98c13e2f077f45ce5167", "b1c4c501c290a5da", "0010", "0100", false)]
        [TestCase("TDES-CFB1", "20e9c775bf57da54efc83efbdab5a4803b5bfbce2fa7e0f1", "35ea818c278f5cda", "10111", "11100", false)]
        [TestCase("TDES-CFB1", "8f10a876856d2a75b0a2ef152a43257f64ba1c0b75459498", "a8d607b3a2efe8ce", "000110", "001011", false)]
        [TestCase("TDES-CFB1", "761913ada80bdcb6256ba73d4c9e51ae970167b9efb364c2", "8a5c855d4fbaa2a5", "1010101", "0001111", false)]
        [TestCase("TDES-CFB1", "02a8165ec2152397c22a52752fcb08c423da0197e9314a10", "629f6b6d6ccb4503", "01011011", "10010000", false)]
        [TestCase("TDES-CFB1", "8a7f54685e198329f81658b07a0e321c5bbf2957fb2f1ce3", "574e5e8290aa7bd8", "001111110", "000010100", false)]
        [TestCase("TDES-CFB1", "e9abe5da73f297732c0e1ffda4920dfd7c10a2a7640bec79", "ccd464eb21d0c318", "0101101010", "0001100110", false)]

        [TestCase("TDES-CFB8", "6dec8a104a6ef437d61f914f0e51b5fe1579d9d386b0cd5e", "f5fca134eaa643ac", "5d", "15", true)]
        [TestCase("TDES-CFB8", "0bbca2a8610bfd086d8af79b5eb0cb7931ab4ca2a1912a98", "6e110e57ed479683", "03a9", "11c6", true)]
        [TestCase("TDES-CFB8", "13ce2ff7daae19d9b6e3193bc7b6d3ae45ad07b90bc8ab1c", "48c6b7561341fab2", "d8a99b", "04b400", true)]
        [TestCase("TDES-CFB8", "67620e98fdc4c2385dfea10dfb0ee9041a0e134ffd402f29", "2d8027bf020aaa0e", "d0d7c79a", "fe4f6d24", true)]
        [TestCase("TDES-CFB8", "e931fd9eda34a2dc9419ba86755e70fb10e585cb3d0b6470", "d672b072aea52a09", "7694a36c9f", "0974464a11", true)]
        [TestCase("TDES-CFB8", "64ba152513a8b361a115a7548cd9047a5babcd9d3838f113", "2f0258131a11fcfa", "bce582d40eba", "591539bdf069", true)]
        [TestCase("TDES-CFB8", "7691b619ea37f40eb01adf38259bf84f017a0b08f7a4a731", "07ff9f18cec05f11", "a0260fd4395bb5", "3935d8c9ce032e", true)]
        [TestCase("TDES-CFB8", "7acb4a6d4f37897029e5fefb8cfd2c4c499b4ca7f8d9df49", "c925db87872e384e", "c482190e780ebe87", "6639c158e3d1488b", true)]
        [TestCase("TDES-CFB8", "8f4f7aab25043720f4fbae01aedf071c68a283689b08ad20", "17fdf67a5290baff", "67c3aaf34a1dce2ee6", "ca179ffc8ef50ba9a5", true)]
        [TestCase("TDES-CFB8", "da91497c075b341c856e7680a45b080737e39b2a8aad231a", "7d8a471781e5452f", "f086428a6dff44770e26", "dffc72ea63cc69549407", true)]

        [TestCase("TDES-CFB64", "46b689dcf716649e37a2dca1373e7ab9b3793ba780020b3d", "ac22fca72b818b3a", "dd3cea7250c0056a", "dad91d22970733ed", true)]
        [TestCase("TDES-CFB64", "bfc2671cfd8abf4016d0989123b320a7cbc4f7795d57b03e", "a435397983f22d55", "64bf085273c2ae916521335f22b5588c", "727d115752fb38505a2f683d671d9fe5", true)]
        [TestCase("TDES-CFB64", "aea81a51f76179dc5102ae5b80fb734cc88c5d4570687c02", "baaf96fb2f57c631", "c7222d26a0f5d7fed65201fd9eeba30ec6a521d7ebf91c3d", "14bc77e3359aee35912557009193d761dccfe72a28dbd89c", true)]
        [TestCase("TDES-CFB64", "4c0145c415a2addc4cad25a74ffd7931839738b0617964fb", "c7fd6406cee01813", "0b79f1f0ac815a761c683c429e7793f7929d5d279906e67efaee493d44b834ea", "f53149b57475a84087be3193b9864595745cb3de80f5ed83aa16644f1927a785", true)]
        [TestCase("TDES-CFB64", "bafdd59dbabaa457491004c262dc9d496b1540791a01ce19", "f6b4090ee4e0e49e", "bb993b4d21bb1b1f9cbb0bc58b8b02e93f4b73c65c1acf95822c4c482d1ce57bda84a1a86ccc199a", "da33500f202d983d7915d4e84e4487b067f900ae292345a33ffc5f0207be647328eee64f11b53787", true)]
        [TestCase("TDES-CFB64", "abd6b6203e83cbdc5191a79efe31169b10c4a7f26873c17c", "5b47393301d64813", "bc9262ab95b723129b314f01bc47e20c885988c893bb173b1bc45c90eb211a0f7f6f504d690abba0f41f9cd60609bb27", "f1e0b0b4fc00ba7644af3df470b5c78399710c5e46c0a43488bc52fed6bb61f5efe58b2dfa7b138f0d44625c2c1fc79e", true)]
        [TestCase("TDES-CFB64", "d3cb01b67ab5d6abcbbc61b5da3d16316e19e9f87a4a8f8f", "721d45c8fc17d2ed", "7b772a4ab0e11266275a4c049f343526e893af2fa2220c3d47042fb96c0eed2d4333b955653f8178cec9d3f8296244294f13125f0a07084b", "2707ccab9860ac087ff8a2d73ede4fa5bef0861c3b3dd33f316e89fd931ae12b6d759dbb93a169ea7d216b0da22a54a71752c1ed24a0d1be", true)]
        [TestCase("TDES-CFB64", "677c04027f1fadae5b2f37fdba439b2ab0da1ad523eaf173", "63a8f95642a47f98", "7856992a93752bea3d03254d5824ab4031c488b010dc65ef0df03f5a26dd18398fd1ece6499be632f625d404ec3ed943889d61f6c4235aaa0c61a2144e5abbf8", "0b91d2803a91d1ec7d898a6d40346057adf6fd81942a412df14505acbcd6de9842394cbde0758d2d1e68dceeff34a896bc153606f051b4f86bb2d542828f5232", true)]
        [TestCase("TDES-CFB64", "d6150270efd3fd29e919f4890db3541c209d80f2ab075d57", "fd1bafa075ebd6a0", "f65562f386a998f5d112c18b1a54e3b5328fe7c2f7b943681b4ad6142739137fbf28628227dcc32b06efb5106ac956e30dcccb47d7cfe2e2a3cef6b446df14a7cd685b670790a973", "51434412085bd79bcafc6f276094b41152375c67db0f068ef7f1a909d173e54a79f9dbbf4dca2c64699aaa81c4532fa296fb42e417101fec5da5f899ca6e58b77331d801e1167d8c", true)]
        [TestCase("TDES-CFB64", "2a464f98497c3b6b4cbc920d5e1a084c2926b698a423b316", "73e80e3c40364188", "7101553f01ab8950082c8df256dde4ebaf4785a8d9e78c24f42e300b0accf51ee72e22d92b4576539d8a1767c23fcb11aff76a1a1b5894f8b8209ff0c466d37dda23daf50ff0726a3a1165ce93b3bb5b", "66ed1d2acc7cfcf907a3a41b8d869b0c2fafc926badc4c6d12097e80f217c4fc26156067363b14cb2de870977d7f7f7394fd798bcfe3798ed9214fe6504e41225eaf6a9a922fbb2815735b2b3c4ce436", true)]
        public void ShouldEncryptSuccessfully(string algo, string _key, string _iv, string _plainText, string _cipherText, bool isPtCtHex)
        {
            var mode = ModeFactory.GetMode(EnumEx.FromDescription<Algo>(algo));
            BitString key, iv, plainText, cipherText;
            key = new BitString(_key);
            iv = new BitString(_iv);
            if (isPtCtHex)
            {
                plainText = new BitString(_plainText);
                cipherText = new BitString(_cipherText);
            }
            else
            {
                plainText = new BitString(new BitArray(_plainText.Reverse().Select(x => x == '1' || x == '0' ? //make sure only 0 or 1 are in the string
                                                            x == '1' :
                                                            throw new InvalidCastException()).ToArray()));

                cipherText = new BitString(new BitArray(_cipherText.Reverse().Select(x => x == '1' || x == '0' ? //make sure only 0 or 1 are in the string
                                                            x == '1' :
                                                            throw new InvalidCastException()).ToArray()));

            }
            var result = mode.BlockEncrypt(key, iv, plainText);
            Assert.AreEqual(cipherText, result.CipherText);
        }

        [TestCase("TDES-CFB1", "ba70d09252f87ff78f5267fbfe2f9276e3b054e3b9f286b0", "86a5624fd0629b94", "0", "1", false)]
        [TestCase("TDES-CFB1", "e0588f439401918f3e208a4c7079fbdcfeae4f5b9d10b9a7", "86f68e3e34f0d410", "10", "01", false)]
        [TestCase("TDES-CFB1", "868679b01340758fa8944a3402dac22a9b57e99173700162", "fb9596d732fa88d8", "000", "100", false)]
        [TestCase("TDES-CFB1", "928cfb498680e57ac2798a2a7ce3c452070198326b257cd9", "9965af752344d208", "0000", "0101", false)]
        [TestCase("TDES-CFB1", "9b20fec249686ed69837587357041ae98c7f109be537e949", "8cc4b487d588c968", "11111", "11001", false)]
        [TestCase("TDES-CFB1", "89b398b52ad6543edc0ba1346d0bce5b9e0773f40d108c2f", "b1e3ae99a6048b81", "100010", "110110", false)]
        [TestCase("TDES-CFB1", "d631f8d59780a43e4a4ad6fedf20e9544c6b6d045d29b008", "c1439976f03f518c", "0010101", "1110000", false)]
        [TestCase("TDES-CFB1", "e532863daba719fd0db5c22a26c4a7802a58b616f7feea4c", "e990a4bf131930e4", "10110110", "11110101", false)]
        [TestCase("TDES-CFB1", "7f9e57198cb0b5860dbc58f4858c80effb76ad3b75d962d3", "fda9acbfa062da69", "001011010", "001110001", false)]
        [TestCase("TDES-CFB1", "bc54975d57bcf89e25d02fe09876769d94f14304bc1351c1", "5adc17d44040e4c1", "1101101000", "1111000101", false)]





        [TestCase("TDES-CFB1", "8c0e01f425236bfe3238f834434952ce8c0e01f425236bfe", "4336567c29b490f5", "100", "010", false)]



        [TestCase("TDES-CFB8", "139bf28a97e0e99b3db3c1732c461973156e430829e9f1f1", "28db7111b723f3b8", "13", "e7", true)]
        [TestCase("TDES-CFB8", "c4fea2ece626d0ba75010b92feda4f9710f4f2314a29f78a", "7c022f5af24f7925", "3d11", "1f83", true)]
        [TestCase("TDES-CFB8", "b598d0adfe0bd3f120fb291abf982abfbcd5136825ef0802", "1ef39345ac75ce92", "7dee18", "b7b2ce", true)]
        [TestCase("TDES-CFB8", "e691e3ad070d2aeaba5d70f22c676eec5e45012c737f2f45", "c1c50f9e5cd57fb7", "10ec2cd5", "923dc873", true)]
        [TestCase("TDES-CFB8", "615425e57fcd85a48a4925c8df83155b8f62ce7f855e83b9", "1a811b0f5ab9cb38", "3b8961e1d3", "7c4e75dc05", true)]
        [TestCase("TDES-CFB8", "b6d9971cb6165d8af4cd5110a4542c64349e3425b3a2fda2", "ce12ea680b3b124c", "3a18cba665ea", "b113feb710a8", true)]
        [TestCase("TDES-CFB8", "5898b0ce6734bc49024683ead657f8d91aba01f149763b2f", "3de9ac40574eaa01", "7a5c58bb3a3f14", "77ce826e05e05f", true)]
        [TestCase("TDES-CFB8", "dad9a797a41c38fe9126bf5816263ba7389df73e9b5d43fb", "6982a79d7bcc8bec", "6dc7f1c7c64eb7d8", "d1d6ce808d268987", true)]
        [TestCase("TDES-CFB8", "ef0e9252679868ae6d8fdc02d5404597159e9426948cc7d9", "b35958f6ff2906cf", "744c1e5426c96dbb93", "5c3e6677d76e323f3c", true)]
        [TestCase("TDES-CFB8", "cd61621026f4084031c44310fe98546e084013e96b0449fb", "5b25634190db8557", "68d7335ea143771e70fc", "452c83ccb4a7fcdf41e6", true)]

        [TestCase("TDES-CFB64", "0bc4319137e3389dd0e934013ba83868927992ceb5ad76f8", "c0a9201a3c167779", "e3a9ff52df4204a1", "9dcba0b991cd4023", true)]
        [TestCase("TDES-CFB64", "ce8c800d62dfa791cb38f16ef49489dfe57fc7d95b5bb6c2", "f79cbb26c0cfbb9e", "2f2b2215003a60d19c41b03e7700f68d", "38d7d53f002b12170c8d9ca906cd93e5", true)]
        [TestCase("TDES-CFB64", "08f7c77c1c68a864cdb094e5ec1f23f4a42f58f194525410", "ed897a8b23a91be6", "83efc196960d3a3c371ed09d6b69d5d70ee73087ec93aee8", "68888d0018adc4ab6613e0aaaac5927fac7a83bc096add72", true)]
        [TestCase("TDES-CFB64", "bf10a1d62651ba2f92f71ca7491649cdcb4f453d7a439416", "6f048d459102a21e", "46f4a59738fb161a28ee0780a8585c8d2b210b27fa84d41ca8d167bf393c7024", "e4d3e6fcfab02ccedc4307c0bacc45bbeb141d6784ac66942a7907d92471af7a", true)]
        [TestCase("TDES-CFB64", "a280c2cdead389925dbcf1516bd904135b86fe9ddf34914c", "f46228d7377e689a", "ed4008df79a677f403e1807699a83007417cf54b560c653a32b45fa2e1418922e640a4d202e53310", "5a0b68f78691fb0835259d4d2268f6214f2435409a904c2991d05dc787bc6f51e8e2666fc9ce329e", true)]
        [TestCase("TDES-CFB64", "1a9d107302677351e07c622951a1d08c80831ce96b237391", "3941fd7e49b01642", "15f70ec9cae1240c3eb640d16aacb8088651711519ea6fa09ea3c12dc90383f5483550d8a2b6489d5283fcf73b4f2256", "31f39915cbd90d8e22cd836b06f1eee99181e453b04e441bde6f1c293333ca739e5a61ce6f844e0318e93b26344b3323", true)]
        [TestCase("TDES-CFB64", "7aa86ea131fb673de3bcd601c892fdd5d68519f734d51a4a", "ad695cc1c843901b", "8f969b5434bfd723e1c38488fa45e8d6fcc43d1bbf03f808ae9487ef74207a318480588ec7bfc9957a82379880b9c1d779332cdbafc99085", "e9f43249c28d370a4fb691bb393e6444d9eee0620512b94d2a36d7989b2e4e5829599088f426e2ac81d60cb50c8455251f63ae7363482eb1", true)]
        [TestCase("TDES-CFB64", "268f2a8acb25345b5d8f926d16a8ad1a5b86fe9ddf34914c", "f46228d7377e689a", "6146285eafb005f411aad4cb16859e7f7b76b88844fcbd425ce7cde1c818038656ff90b1908b930cae2e5bb054695b32836c445c3a90d876d09c660b703bb219", "5a0b68f78691fb0835259d4d2268f6214f2435409a904c2991d05dc787bc6f51e8e2666fc9ce329ec38946d5025f4c6999e5172e183bd9df618f96bfb7e2d881", true)]
        [TestCase("TDES-CFB64", "45f2a4c4732aec62f10badeaef0bef918a975d7f2c9bd0e9", "7389671b0852b480", "0c6c767fd97f69da1e2519d9a4cc873afca8b971e46f7cb101381c6e6b86c15936cb9e63e7969f78bda65045179de8c8750798f78935ea6191bce6797a59a2996b9220a5fa368603", "51465f171131583db3502b7342525a88601fbe3ddd18c1a688a0ced345db06b060b5352a21270493a1b67048db3c7a9106ea20c8238e8dbed2128d5a6945438347df1e4066bf091e", true)]
        [TestCase("TDES-CFB64", "b6c72f972fe66d4abff898bc7c232c0e7f83679b469eefa8", "af500fde7ee0a87a", "138df01b610585a5c1bd6aff33ff32c51294e7b4a78f8d21131fbe5c0d7f08dbc2c334aca11758c005c6586a5f749facf41f5bbe0b015d61169b9c81ce132232bd9298fd207ae237226b604145e0ed8f", "052b48b4da1a7482b5f6d8b44bf3e5c2c2ee121765542f189d40f6631ce3e4545c2947ce48999738397e09c5234462f911240969ce0461c2e598550cccce35305d69c0b885eb82d740b960a9c57a71bb", true)]

        [TestCase("TDES-CFB8", "0123456789ABCDEF23456789ABCDEF01456789ABCDEF0123", "F69F2445DF4F9B17", "6BC1BEE22E409F96", "07951B729DC23AB4", true)]
        public void ShouldDecryptSuccessfully(string algo, string _key, string _iv, string _plainText, string _cipherText, bool isPtCtHex)
        {
            var mode = ModeFactory.GetMode(EnumEx.FromDescription<Algo>(algo));
            BitString key, iv, plainText, cipherText;
            key = new BitString(_key);
            iv = new BitString(_iv);
            if (isPtCtHex)
            {
                plainText = new BitString(_plainText);
                cipherText = new BitString(_cipherText);
            }
            else
            {
                plainText = new BitString(new BitArray(_plainText.Reverse().Select(x => x == '1' || x == '0' ? //make sure only 0 or 1 are in the string
                    x == '1' :
                    throw new InvalidCastException()).ToArray()));

                cipherText = new BitString(new BitArray(_cipherText.Reverse().Select(x => x == '1' || x == '0' ? //make sure only 0 or 1 are in the string
                    x == '1' :
                    throw new InvalidCastException()).ToArray()));

            }
            var result = mode.BlockDecrypt(key, iv, cipherText);
            Assert.AreEqual(plainText.ToHex(), result.PlainText.ToHex());
        }
    }
}
