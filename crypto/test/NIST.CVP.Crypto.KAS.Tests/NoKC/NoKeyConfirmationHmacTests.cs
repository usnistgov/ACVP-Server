using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using StackExchange.Redis;

namespace NIST.CVP.Crypto.KAS.Tests.NoKC
{
    [TestFixture, FastCryptoTest]
    class NoKeyConfirmationHmacTests
    {
        private NoKeyConfirmationHmac _subject;
        private readonly IHmacFactory _algoFactory = new HmacFactory(new ShaFactory());

        private static object[] _testData = new object[]
        {
            new object[]
            {
                // Key length
                112,
                // tag length
                64,
                // DigestSize
                KeyAgreementMacType.HmacSha2D512,
                // DKM
                new BitString("f70f312c20850cb5207f5226cb5a"),
                // Nonce
                new BitString("edda25fd7a682353cfa4f871d7a0048e"),
                // Expected MAC data
                new BitString("5374616e646172642054657374204d657373616765edda25fd7a682353cfa4f871d7a0048e"),
                // Expected MAC
                new BitString("a7553d35d0872d8b")
            },
            new object[]
            {
                // Key length
                112,
                // tag length
                64,
                // DigestSize
                KeyAgreementMacType.HmacSha2D512,
                // DKM
                new BitString("7f037d73af3de30509d33d94016d"),
                // Nonce
                new BitString("9633299362fd88898e93b762112207ba"),
                // Expected MAC data
                new BitString("5374616e646172642054657374204d6573736167659633299362fd88898e93b762112207ba"),
                // Expected MAC
                new BitString("db5492ddb382a620")
            },
            new object[]
            {
                // Key length
                112,
                // tag length
                64,
                // DigestSize
                KeyAgreementMacType.HmacSha2D512,
                // DKM
                new BitString("c512935b6f1492978a6118c3742e"),
                // Nonce
                new BitString("5d5ec2d29e36a523fa70524f1e510475"),
                // Expected MAC data
                new BitString("5374616e646172642054657374204d6573736167655d5ec2d29e36a523fa70524f1e510475"),
                // Expected MAC
                new BitString("f8fe920ebe9f3287")
            },
            new object[]
            {
                // Key length
                112,
                // tag length
                64,
                // DigestSize
                KeyAgreementMacType.HmacSha2D512,
                // DKM
                new BitString("52efc5e1a6ba49c781c1453418ef"),
                // Nonce
                new BitString("df0fc73abf865ec37d85f9686a659dea"),
                // Expected MAC data
                new BitString("5374616e646172642054657374204d657373616765df0fc73abf865ec37d85f9686a659dea"),
                // Expected MAC
                new BitString("d0b0ba6b1cb98725")
            },
            new object[]
            {
                // Key length
                112,
                // tag length
                64,
                // DigestSize
                KeyAgreementMacType.HmacSha2D512,
                // DKM
                new BitString("04d4cf009f2f2bfab209c6ee6df9"),
                // Nonce
                new BitString("dbfcf03f43110639da015efd69fbb65e"),
                // Expected MAC data
                new BitString("5374616e646172642054657374204d657373616765dbfcf03f43110639da015efd69fbb65e"),
                // Expected MAC
                new BitString("144f439c9493c809")
            }
            //new object[]
            //{
            //    // Key length
            //    112,
            //    // tag length
            //    64,
            //    // DigestSize
            //    KeyAgreementMacType.HmacSha2D512,
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
        public void ShouldMacCorrectly(int keyLength, int macLength, KeyAgreementMacType keyAgreementMacType, BitString dkm, BitString nonce, BitString expectedMacData, BitString expectedMac)
        {
            ModeValues modeValue = ModeValues.SHA2;
            DigestSizes digestSize = DigestSizes.NONE;
            
            EnumMapping.GetHashFunctionOptions(keyAgreementMacType, ref modeValue, ref digestSize);

            NoKeyConfirmationParameters p =
                new NoKeyConfirmationParameters(keyAgreementMacType, macLength, dkm, nonce);
            _subject = new NoKeyConfirmationHmac(p, _algoFactory.GetHmacInstance(new HashFunction(modeValue, digestSize)));

            var result = _subject.ComputeMac();

            Assume.That(result.Success, nameof(result.Success));
            Assert.AreEqual(expectedMacData.ToHex(), result.MacData.ToHex(), nameof(result.MacData));
            Assert.AreEqual(expectedMac.ToHex(), result.Mac.ToHex(), nameof(result.Mac));
        }
    }
}
