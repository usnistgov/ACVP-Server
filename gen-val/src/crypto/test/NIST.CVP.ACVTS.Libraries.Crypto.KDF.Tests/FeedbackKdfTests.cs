using NIST.CVP.ACVTS.Libraries.Crypto.CMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KDF.Tests
{
    [TestFixture, FastCryptoTest]
    public class FeedbackKdfTests
    {
        private KdfFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new KdfFactory(new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory()), new HmacFactory(new NativeShaFactory()));
        }

        [Test]
        [TestCase(MacModes.CMAC_AES128, CounterLocations.BeforeIterator, 8, 512,
            "6874c099a14942d5bcd823183a4ceb9c",
            "0909d62821ec989fe16d6d77358126d272fff3e2dc4795c5a9421bee65be679b9f651668fdbc2c13d2ef4932f8830b56e5e1e0",
            "4ab31c84730527fbf008e446501bb26a",
            "265062a5de896edbfc0d071bdfb6dfd18901f3786cee3c401e53c198e80e78bab17c7049c723d4cd9d334952509c44d7e7bc16627a1e7177b80157a3c56ac21b",
            TestName = "Feedback Kdf - CMAC_AES128 - Before Fixed Data - 8bit Counter")]
        [TestCase(MacModes.CMAC_AES128, CounterLocations.BeforeIterator, 16, 512,
            "ca26c14fb4c6544e006d4d2f0080fcbb",
            "05ccc1d72d7177aa8a5d31f7f5dbd01a2bb34eed4ab56ec7b3567ec39f511f2121d41c8ae367f3b110b9688097419e727fbb5e",
            "fb73ad9683e6c637a1e3369e4a403d68",
            "ca6768c1adebbe004976b83d555a589868b288c0462ce6bb7abf98e9ecc59cd4018867f9c98ab9379d6a1873ff34a7f982467e947545ff0892dcd8cdc8014a04",
            TestName = "Feedback Kdf - CMAC_AES128 - Before Fixed Data - 16bit Counter")]

        [TestCase(MacModes.CMAC_AES256, CounterLocations.MiddleFixedData, 8, 512,
            "65d6ebb79a410c22ccb45c102d18550128d5b223beb2536adfcc1a4d2d76422e",
            "525e85e3615422480d6e07c685ee67b0fe733db83eafe134c8236fb740703209f01d78be1355418e5ae91bd67eedb155427ce8",
            "",
            "9cceccd7e7c11d11054eea092967d95f8705fb6416a6fc3edd05886f88f742513b0d49a1c50f1c1ba342879ddbe002f9f4b2fb0a7388b634a0301b8ba143e9c6",
            TestName = "Feedback Kdf - CMAC_AES256 - Middle Fixed Data - 32bit Counter - No IV")]
        [TestCase(MacModes.CMAC_TDES, CounterLocations.BeforeIterator, 24, 2048,
            "88cc0591e7727235d5fe0348ae7ba6d2b032ff6185c299d8",
            "4a30f359d11f0b368c4e1b7d92fad79556a3a06ad9ebf88e237090f568115d69210f0ebb6253447797fc728c221636bfaa43ce",
            "",
            "0b6110f40e024406b6f56b169b51c0e91213ab406f9c17c283b2ae5429664eee145449a52803d091db181000db93df30bb4a1f8863c9aa21c87f7435b9c2e09512f9093263811413685a9379c94adecede1c196df932531430fc82ae32d586299fca620480630a5b2d6c6004acb88423de3b479123aa345cb88f7adb81ffeceda956106766451784555616ea8d6d58919fbed046e88378865d386eb1b9afd4bab2384b1445574e2f1a4750b30a4bd5722e6eea837a29237b026af3cb7c5c7ad1044ddd7b20c1f60f8570bb792b9e029222418cf1a26dbed3ea6874fb825828c0625fdb11966b9bd7c8ce9b6b651ce263b07f49059cf4132efc7e082c0b9c0f38",
            TestName = "Feedback Kdf - CMAC_TDES - Before Fixed Data - 24bit Counter - No IV")]

        [TestCase(MacModes.HMAC_SHA1, CounterLocations.None, 0, 512,
            "fdf07e2dbb4d4b191a4b3b890cd89447025d97b9",
            "9ec57b91f8532aa5845960ebee87e70fe5c8c5f030d3f7f6c88bbb0eeea6a4cfca7b13509d2e62506cf00e42a8108e9a71657b",
            "ea4cfd927b6e30520e987c70bc46607481f02ce2",
            "b8c1a0aa344962d1525a182120dcbcf601f436b4ad8ffe056cd4e3f85e63a35bcf147f2a48aafb431b72db339c2b28b4157634c2d7e80a93a15b340f7b1507b0",
            TestName = "Feedback Kdf - HMAC_SHA1 - No Counter")]
        [TestCase(MacModes.HMAC_SHA256, CounterLocations.None, 0, 2064,
            "e237081bf324bda438fae834a8fc3eeb41af181b3354a3d81a3f9fe8da7b7ac4",
            "52ba4dd1f11f005cc25187a6e1bd55b876962186ab5ff3813d6632718aee7244a71b218a1428d2387b1479d2765793c453001a",
            "276fa8bf276b1e6abc3ec1bcc1fd5ad85c5fdea93f3fab9c63268397a4aa59dc",
            "27691bf1fbd8d357fefcd744f54cadf520d75e176d933251509f91d17d513954d925e3e67f061d8ce227c0b18d8f87709360db7a5753b8547c3312b58fd07ff02768e4bdd38c13b2186bfa1594a9b11cb66c7bc469d7fabbb81fbce21797f55f40819d5c905cedcb8492bb7e315ffb2e6a62108df3dde45673db23d709c90397366f9f0867970194db388c509212dc10c28379f48f46b713b5f94f782ad1d129de2703da3d25d2309ae5e2e2d9adc29ecb2ff2f3233850f8ef88577bfad4d8c07666e55081d3ab5911e3c47336cbba34e7c9e14ba8dbd8de21f570a22a36aa462cb0ebbb94322341710c2a7a932ef7a5a96cdb768463f817e7d4930d3bc9a52f28f6",
            TestName = "Feedback Kdf - HMAC_SHA256 - No Counter")]

        [TestCase(MacModes.HMAC_SHA512, CounterLocations.AfterFixedData, 32, 2400,
            "5bad564345741ec8cac911b9527dc02b3a16d25d439f890983a97448bd71c63470baec0204dca3752765aa671569331c49e8f5708891410a8874661eef06adbe",
            "499ced4f4ebabbbb80a7d86a19164fd6e1043cea1e00650b76c001273c6f2079d2f2df3e68b38880437ee6de6635018dfaeb0d",
            "36fc832a42a9cef443a226e102858eed00faef1aea3c1ed31f8607b76620a10e364840a0f4b0bf5e8772908397f7adf48583a948c24839dc9f09471005e25553",
            "75aec3278b8574c96b99dffd7cf1698dcb0a570f7fc26060c6e94d2efa7368457c1d762a55374a25baf33054910038370e1acf1a51b7da3f90632af12d693ce91aca44aa9e63293f44280f997eea39ba80cd5215edfbc01f5a505dc75180b48079a2d8db6b0d53f9cdf83229a26e810aa93fa57b5eeaaea321a8e55266f02280ae3f713a848a855df1f9a813da98d78fd71876446246e37685ff6111158a592e3e2aeaea599aa21306ad301ca9a381b908134a6ec18e85408a757e98a7990599e371e61446d5a3d45119139503a41c34d6c372a9144d833364d9ec3ab4279da180249de2641e346019b99361e7fb26347d6d3d45b3f5830e34dad822b8eea256462ab45e341153fc98c354aa7067e3826ecb8c21cdfcec2a4fc85a5679c526c65029201442791a6118e58ba5",
            TestName = "Feedback Kdf - HMAC_SHA512 - After Fixed Data - 32bit Counter")]
        public void ShouldCorrectlyDeriveKey(MacModes mac, CounterLocations ctrLocation, int rLen, int outLen, string kI, string fixedInput, string ivHex, string kO)
        {
            var keyIn = new BitString(kI);
            var fixedInputData = new BitString(fixedInput);
            var iv = new BitString(ivHex);
            var expectedKey = new BitString(kO);

            var kdf = _factory.GetKdfInstance(KdfModes.Feedback, mac, ctrLocation, rLen);

            var result = kdf.DeriveKey(keyIn, fixedInputData, outLen, iv);

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(expectedKey, result.DerivedKey);
        }
    }
}
