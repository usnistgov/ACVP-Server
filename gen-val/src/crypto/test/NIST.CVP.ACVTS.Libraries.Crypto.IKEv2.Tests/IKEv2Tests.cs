using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.IKEv2.Tests
{
    [TestFixture, FastCryptoTest]
    public class IkeV2Tests
    {
        [Test]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, 1056,
            "1df77d01277c2e29deeaeb353e2c967b", "ebff4bf4945f9734db7b3af4aa7ed796",
            "e2fcb85717960683ff1700d05718b182a5b0be7133a02ad5b0a4a270", "31c5534a6d2e30b97e4d12526166aae0cb8e1d40c630e506252074ca",
            "48765233291b2a42", "f5a3ee5d92548a2a",
            "18ba6f5e429fb65e1ec26d90f929d67b8727e30f049f20ae66d56c9a", "370393be2f27d4b1dac17e8ab2ab1267985bdd38382a93a837caf1dbbe8cb0d313d2b6579eda3f552ae5e9a4d1f50d5e9285d8a6d8afbd398de5ab977383443af2a776618c80c52314d05f5faefbef95e1bf04bbe3879fe41d6f6ffd6d6a13b8853944f6c1201055abcec41005583a00269a301747f827e42eff906bf1f86d52fd3870c7", "ab9ed337fd24bdfa7d493534ee958f0f187ed2927eb12e6ffc2b45b2469ec7f0420e325d865dda9e2dd86dc53b95f0ecd45a09125359f652f5d64b1dfa8f2dfc6c08209cad70e841546e64eaa900e8132beeb7b65f994e8934dd6834478ecd03658a691c4ab5bd1e27c5cff7eba343027065a17333574f914e334b1ef20b08e3293b29b0", "87831a36e13eff758e1a961caa049cdf0696b9b939bc10a4f76857a04b6f0bfee01b2e54105d0a8bf2ae45bcc7f1e561e7001e9f58e6a747a4d6b930410d8dff677fbb09e22cbb89d7e19f0b9b2c7043471e11a2bbed576e2da738e7875575e4bfedfcf39730c36d78ce9656f50f0099786203128a20a80f5b4fbc145f12ccce3ff143dd", "e7b26b386ccbd433d333a872d90cbf99b14fdab6b0e8717f5ff3965a",
            TestName = "IKEv2 SHA2-224")]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, 3072,
            "0a58fea9c1d4fee2e74e1aad6fb02dd5f318eb127465a112be8dc3c04d5dba62eae708f9dc1b2362afd6ba4bda3cf5000f4979fbcbadbcfda9291593829c4e905c21db0c2ee15175ce77a8a0dadf25c7f175bc53fd91b636a9b3c50b203de9b9d6dc463dcb4048f101e26498ceaed3afbcd931394a7e099392dfff82e5d6fa85c2e8a8ab90ad363a66795dc32798847e405acd87988140f973058917273a6b0bafb962c75e2b1a21e79c08beeb82168bf8b4b57f3dce867bf7862a7b8acbc21be838021913daf64340c055ed17aaf7b6e1d0ca33ae890ddbdb83f4e615aad5e3bff58c3db70bac81a0dfbabe5391fc62bca4659e22a0741e01d8d06e67d9bb41", "e4fdfa68df6171d9eaff8b7b5ad31e7c6098d259f3397c9854cbefe7449b0864c1df16ddece8b26567560f8d59182a44c19149c2d0d72606a160021ade3d23c7c6936c91a4b6bec37adf38cfc84b851c68dcee06f81306f63abbefc07e231e3058c49437a557df617eac01663593d2a3e594699284c0a35eae6931492ccb2a43db72a600a3899205d681d2b76b1280e456339bb10e0c584aecfdae31c77a4c40dbcb345f782ac32835d4c220c1e97254ded6a40075dd717b00b69c86a49e801773026536ae02d58a649827f1ba67610ee936dffd11c9d5a9156631916a0a0046ec0ba42f56f2a18de83d5d71a985b0cc9eec2c06b3d907ca58454419f374069d",
            "9d469de41b4739c7dd01f19aa94f17e48150ae9dbede94832142db19", "c2987b0767006c3b9aea491bfed1646a3d7194639492b8d45b23b383",
            "c8de93111750e409", "32d51b33508997fa",
            "eec13ac67c2e072abb684ab4e7b6ce3e7f400af8b96bed48e113497ed337f8d4", "bc83518f63b762874ac57947cdfae33a8d9724df0b83ee4b6df38c23fc2ad829f4d70b5a68dd1f5a52416f09758638aedf919c09a17c5298cdb703bb75b1005cba0109739aaa4efd2fe7534c95ad05e546e4419931270eddcab68ac1589d7fde47f5b9e8d5b614cb24ec3caf515497a9e99ee6f1b318ff153b539ef071dcaff9b41945dcb780700d69e5dc888a91de195c52825badfd0ab4c33cec71ab5c2b4badefe5455455ce5d8ae91721a2091bcccc1514e69482c9c8da1d9f0f187eedda916393570a8c0a8dfabc26d8ed31e8ea47662515972a22d11d692bc1fac6395aecd82a01ab78daafcd6f3070b1201f158a19331b5d21ceee66fccd2ea37bc07648b9539a52dbf797af53e31a0e8706dd86cf5081ebabd095f281d23c222dd3bc5abf06869017ca8f751cafcc85abea6ba88c139d1f32a12f7f486cf57eae5d85551b97f3963616d8f597c70efa98c1e1fa369fb77a80b1ec5fe533890672c4551172eab1b7fd68ad08c4d4c69f2a867302065ca730aa6915bc2cfb6d93820977", "c725012285d56e53244a9baaaf95abfca543708d333a3f3a86ba660e00edd4d489bcd631819fda0413382edd9f53d1ed718e056c2ad91d38de90f06cd434de70141d051c021e7ddd6e4137946b4770dca59a4decabdd866852eb557fdd995646613ff2923f5f23aa58c8fbd5ad52cf2eac9bda97faad5596695f4a747a0f1cc6a1f78b2181138fb19ac24574d2e6b3a40d6e688abb919a27d793b76568ec66bd099f6f476b705c1472d774a1516a11311b887688f36649e24227640ea8d8666b9f452217e7103b6f0a92b6adb8fbda650ba58d6ac13ac6f6b6361a5d1c44f1e9f22196c96295115c5d01a120e7265962d20353a049286908da776c2165f706a16f43d85fcd319f89c4386340bcb8ed10df38345cb68af719abaf997e9789a5f19a392dcecd78168ba282f0b30555c7a47031a3156d12d7158aa41b863b8394d30fec63120e89cbc5750eed7786f7d5961b1f2e6faca641ed776d2211bee757b68b77f94056172553715da9fbd1c9a8728f28a2f3c9714a4917490132562484cd", "140a1f2a3c94318ece6edfa8c63f37d8d45982de8b6cbfe34011150db0253e34de3796d4aa579954a0bbf234794eb6e7ece2a8f91100f5507df4ee8e66c7813d4c0ff64d484a3494c54b0d9c605ea24234a49a010ce5c352feb9e1060c8c787229466296cdc9d11bb9d221ed2bb39544572464254ec02a0fbe777b07402c775eb6d3e53cd16214c707f851942cc7da92a811b6c5c82f29d9fc3751b0ebff9898fb14372402a6989ffb9cc32dfe7e2cea32fd882e09804d7fea34ce3b5eb6f303e095c48a452b0a3eccada81af22195ebbaad289aeabdab074c94e751496bde39d868ebf1d693f36d1c3c672bbb59c9f77aeb68de57d77da231b7f13d6d277f600c3d3086c4de7b3a59127f4cb9c118935574f30e1b7c88b52c8c3531fdbd38ee3d14c69e81803ced6e7501fa0180f6d110b648272002dc1ad5bf09dc83188011da1d48632e0d2453c8abdd8e9fd32a576aeb9ddfb0a024f674441c20a950d9bd8cd7b30933cc0d8e87b55e8aa9d913579b410417b3acc13c14bf73c457327873", "05f796c8df322a7e33283ae5fa2f376d8d5efa0ac3745fc548e430031424c121",
            TestName = "IKEv2 SHA2-256")]
        public void ShouldIkeV2Correctly(ModeValues mode, DigestSizes digestSize, int dkmLength, string niHex, string nrHex, string girHex, string girNewHex, string spiiHex, string spirHex, string sKeySeedHex, string dkmHex, string dkmChildSAHex, string dkmChildSADhHex, string SKeySeedReKeyHex)
        {
            var ni = new BitString(niHex);
            var nr = new BitString(nrHex);
            var gir = new BitString(girHex);
            var girNew = new BitString(girNewHex);
            var spii = new BitString(spiiHex);
            var spir = new BitString(spirHex);

            var sKeySeed = new BitString(sKeySeedHex);
            var dkm = new BitString(dkmHex);
            var dkmChildSA = new BitString(dkmChildSAHex);
            var dkmChildSADh = new BitString(dkmChildSADhHex);
            var sKeySeedReKey = new BitString(SKeySeedReKeyHex);

            var hmac = new HmacFactory(new NativeShaFactory()).GetHmacInstance(new HashFunction(mode, digestSize));
            var subject = new IkeV2(hmac);

            var result = subject.GenerateIke(ni, nr, gir, girNew, spii, spir, dkmLength);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(sKeySeed, result.SKeySeed, "SKeySeed");
            Assert.AreEqual(dkm, result.DKM, "DKM");
            Assert.AreEqual(dkmChildSA, result.DKMChildSA, "DKMChildSA");
            Assert.AreEqual(dkmChildSADh, result.DKMChildSADh, "DKMChildSADh");
            Assert.AreEqual(sKeySeedReKey, result.SKeySeedReKey, "SKeySeedReKey");
        }
    }
}
