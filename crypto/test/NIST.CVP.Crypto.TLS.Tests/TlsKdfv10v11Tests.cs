using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.MD5;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.TLS.Tests
{
    [TestFixture, FastCryptoTest]
    public class TlsKdfv10v11Tests
    {
        [Test]
        [TestCase(832, "85b95dab045bc3061065744a2d0894eab1c0237f3430798560fbd7a5ed507783610ac72bc4f757cabca7562521da6e14",
            "14035c36b23bb0757e8973bbd947c26eca1e8de7f549e34b7819a0c450c332b3",
            "1d146e82718307381e576f9df2b6fbcd26a2cdbb07a9a9a206e77bc27fa163ab",
            "d04bd9b4c7eefc8399977f5e3497fc82af5de8bb4e741dd5f9e83dc512f68d62",
            "36b8371e9b411fe0e835632817c7af03e8db74e5a548e2999c8494c7af6ab1c2",
            "d587a843e09ac02f867c24b13fbda1131081da791791801633366f735a6c68a26f24530a5aa51c1adaaba436caab4208",
            "8db335a4e881d7ba3171863c3c43e30227baf82bcd032021ac98e0535bad1a752d8d34bc0d5016ac860446cce92e8d322a3c0e9d7f3ba7f9014325cfc1b518df9feb25361808a2d151c3749cb7b4cb2827306d6bb8d458d6b45791ad0ccc8f102a8602f110022b7b",
            TestName = "TLS 1.0/1.1 Test #1")]
        [TestCase(832, "d671cdfae1e2dda3a01e80d76d897517c37a60d62e89312060c422dca1922fd8ec0f70d8bcf12f413e9e086481561d43",
            "dbd90af84002dd924acf80f270c04a5a1c900f43689a6572f47c0808351ad755",
            "3572440795426af5927b811a452a06388464b231858f5631f406048b106a6896",
            "cd12ae6a6496f4932a93431f0d6ffcbfaf20132aebf07ad429cc6862c378c826",
            "57b58338bac41e526c8b9d5eaab9da368637eee18d0469b69889c231dd772a2d",
            "c5753fafd0f3344cde9eabec621189f444b15dd46cbc744b551f51195437fb32200cb3ac08145dc97b09ac4943cfc74e",
            "6b8ae7bf624b5f3ea88fe7e56784d437c49404419223060f5ced0bc8e03acf656e5fe59adc800830700e9a80bb733bd3d4a2284d5f20e69c72babff47a44e7191b650cb3e3df05184675c4053e2cd1d0e0d45f87cebeda1a7acd059e99d9fcf58daa9d79f87e0e61",
            TestName = "TLS 1.0/1.1 Test #2")]
        [TestCase(832, "bded7fa5c1699c010be23dd06ada3a48349f21e5f86263d512c0c5cc379f0e780ec55d9844b2f1db02a96453513568d0",
            "135e4d557fdf3aa6406d82975d5c606a9734c9334b42136e96990fbd5358cdb2",
            "e5acaf549cd25c22d964c0d930fa4b5261d2507fad84c33715b7b9a864020693",
            "67267e650eb32444119d222a368c191af3082888dc35afe8368e638c828874be",
            "d58a7b1cd4fedaa232159df652ce188f9d997e061b9bf48e83b62990440931f6",
            "2f6962dfbc744c4b2138bb6b3d33054c5ecc14f24851d9896395a44ab3964efc2090c5bf51a0891209f46c1e1e998f62",
            "3088825988e77fce68d19f756e18e43eb7fe672433504feaf99b3c503d9091b164f166db301d70c9fc0870b4a94563907bee1a61fb786cb717576890bcc51cb9ead97e01d0a2fea99c953377b195205ff07b369589178796edc963fd80fdbe518a2fc1c35c18ae8d",
            TestName = "TLS 1.0/1.1 Test #3")]
        public void ShouldTlsKdfCorrectly(int kbLen, string pmsHex, string shrHex, string chrHex, string srHex, string crHex, string msHex, string kbHex)
        {
            var preMasterSecret = new BitString(pmsHex);
            var serverHelloRandom = new BitString(shrHex);
            var clientHelloRandom = new BitString(chrHex);
            var serverRandom = new BitString(srHex);
            var clientRandom = new BitString(crHex);

            var expectedMasterSecret = new BitString(msHex);
            var expectedKeyBlock = new BitString(kbHex);

            var sha1 = new ShaFactory().GetShaInstance(new HashFunction(ModeValues.SHA1, DigestSizes.d160));
            var subject = new TlsKdfv10v11(new Hmac(sha1), new HmacMd5(new Md5()));

            var result = subject.DeriveKey(preMasterSecret, clientHelloRandom, serverHelloRandom, clientRandom, serverRandom, kbLen);
            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(expectedMasterSecret, result.MasterSecret, "master secret");
            Assert.AreEqual(expectedKeyBlock, result.DerivedKey, "key block");
        }
    }
}
