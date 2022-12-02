using NIST.CVP.ACVTS.Libraries.Crypto.CMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.CMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.KMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.HKDF;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.IKEv1;
using NIST.CVP.ACVTS.Libraries.Crypto.IKEv2;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF.OneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.KMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.TLS;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using KdfFactory = NIST.CVP.ACVTS.Libraries.Crypto.KDF.KdfFactory;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Tests.KDF
{
    [TestFixture, FastCryptoTest]
    public class TwoStepKdfTests
    {
        private IKdfVisitor _kdfVisitor;

        [SetUp]
        public void Setup()
        {
            IShaFactory shaFactory = new NativeShaFactory();
            IHmacFactory hmacFactory = new HmacFactory(shaFactory);
            IKmacFactory kmacFactory = new KmacFactory(new cSHAKEWrapper());
            ICmacFactory cmacFactory = new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory());

            _kdfVisitor = new KdfVisitor(
                new KdfOneStepFactory(shaFactory, hmacFactory, kmacFactory),
                new KdfFactory(cmacFactory, hmacFactory, kmacFactory),
                hmacFactory,
                cmacFactory,
                new IkeV1Factory(hmacFactory, shaFactory),
                new IkeV2Factory(hmacFactory),
                new TlsKdfFactory(hmacFactory),
                new HkdfFactory(hmacFactory));

        }

        [Test]
        public void ShouldKdf()
        {
            var twoStepParam = new KdfParameterTwoStep()
            {
                L = 256,
                Salt = new BitString(128),
                Z = new BitString(128),
                CounterLen = 8,
                CounterLocation = CounterLocations.AfterFixedData,
                KdfMode = KdfModes.Counter,
                MacMode = MacModes.HMAC_SHA224,
            };

            var dkm = _kdfVisitor.Kdf(twoStepParam, new BitString(128));

            Assert.AreEqual("EB9436CDC0C6FBC168A3BDE32929C104C2E4F4C1DEA2CA3485A7799E49870E0C", dkm.DerivedKey.ToHex());
        }
    }
}
