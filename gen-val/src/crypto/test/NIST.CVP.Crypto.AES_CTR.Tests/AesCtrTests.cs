﻿using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Fakes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;
using NUnit.Framework;
using NIST.CVP.Tests.Core.TestCategoryAttributes;

namespace NIST.CVP.Crypto.AES_CTR.Tests
{
    [TestFixture, FastCryptoTest]
    public class AesCtrTests
    {
        private Mock<ICounter> _mockCounter;
        private CtrBlockCipher _subject;

        [SetUp]
        public void Setup()
        {
            _mockCounter = new Mock<ICounter>();
            _subject = new CtrBlockCipher(new AesEngine(), _mockCounter.Object);
        }

        [Test]
        [TestCase("2b7e151628aed2a6abf7158809cf4f3c",
            "f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff",
            "6bc1bee22e409f96e93d7e117393172a",
            "874d6191b620e3261bef6864990db6ce",
            TestName = "AES_CTR - Encrypt - 128")]
        [TestCase("8e73b0f7da0e6452c810f32b809079e562f8ead2522c6b7b",
            "f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff",
            "6bc1bee22e409f96e93d7e117393172a",
            "1abc932417521ca24f2b0459fe7e6e0b",
            TestName = "AES_CTR - Encrypt - 192")]
        [TestCase("603deb1015ca71be2b73aef0857d77811f352c073b6108d72d9810a30914dff4",
            "f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff",
            "6bc1bee22e409f96e93d7e117393172a",
            "601ec313775789a5b7a7f504bbf3d228",
            TestName = "AES_CTR - Encrypt - 256 - new engine")]
        public void ShouldEncryptCorrectlyNewEngine(string keyHex, string ivHex, string ptHex, string ctHex)
        {
            var key = new BitString(keyHex);
            var iv = new BitString(ivHex);
            var pt = new BitString(ptHex);
            var ct = new BitString(ctHex);

            _mockCounter.Setup(s => s.GetNextIV()).Returns(iv);

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                key,
                pt
            );
            var result = _subject.ProcessPayload(param);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(ct, result.Result);
        }

