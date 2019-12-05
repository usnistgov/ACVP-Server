using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KDF.Tests
{
    [TestFixture, FastCryptoTest]
    public class PipelineKdfTests
    {
        private KdfFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new KdfFactory(new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory()), new HmacFactory(new ShaFactory()));
        }

        [Test]
        [TestCase(MacModes.CMAC_AES128, CounterLocations.BeforeIterator, 8, 512,
            "c6254d95dd108e9bb29e0053ddeec351",
            "22f498fc9b8d4b72188bce30ba9875fc2b0eb3fe76874d85426e6e5b3b237c9f445f2da20a60ab189802e2c152c4a3602aa342",
            "1e133a952df55a11ee038120375f61e7c0162842c817160693b1f39dc0b795bc6f3691db775cf3af4b0a9f69fecbe99679fd4b4873dda743f5c6a2d2e873f26d",
            TestName = "Pipeline Kdf - CMAC_AES128 - Before Fixed Data - 8bit Counter")]
        [TestCase(MacModes.CMAC_AES128, CounterLocations.BeforeIterator, 16, 512,
            "343eccae7e7e233fdc819ecfabf11735",
            "44465519cee317a678247ec5621c6b06e07f42497028261b48a55a916f1116abdd3c92dd43c372b4e7ee953309a6e356c7dec1",
            "e424531e6ec5fb56d43d02cdb67d3bb92652c004ec2fea8a3feb66b83ea44b5d50487bdce7861380684802e7e3a145afb02b033d755841e7906924e87bb30001",
            TestName = "Pipeline Kdf - CMAC_AES128 - Before Fixed Data - 16bit Counter")]
        
        [TestCase(MacModes.CMAC_AES256, CounterLocations.MiddleFixedData, 8, 512,
            "94450a5edfd8f17f4722faf68ae0d7a2bb725aee3b0f6502f712ada0f33a9bc2",
            "62100bfd621c06b107db99b1dd6d8eebd9d93f4fa1f8fc4501c02b591f54d7b2de0cbd69c52dd48c361e7bd6d88688607332ff",
            "7fd7553297ef5b5dfa25706524296288f19abd7344b7445fb74bd33ea894493b9616e72bb433a51b7a6c42255c89ed954a0e3530fb85f8727681fb04c817367b",
            TestName = "Pipeline Kdf - CMAC_AES256 - Middle Fixed Data - 32bit Counter")]
        [TestCase(MacModes.CMAC_TDES, CounterLocations.BeforeIterator, 24, 2048,
            "73a03812f851f2b34b83e61f9fbbc20ab89a65e7f9e1cda6",
            "f54906c994a3ea51a2cf95ee59cd8343efac94defff3447715426adf62f98ac4260d00dbcfcf2545d59062af136a986568b70d",
            "6f8d7cd238c816a98f661b37e22b7159f01c3835b16a1b37d43f1d66c82c9311154332c140fc74f0e8586ac8a5d2c5c52b28a41e6c0be1bd8c29d8080b3f0f07e54d8fca9c63e30aeb276c1bf362e4d91c4d0e326d9fd3f47373228eb9d967d09c720ad2b0721ea8d620a107ce0e942726ffd3dc88dedb5425d2e15e1d1ee15b517d2ea1ea97e0d07414d31ab40f6392db0def3487c93a8e1d35947b48c72c2a1d0dd699ac16524771c6b522c71918d40f0b67f39ea85bd6e2fc62dfe6a084ced22b7cbef53c2ce251a7a58936035ebf6f1899d25f30099c7f277a61fe8c5c6300b2827a1be579a2646799617159f4fc756dd7cb331764550e93f6e7818e520e",
            TestName = "Pipeline Kdf - CMAC_TDES - Before Fixed Data - 24bit Counter")]

        [TestCase(MacModes.HMAC_SHA1, CounterLocations.None, 0, 512,
            "278d5148bb874bd98d604b4417e97f95ebf67dd1",
            "4b90bd93e1b9bb7d385bbd11340ca853c783c2d3284c602aab8fe24c2f7c3502dba1177569ce6f58354a07999360dc0ad8d334",
            "f06a2a6e5dd085538b75c6e10e96509efddfe6b34b2497227401a24fd8e09453f3ed8ab187d16efd912abf9a234227c54eff463f16172795dc4666ab10fefab0",
            TestName = "Pipeline Kdf - HMAC_SHA1 - No Counter")]
        [TestCase(MacModes.HMAC_SHA256, CounterLocations.None, 0, 1040,
            "d7786c20f73e2c7eebc10ac4332b20e86a8f9e9c8a5b7ecb12f7d24df6d0ed14",
            "6aca9a3c95ee7d471cebe802f6bed855fc81f5bcb72c1f70406f310fe687d1aeb59e701a6639c7819048be19e8108c4e120e36",
            "02b0cb13bcb89cc1f37751b4bb952ef49a38d3b4a5b431d6d7c381ec9672aaf9050f49585e0b48d233f14a27a073d2e8444a15c1797944b31017259d5e477885339f47ff6da43124749b3676a944b47463a4f4bb68deb1b03cb353cfdd840297b2a1ab460f6ab3022cc66bdba3308e162c2efa39d4f2636433cd69469e397ec4f10b",
            TestName = "Pipeline Kdf - HMAC_SHA256 - No Counter")]

        [TestCase(MacModes.HMAC_SHA512, CounterLocations.AfterFixedData, 32, 1600,
            "e8cd0166a8af6220ce2d79b220a58d26307c58280012d714886e76aeb9ae96d6ccb4705d61a72ea4b8755a5856d8ab0c924f046c83455cd98c04b9792dacd841",
            "1a6c8d000e58116d0ef0514a973ec8984052caf4becdc7cb05fce4b9e68cf47329e2371d2f21b6a6bf8a83adfb034715145dc4",
            "8c380e322dbe2d741423a4411cf77d92d40647e51f3a1ba94c7d3bdf880e446fdab41f8e6c292ebf6b15644765050012ca5aef09f6d3f6ea1141f9317787edbb58760a267e5228ecc1424327798fabc933bc87dbda160a15afb92c4a54e6709f361dcba978958316d3e02f77de3c278736e3ba2a3844fe3c3365dd8d749e5e6d85f9ec2f3dd52162e3c93e89d51c36372d005b0e529cec8616e3abedb0149566381c740631b124734d7acefa93be40993bc3dd0a52583852e87385fa84c85cc4fcd6727e764ea2e8",
            TestName = "Pipeline Kdf - HMAC_SHA512 - After Fixed Data - 32bit Counter")]
        public void ShouldCorrectlyDeriveKey(MacModes mac, CounterLocations ctrLocation, int rLen, int outLen, string kI, string fixedInput, string kO)
        {
            var keyIn = new BitString(kI);
            var fixedInputData = new BitString(fixedInput);
            var expectedKey = new BitString(kO);

            var kdf = _factory.GetKdfInstance(KdfModes.Pipeline, mac, ctrLocation, rLen);

            var result = kdf.DeriveKey(keyIn, fixedInputData, outLen);

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(expectedKey, result.DerivedKey);
        }
    }
}
