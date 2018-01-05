using NIST.CVP.Crypto.AES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES_CFB128.Tests
{
    [TestFixture, FastCryptoTest]
    public class MMTs
    {
        private Crypto.AES_CFB128.AES_CFB128 _subject = new Crypto.AES_CFB128.AES_CFB128(new RijndaelFactory(new RijndaelInternals()));

        #region Encrypt
        [Test]
        public void ShouldCorrectlyMMTEncrypt128()
        {
            BitString key = new BitString("6338d754c3f35ce008a8ed612e79f423");
            BitString iv = new BitString("353f818e1dbcab65d52115a0fa675faa");
            BitString plainText = new BitString("6c1533fde353c984ac1f3516b7f315a3cad26feb2e2e0088a90ad02fac5c0a33ccb50b4a7623b994df5716d93b0315132f86ac2c88cb2a082d6ecbc652218943ef4f36e071ffc886affaf4e71f30fe064a58cff97dd748edc2e3d66c0ee846dbbc2adb473aad9e610352f6c6cd427884038f00da751901404faf19a74777e9021b9023053bf5e42dc2a54500ab012d9642756958d85afe08bb1bbd02669719c1");
            BitString expectedCipherText = new BitString("8ad652ff0a1d8bc599dd7176e8be92b1fd6d8febb9abe6e2c942f8e1376ae316aee80abaec45494133aa50f8f0d5c87a8132009d5f05de03b683f27b1755f9f0cbcda5eb762272c025c7eedd2c7442b4723941987eec91b69679cecf27d9b187c9250395cd302f463af0a85fee8da0e074f61a3b55ef39e9ce34afceb324eb7826587117b9d4a9c6372a580d4689e88e0d28ceba8b6e8d543c2788f471111fdf");

            var result = _subject.BlockEncrypt(iv, key, plainText);

            Assert.AreEqual(expectedCipherText, result.CipherText);
        }

        [Test]
        public void ShouldCorrectlyMMTEncrypt192()
        {
            BitString key = new BitString("bbffaf95aef69c9d6a80fbc854c66321dac6ec30f1f541c3");
            BitString iv = new BitString("49559b3af62cb9335089a0c60083d7f1");
            BitString plainText = new BitString("25ad4c7fa527320a8d15b9c7a03e55e8958d1bc4be1e8085e999642084bf4f8b0442fa2a87da118ab7953751f26f4b75a922e9ad063f3425d27ed37695523ad5ffc12b8a86737ca551091aec55b04e17bef53f4090dbb9958e2a2c505b0bf8e9dfd2e789ec1bfbc321186b7f0f29e8369bad2465a319953be54777951c10102d6b1c370efff9174cf16b31f16903a03a09f3a104892152819e7aba2f21890a0b");
            BitString expectedCipherText = new BitString("3861811bfce4188a1b2d6d03ba2a1982473966205b7702c65cc77326ede8918c73a534d90132f5d106c5b7ba33b5370caeb11eb8726f6bc2d00d6a3ee8a61514c588d1ff3e3fcceed7efe269945d9b27f70a145e296ede434284f73474654d11d047300c27ab35a695198e3c87d7d5879af5edb4ee48fd40ecffad5d14b49da805ceaea338507701a3671cc15573e2bca481ba36abaf27803a411b299e215436");

            var result = _subject.BlockEncrypt(iv, key, plainText);

            Assert.AreEqual(expectedCipherText, result.CipherText);
        }

        [Test]
        public void ShouldCorrectlyMMTEncrypt256()
        {
            BitString key = new BitString("2f62e013a8cd6381b9c6312fa94ed4cd7240519f608f5cdbdbcfc85381f74153");
            BitString iv = new BitString("5ab09ac9673b24e8116c46a7aa5c8503");
            BitString plainText = new BitString("aa7267ce34de3d3a6c088d22673cc0606845c1405ee4a790ad7a94f63108dc8780596a94707e60c7da7bfe6f9d40c519e5dbdc60adebb2068258e5438a41635cc636cf7894a826b0789da2651ba8ded694193cf1e771846acb19cd0bb153a14b01705c21a744d51aced740aaeb5d53bffe68a89c155d63e5112414f6ac24df79bb6fda38b139b62f658fa2e615466cfeaa31e9093d97989fdbe382ae859e6411");
            BitString expectedCipherText = new BitString("f96f90ca2a38199338ebe6d2cb3b3fa62f2d96e4ada433f03ef9cfdde5f29de6d48f5f6e5cbd61ced68de2484d9c531ad2fa51c54fc2700eb77747dbac6dda9975887365367cdc163d05710749169892cad88033a390f60f099b945b1496164609c6a48654a4e7f355779c1ea1c313c5394162a19bbdddfcd0638650710e0f87b0c9f0d9c3a67e0310fe20d8c0cc690c2546a81e540376521adf376dc7ff4456");

            var result = _subject.BlockEncrypt(iv, key, plainText);

            Assert.AreEqual(expectedCipherText, result.CipherText);
        }
        #endregion Encrypt

        #region Decrypt
        [Test]
        public void ShouldCorrectlyMMTDecrypt128()
        {
            BitString key = new BitString("d3d96886b5c14668e1055acb1b7012f7");
            BitString iv = new BitString("1b5e32c803cea56e5b2ca1778616df0b");
            BitString cipherText = new BitString("d02cd35c3095388cc5b0a167378cad9bd062863f99271e0ac05db8c33d2eabf8b5bb62f39fb2a8eaa51e22764d841d46d82b8b156bbb47d96c0aa13e2f321652d2e75f826507aec54e733417ec1ee6297b638157389ccbdac5f01c3fce6c1e17ed55d1f1cbbdd2858955dc325e828fab81b370ed49f1327492b834ae64054ab0cfb0c02819fc9c921c6d24afe9d9a033b1c261bee6e3020e9d09ee7337242283");
            BitString expectedPlainText = new BitString("03ab8128de94ac7c9361eea1d5aed99e60c56fcc2b1c405a9bb23f1cda8b7f8c91f2eba45aa5ae4d8e6fd131f838c74dac4a12fb98595503f8375810a5337b5e32f93f6601770a2c63b879e5c5f2f6c6bbb6adbcd091abde831e6b612a8b030df0e8fa0bfeefcac1cbc94acb5ee6ace0185f8e0023ba6811f79144235c72739f09a1fdbfd54e52e194ff6855702fcfc03b092840b7f3f94207e7c71e55c517bd");
            
            var result = _subject.BlockDecrypt(iv, key, cipherText);

            Assert.AreEqual(expectedPlainText, result.PlainText);
        }

        [Test]
        public void ShouldCorrectlyMMTDecrypt192()
        {
            BitString key = new BitString("55fc70cad1ecbae0f1a7781c1ad04fde405bf74dfc4cef3d");
            BitString iv = new BitString("caf285da21258068320005d828913ac5");
            BitString cipherText = new BitString("7abfaf14f70acc068c4d3e416a45dc0e442a13fc058f36debe891613e213d30ab5b1f1040254ae1c0c7e9385c8a64a1449deaffe7cb329e215553b0ac9f856eebe79f2f94045dd90b577d251d3404d98108c9e485ae23cc8098c5d4b12f9c3557d5fd97b19a4fe6a6e7f212af3db8ea4817c076208e417978178a35f23dfc247daabcf10f53bbcb01edfa89a903eb43f84f513d5ec816157645f36cd6671facd");
            BitString expectedPlainText = new BitString("06af00fc828deb36201491a7ea3270fc1419a894851f695b019058e7494b3d3fad82a7d7ba2e443204c5d31c344f4cfd3586092ef7b9d8abd02cd032012cb1f3f19c8ec42fe749df54579f39819e361e57e08f76b056a6471b6615c5e4ef3bb3d6e8e03d9cf98b53c1fb3d807a0b7b653664fdceb73596a7d9695b35bc41bdebb0260cf46cad693c554896d2e313521ddc2432f5ec278012bb4fc058fccc6b1e");
            
            var result = _subject.BlockDecrypt(iv, key, cipherText);

            Assert.AreEqual(expectedPlainText, result.PlainText);
        }

        [Test]
        public void ShouldCorrectlyMMTDecrypt256()
        {
            BitString key = new BitString("addb4b410381a68573bc9618cfa811295a0b19bfd043e24ca77d378695281a72");
            BitString iv = new BitString("06f5fed3384a800e84b49eddd7b6e372");
            BitString cipherText = new BitString("d1a37db3b00c737b9db65f3536da69fa7d7f47377f92045d343800daa763394956a4c8e5dc40af44cd85d142e25e423b5626c17d8fba83fc01d23348509136bdb45a33f9a2e6d394a993c50b9aff06fcebd65f995de82f57dd7f6ae6a8efe266b36ec5d84a2667d2f7894277a5e53ca70538287331458ed490e7af9cf9a6c4ac1b8887681c29f468800e51704d376ca36cf324f353f92adce1b3095289dd64f8");
            BitString expectedPlainText = new BitString("71d8b587a657b0e75ac85ade67cae26c8c8d2c5bbb53959af6ed008ef0eb4339a2bc39263752c8ecd49f3914f157dafe45e7b48b7f8e7bbc9373d0ffa270928b18c955199f36336ab3a641dbfa27b022986acba3a58ec8fc02c54303b086bc848156e4db355a5546290ca7a81a24a1118c6b37aae145817fa324e76396f79689b2f309ced27e6e48d3c7823adc0e1f4224f1fdd68d4dc22c6c98620c524334de");
            
            var result = _subject.BlockDecrypt(iv, key, cipherText);

            Assert.AreEqual(expectedPlainText, result.PlainText);
        }
        #endregion Decrypt

    }
}