        [Test]
        [TestCase("2b7e151628aed2a6abf7158809cf4f3c",
            "f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff",
            "6bc1bee22e409f96e93d7e1173931720",
            "874d6191b620e3261bef6864990db6c0",
            124,
            TestName = "AES_CTR - Partial Encrypt - 128 - new engine")]
        [TestCase("8e73b0f7da0e6452c810f32b809079e562f8ead2522c6b7b",
            "f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff",
            "6bc1bee22e40",
            "1abc93241752",
            43,
            TestName = "AES_CTR - Partial Encrypt - 192 - new engine")]
        [TestCase("603deb1015ca71be2b73aef0857d77811f352c073b6108d72d9810a30914dff4",
            "f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff",
            "60",
            "60",
            3,
            TestName = "AES_CTR - Partial Encrypt - 256 - new engine")]
        public void ShouldEncryptPartialBlockCorrectlyNewEngine(string keyHex, string ivHex, string ptHex, string ctHex,
            int length)
        {
            var key = new BitString(keyHex);
            var iv = new BitString(ivHex);
            var pt = new BitString(ptHex, length);
            var ct = new BitString(ctHex, length);

            _mockCounter.Setup(s => s.GetNextIV()).Returns(iv);

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                key,
                pt
            );
            var result = _subject.ProcessPayload(param);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(ct, result.Result);
        }

        [Test]
        [TestCase("2b7e151628aed2a6abf7158809cf4f3c",
            "f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff",
            "874d6191b620e3261bef6864990db6ce",
            "6bc1bee22e409f96e93d7e117393172a",
            TestName = "AES_CTR - Decrypt - 128 - new engine")]
        [TestCase("8e73b0f7da0e6452c810f32b809079e562f8ead2522c6b7b",
            "f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff",
            "1abc932417521ca24f2b0459fe7e6e0b",
            "6bc1bee22e409f96e93d7e117393172a",
            TestName = "AES_CTR - Decrypt - 192 - new engine")]
        [TestCase("603deb1015ca71be2b73aef0857d77811f352c073b6108d72d9810a30914dff4",
            "f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff",
            "601ec313775789a5b7a7f504bbf3d228",
            "6bc1bee22e409f96e93d7e117393172a",
            TestName = "AES_CTR - Decrypt - 256 - new engine")]
        public void ShouldDecryptCorrectlyNewEngine(string keyHex, string ivHex, string ctHex, string ptHex)
        {
            var key = new BitString(keyHex);
            var iv = new BitString(ivHex);
            var ct = new BitString(ctHex);
            var pt = new BitString(ptHex);

            _mockCounter.Setup(s => s.GetNextIV()).Returns(iv);

            var param = new ModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                key,
                ct
            );
            var result = _subject.ProcessPayload(param);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(pt, result.Result);
        }

        [Test]
        [TestCase("019F68727783AE60F9CCB5BF611D9B26",
           "C460F44FC6B84814735567794ECA496B063A5752B0BF41D25689E575F202B97E03B27BB2E56D6A59F21831B95505EADE41A7C11A9B9FD94013D9BD31123B2DA50EBC7E39A88B13C854F3F189FF55AB13BAF7B01FA7C83B82EC233EF836BF4DE4A2310E0119A5D5F2C769E2D621C56351D1F3F05F1FCFD61DBE98DE7D7DDA3137CA134F63A67D07AB56D0EF7EE5F91ACAA1BBD367F717F8F194B47E20502CC2B6E8C8D03C8F050CA45BE77717C16BD13CB82FAA3F9119FF9A4FDE68B3216293CEE7B1E9B736D3F94CFB6481F7B247D0FD67A083E4FAAA67D17F060F89510808DDDE295F926839DD3B13A5EAF8EB7FA15E68BABB2A3F72F7474352F1130E0DF1E55D6D7653A639B723CBC256F2F1AA24701A1D9743ED664B557D05CB1F5CA8FE6997DBA30FF0BB7CFFFDA8908AC7744C0E714DBC0D7689C1007D670F2238B4CB5B4D1D663EB995BED62B0DCCAE66C8BC8C2804D428044C87FD1A5DA5AE92A70DC957F753840075124676C059FE86DB5D2316A1CA467E2353A1A267B3CBDC3496840B538AC4ECD32EF24A534B57ED2965CD2882E3292C191DE1BC3F572F675D9AC1184727EBA986A6F9CBF65F9B0410E1FA961A2877F5AFCC333E334B92CCF9E9AAEF4F68CF23FBE851F9D9D8F84D5B1408D2242950078A1180164BE791D89FD2A4EFADF71918B31EC06B3001072FE37F26C523329F9DD857E768808242BB5481FAE008DCCC82D92B04C3C7295F717900998DA3E6599AB2ACCB797AF243B7E187948F7280307C0DC9AB7DCC524AA4D1615BC33A2343E8116D613858D94C85A8D01511CD30D3D5C4D3B1E6CE6A0714E52452C678EC88F79862BF4556F450BEB07AE8D46630C92428D3B0C21ADB4BD0B3B72DA897FCFF91A47C419754FBA97496CCC21A3BBACC13CDEAC2777D2D73DAF913410069E68A7294238E3243960EEA4B2EF08EB645E4F4B8978F226E93B433AE842D63E5F4A3875A785F1FAE84AA5C0D310B3261225F277626F40D6DE5EBF3433DB6EDF31C1C566151B0156C5B1AACA4FD075A87F8716572207F024A04D90B8ECB7AC9304F5A158B922BD387D248F6283F1147F079BFBE07E73C4D31A8E88C2538E7B45143ECB544F8D447B6794DAF6EB0AA3390FE437680CB0D1D059F0D5FD9E14CD481B7A6D373A14534F0408A150541EC3F07C26E710D696BF7175F578A2CC487418E52F15BEFB2F5FE7050DFECC3247C5C293A246022DB277312968568A0B584D02C4E3A829E15792C23914D34DC7E8E9CC8F83FB194B48FC9D4907169DE188FBD84BC6A52C1455568EBB915B2A4FB03830063813072098C97A0FBC7F47E64E53843F12FDF028E004928B2BA214C30C49B84EFCEC451D2AFB49C6DE1C18F7D1AA5DB61E269968773CE28D20321BD280482BC5672980AC19A255BC667755C281C2BD4774A51B281F1F688D885EBF80A7A03E23EE9ABA9EE1C25B295EA887EFBA6AEBEFB2C372CE7C0A6011B8727E64CBB454FE57103D6DA2700AE76387327F31210FF6C38B450C9E85341C086D1EFCB9CEB09127C900CA51AF6B07C01153A026788ED76F28238C458CB767DA9F0D1D77CB08B73768E7CFF87F2EE7078A8955FD0EBB157A86B7B275E7FA1F99093970967282496740BEE18E096E26EAA9644FAC4061041F93800A146007E64A75C9569609907BCA25A65E12CAB90A90DC227DDE4485525FCF8A26EE5C7F1DAEC1AC3703FEF30D27428421A2C0C399BA6E56937DA6B7F6ACDD559916B7BE5E57592ED6547368E76FA024863B323FB55890DE9CAAD666CDA800AA7CE662AD4BC8CF63D2762C2A890B0169E4746FF7574A3BFBC3F3871DCB2BA5028EFC63E8D98498F6328979E15454C213C8919262BA8FA6E0DF3C31D4BEF1FD5C57808F5B1ECA0B76D71A86FE3B0B37F5C8461FB887FC931E5782B2D4FB7000B987B4EC9C5A9A59D85A1A6F4DADFE6382012D44CFAB7511341CD9E568A25591466824FCAE8D49231D05CB3D9930BFF83CFB652A8CB668DE745379D57DA76FA387B3AFFD963B6EF73BBA0B3889F974ADE2579CD845ED44E655BB280D7474DD283E45270DEE924FC9C3AB17D1ACCE899C60EE4A4ADF9DE8BEC18E9FD5F522A2438BB7F70395D59C3C926C662EAC1674F5912D3016C858D03558CCE38ADEE23639D9B3E811B42AB9910512A3AFD3318BB5097EC83B5C7C0773C9BB2E5F76A15F461C496027CB8E6459589C92CFCCA9F0871C5A2C3E28527349DBB3A02874122DA7D27059C3B8980F35ACE08DD",
           "7C29062D4F44A62D2F09DE5287A03775D26ED17EF1A30E279E90DD8F11052FF3B83C4D3E7B9DA22DC97FF1CA578B7FEB8C454BC8413FB5902EC56711E38019F84B90A16ACCC7BA771BAC5CB8B0454E28F4E0F77BC9B5D80D7E12DD89D5D1D2414EEE59E76C9F7EE02CC8B3E3B4111B30BE4D47A34B1CFB6A2F37FC776464F749D377495985468FE75987A4286BF3AD91EBCD254D3CBA47E3266A25DFE7A0323585181E864D162964A821A2C3D59FD5408D61B940438C69731A04664A6AF2D5723DE07476B9A242E644F30B71E0604C358BC6751B225BA45896F9DF02BCE4F7688F00BFD31CA1B5C9BD7D612BB83F81AF09A6496AA6DCB94FDFC87EEBCC189C71408AE000F3944045E95D61245C6F88A8E1D67AE904FE1B5B9BD941174B1385A2186D7B65338C18B1112257F87F0A3D95419FCC78E8AFE0B2C3D85500CB73BC609EF2484FF65CD55A4681274B7630F487F80946F766250BE2EC291BCB1AB607386DA3A32FEF1772782EBC2BD65CE1D7943EB18B9CC7D8271DF93F983473AB57938863EC3AD1A5C9705E30F7CC82BDB9121A25953D5434F0474A43972B8263D2A0E77D6784CA339E7708DE245CD0374A4C0E4F7F88E70E9EB0CCE798ACC30908BD09A1943202A5AB97FEF444CB84152093FC0C773AE016EA010E410D96E1F7CAAF602A87E264BAB83A6AC2C2FE502D5D38DCE738E61ECA32548CA8CB6EB72C6D1C13F189D5280A42F7742B5EA8DAFF580FC897346FA6EE5F17D8CE8789F6FEE12E608EEECA0FDE310CC1B681C812E1377979B097076F9E5E72EA065BA106D1DD566E7F78DDC1A2D0DC2378288FD4F02B0D36AD8064606D970577A6FB28E25323119A627A486E675595C9EF0C7A4E4C8B1401C702E9C26B99BC2CB669DCCC6529318FC697C6CA3567492873C3D85B4BC1AC9623127B2113821EFC6279585F51426C1120D4306B6D1CD754752E64ED42212F79C9A6ED2A7633C8D8F9F054C7710AA6CB14116C4DB6448045EDB7CC99F573EDFAE24874E331E295192A8ADE182315A0F716CF285A524DD08F6EA0C1FE8E4FB0CB4C6215FD095F08D55B4F57F6D349E3901D41171C83123D2409125C91CF78C2B3DBBF4D2B57844FED6A68F36EDA9744069DD335158D2D684304F709C953264F5310FED44DDEF3C4D1ED5F09D2D900ED4013459EE3A88567AA067776997A6EEA3A0BF7BCC5D2694EE945CA8104D6D4BE8AB29143E50909D6BB6440C3C5E6097DAFEB61C3E025CADA09C22F91FD21C2ED5B5AE210659B886A866EB450C115C45E1C9341B27ADD6881D32565DEDA97A8DB9ABC0ACE876519A368EFD680C5B1AAAF4C60C7E728ED611496C18A1A3268795D8F81B98E62D86C908014E5639519EE80FFC23BAE358BD909B78B66496290B0F26F6C2B2251B8E1E8A718E6B15AB06ED9AD8BA73C18BDE71C448294CC45AC57BC45AD33A2D68A885D128DC5A830E3E785DFB44921CD8A73D0B45D6100F002262D2DAD36C11425BB53AD42D9FDE90EA115928BFDAE9ECF39538464C6F893956B54A50EF88C2E8292B7F240A40A5312731CFC6E6BEC7DCADE3D248800EF835A5FEBCE03CE17C509FBC04D1DC0D0B0D22AE2CB53AEB50E94ABD3E78CA63CD13C5E4D82B9127A8C6A6443D6BBC8AF280F18E54C1AC89528F23C974027E9A704C6ED545135AFE02103DCDD53A434B4BD4751A733F43C6E8E8583DDC595EA30F56F27EFAC249222F751C8303AD66AA46C3A8D50202F3C95AB1907DE0B63B06FC943ED8DEA44B18D96A9FF9F6FA432BB0D4850EE9EA1C25DAAA1652DB1CCE2A352BE305F3694A6B1EB83C53F58D384AE5AB92E656DC152C4FD7B61DC36C48DA8A05854B1D702CBA155C91DB4D642B8F8E4ABC14D97A1D07E3C5DE496C9F94ACD9C19B38A9911058D56BAE50A82CAFECC78E1ADAB6124B1BFD450BEE1A2CFA522313D386555F5FFFEE372FA402E3B83477428F936C442F1968F4E20A61646506CE00F4CC95FD79B26B04F115D816B369F2B8B4015D6D0A4D4F8DBFB16A9D8F9174BC43BA6FCC311265BB810A7DC033FAE7DFD4CC64D2ED30C0D0BC3A41150122F709E595EA77A2E4282E3910159CDF8B15AFFDD6874FE4A13958CB16914DD1F7B649A3E8348B14253A20E51A74EF993E8F0F8B2D13EAEEC8B816DB4EE3893F021731417421913FAC50F10C800125FC3F0B6EB5FD3C7C7C894D4331E4948D7C70E1F1C2F15620489BEFBF835A0DC745F9871F3E66A11ABFE9E3C77B397B160E6F9004B0BA7",
           "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0",
            TestName = "AES_CTR - Counter - Encrypt")]
        public void ShouldFindIVsEncrypt(string keyHex, string ptHex, string ctHex, string iv)
        {
            var key = new BitString(keyHex);
            var pt = new BitString(ptHex);
            var ct = new BitString(ctHex);
            string[] ivsarray = {
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF1",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF2",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF3",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF4",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF5",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF6",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF7",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF9",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFA",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFB",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFD",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF",
            "00000000000000000000000000000000",
            "00000000000000000000000000000001",
            "00000000000000000000000000000002",
            "00000000000000000000000000000003",
            "00000000000000000000000000000004",
            "00000000000000000000000000000005",
            "00000000000000000000000000000006",
            "00000000000000000000000000000007",
            "00000000000000000000000000000008",
            "00000000000000000000000000000009",
            "0000000000000000000000000000000A",
            "0000000000000000000000000000000B",
            "0000000000000000000000000000000C",
            "0000000000000000000000000000000D",
            "0000000000000000000000000000000E",
            "0000000000000000000000000000000F",
            "00000000000000000000000000000010",
            "00000000000000000000000000000011",
            "00000000000000000000000000000012",
            "00000000000000000000000000000013",
            "00000000000000000000000000000014",
            "00000000000000000000000000000015",
            "00000000000000000000000000000016",
            "00000000000000000000000000000017",
            "00000000000000000000000000000018",
            "00000000000000000000000000000019",
            "0000000000000000000000000000001A",
            "0000000000000000000000000000001B",
            "0000000000000000000000000000001C",
            "0000000000000000000000000000001D",
            "0000000000000000000000000000001E",
            "0000000000000000000000000000001F",
            "00000000000000000000000000000020",
            "00000000000000000000000000000021",
            "00000000000000000000000000000022",
            "00000000000000000000000000000023",
            "00000000000000000000000000000024",
            "00000000000000000000000000000025",
            "00000000000000000000000000000026",
            "00000000000000000000000000000027",
            "00000000000000000000000000000028",
            "00000000000000000000000000000029",
            "0000000000000000000000000000002A",
            "0000000000000000000000000000002B",
            "0000000000000000000000000000002C",
            "0000000000000000000000000000002D",
            "0000000000000000000000000000002E",
            "0000000000000000000000000000002F",
            "00000000000000000000000000000030",
            "00000000000000000000000000000031",
            "00000000000000000000000000000032",
            "00000000000000000000000000000033",
            "00000000000000000000000000000034",
            "00000000000000000000000000000035",
            "00000000000000000000000000000036",
            "00000000000000000000000000000037",
            "00000000000000000000000000000038",
            "00000000000000000000000000000039",
            "0000000000000000000000000000003A",
            "0000000000000000000000000000003B",
            "0000000000000000000000000000003C",
            "0000000000000000000000000000003D",
            "0000000000000000000000000000003E",
            "0000000000000000000000000000003F",
            "00000000000000000000000000000040",
            "00000000000000000000000000000041",
            "00000000000000000000000000000042",
            "00000000000000000000000000000043",
            "00000000000000000000000000000044",
            "00000000000000000000000000000045",
            "00000000000000000000000000000046",
            "00000000000000000000000000000047",
            "00000000000000000000000000000048",
            "00000000000000000000000000000049",
            "0000000000000000000000000000004A",
            "0000000000000000000000000000004B",
            "0000000000000000000000000000004C",
            "0000000000000000000000000000004D",
            "0000000000000000000000000000004E",
            "0000000000000000000000000000004F",
            "00000000000000000000000000000050",
            "00000000000000000000000000000051",
            "00000000000000000000000000000052",
            "00000000000000000000000000000053"};

            var ivsCorrect = new List<BitString>();
            foreach (var ivgiven in ivsarray)
            {
                ivsCorrect.Add(new BitString(ivgiven));
            }

            _subject = new CtrBlockCipher(new AesEngine(), new TestableCounter(new AesEngine(), ivsCorrect));

            var param = new CounterModeBlockCipherParameters(BlockCipherDirections.Encrypt, key, pt, ct);
            var result = _subject.ExtractIvs(param);

            Assert.AreEqual(ivsCorrect, result.IVs);
        }

        [Test]
        [TestCase("C0EECE36B839C891C765CB61798D6B3C",
           "C4CB467D893016E88ED46798EE93394ED7CDE3EA77A55E89FAED83EE5213C2EC349A35EE24554DA3D846DB17C2C27E4B541E743B7A9FD6FABA6AD3D7D3EE2E6AEB2A5B6CDF8E6C4C085D894EA019F4CDC457DD817EAE178F06147D90A1560161947B30B79043A2329FF2EA922EDD0F37FB752DD450C3E61CE2EBAF81C10C3EF3EFB72BAF51F042F22B2E4464CAF97AB6963E9DE049F019BF2B81A7D335CD60E457B48BA994A676985677C56A83119D89BEE54F21F196A77B61BF55C7899869F198C7766883399CE5608E77F16C7A7F8149316C9EBFF9ACCD1C0FE2308CD727CD6832E50293E7BA2A830E1B9D63C48B43EFE29EB066D1237ECA094F3E641FF47B2BDAB6F8BE6F63E8E1D26FE27776D76C4E464F805253A81E2AA471F6CF9CFCF3F092143B31D98A3F62F1030EF04E223B001EDB6012E50E4016E7D249AEC6D9E5E05F3099B2D78A29A761666193BE05FDED1FBCA982DD52212CC612BB0331A1C85297CCF64B0F5BBCC75D3D65063A0728A3D57A4F8E2C7CAC567BA80D935286E5C76C9B9AE6A9D2686F4BD5FF89A99DD4D4A60E8A176835EE32B29F1D77CA089D5CC70EA40FDBCEB16092B7C41BAF5FF59920D89CF0E3ADD510AB76B55A32FA2710F1AEE99E36F5AD42FF6DC43E4699A968FA5071F1188F0AB1ADDA048811B0CD56F60BB458C7EFEA91C9B57FEE5E82EBAFFFC60B00511D1CA606B18E208776F77E5C23C13249A82E0551C30E343511623BFB8BFF082F2A7AB73EFE01F6F53B55D8E9B4FFC1BEE73BFFCF3B2ACBD45AA407A7835CDC4FE1335CE47F1985A8FD417B8EE38ED9190ABE57674FEA63865E365783A9F336B7E3BC788E19CB7C3EA34EB99EB02A0BB228B19BB913BF0B5AC275AFFB5829F7B7E02CB8EF4002CAA5786B275B4426FB0E32B0ADA0AB16E49CABB991E371F4AACBAAFDC4AC383E62D55146D0F75CB713545921F003F4A1B4A5D9F12EA3D81CEC48628412587A552F23B3A1425FBBBD8ED95D69005D3D0D4FBD553F026862167794A68BB5EA9171BE5A7E2E10445E0AE4DB5A90B94FB092D4186EA4C938AFAE51C3E5A06A9F99E86A17B9B1F01125BE611E77B116B3DA5D34102B4ED3FB88780B6E3F3502BF295B8D714493ADE5A3FF66429F51B0BA1C91F9E86093FB265AAEAB679693118C000170276C3345D34695A426752EA933DFA10151C203C09983F9A48967F37A4726DB7049848F5C1EE2BBEC212D7F1FFF38125A11DB3768714AF4B7C851288F32B6D1A5942DE1453AB2DD892A750B8E3BC1F6B2F7253119CC709E2BCAC90FFB6CE5612CBBBAE1F5EA098042F404D80F75BF81D74D947593E38D607623CC4FBAF5D18D0285814DBC9944476CB914A62CE6C1F71A8404CDF9E8E6AD08777A39317639579A30BFD06DD906DC1A8E5D7A9195D7F15DFDC6F44D6CBDC255EE26C3755ED5CA33A63C03D81EF5E079523AB7AA3C9C2F3AF09806406DFDD73497EE63B97F2E554508B62EC511E1F50406614A43F2594452E042A4B41AD2EAD40115B914F9E7DB9D7EA90481EE107C954C211257D2D84951025DA15ACBD3EE0C371DD58E77422519B91D1C3BAA47AF829C708709DE21F1D6A431561F0194BA9AE4AFC89E921905D42AF179C3AFED82597CA2EE856DD3BB00BB67A87C16500A33E0CC292DA12C258BF5F5DD9922AC3E4CAD6D700CC79A670DDC6ACAB7A5FBC9088A9C70EE3A30C7EDAEB620D4E1976B851F4E78E05995CF51BA635D6819D44216CABE318D325E9B254DCAA197EC21713AD3E48BE81293C7BB29A24CD55515B7504E0DF652D13E69AA378A732051F9F40638B37DD1249023C01102CB9B08D2AFC1EAF290E028A9ED51B5AF793553D8BEF53E55A705D6A242DEED1FFCBF8F1F5AB0BFC7F90FEF325CE0B95473676F11DF0EC15BBA63836F88E3017F12848AA6A52916E94633E78051F8B82B755E86445E00A387653448719906FBE08F402B140B2EB3FAABCE118395279003D51D7683C491C03E9996D45823C2B758B77CA8E1FC8D48EBE24AD61E3E3C1D3A8D6C34505FB25D372227A25319954482EC50C0B2621D6F096ED53522C9E2653BEAEEEE6845A4A9772F0AFF6B96B6C0186CA001A47807297BD579F508298C422293390ADAAEB122EEF438BA8C99458363DD5FAF0C04F974F4C99C1A67929DAFE99699B119199E771BAC069DECFBF218EFE187A6D976D9BB4453237925CDCFF49224B67FB13464EA833825E5C03DC71742C2E8368730D4A0154BE57E0322B04192C6",
           "D6E6BB50365EEEC1BC85E6EFED7DDC896517E51A877BD6F8292BFA2AAFD019E0525259527B8025288ACB144E904680FA7CE0F0CF7DB696C356353D706572FF7B4E11923A135E437962F0B309AC6B1AFB47AB217945DA6BF09FD9055ADFF8317918B2E424AABC548025B0EAA3F3733AC6B76FDF136CA2C75BD7758866D46E0AF75EA48267EDE29883182124B7AD6547366C986A3433EA55601482389B2C749098546FB8E930B246DDB6B37DBB3C498300C9F540DCB924F31FDF15C1A41C11A652FA64EF590754305BEAA12B8D4702734272BB5264C63BB96C413F5CEF078DB92A9693F2BA1F870706028D9EDFAE6485DF105C28E9E61A6FE227A3239D09FAA983AD623FAABDE3CF70DC0F54DB676571904219DC0861223846B06B695DE4EA3953A0961783703E88958296CA1C617D89AAA25D016F44C93BBE3D466DC2702F2D8EDBF770740E4E0BDD94FAD3DD6E8B57A0073C03DC85A3D0A733EF1783AE4BD21CFC7A15B929229F539A4688118AE9FA37D228B6206155157F008AF277D7476A4E39A3E9CEB3F7556AEF2DBDB53D4B94849AF17BC745BF085CBCF4E7EEB453E034272A4A613FFD7140CA02E454D9B67355A18AEA26D916B224D7AC58CCBAAAED6CC321175E993018EF4749EED351967382A6F575761A50B660C10CB35147D3320ADCDD27BC08021324BE84BDB1764257EAC3406D61980C671078AF980C48969D37EF7D853EE9BB9650D4485E0296A8323F97F434422A436D15D09D5C2CD228910F0BC5388F4F51994D6EAB6A12167D43FA924DC52A33F1F8B98344096027507A9A66F29EE4B96AC5D41C1A8F72923D98299B27DB2EC017364AE4A6937A4A026537C6558CEFD7AF0FD18BB25441662685D2D7F571CAAC3F26376FE8FB04291B931D9FEB5603E500EB47376A34F2229339648828B6ACAAA44494B213DC102D2879D61EF92993E37DD8BE945BEE36CDB622F8586EF43D8CD44CC52326ED35AE5E7E6D6B85AED4F1F5D25D535E47340B0A9D48B888DBF4610E2D9F3D6D948C9E44ECBB2132A8D6AABA45438471CEBE08ABD77E7A786AA7BAF477455084FD7CB5ECDFEE582FB14CFB9C41DB14B96AD06E1384E9118D5C6F08F4109F4BD782D78CCD910D227FB231C240DB342017914A0A8EC495B48CB7F6000459EBB571A7D5B6F90BBFE862B9FFC5775D2E3EF822481A79E12B7A70EB3EC34DC83897DD87796983D6899DDDB19D856A2E82E2889DD3B809DC68854A26348662EF2C680319FD4D3A03EBD896268E59A9D502D8E9F948D3A09051CCAE4FEF398624BED9E599791509416B1C0CE4BFFF79BFAAA718B7E15CECB8A46156A4E0EBE39DD7A829C955AF14B18F3A1FD4EEE6433E72740734DA98E110EFD3BE2295FE98D8BC3A4846BF20F49F3F96C5E759A496D374C750809C3A6D3A7BD9AC5565C1D386891A09C4BDADF5958A602C14FAC46A7D32322E5814F6036AE0BC933870EDA58CB989F103A70C85248F24F39BED1CE4F599B0CCF5428FBD5455AED1F8AD6C970B62D614829E4D6809B232AB1A22D66E023DB337CD1E04D8E2C1C8D985668AE2E037C8F06425FB874001AB98ACAC5F1B6300391DE6F6FA8972980029F61B377F18A62DF6B3488207DB92E4C2E74CF52E5539630B4CF68954684B549FA80B1AF2B84CDC470375AFBA08741E04F5C9BC2CAAB299C245EB4A19CC573778C6040F6041A4E8643D00FC9A997028AEBDC8F8505155929E67D02A05B07F24822591F15E950DDE6FB49257D52BD4E29FB1570F1C448AC8D664362F9AE1F838293FA13F4EF2F22694E281A4E92BAD10CC344907DCBCC11FA2192A34AD34E1BB787692DC04E384302328AD493F1CB61AF3395E00EE501541F918574443EA8F7131F25F74E1629E92BAD5D01E1EC1631EDD0450765DE9F3C2BEA0D905B6A8C8587619D2E3EAB2EF2EEDC40224AE9A7F404F45C29FEF4E13979D0200CF734D733657073AD43AA7F5306E35A6496880FEA8BFE0889A41AABAD9B922656144989762001E28265786F559E2731C715A29031C277AFA0505CF23D0169EAA6EB2639FDF6D3B595A65E45F5FCC7C742854C613A4188D83879FCB476D29D729A075CA10924B0B82A609B7D68D1BE43A6E553A3BACD8069147323B165E02912DB83A6ECE6B63D22F3A79F5AAEDF5E77D7FCF99EDD2B7DE78BAD1E49C149FA363CB5D1233DB5B4D9759DFE99DEFD266A83D12F09AA62B56C67277F4826BDEDBC0CD9A6145295C6BF5F4E73CB4593D89C576531595",
           "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0",
            TestName = "AES_CTR - Counter - Decrypt")]
        public void ShouldFindIVsDecrypt(string keyHex, string ctHex, string ptHex, string iv)
        {
            var key = new BitString(keyHex);
            var pt = new BitString(ptHex);
            var ct = new BitString(ctHex);
            string[] ivsarray = {
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC1",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC2",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC3",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC4",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC5",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC6",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC7",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC8",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC9",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCA",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCB",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCC",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCD",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCE",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCF",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFD0",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFD1",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFD2",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFD3",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFD4",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFD5",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFD6",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFD7",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFD8",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFD9",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFDA",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFDB",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFDC",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFDD",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFDE",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFDF",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE1",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE2",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE3",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE4",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE5",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE6",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE7",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE8",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE9",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEA",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEB",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEC",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFED",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEE",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEF",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF1",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF2",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF3",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF4",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF5",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF6",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF7",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF9",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFA",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFB",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFD",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF",
            "00000000000000000000000000000000",
            "00000000000000000000000000000001",
            "00000000000000000000000000000002",
            "00000000000000000000000000000003",
            "00000000000000000000000000000004",
            "00000000000000000000000000000005",
            "00000000000000000000000000000006",
            "00000000000000000000000000000007",
            "00000000000000000000000000000008",
            "00000000000000000000000000000009",
            "0000000000000000000000000000000A",
            "0000000000000000000000000000000B",
            "0000000000000000000000000000000C",
            "0000000000000000000000000000000D",
            "0000000000000000000000000000000E",
            "0000000000000000000000000000000F",
            "00000000000000000000000000000010",
            "00000000000000000000000000000011",
            "00000000000000000000000000000012",
            "00000000000000000000000000000013",
            "00000000000000000000000000000014",
            "00000000000000000000000000000015",
            "00000000000000000000000000000016",
            "00000000000000000000000000000017",
            "00000000000000000000000000000018",
            "00000000000000000000000000000019",
            "0000000000000000000000000000001A",
            "0000000000000000000000000000001B",
            "0000000000000000000000000000001C",
            "0000000000000000000000000000001D",
            "0000000000000000000000000000001E",
            "0000000000000000000000000000001F",
            "00000000000000000000000000000020",
            "00000000000000000000000000000021",
            "00000000000000000000000000000022",
            "00000000000000000000000000000023"};

            var ivsCorrect = new List<BitString>();
            foreach (var ivgiven in ivsarray)
            {
                ivsCorrect.Add(new BitString(ivgiven));
            }

            _subject = new CtrBlockCipher(new AesEngine(), new TestableCounter(new AesEngine(), ivsCorrect));

            var param = new CounterModeBlockCipherParameters(BlockCipherDirections.Encrypt, key, pt, ct);
            var result = _subject.ExtractIvs(param);

            Assert.AreEqual(ivsCorrect, result.IVs);
        }
    }
}