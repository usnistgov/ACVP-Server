using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_SigVer.Tests
{
    [TestFixture, UnitTest]
    public class PKCS_PSS_Tests
    {
        [Test]
        [Ignore("Known error, testing PKCS vs PSS from CAVS. As of 8/16 this is fixed in CAVS")]
        public void PSSTest()
        {
            // From old CAVS
            //var pHex = "d40ad5636fa9150021cffd0fe8aae08bbec4ce6ec66f38cb45497712c2d61593f4a32fde42ee04e98feecfcf3579aaa02cf2364957ff6a56f6fb85cbc0bc40d996602243b0d87cd391aaadd53478456bb6ce4b97ef64ceba08a736a5d0145a0ee158b2bdbdf310232ce785197f930989a110da422e85c0de82d9fef456085097";
            //var qHex = "c90362c66a58b9a0eae11fb1a93be894814a6b143f3dcbf23a23ae37298ac9ef3c703ab311be9192ef6cf7099541475ce5deef64ca6477bf5247a19dca0c1acfbc3977fb42cea8e23e8ee1a16f61d3522ca36eeaaa725d6a77a54e2e6d08bc26d3693ca3a1bdf1481ccaf360a14cd9edc986774ca1af3399e2c7c79713b3391f";
            //var eHex = "89e191";
            //var msgHex = "6155c152159623fed936c1edf46ec03d5b6f5dc8d4f530545b20b20ac580fe2aa36ed4fc69a3e30ad003488c78ba5224d28a88cb3d63ed0b6f35827017020d39c0371952f93c3e87b34aa2f2ea7dc0b5882a68ef867207e85417298f7b8f3c278157973d0c8abbdd4bb2d80592de935a44f5061af6490852d16faf51384f145c";
            //var sigHex = "2b4549a4842d3e6e8642f995b4e446fd4effd19d7974fe3d1d41ca6c2e28056e58fd5e79d7a2150f0850f08bee48280d23ad8f1dbad36e34548f6d652e8d4a890b6680a1256822cf7f94cc23e7471fe308094a5ebdd97ec49137aa61353e7fcd852424b4ca90f869d544073d8e41987dd1a2495711ff01f51c4c4d4c3eede4733936eea6afa2c888f7237a3dd9e6ea004d3d4161805b5d91c574efccddf76feb2349a57f2a5e74a89c333def73b71e01158387dcd6fd02922eb378c041eb9cb838b584c1eeefe3a525cce52b150064196f94117b70f1679ea2e581be5dde99e690d1c480428e328b1cfffbd8d5b998aeecb7f3c2eb31a25a04957d200b20c050";
            //var saltHex = "abcd";

            // From updated CAVS (with the fix)
            var pHex = "b514ddeaabf5674eca7ffd946be506c1726b1adaedacd6c5872fe34f11f2a617be16bc9f3fb3002b3989177a62fb308e07fabb2de01598e34b563a4adee5aee7";
            var qHex = "cd7c4b4e9144b439c99c01962bf575c952e48bc767b3183e41a27d5da4910a0735df106ec0b284a6962dbdb5cde34b13e9f3eb03ccccb251bba2a1241bfd23fd";
            var eHex = "b413fb";
            //var msgHex = "4f0749b588bb4d87b68148edc0f57603dbc9d2aafc672b4093d87aae76e573a81b7c5a300b55185c93d48e5abd3229b17fd8b7c9310b79ae44ce791132041d90b1d6fd372c54f246735f6f34b2c615f5a0af8179f83969752d5fe7655a2014d277dcd9d0d30202ccbee8948486fa6157a617d9bf3938231cb5536db3d57f80f6";
            //var sigHex = "5d4788003aaa5d03d5297981a87491d763c0edd545875c2131f8c62e164ed5a93c75669dfe31ff6d21ba19d0f6c97ba405dadaec4c6f3210789e77c3a03881a3c65e2d608717c4785a6dc00e047728e12482d7c9d072b99343d263f31d28f2cd527d692b6eb7c5e8758ef79aa6a5d0bc033d0da05c69cd4eb88ec1a06a7240271e9c3483d1a4e03fc3a6f179abe767d15186bde7d6af7a8d8f2c1f21b30a8b8e5c337671691c60112aa9def426e17854635a0103e570dec71a062178de29e4639f89257c6a32376d617ea970785bfc2a074a8127062454ce8a5fbb3cdfb8cd240393232b321c6d50a1340b5275b82c09895e9cbde75768cb878df457c55c203f";
            var msgHex = "63af203d44066da08353a7d482c2570d38bd50a5270f40618e4986f6df6443cc73a9b4017dcebfb13d62cb75766bd529214dc0f363eded80bd170c91831d32b78fc12b5a365511007b211dcc8305be454960e85a9330afa2257a14e834ec1fe61dc12e5157e58c16a65945e492da39ea18bd70e29f1ead4e2e394601db1832e2";
            var sigHex = "04aca5e5b0e72ce30df1ac4480120bead7630ff8c9eb3b0e9b4186ce82705d3d3e0aeaa5653e2b493449d0cdcaad6b987d733982ad20bc0985f643805b8be41b4b7addd7bd3e549bb4e8f701d76a63af23c275e547010b93f2cfe717e2dfa75db7e609d1691c6853825a094296be620dfc76331bbdb53c1ef6542d8e5ded1373";
            var saltHex = "f360c1d564071c0e190602f4e0844d060b1d76ed";

            var p = new BitString(pHex).ToPositiveBigInteger();
            var q = new BitString(qHex).ToPositiveBigInteger();
            var e = new BitString(eHex).ToPositiveBigInteger();
            var key = new KeyPair(p, q, e);
            var msg = new BitString(msgHex);
            var sig = new BitString(sigHex);
            var salt = new BitString(saltHex);

            var pssSigner = new RSASSA_PSS_Signer(new HashFunction { Mode = ModeValues.SHA1, DigestSize = DigestSizes.d160 }, EntropyProviderTypes.Testable, salt.BitLength / 8);
            var result = pssSigner.Verify(1024, sig, key, msg);

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.Fail("Good");

            var pkcsSigner = new RSASSA_PKCSv15_Signer(new HashFunction { Mode = ModeValues.SHA1, DigestSize = DigestSizes.d160 });
            var result2 = pkcsSigner.Verify(2048, sig, key, msg);

            Assert.IsTrue(result2.Success, result2.ErrorMessage);

            Assert.Fail("Good");
        }

        [Test]
        [Ignore("")]
        public void ANSX()
        {
            var nHex = "db42b6b2b0c8fd7948d7fa5c4d46d35e30ee0b9a37ddaba1a5221abad2c1b957e85e9592cea0c8405b07e1ba2b7d8f66a3f265c6bcc0fab629ab3842539a45c4394d09ae8ca14681344fcb61d7f9465a1b7b1546659188033c2e8d4af0287a021fd54d44d02f803cb0280d25a7adf50da0b0834d10c2855e74cb09b52264ef5f457cfe2ded5c9f387e9c05b1d9c8466576604dd43a328eaaad6fda32b49c34f589a4e9675c1a93c22ba5ac3a80678284741a61850f8757b7452f3bae386149d895d5392994940d67b28fe080e9e1ca9a8ba6703ef21b9044fe239ad438cbdb8bf622f220d05839f4f806519cb86baa4073bca6815af7de67ea1957e0a711def1";
            var dHex = "0dc038f1e6baeb8022a3cd2719f553eeff581b848a3aa9e673f3bf01004af288df013815c5516b35d5c960115dc84e69359bcfc315a7287df9b1b918c5c12dd768adb9e76972af788f312a536efa74f467d800b3be1bb1960dad7b0ee76b2208e22d8e3b63d15c5239d4fcc51bc1849f2bd6521d7db203d7906e9e26489f45752b579c9b7a9cacc7503c96d2af7cf2f3e0c519f4d0df40142ada5fcd109811c02d09086fc22eeba0b7a416c89c9f0aab8a7e2ac5d0be10a8396d9bd4795c2c050a0f305e020f554fb09b0691ee377a6e79dd0c237d2bc10b465098a58928a72f6e098949295581659d9ba794a26456e4cf2063a8789e197b60c5d843ca8841ed";
            var eHex = "0f5f13";
            var msgHex = "6414cf69a77a510c89b62ff580fb2701c723f408a7e9b9b3f458a38dda83ca5dda7759b40f6eefc2b645456ded3010a5bbad29f151b6eb4e8f3bdc3acacf900ccda2cb861dafe6ad823674d8c26d2508b073ea7765855974ce74546d6756e54e457c6d0a5aa5fe76f571055e88192cd3ae5c7ec26cbdcccbb7eb504f378191cb";
            var sigHex = "0fac537d91442ba97fd2c3cfa55568fc731d9f5eb9b55b9bce0d65a9c431cc40b4cd4c699362f092f56f0e56a7e3d3825bd26531e284e976faf3e8264875f2e99f7037d37491e8b256743fcefc771341bcedc71fad3b1720b50c92a23e3781e8013209e3b0b76b89c9480684b55f9ff0814e8fa85b766efc800ce5078147deb2f7653be317a99f0052cb47b48728a9115b07b5e1a4818841622f623dc4065531b283f33308089fb0e48cafcf1b460793beeaa7e22baf028cc17f03b2edf06b2c69cfe495f326f16d4b4e2f3ea1050293914970ed4aba2cb1d03e1d637b55da06c44dc40f58b3cc5630ffb0a4cafb590594352afd4d6eb241c392d12b421d57a9";

            var n = new BitString(nHex).ToPositiveBigInteger();
            var d = new BitString(dHex).ToPositiveBigInteger();
            var e = new BitString(eHex).ToPositiveBigInteger();
            var key = new KeyPair { PubKey = new PublicKey { N = n, E = e }, PrivKey = new PrivateKey { D = d } };
            var msg = new BitString(msgHex);
            var sig = new BitString(sigHex);

            var ansSigner = new ANS_X931_Signer(new HashFunction { Mode = ModeValues.SHA1, DigestSize = DigestSizes.d160 });
            var result = ansSigner.Verify(2048, sig, key, msg);

            var result2 = ansSigner.Sign(2048, msg, key);
            Assert.AreEqual(sig.ToHex(), result2.Signature.ToHex());


            Assert.IsTrue(result.Success, result.ErrorMessage);
        }

        [Test]
        [Ignore("")]
        public void ShortSignatureCheck()
        {
            var nHex = "F12221C74D8BBD1B2402EFF7DA96411A40AFD735B513DB7EC361DE6EDD995249AF62F94B17DA8B26B304209E98C3212CF01F8A3D28863732AF966A1810F95EB9DCAD9E3A68BF134BC7C17F49959A5045DCA0A0AD24C3FF3744F7CB59C1C1A4636BD7B10219F997EEAB5690F47E7E6FEEDA3765B71B79FE3B4045F05C05A860780ED9A4672817D3486522081DA45240891A262365EE64BFA99A7D77BF116EA5FC47D35CF72B25104D3FE7373C829830CBA9AD633133943C5C16CFF53296F3457EEAB4B5D1C53A70FD8F3A2924239FC83ABEAF9903C4C6F10F99BB0C4F496B29D5B546C14DC68111087B937D6F33545015D025E9D4BB14FF5736BB1340F46E80B3";
            var eHex = "018769C7357B";
            var msgHex = "A7AC517501DB1A5F10A8261B580E840AC8C1B8A6D6557FDAE533B3D8329DEF85831FB2A6A5905A71DF7CE2E618417AFD1F7B1F2043151E5952F7065C4CFADBF38E3F5DD30BA486E8E1FCBBF4AED2D1EC9000D20E907DC5E1F2BE4B2B793C6D9B7191B8EA2D0AFEF289AF9D08F611189D237F98BA954C8446298BDD0A970F987F";

            // This hex was given previously without the '00' in front causing an error because the signature was too short
            // This has been fixed in all signature algorithms
            var sigHex = "00AF6DB937FE8858B6649C1544CA791B3899B2F112E1460C7157E43FD0D2314DAA2864A902E823984C1EB42E9C491BC2C29BF22FC014ABD4C20D9F6B555C0FC7FCCEBA60BE37DBA69D6391788F91D7DB4BDB74F45F8E4FFE0B8E241B6816A557D8BC2E337985AA440827612C6B8BE3DFD1E61FF6711A786A45F9C88AFF784777E632004B32953E07F3FB57D8EFE7354B0A15EAA9715FBFECBB62106766C2F6137F3E78B8C6B148CC1CC401847588EFF35147D996A511CDB9F5CF84E8F6DF9BA5D379033DA49F38B5A18F71CD46614B866F30AC464AEF3CD92296A12A1AAF6053DF10313244A7DE548D1C57313F6651347B1B42B559BFA1444413580B997629F0";
            var saltHex = "3E5C464A47488231BD91FF";

            var n = new BitString(nHex).ToPositiveBigInteger();
            var e = new BitString(eHex).ToPositiveBigInteger();
            var key = new KeyPair { PubKey = new PublicKey { N = n, E = e } };
            var msg = new BitString(msgHex);
            var sig = new BitString(sigHex);
            var salt = new BitString(saltHex);

            var pssSigner = new RSASSA_PSS_Signer(new HashFunction { Mode = ModeValues.SHA2, DigestSize = DigestSizes.d384 }, EntropyProviderTypes.Testable, 11);
            pssSigner.AddEntropy(salt);
            var result = pssSigner.Verify(2048, sig, key, msg);

            Assert.IsTrue(result.Success, result.ErrorMessage);
        }
    }
}
