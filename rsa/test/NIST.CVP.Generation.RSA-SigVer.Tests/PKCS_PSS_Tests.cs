using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

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
            var pHex = "e728bacdf67527b8d00ecaffba65dda8b16ade5269209ed89a7838934cb800dd93cae1b100aa73f08c47636095168d31cfd41d26e95df2b0f872d51a16adff145a6cf8aa0fc48369ced8f7e5a87136985f73eb2c88c1c13a1c94efacbc5583872fb11cd22695d69644ce899ee61bb1e2aad20fe82be4e33dcc8a843984b26b81";
            var qHex = "f74e707a474bc7555199462cff90b713ce5c76ae4877f074f574bcd80bfcdab57a2771226ae6531c29d79da14b140e7ac12acf4aa2b04a939b60e6338c055b8b94f996085be7d981048ab9b595e8cde52ce972e13805b463ca671c9f3e45d40492266791488519d30e2915f459f4eefc77f02e4dcec2e08ddb3856f839d7b8eb";
            var eHex = "3c3761";
            //var msgHex = "4f0749b588bb4d87b68148edc0f57603dbc9d2aafc672b4093d87aae76e573a81b7c5a300b55185c93d48e5abd3229b17fd8b7c9310b79ae44ce791132041d90b1d6fd372c54f246735f6f34b2c615f5a0af8179f83969752d5fe7655a2014d277dcd9d0d30202ccbee8948486fa6157a617d9bf3938231cb5536db3d57f80f6";
            //var sigHex = "5d4788003aaa5d03d5297981a87491d763c0edd545875c2131f8c62e164ed5a93c75669dfe31ff6d21ba19d0f6c97ba405dadaec4c6f3210789e77c3a03881a3c65e2d608717c4785a6dc00e047728e12482d7c9d072b99343d263f31d28f2cd527d692b6eb7c5e8758ef79aa6a5d0bc033d0da05c69cd4eb88ec1a06a7240271e9c3483d1a4e03fc3a6f179abe767d15186bde7d6af7a8d8f2c1f21b30a8b8e5c337671691c60112aa9def426e17854635a0103e570dec71a062178de29e4639f89257c6a32376d617ea970785bfc2a074a8127062454ce8a5fbb3cdfb8cd240393232b321c6d50a1340b5275b82c09895e9cbde75768cb878df457c55c203f";
            var msgHex = "fa14959a20407be680955be6e18f519463a2949e107ba8c4b488d582eebad7f0888ad7e6043a9a63775df9309b9991c01facbb5b41adbf7c9f9444774afd0b5e1d29dd264d46c20640f07420a3f71363b7631ab0ebb5923bb070863589d8b6a861fa8fa222ec5b958356b6fe22314145d2cf9ae4361c8bcc8f238104d2d44094";
            var sigHex = "502df83148dc7dc59a1f9ca0e9b8c31b4c399a146cb89cbcc67178b503f0926570644c4d3107c6cabe684083c95ffbc1cee4d37fc7e600325e14e5afcba34f80788e54cb047f27b02b6ce0792cbf4159e10cb3a37e7efea758a8c04833c3ee9e74ecd61335970ab5e45919f5f81b2a99af01baf8be0ac63cc863243cac2d484044e60c6a7fe32afd77fda1b9dfafad02ec4841a95e08acb486584946fc23d6c105b1032f464f1f1ec8cb9b46c536b1c7ab30262ecdcfced11a291e4c6dceef3add80f12eb5356ab9d4ca845a3aff8c35b49570d68d6196e369ab3f464914a7533935a266d5a557de019ba1e98453f286ec5236168509f13eb71fcbac258e9764";
            var saltHex = "abcd";

            var p = new BitString(pHex).ToPositiveBigInteger();
            var q = new BitString(qHex).ToPositiveBigInteger();
            var e = new BitString(eHex).ToPositiveBigInteger();
            var key = new KeyPair(p, q, e);
            var msg = new BitString(msgHex);
            var sig = new BitString(sigHex);
            var salt = new BitString(saltHex);

            var pssSigner = new RSASSA_PSS_Signer(new HashFunction { Mode = ModeValues.SHA1, DigestSize = DigestSizes.d160 }, EntropyProviderTypes.Testable, salt.BitLength / 8);
            var result = pssSigner.Verify(2048, sig, key, msg);

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
    }
}
