using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.MAC.CMAC;
using NIST.CVP.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests.NoKC
{
    [TestFixture, FastCryptoTest]
    public class NoKeyConfirmationCmacTests
    {
        private NoKeyConfirmationCmac _subject;
        private readonly ICmacFactory _algoFactory = new CmacFactory();

        private static object[] _testData = new object[]
        {
            new object[]
            {
                // Key length
                128,
                // tag length
                80,
                // DKM
                new BitString("0af31752b062b1338f4cd7b697ad9165"),
                // Nonce
                new BitString("b01474a065a9d460c5e83bfdbed52f31"),
                // Expected MAC data
                new BitString("5374616e646172642054657374204d657373616765b01474a065a9d460c5e83bfdbed52f31"),
                // Expected MAC
                new BitString("154f94bf8bbbad274698")
            },
            new object[]
            {
                // Key length
                128,
                // tag length
                80,
                // DKM
                new BitString("bb73401acb496433b96fad6dec312a09"),
                // Nonce
                new BitString("7252b825ed99fa5e8c216f0f81657d9d"),
                // Expected MAC data
                new BitString("5374616e646172642054657374204d6573736167657252b825ed99fa5e8c216f0f81657d9d"),
                // Expected MAC
                new BitString("7049843e29bbfd1d10cb")
            },
            new object[]
            {
                // Key length
                128,
                // tag length
                80,
                // DKM
                new BitString("887ba693de6b6b71109c9e359576d1dc"),
                // Nonce
                new BitString("e19c76f2d1ea6cb2f0b4e11f80bed355"),
                // Expected MAC data
                new BitString("5374616e646172642054657374204d657373616765e19c76f2d1ea6cb2f0b4e11f80bed355"),
                // Expected MAC
                new BitString("bb5f70154cc4325f3460")
            },
            new object[]
            {
                // Key length
                128,
                // tag length
                80,
                // DKM
                new BitString("0540b6abcd986b78d7bb7a1e70099cc0"),
                // Nonce
                new BitString("c0204534bdcf83daee1b4cbcd30ada3b"),
                // Expected MAC data
                new BitString("5374616e646172642054657374204d657373616765c0204534bdcf83daee1b4cbcd30ada3b"),
                // Expected MAC
                new BitString("ff6f874cadde424f885e")
            },
            new object[]
            {
                // Key length
                128,
                // tag length
                80,
                // DKM
                new BitString("9fa630e91f6b420689d2060b843939d7"),
                // Nonce
                new BitString("d4ab56f5a790ca6ff2c4316f9435126d"),
                // Expected MAC data
                new BitString("5374616e646172642054657374204d657373616765d4ab56f5a790ca6ff2c4316f9435126d"),
                // Expected MAC
                new BitString("3563b6c3d59a55fa9cfa")
            },
            //new object[]
            //{
            //    // Key length
            //    128,
            //    // tag length
            //    80,
            //    // DKM
            //    new BitString(""),
            //    // Nonce
            //    new BitString(""),
            //    // Expected MAC data
            //    new BitString(""),
            //    // Expected MAC
            //    new BitString("")
            //}
        };

        [Test]
        [TestCaseSource(nameof(_testData))]
        public void ShouldMacCorrectly(int keyLength, int macLength, BitString dkm, BitString nonce, BitString expectedMacData, BitString expectedMac)
        {
            NoKeyConfirmationParameters p =
                new NoKeyConfirmationParameters(KeyAgreementMacType.CmacAes, macLength, dkm, nonce);
            _subject = new NoKeyConfirmationCmac(p, _algoFactory.GetCmacInstance(CmacTypes.AES128));
            var result = _subject.ComputeMac();

            Assume.That(result.Success, nameof(result.Success));
            Assert.AreEqual(expectedMacData.ToHex(), result.MacData.ToHex(), nameof(result.MacData));
            Assert.AreEqual(expectedMac.ToHex(), result.Mac.ToHex(), nameof(result.Mac));
        }
    }
}
