using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.CMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfHkdf;
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
    public class KdfMultiExpandTests
    {
        private IKdfVisitor _kdfVisitor;
        private IKdfMultiExpansionVisitor _kdfMultiExpansionVisitor;

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

            _kdfMultiExpansionVisitor = new KdfMultiExpansionVisitor(
                new KdfFactory(cmacFactory, hmacFactory, kmacFactory), new HkdfFactory(hmacFactory), cmacFactory, hmacFactory);
        }

        [Test]
        public void ShouldMultiExpandSameResult()
        {
            var fixedInfo = new BitString(128);
            var salt = new BitString(128);
            var z = new BitString(128);
            var hmacAlg = HashFunctions.Sha2_d256;
            var l = 256;

            var hkdfParam = new KdfParameterHkdf()
            {
                L = l,
                Salt = salt,
                Z = z,
                HmacAlg = hmacAlg
            };
            var multiExpandHkdfParam = new KdfMultiExpansionParameterHkdf()
            {
                Salt = salt,
                Z = z,
                HmacAlg = hmacAlg,
                IterationParameters = new List<KdfMultiExpansionIterationParameter>()
                {
                    new KdfMultiExpansionIterationParameter(l, fixedInfo),
                    new KdfMultiExpansionIterationParameter(l, fixedInfo),
                }
            };

            var hkdfResult = _kdfVisitor.Kdf(hkdfParam, fixedInfo);
            var multiExpandHkdfResult = _kdfMultiExpansionVisitor.Kdf(multiExpandHkdfParam);

            Assert.True(multiExpandHkdfResult.Results.Count == 2);
            Assert.AreEqual(hkdfResult.DerivedKey, multiExpandHkdfResult.Results[0].DerivedKey);
            Assert.AreEqual(hkdfResult.DerivedKey, multiExpandHkdfResult.Results[1].DerivedKey);
        }
    }
}
