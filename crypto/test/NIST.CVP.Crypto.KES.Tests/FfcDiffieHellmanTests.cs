using System;
using System.Numerics;
using Moq;
using NIST.CVP.Crypto.DSA2;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KES.Tests
{
    [TestFixture, UnitTest, FastIntegrationTest]
    public class FfcDiffieHellmanTests
    {
        private FfcDiffieHellman _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new FfcDiffieHellman();
        }
        
        //[Test]
        //public void ShouldConstructWithoutCast()
        //{
        //    HashFunction hashFunction = new HashFunction(ModeValues.SHA1, DigestSizes.d160);

        //    var req = new FfcDomainParametersGenerateRequest(1, 2, 3, 4);
        //    var ffcDomainParametersGenerateResult = new FfcDomainParametersGenerateResult(new FfcDomainParameters(1, 2, 3), 4, 5, 6);
            
        //    Mock<IDsa> dsa =
        //        new Mock<IDsa>();
        //    dsa.Setup(s => s.GenerateDomainParameters(It.IsAny<FfcDomainParametersGenerateRequest>()))
        //        .Returns(ffcDomainParametersGenerateResult);

        //    Mock<IDsaFactory> factory = new Mock<IDsaFactory>();
        //    factory.Setup(s => s.GetDsaInstance<IDsa>(It.IsAny<HashFunction>()))
        //        .Returns(dsa.Object);

        //    var dsaInstance = factory.Object.GetDsaInstance<IDsa>(hashFunction);

        //    // TODO yucky cast... is the only other option a ton of generics?
        //    var domainParameters = (FfcDomainParametersGenerateResult) dsaInstance.GenerateDomainParameters(req);

        //    var keyPair = dsaInstance.GenerateKeyPair(domainParameters.PqgDomainParameters);
        //}

        private static object[] SuccessSharedSecretZTests = new object[]
        {
            // Good ol Alice and Bob.
            new object[] { "Alice", (BigInteger)6, (BigInteger)19, (BigInteger)23, (BigInteger)5, (BigInteger)2 },
            new object[] { "Bob", (BigInteger)15, (BigInteger)8, (BigInteger)23, (BigInteger)5, (BigInteger)2 },
            new object[] 
            {
                "from CAVS test1",
                // Private key party A
                new BitString("a388f6b35f25619d1e76ed7cc6d10f8f3a6d919e6560c3f0090eb96e1f27d908").ToPositiveBigInteger(),
                // Public key party B
                new BitString("a6f3b734391112b2ae81cb3021bc1275383c4a6c80b9a1265ae3f5a8256a0a621667f71e3e081b86bc427d5adddb901aacd4e3b7b48170c7d13cd9dccd7b04072d97a7c39fc44c079a48fa3d2495e90659786275f13b5ad2402509dd12637630de980369c5e55953091df4ef3e369880547737df1d7bd7a9d437488cb01a94b444c05ae33d6ae6126bd0c9c7be19adc752f210f68ee7657a9151ef3299b4e4e12f19ba9f3070cacfe1af329e69a2bd6416c51c7d33b183702dbd20df91ca09c5190321f59f55fca903546cc42c3edcb3c4eb30b3a4228fe2e1906484174715e0b08d9d93c4588c33739f416ca45a33faa861dc6e9fb7831cc95d2d2741533be3").ToPositiveBigInteger(),
                // P
                new BitString("d5cf9ba288ff8438650904a5fe2eeb8bf6b52b691a455b21bd3b37998f82544036ae61fe436039c66feab83ba21a5ba13e7b4c1692ac82c65309eed54e593efe9831cc82bfd11e9552d4eb7d8f2c233024bc10819c57093890ec19aa9ee915e524d81c285928a5b87acc7f496f93689ed59b15183689ec5e487fdc9fb994c4bf7e1dba57f8f12c17e404f68b5506d69f4aa98146f6ba3be1f3397e5d41c4235a18b38c75417bb9091a487db1fc89eb2f73b4ac6f6c12dc5634a342cd9d10605090bd544e1c0813aaaa3814166df84a302b8df38babe9c31e4a2c64317046bdb841fa8717e22dcf1496f9ad94c17f1ff9175567d02ab5e30601e2e5b0884e9a77").ToPositiveBigInteger(),
                // G
                new BitString("37a7b2a094ed6253784d51c71a4a7407f0727df15e480a02b0ff2f9befdb0e92c0d63e482c6909e5337373abe347ceaec25a9f2a23f1770c2447bfb5c35dbda80d3f00d8b6569a0165a3d52f1cff03e02b67be47b2f26ebd0fc13299fd0317719c3bcbbcbba0a982915b5d68fb4c5c483c3df12052b56ceacd16176c783d56422b1366cd5ce65922b734a1780acf35b4a658cfe8166469ace04b87a33e1e8d4603ae3f9607d1e708137d581aaf2cd86608ca06e52cddbe475f8bdb4597cd5b55f47380276c86d08d8ddbbef4ee3ca76cefe6ff9c74d4d5d1be3407e30720be34cf41f482be8bc6ed6e89ea7cc2d164da1dc1c9d0e2762eef983dc9e96f3bfd50").ToPositiveBigInteger(),
                // Expected Z
                new BitString("93dd543113b567adb45dad4149a7503768e5c4d43964a578357541df39353ce36910a75ae7833914e105f732f0dc34339007dfb14aba37a65ad825a2a63f70e5522bdce9e2e757c103104139e09671668d8f1c3edc2d4b41a623f0b24332e1d0569e992ca56855443e2d1c587b853dcc4fa816ca63f95f6fd1fa735d58389a3110b4863fa791168ae509ba1a87fa1d9e1ddde40e91126179c9cce0770555078e788add2497f9f30934a917acbc6e8377e9c20573fd594aa5e488847602ba83e721a8a7f24c198bdb81128edef59949e359b590ed933810b692cec57c65ded444ba4a221d86ad5c4ad70d1dcd2b47cc0c70cdf2ffaaf9d893e3bf3a5d0880bf05").ToPositiveBigInteger(), 
            },
            new object[]
            {
                "from CAVS test2",
                // Private key party A
                new BitString("41f8582a2f3ac19cc925fb5cf72ec98f89630fec8e71853397870c3fda1eb08b").ToPositiveBigInteger(),
                // Public key party B
                new BitString("4fc5ed02abf0791bb01ad33527bd16ee58f875bbc85de5b513ccce95d05d6c735e726a102f11a69152f1c430b027ef3f2049e07db96f448d9b016ebd9258cd33175c06c00d107c99ccfebc8e77b19c61ed7c9e1d8a3e912a893b30ec9ab1d59d2bd5133c9669090c7bcc48318a21cabffbe56477e9fda36aba9c5e4462854fa682f503de3e895579b0c51a2f2a6b1fc59bee015e8bd89041a2efafbe8ea491215a88302f6a4858ea3c0d846e52f6ea070b61dda82009bdf0ed13a077ea1777233e059fed4c45249c5bbba1fbaa6f574058ec58d56e6b3a486e0aa306ef2d63d70617f6b384fe238e82832dfeff6317aa887aab894efc9b60e6f5127581fe735c").ToPositiveBigInteger(),
                // P
                new BitString("d5cf9ba288ff8438650904a5fe2eeb8bf6b52b691a455b21bd3b37998f82544036ae61fe436039c66feab83ba21a5ba13e7b4c1692ac82c65309eed54e593efe9831cc82bfd11e9552d4eb7d8f2c233024bc10819c57093890ec19aa9ee915e524d81c285928a5b87acc7f496f93689ed59b15183689ec5e487fdc9fb994c4bf7e1dba57f8f12c17e404f68b5506d69f4aa98146f6ba3be1f3397e5d41c4235a18b38c75417bb9091a487db1fc89eb2f73b4ac6f6c12dc5634a342cd9d10605090bd544e1c0813aaaa3814166df84a302b8df38babe9c31e4a2c64317046bdb841fa8717e22dcf1496f9ad94c17f1ff9175567d02ab5e30601e2e5b0884e9a77").ToPositiveBigInteger(),
                // G
                new BitString("37a7b2a094ed6253784d51c71a4a7407f0727df15e480a02b0ff2f9befdb0e92c0d63e482c6909e5337373abe347ceaec25a9f2a23f1770c2447bfb5c35dbda80d3f00d8b6569a0165a3d52f1cff03e02b67be47b2f26ebd0fc13299fd0317719c3bcbbcbba0a982915b5d68fb4c5c483c3df12052b56ceacd16176c783d56422b1366cd5ce65922b734a1780acf35b4a658cfe8166469ace04b87a33e1e8d4603ae3f9607d1e708137d581aaf2cd86608ca06e52cddbe475f8bdb4597cd5b55f47380276c86d08d8ddbbef4ee3ca76cefe6ff9c74d4d5d1be3407e30720be34cf41f482be8bc6ed6e89ea7cc2d164da1dc1c9d0e2762eef983dc9e96f3bfd50").ToPositiveBigInteger(),
                // Expected Z
                new BitString("93dd543113b567adb45dad4149a7503768e5c4d43964a578357541df39353ce36910a75ae7833914e105f732f0dc34339007dfb14aba37a65ad825a2a63f70e5522bdce9e2e757c103104139e09671668d8f1c3edc2d4b41a623f0b24332e1d0569e992ca56855443e2d1c587b853dcc4fa816ca63f95f6fd1fa735d58389a3110b4863fa791168ae509ba1a87fa1d9e1ddde40e91126179c9cce0770555078e788add2497f9f30934a917acbc6e8377e9c20573fd594aa5e488847602ba83e721a8a7f24c198bdb81128edef59949e359b590ed933810b692cec57c65ded444ba4a221d86ad5c4ad70d1dcd2b47cc0c70cdf2ffaaf9d893e3bf3a5d0880bf05").ToPositiveBigInteger(),
            },
            new object[]
            {
                "from CAVS test3",
                // Private key party A
                new BitString("60872d8c70ed18f467e4b306b22f970873a46d3347909c119b5107d160fd9129").ToPositiveBigInteger(),
                // Public key party B
                new BitString("55b25962987f35576d371abe075741d4b20b04e989d4ea6dcee252b3d90aee720445816127ad49d4d8144e6f5fb4e1d459abbc48bfd419f33489599f4ad56e0f49d9b914d66bd18e159917f390073edeb0a186a25ec07dfa24585555eb0fa73a36551e6f4becf8f18e5154638f9f46539679438d68ba06db780a02416add027b2d36688bf1988d376148d9db6cbe11b6bebe4cadbd0a0a60d73e95d7438d5b8985bd3147f0fa09a638d229a175d0a48cc764d97643b962a202ea0fa283d869e2685b67345cc70771276c584ab6525a803c4649282572637ce378777b1d52cca631d229b052c8f10dbe2f5d408a4a43459b6cdaf4e7f0f6abd12b290b7f253942").ToPositiveBigInteger(),
                // P
                new BitString("91d80f6b6958b04e87da7e3258db8dd60ba376cdfabdd635ce04a69e62bb34202f2a1f8fc4460a12cd2f265a2ab03cc0360597dfa6ce72b25b4545dd1769d47a94d82a079a0a7b8101cb628c9c6e25934f2d91caef70c036e7ca4f6567aa053ab5bbf0e6e3524ccc5204cf9884bf9a09145277106efaca89c76662c926a40337b4b2800a00e1db8592d259973e7078fae64636355846c4d3a020e451a6e36c119919d6f16d5033bf7b3159c34efc41ecde98b7431881f37ae9f26aabe19e26bf93476930939362c44465f1e7d5c46ad381da742703a685c8323a590f0e3ad462547139a880d1ea59aa712f8ea0cb3605cc8a1af8608acc30112488c396c806a5").ToPositiveBigInteger(),
                // G
                new BitString("15a498872f9758485c0253357b957e67ffbf320d955fec5dcf8d17acf715bb2de1de7872efb4c5902fc71c5cfae4ae121cd635e5ec07529a691813e59040720fbeee9262ed2636996cc0cb3ee4d5de16006903ce9dc883a0d3d13da66ef2768ff5b8008e4dc2b188de69bd55d1187bdba5e91c2c4fe25e0b00e393b4ca590ed38e4557b78a7aa393802db7a6046eb8d6927689c3d7f9391e9803f71dc015fb78c057e4c8b52217591ef4a7919a6985e049ca5f58a2ac303f1b0a1f52cef2ab140cca08cdb9707370ab8bbcbaef91c2a250c4630e351fb63335660dcce383acb265ac615322490e2e34311ac6a2cc2cd62908828a1913943fe19f12d3a3b624e9").ToPositiveBigInteger(),
                // Expected Z
                new BitString("3f4863bf95277b51c6112191e9e9a042610b533af346824cfef15a18ec9906ab193eac3c51be6ef23f050004215bd275b824e9b66e115a2610b2645bf0345f06e441b6420687358fe4e69a32942bc399efa4a3e757e725a209bac0fa46adcdb26cb466128ff8b79c8f437595a73889020ac02f606c775570a5d048734d7b85fafa8df8ed5c6d34fec3acf1288a4efdb1811d26548a360f62151ddbc84f6f424adf87188e26a97f23ec8a0817a0166359ad1607f4933a7f0de22c1401fa755ba384faeacf7a3cc59a69ebd3293a4df6548b80efc661334ec3eb10dcb66a9b871d2284e983456cefe03e33eaffb6cc2a1ff67ca93274c18871d11eeacaa72a7671").ToPositiveBigInteger(),
            },
            new object[]
            {
                "from CAVS test4",
                // Private key party A
                new BitString("1c365b33dc63b716cd8d2158e64c8a22b2c53711c8a2fd83b1b940d60645e94e").ToPositiveBigInteger(),
                // Public key party B
                new BitString("4def03e88563f4992120c7940dff3b2c6da81c82a4b132c52916fd48811f0d6ce96fe7224e477df3a594566d0d227c561b201b36d28e19e21489961ffb773bd1849e737d2f35cd5f5ea0a24ee4577501a79c3daf4c7030a5248962e818288189addca99bbda6c210d7e695810048c48556c5f23c84892fc2a5f13d595a22a12ccf51359aeb8d3fb1ad5165fedb7ddff4281b9d257915bd90e1142933871ddb2bf8592a1c2cdef506980aac35ff8a0f0887a5384ab05084cd981fa954a60141b3ecc9fe264d69a26c120b7562af3bada0c969d2f28b83c4eb147769cd74bcfa4a02af098291974efd64a928b50ad7062a2555dae73ece0a8ef83027194aaf05c0").ToPositiveBigInteger(),
                // P
                new BitString("91d80f6b6958b04e87da7e3258db8dd60ba376cdfabdd635ce04a69e62bb34202f2a1f8fc4460a12cd2f265a2ab03cc0360597dfa6ce72b25b4545dd1769d47a94d82a079a0a7b8101cb628c9c6e25934f2d91caef70c036e7ca4f6567aa053ab5bbf0e6e3524ccc5204cf9884bf9a09145277106efaca89c76662c926a40337b4b2800a00e1db8592d259973e7078fae64636355846c4d3a020e451a6e36c119919d6f16d5033bf7b3159c34efc41ecde98b7431881f37ae9f26aabe19e26bf93476930939362c44465f1e7d5c46ad381da742703a685c8323a590f0e3ad462547139a880d1ea59aa712f8ea0cb3605cc8a1af8608acc30112488c396c806a5").ToPositiveBigInteger(),
                // G
                new BitString("15a498872f9758485c0253357b957e67ffbf320d955fec5dcf8d17acf715bb2de1de7872efb4c5902fc71c5cfae4ae121cd635e5ec07529a691813e59040720fbeee9262ed2636996cc0cb3ee4d5de16006903ce9dc883a0d3d13da66ef2768ff5b8008e4dc2b188de69bd55d1187bdba5e91c2c4fe25e0b00e393b4ca590ed38e4557b78a7aa393802db7a6046eb8d6927689c3d7f9391e9803f71dc015fb78c057e4c8b52217591ef4a7919a6985e049ca5f58a2ac303f1b0a1f52cef2ab140cca08cdb9707370ab8bbcbaef91c2a250c4630e351fb63335660dcce383acb265ac615322490e2e34311ac6a2cc2cd62908828a1913943fe19f12d3a3b624e9").ToPositiveBigInteger(),
                // Expected Z
                new BitString("3f4863bf95277b51c6112191e9e9a042610b533af346824cfef15a18ec9906ab193eac3c51be6ef23f050004215bd275b824e9b66e115a2610b2645bf0345f06e441b6420687358fe4e69a32942bc399efa4a3e757e725a209bac0fa46adcdb26cb466128ff8b79c8f437595a73889020ac02f606c775570a5d048734d7b85fafa8df8ed5c6d34fec3acf1288a4efdb1811d26548a360f62151ddbc84f6f424adf87188e26a97f23ec8a0817a0166359ad1607f4933a7f0de22c1401fa755ba384faeacf7a3cc59a69ebd3293a4df6548b80efc661334ec3eb10dcb66a9b871d2284e983456cefe03e33eaffb6cc2a1ff67ca93274c18871d11eeacaa72a7671").ToPositiveBigInteger(),
            },
            new object[]
            {
                "from CAVS test5",
                // Private key party A
                new BitString("181703752e9a855aae9dd4f9036c3ef4c0456171dc312425e3d84302fc85e6f5").ToPositiveBigInteger(),
                // Public key party B
                new BitString("0dd9162ce633ad311e74933df4a7af1311e73f321cb99722e83c63859e317702013c24ccb230053847851a4ec1afdb1d8775b1f53d0c63ceb7d1d128842946c841eac94119e54acd8aa478ab847a9d2f2a75a7e28559159b33841c3a83196fcbcbe0a962d5a1bc98c8bd8ebd55bacd1fe6860975b6fd0f5bb13378953ab1cede42d444b456c58aa6a7bf01dba8a4f06d0736550fc72c2e725b501271714b855150719f2f69a6cc5f0658b5d8507e665ad966868bc3126a5d808f9fcfd519aa0c5fe1f5e4bc6979f7db056d09315127738c4677f0b4a74df4300f601b0bba2023709c89830047efb864e459bc44e1eb584e8fc3e03047c695e72a2fb420afb598").ToPositiveBigInteger(),
                // P
                new BitString("b2bb1fe02fde308e7f8b66c8b31dd440772b53638b53b0ac8ec3d78823e629a51ba403c7fda4be296d18a6c19a29c90ba793408a14f5dc799dc6e3df6dfda113d0eb18c37142e494db18d0437537a2e876104661abe529c42f35a6fd6f32204fe987e852a5bf71802f5d287c47aba70df8189c7041f9f94b233b8fa0ec9e8cf9d872c9945fdc8ecda0460391e54df0d0ac9556f1946617ae9c0f42c427dc21da61c343a1770c779cbe77cd1ddd12ef0752beb509efce6c2be2bb90b64211fa754f52c20aecceb2b582194136eb959eecc1a8e5e0e51826952063f61a1e970c0e20d510731428f3c4f8dffde61739f1391d7d7c9f04e9f317e41b486e67c00129").ToPositiveBigInteger(),
                // G
                new BitString("0f67e0d9d1053bc9d6e78d63462da17665b11561d04558c883dce94274d1578f5c99f7a9bf27cb2f1b7dd5e21de69f2ea7dc6ed39b4586cef29f3c51c7c9a176e2baeaa285eb0cda84eb626e1761a4fb5ca1e6f6204a79828b9980ee448761ba156da5e9672c6aa92d06926945d11428788a0d6e1178197c4d9d9cbc201d6079866f0b87989ca7add39f2a92d8509d923060c14327a0deaae4653a59060d726c3b3b6094ce631581c2df09a40811900d493e6c4eb93167603b777aa352974b8bc4445d7c189bd366c7385306aa8c5e74a31acc529264597b1b4b6f89c93207b06ec0b9ef74556601b1c31755d3f7eb3d84e401beb7be70f63376fa63900bd434").ToPositiveBigInteger(),
                // Expected Z
                new BitString("86ed2ef3f65ec4da7dee1b348951fd4441cc7a14e1ebc7402e20c43b259ced5122615724b16f06bbd83d9c4aa8b22c13d2c6e8016ee851f788d0a5da6655f0b44881d10fcfa69fc4c4c89883fcdd30eb303637c6dc87d963b3f6e82153e7129f29080fe5b34754c26ff7f598c17115363022bcb783243c64a915761c3a3155f5f61e6b9de0361f6bc1cde53753ca0e0ae58f89f4c579f6a4e0ee19d38a9391e1d6659e0e17765ac5556e5511d7b17a584259113c433e06e32a55ac808e1538c51f0914aed6982f145151de2118ec675ba1c05369be4be00fbb685b1375b29d1b6dbfa994a6a28721f6937961882d1664e173394bb9e9862ffea3ef513ddf55df").ToPositiveBigInteger(),
            },
            new object[]
            {
                "from CAVS test6",
                // Private key party A
                new BitString("f9ccdd6bd44745ad48ce22800ed3fbf1bb63e28aa8df8c72640893119da0757d").ToPositiveBigInteger(),
                // Public key party B
                new BitString("67458f01dd6f5d202f20afcae15faee679abf6d988fcfb65afcbb32f8d992472593110d158097ff1ad01a6ba19594c7b3549d308b0adc6cb4adcf6eb6e4fd0675c5f3bba4ee3734eb1d8d15b4beaf5c9b82f6ef84b7c3300c44004a5f00b61d581c3753fe5274f55099e8c79f1deeb13d78c5467bb9a69db91309c0e1f930db1c2e1e4241b6d3acb0a93f436a705cf0a1c21392ca5a68ba6488c2f8fd21e867322136f76e5729f2e141bd69823bf2d7ba54fb2cd7410c7cf2258266f4d30062addaf9c97e66e74a8220150fdf8a3ed77b875fe8356aa5886079a50e6728ec4ffc2b087869360c390055d0561038ef7e4782e52ce2c6ddabe0c386469952299b2").ToPositiveBigInteger(),
                // P
                new BitString("b2bb1fe02fde308e7f8b66c8b31dd440772b53638b53b0ac8ec3d78823e629a51ba403c7fda4be296d18a6c19a29c90ba793408a14f5dc799dc6e3df6dfda113d0eb18c37142e494db18d0437537a2e876104661abe529c42f35a6fd6f32204fe987e852a5bf71802f5d287c47aba70df8189c7041f9f94b233b8fa0ec9e8cf9d872c9945fdc8ecda0460391e54df0d0ac9556f1946617ae9c0f42c427dc21da61c343a1770c779cbe77cd1ddd12ef0752beb509efce6c2be2bb90b64211fa754f52c20aecceb2b582194136eb959eecc1a8e5e0e51826952063f61a1e970c0e20d510731428f3c4f8dffde61739f1391d7d7c9f04e9f317e41b486e67c00129").ToPositiveBigInteger(),
                // G
                new BitString("0f67e0d9d1053bc9d6e78d63462da17665b11561d04558c883dce94274d1578f5c99f7a9bf27cb2f1b7dd5e21de69f2ea7dc6ed39b4586cef29f3c51c7c9a176e2baeaa285eb0cda84eb626e1761a4fb5ca1e6f6204a79828b9980ee448761ba156da5e9672c6aa92d06926945d11428788a0d6e1178197c4d9d9cbc201d6079866f0b87989ca7add39f2a92d8509d923060c14327a0deaae4653a59060d726c3b3b6094ce631581c2df09a40811900d493e6c4eb93167603b777aa352974b8bc4445d7c189bd366c7385306aa8c5e74a31acc529264597b1b4b6f89c93207b06ec0b9ef74556601b1c31755d3f7eb3d84e401beb7be70f63376fa63900bd434").ToPositiveBigInteger(),
                // Expected Z
                new BitString("86ed2ef3f65ec4da7dee1b348951fd4441cc7a14e1ebc7402e20c43b259ced5122615724b16f06bbd83d9c4aa8b22c13d2c6e8016ee851f788d0a5da6655f0b44881d10fcfa69fc4c4c89883fcdd30eb303637c6dc87d963b3f6e82153e7129f29080fe5b34754c26ff7f598c17115363022bcb783243c64a915761c3a3155f5f61e6b9de0361f6bc1cde53753ca0e0ae58f89f4c579f6a4e0ee19d38a9391e1d6659e0e17765ac5556e5511d7b17a584259113c433e06e32a55ac808e1538c51f0914aed6982f145151de2118ec675ba1c05369be4be00fbb685b1375b29d1b6dbfa994a6a28721f6937961882d1664e173394bb9e9862ffea3ef513ddf55df").ToPositiveBigInteger(),
            },
            new object[]
            {
                "from CAVS test7",
                // Private key party A
                new BitString("").ToPositiveBigInteger(),
                // Public key party B
                new BitString("").ToPositiveBigInteger(),
                // P
                new BitString("d2b4ca595c5e4ab4b20e102e2d68b41b46a52dbefe43b7f4ff3b0754bcfe4668e2ffe1c4404c6db337be35cb62f9fc0c2329220b0a4d14be8306dee81b6a1f890bf90a5a94fb3aeb6c72e394f6ffc1d2fdccfc405cb7990edd58086df5f871ea7976c1cc797eb89bf0f0a522c2e23df1d8a08586b40b7d5fb8b7bbfe12fc770793cacc76293a2d24433aca652854fe206e5447783eddf5343c194d32cb568bcdfb65c5677a14413d5e2d7e2842cfd8f1d7de28072365ace49938e2663b800103f6f29121b1be92d64b58ec4007676c90dec348210cd646f6aceea41a796f6207b3a7a3f61dc57af7586fcf0cc4ec8a604828920fade4544816f1c9e9fbbb1cab").ToPositiveBigInteger(),
                // G
                new BitString("be83516ee71a46a9bd63ef0e32d671bf53cc750148c03c668c1ba868186bb2e655a088f51f4f0c166c54e8b8a4554ff6bea236e7362d67dde316cffd5c7277634f561b7b115858bc405f757f265af1277dfa20823fceb7a55f7da7bff5225956f902e74703f411978e379696847e7f638e88161c09683c4d93560667c2f6a0db7d7430f27d0a1d8978cb2609b13e53763a03b7513c3b28247e3e6100c4d954953004e9a3825d38808128bf33900424a798ecb9bf38041a8458a4d9d9ab1f95a45828b2e852fe8c0e4b2023069ce4614817c36db2b8c32de2f6ffb724da38c13e64720db5ce5bb971d67ae6f1238b7b12bc9d917eb33ce90ca5c035855009d952").ToPositiveBigInteger(),
                // Expected Z
                new BitString("862a8bfeb9ff45a3d8ca38a261eb30a2d846c21b24a7c7de6e4549f97896190c0fe450b5a46a4251c6696fbf5831f8800eb50b17fa1cdf9767a653429d16fe13616c0215eea45e82bdec1ec90636416f8cbe348dfefa397cd6116a04ad6398a63b48408b900defc9512536df2c05877d67747f9eaec16dd0cad6e0eb2fb41a240e6fc66fec337c1fdba2d58e24f59f44a7bbae1af27cf2d48e990aa62f6834d8f0438e476a8425b1db87a7811e518579348e22d616abc70ce0a0bf9a72deca24db4113f0dfcf971a2dfae9bc7ea3e7ff424390486cd1f3b83ad7c299527c3c658eae07eb7ee6e6d548e1a19ad985bf24369308ef9b27c90e83171a8f50080579").ToPositiveBigInteger(),
            },
            new object[]
            {
                "from CAVS test8",
                // Private key party A
                new BitString("c0835b5fff04a6bf4eeff1678359d51e7de013ed0b0eb21ed2328e27c1591ae6").ToPositiveBigInteger(),
                // Public key party B
                new BitString("c1f3701b3ca9a6e8f2d809db434ba79fe1a40c246ac536ac1ba8f02686a88916c2a91236d3b29d9f943305b5891c3242131bdaf9a1d9858d36f2a986bbeb477fc5e17dea0ab1f233b2d9ba2b231149dc1884bd190085cbd59ca8ae3affc083c94b22e474b7afb263c4ef616ff552e16ac297936272f5cf37774fec14a603ba739b57b35365a69c90076b25ac74059f40dcae62189b6c70af0362ef04d3f9116879757e64e9a32d006ca275c95e19b6cabac84fb39b4505ae700591d1c4d13f41b39c84a0baf3520821b4c413b9947a683f2a25e8de522bc6cdc098e04be29369389b398f1f4efb587bc605b94d891307c658899f1040a27b94fd56232d61474d").ToPositiveBigInteger(),
                // P
                new BitString("d2b4ca595c5e4ab4b20e102e2d68b41b46a52dbefe43b7f4ff3b0754bcfe4668e2ffe1c4404c6db337be35cb62f9fc0c2329220b0a4d14be8306dee81b6a1f890bf90a5a94fb3aeb6c72e394f6ffc1d2fdccfc405cb7990edd58086df5f871ea7976c1cc797eb89bf0f0a522c2e23df1d8a08586b40b7d5fb8b7bbfe12fc770793cacc76293a2d24433aca652854fe206e5447783eddf5343c194d32cb568bcdfb65c5677a14413d5e2d7e2842cfd8f1d7de28072365ace49938e2663b800103f6f29121b1be92d64b58ec4007676c90dec348210cd646f6aceea41a796f6207b3a7a3f61dc57af7586fcf0cc4ec8a604828920fade4544816f1c9e9fbbb1cab").ToPositiveBigInteger(),
                // G
                new BitString("be83516ee71a46a9bd63ef0e32d671bf53cc750148c03c668c1ba868186bb2e655a088f51f4f0c166c54e8b8a4554ff6bea236e7362d67dde316cffd5c7277634f561b7b115858bc405f757f265af1277dfa20823fceb7a55f7da7bff5225956f902e74703f411978e379696847e7f638e88161c09683c4d93560667c2f6a0db7d7430f27d0a1d8978cb2609b13e53763a03b7513c3b28247e3e6100c4d954953004e9a3825d38808128bf33900424a798ecb9bf38041a8458a4d9d9ab1f95a45828b2e852fe8c0e4b2023069ce4614817c36db2b8c32de2f6ffb724da38c13e64720db5ce5bb971d67ae6f1238b7b12bc9d917eb33ce90ca5c035855009d952").ToPositiveBigInteger(),
                // Expected Z
                new BitString("862a8bfeb9ff45a3d8ca38a261eb30a2d846c21b24a7c7de6e4549f97896190c0fe450b5a46a4251c6696fbf5831f8800eb50b17fa1cdf9767a653429d16fe13616c0215eea45e82bdec1ec90636416f8cbe348dfefa397cd6116a04ad6398a63b48408b900defc9512536df2c05877d67747f9eaec16dd0cad6e0eb2fb41a240e6fc66fec337c1fdba2d58e24f59f44a7bbae1af27cf2d48e990aa62f6834d8f0438e476a8425b1db87a7811e518579348e22d616abc70ce0a0bf9a72deca24db4113f0dfcf971a2dfae9bc7ea3e7ff424390486cd1f3b83ad7c299527c3c658eae07eb7ee6e6d548e1a19ad985bf24369308ef9b27c90e83171a8f50080579").ToPositiveBigInteger(),
            }
,
            new object[]
            {
                "from CAVS test9 - leading 0 nibble Z",
                // Private key party A
                new BitString("4443c52aed4a0e291501b135cb264bb081e8b9a6f716936323b8ae59cac8870a").ToPositiveBigInteger(),
                // Public key party B
                new BitString("af6be06cd92d21fd1e215a294a14d92109f780c475893b53aa02f2a0a92d9f1be0ee826043cd74fddb7852bbc4da807291c8f86031e14f525a0e18097d8ccc2014be61a1a136788148e6d6ba0ca95f6b693e64c26d6bedcd6a499167573d05233d413ed02e20035b86d310c965ae226aae0105962dbc5cca61146f83dadf61f38274370b1e102044454efbea1965a237fe8f953ce39e4d98d08e625cbab578ca0c10747d98bf3af206f21a236ad6f4d97fa04edc3f0bc41516b826877cdb2c1e3291d58d3b30c4bcfc32da53e8d73df21c419a52b321abc664a1b1a59112de9d76ac60ef27d0cba861918f10f5dd6b9bb216d77737ffde6449d8c5a8f9b1c2ba").ToPositiveBigInteger(),
                // P
                new BitString("d5cf9ba288ff8438650904a5fe2eeb8bf6b52b691a455b21bd3b37998f82544036ae61fe436039c66feab83ba21a5ba13e7b4c1692ac82c65309eed54e593efe9831cc82bfd11e9552d4eb7d8f2c233024bc10819c57093890ec19aa9ee915e524d81c285928a5b87acc7f496f93689ed59b15183689ec5e487fdc9fb994c4bf7e1dba57f8f12c17e404f68b5506d69f4aa98146f6ba3be1f3397e5d41c4235a18b38c75417bb9091a487db1fc89eb2f73b4ac6f6c12dc5634a342cd9d10605090bd544e1c0813aaaa3814166df84a302b8df38babe9c31e4a2c64317046bdb841fa8717e22dcf1496f9ad94c17f1ff9175567d02ab5e30601e2e5b0884e9a77").ToPositiveBigInteger(),
                // G
                new BitString("37a7b2a094ed6253784d51c71a4a7407f0727df15e480a02b0ff2f9befdb0e92c0d63e482c6909e5337373abe347ceaec25a9f2a23f1770c2447bfb5c35dbda80d3f00d8b6569a0165a3d52f1cff03e02b67be47b2f26ebd0fc13299fd0317719c3bcbbcbba0a982915b5d68fb4c5c483c3df12052b56ceacd16176c783d56422b1366cd5ce65922b734a1780acf35b4a658cfe8166469ace04b87a33e1e8d4603ae3f9607d1e708137d581aaf2cd86608ca06e52cddbe475f8bdb4597cd5b55f47380276c86d08d8ddbbef4ee3ca76cefe6ff9c74d4d5d1be3407e30720be34cf41f482be8bc6ed6e89ea7cc2d164da1dc1c9d0e2762eef983dc9e96f3bfd50").ToPositiveBigInteger(),
                // Expected Z
                new BitString("0ee825ae93b7960b62f7096bee6544fdb4e4c238902a025f07d5acbb4699fdb9d5c783d31187ad1b1db48996fa3cda6e890602dba94183697abef29d61b8ab005fb29c8bebd4001b771d12b3e5977dfe096ab6c23f20352f2c32c4e34beb17e6acbe5fb0ecb8bce39984910eb859ff522c10628fc0580ef892a455b60445cf17bcbb2ed100dd4879ae1c46ea7a78eb085dadfef0c8b02450e322dbc150c9ffd87a862bfb47652bf27946f7ae28270f3bb67658d41d5592fb66587aef8076feb1a93652d3140c1a64fb2d37daac46d7babd5c2ec261f63bd8ad10e40ac156739d5ea052fe18f2e650b61f256af51c691ff4b7958672fb11bb5e114b8f62e8d3c1").ToPositiveBigInteger(),
            }
        };

        [Test]
        [TestCaseSource(nameof(SuccessSharedSecretZTests))]
        public void ShouldCalculateCorrectSharedSecret(
            string label,
            BigInteger xPrivateKeyPartyA, 
            BigInteger yPublicKeyPartyB, 
            BigInteger p, 
            BigInteger g,
            BigInteger expectedSharedZ
        )
        {
            FfcDomainParameters dp = new FfcDomainParameters(p, 0, g);

            var result = _subject.GenerateSharedSecretZ(dp, xPrivateKeyPartyA, yPublicKeyPartyB);

            Assume.That(result.Success, $"{nameof(result)} should have been successful");
            Assert.AreEqual(expectedSharedZ, result.SharedSecretZ.ToBigInteger(), nameof(expectedSharedZ));
        }
    }
}