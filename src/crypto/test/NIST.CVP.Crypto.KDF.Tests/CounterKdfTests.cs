using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KDF.Tests
{
    [TestFixture, FastCryptoTest]
    public class CounterKdfTests
    {
        private KdfFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new KdfFactory();
        }

        [Test]
        [TestCase(MacModes.CMAC_AES128, CounterLocations.BeforeFixedData, 8, 128,
            "dff1e50ac0b69dc40f1051d46c2b069c",
            "c16e6e02c5a3dcc8d78b9ac1306877761310455b4e41469951d9e6c2245a064b33fd8c3b01203a7824485bf0a64060c4648b707d2607935699316ea5",
            "8be8f0869b3c0ba97b71863d1b9f7813",
            TestName = "Counter Kdf - CMAC_AES128 - Before Fixed Data - 8bit Counter")]
        [TestCase(MacModes.CMAC_AES128, CounterLocations.BeforeFixedData, 16, 128,
            "30ec5f6fa1def33cff008178c4454211",
            "c95e7b1d4f2570259abfc05bb00730f0284c3bb9a61d07259848a1cb57c81d8a6c3382c500bf801dfc8f70726b082cf4c3fa34386c1e7bf0e5471438",
            "00018fff9574994f5c4457f461c7a67e",
            TestName = "Counter Kdf - CMAC_AES128 - Before Fixed Data - 16bit Counter")]
        [TestCase(MacModes.CMAC_AES256, CounterLocations.AfterFixedData, 32, 128,
            "746c44c4129858d89e50e09dc44aec2ab2158c2e0c6bb73b35588e94e33a1958",
            "ebeed6a0462577b6b4e2fe4697c6ae6e1c6b8b9fd14381247bc2cf2c06d7afb55b06389612a85d0a69a1486eb399e7f314b234fd44908396b55f6e67",
            "85e1cd8cea5a43f7f5b626fa7666f550",
            TestName = "Counter Kdf - CMAC_AES256 - After Fixed Data - 32bit Counter")]
        [TestCase(MacModes.CMAC_TDES, CounterLocations.BeforeFixedData, 8, 128,
            "89b3469ce73b4fef33244de2cb772bc239a4261a45993b3b",
            "f1aec104bc3b1735e28f90a6d3aa7cd319841303989bc4a2a0da886c5c5764d0bd7c12d94723133f664a109d289d0f2971cbfec4da2f3b5cbfbc47f2",
            "af52a719396e6eecc4cb323994113f42",
            TestName = "Counter Kdf - CMAC_TDES - Before Fixed Data - 8bit Counter")]
        [TestCase(MacModes.HMAC_SHA1, CounterLocations.AfterFixedData, 16, 256,
            "46bc72010d07189cbb32c0cb27fbb13edfc4a440",
            "144b4c9da3152101684338129db71026a6064acf262847f972526155b8a6562d8453dabf06817b0606542cbecd55c1fda0f092f803dcc2dbfebca6bf",
            "316af25b1b65683d1e40987ee9de6afca18688c7b46ba0e6e7644c14df05f2e7",
            TestName = "Counter Kdf - HMAC_SHA1 - After Fixed Data - 16bit Counter")]
        [TestCase(MacModes.HMAC_SHA384, CounterLocations.BeforeFixedData, 16, 320,
            "f2c722ae0d311e038e444298cbc7ac47ad210c0ff90152db2fb300c4bbc0d6dfd775bedbfe03f6a24f1c8fa3a1e1f926",
            "ff5bf0137660d6c61fc6574274c0eecf6c7da3bc1d7ec6263e31b104c9a6a1dd0973709e1af7b9efb2308184de769af1bc7c7cb4b8cb513ead702924",
            "9a57fde7b16c9554e7650d33d544dd660755eb6775e61e5ee01c19daf9576689da693d337e80adc6",
            TestName = "Counter Kdf - HMAC_SHA384 - Before Fixed Data - 16bit Counter")]
        [TestCase(MacModes.HMAC_SHA512, CounterLocations.MiddleFixedData, 32, 128,
            "4cfbc55d3a2334c71787ea1c4b9426106b1ba327a909d54fc9b3113f4b74617fec68858a05ea9943fffb0623af633f2a16ae87afa37e3f304da41f7b83e4cb91",
            "2d6b4804ed912a9bf3005db33c221c6793ff33ffc90bf559811d63fdd0d06f8f36da610f2d555ea37bf3f1220a8e8a8a8629adbd9e4688b45575d385",
            "5260b2e61f6ad15e775a793c699c5583",
            50 * 8,
            TestName = "Counter Kdf - HMAC_SHA512 - Middle Fixed Data - 32bit Counter")]
        public void ShouldCorrectlyDeriveKey(MacModes mac, CounterLocations ctrLocation, int rLen, int outLen, string kI, string fixedInput, string kO, int breakLocation = 0)
        {
            var keyIn = new BitString(kI);
            var fixedInputData = new BitString(fixedInput);
            var expectedKey = new BitString(kO);

            var kdf = _factory.GetKdfInstance(KdfModes.Counter, mac, ctrLocation, rLen);

            var result = kdf.DeriveKey(keyIn, fixedInputData, outLen, breakLocation: breakLocation);

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(expectedKey, result.DerivedKey);
        }

        [Test]
        [TestCase(128, 0, false)]
        [TestCase(128, 200, false)]
        [TestCase(128, 128, false)]
        [TestCase(128, 127, true)]
        [TestCase(128, 1, true)]
        public void ShouldOnlyAllowBreakPointsThatSplitFixedData(int dataLen, int breakLocation, bool expectedResult)
        {
            var kdf = _factory.GetKdfInstance(KdfModes.Counter, MacModes.CMAC_AES128, CounterLocations.MiddleFixedData, 8);

            var result = kdf.DeriveKey(BitString.Zeroes(128), BitString.Zeroes(dataLen), 1, breakLocation: breakLocation);

            Assert.AreEqual(expectedResult, result.Success);
        }
    }
}
