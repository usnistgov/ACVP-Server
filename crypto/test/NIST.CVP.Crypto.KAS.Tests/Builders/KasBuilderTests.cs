using Moq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests.Builders
{
    [TestFixture, UnitTest]
    public class KasBuilderTests
    {

        private KasBuilder _subject;
        private MacParametersBuilder _macParamsBuilder;
        private Mock<IShaFactory> _shaFactory;
        private Mock<IDsaFfc> _dsa;
        private Mock<IDsaFfcFactory> _dsaFactory;
        private IEntropyProvider _entropyProviderScheme;
        private IEntropyProvider _entropyProviderOtherInfo;

        [SetUp]
        public void Setup()
        {
            _shaFactory = new Mock<IShaFactory>();
            _dsa = new Mock<IDsaFfc>();
            _dsaFactory = new Mock<IDsaFfcFactory>();
            _dsaFactory
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(_dsa.Object);
            _entropyProviderScheme = new TestableEntropyProvider();
            _entropyProviderOtherInfo = new TestableEntropyProvider();

            _subject = new KasBuilder(
                new SchemeBuilder(
                    _dsaFactory.Object,
                    new KdfFactory(
                        new ShaFactory()
                    ),
                    new KeyConfirmationFactory(),
                    new NoKeyConfirmationFactory(),
                    new OtherInfoFactory(
                        _entropyProviderOtherInfo
                    ),
                    _entropyProviderScheme,
                    new DiffieHellman(),
                    new Mqv()
                )
            );

            _macParamsBuilder = new MacParametersBuilder();
        }

        [Test]
        public void ShouldReturnComponentOnlyKas()
        {
            var result = _subject
                .WithKeyAgreementRole(KeyAgreementRole.InitiatorPartyU)
                .WithScheme(FfcScheme.DhEphem)
                .WithParameterSet(FfcParameterSet.Fb)
                .WithAssurances(KasAssurance.None)
                .WithPartyId(new BitString(1))
                .BuildNoKdfNoKc()
                .Build();


            Assert.AreEqual(KasMode.NoKdfNoKc, result.Scheme.SchemeParameters.KasMode);
        }

        [Test]
        public void ShouldReturnNoKeyConfirmationKas()
        {
            var macParams = _macParamsBuilder
                .WithKeyAgreementMacType(KeyAgreementMacType.AesCcm)
                .WithMacLength(0)
                .WithNonce(new BitString(1))
                .Build();

            var result = _subject
                .WithKeyAgreementRole(KeyAgreementRole.InitiatorPartyU)
                .WithScheme(FfcScheme.DhEphem)
                .WithParameterSet(FfcParameterSet.Fb)
                .WithAssurances(KasAssurance.None)
                .WithPartyId(new BitString(1))
                .BuildKdfNoKc()
                .WithKeyLength(0)
                .WithOtherInfoPattern(string.Empty)
                .WithMacParameters(macParams)
                .Build();

            Assert.AreEqual(KasMode.KdfNoKc, result.Scheme.SchemeParameters.KasMode);
        }

        // TODO test valid once a key confirmation scheme exists (change FfcScheme.DhEphem)
        //[Test]
        //public void ShouldReturnKeyConfirmationKas()
        //{
        //    var result = _subject.GetInstance(
        //        new KasParametersKeyConfirmation(
        //            KeyAgreementRole.UPartyInitiator,
        //            FfcScheme.DhEphem,
        //            FfcParameterSet.FB,
        //            KasAssurance.None,
        //            _dsa.Object,
        //            new BitString(1),
        //            new KdfParameters(0, string.Empty),
        //            new MacParameters(KeyAgreementMacType.AesCcm, 0, new BitString(1)),
        //            KeyConfirmationRole.Provider,
        //            KeyConfirmationDirection.Unilateral
        //        )
        //    );

        //    Assert.AreEqual(KasMode.KeyConfirmation, result.KasParameters.KasMode);
        //}
    }
}