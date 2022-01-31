using System;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.NoKC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders.Ffc;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Scheme.Ffc;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Tests.Builders
{
    [TestFixture, FastCryptoTest]
    public class SchemeBuilderFfcTests
    {
        private SchemeBuilderFfc _subject;
        private Mock<ISha> _sha;
        private Mock<IShaFactory> _shaFactory;
        private Mock<IDsaFfc> _dsa;
        private Mock<IDsaFfcFactory> _dsaFactory;
        private Mock<IKdfOneStepFactory> _kdfFactory;
        private Mock<IKeyConfirmationFactory> _keyConfirmationFactory;
        private Mock<INoKeyConfirmationFactory> _noKeyConfirmationFactory;
        private Mock<IOtherInfoFactory> _otherInfoFactory;
        private Mock<IEntropyProvider> _entropyProvider;
        private Mock<IDiffieHellman<FfcDomainParameters, FfcKeyPair>> _diffieHellmanFfc;
        private Mock<IMqv<FfcDomainParameters, FfcKeyPair>> _mqv;

        private FfcDomainParameters _mockDomainParameters;

        [SetUp]
        public void Setup()
        {
            _sha = new Mock<ISha>();
            _shaFactory = new Mock<IShaFactory>();
            _dsa = new Mock<IDsaFfc>();
            _dsaFactory = new Mock<IDsaFfcFactory>();
            _kdfFactory = new Mock<IKdfOneStepFactory>();
            _keyConfirmationFactory = new Mock<IKeyConfirmationFactory>();
            _noKeyConfirmationFactory = new Mock<INoKeyConfirmationFactory>();
            _otherInfoFactory = new Mock<IOtherInfoFactory>();
            _entropyProvider = new Mock<IEntropyProvider>();
            _diffieHellmanFfc = new Mock<IDiffieHellman<FfcDomainParameters, FfcKeyPair>>();
            _mqv = new Mock<IMqv<FfcDomainParameters, FfcKeyPair>>();

            _subject = new SchemeBuilderFfc(
                _dsaFactory.Object,
                _kdfFactory.Object,
                _keyConfirmationFactory.Object,
                _noKeyConfirmationFactory.Object,
                _otherInfoFactory.Object,
                _entropyProvider.Object,
                _diffieHellmanFfc.Object,
                _mqv.Object
            );

            _mockDomainParameters = new FfcDomainParameters(1, 2, 3);

            _sha
                .Setup(s => s.HashFunction)
                .Returns(new HashFunction(ModeValues.SHA2, DigestSizes.d224));
            _sha
                .Setup(s => s.HashMessage(It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(new HashResult(new BitString(1)));
            _shaFactory
                .Setup(s => s.GetShaInstance(It.IsAny<HashFunction>()))
                .Returns(_sha.Object);
            _dsa
                .Setup(s => s.Sha)
                .Returns(_sha.Object);
            _dsa
                .Setup(s => s.GenerateDomainParameters(It.IsAny<FfcDomainParametersGenerateRequest>()))
                .Returns(
                    new FfcDomainParametersGenerateResult(
                        _mockDomainParameters,
                        new DomainSeed(0, 1, 2),
                        new Counter(0)
                    )
                );
            _dsa
                .Setup(s => s.GenerateKeyPair(It.IsAny<FfcDomainParameters>()))
                .Returns(new FfcKeyPairGenerateResult(new FfcKeyPair(1, 2)));
            _dsaFactory
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(_dsa.Object);
        }

        [Test]
        public void ShouldReturnScheme()
        {
            var result = _subject
                .BuildScheme(
                    new SchemeParametersFfc(
                        new KasDsaAlgoAttributesFfc(FfcScheme.DhEphem, FfcParameterSet.Fb),
                        KeyAgreementRole.InitiatorPartyU,
                        KasMode.NoKdfNoKc,
                        KeyConfirmationRole.None,
                        KeyConfirmationDirection.None,
                        KasAssurance.None,
                        new BitString(1)
                    ),
                    null,
                    null
                );

            Assert.IsInstanceOf(typeof(IScheme<
                ISchemeParameters<KasDsaAlgoAttributesFfc>,
                KasDsaAlgoAttributesFfc,
                OtherPartySharedInformation<
                    FfcDomainParameters,
                    FfcKeyPair
                >,
                FfcDomainParameters,
                FfcKeyPair
            >), result);
        }

        [Test]
        public void ShouldUseDefaultImplementationOfDependencies()
        {
            var scheme = _subject
                .BuildScheme(
                    new SchemeParametersFfc(
                        new KasDsaAlgoAttributesFfc(FfcScheme.DhEphem, FfcParameterSet.Fb),
                        KeyAgreementRole.InitiatorPartyU,
                        KasMode.NoKdfNoKc,
                        KeyConfirmationRole.None,
                        KeyConfirmationDirection.None,
                        KasAssurance.None,
                        new BitString(1)
                    ),
                    null,
                    null
                );
            var result = scheme.ReturnPublicInfoThisParty();

            Assert.AreEqual(_mockDomainParameters.P, result.DomainParameters.P, nameof(_mockDomainParameters.P));
            Assert.AreEqual(_mockDomainParameters.Q, result.DomainParameters.Q, nameof(_mockDomainParameters.Q));
            Assert.AreEqual(_mockDomainParameters.G, result.DomainParameters.G, nameof(_mockDomainParameters.G));
        }

        [Test]
        public void ShouldUseOverriddenImplementationOfDependencies()
        {
            FfcDomainParameters newDomainParameters = new FfcDomainParameters(42, 43, 44);

            _dsa
                .Setup(s => s.Sha)
                .Returns(_sha.Object);
            _dsa
                .Setup(s => s.GenerateDomainParameters(It.IsAny<FfcDomainParametersGenerateRequest>()))
                .Returns(
                    new FfcDomainParametersGenerateResult(
                        newDomainParameters,
                        new DomainSeed(0, 1, 2),
                        new Counter(0)
                    )
                );
            _dsa
                .Setup(s => s.GenerateKeyPair(It.IsAny<FfcDomainParameters>()))
                .Returns(new FfcKeyPairGenerateResult(new FfcKeyPair(1, 2)));

            var scheme = _subject
                .WithHashFunction(new HashFunction(ModeValues.SHA2, DigestSizes.d256))
                .BuildScheme(
                    new SchemeParametersFfc(
                        new KasDsaAlgoAttributesFfc(FfcScheme.DhEphem, FfcParameterSet.Fb),
                        KeyAgreementRole.InitiatorPartyU,
                        KasMode.NoKdfNoKc,
                        KeyConfirmationRole.None,
                        KeyConfirmationDirection.None,
                        KasAssurance.None,
                        new BitString(1)
                    ),
                    null,
                    null
                );
            var result = scheme.ReturnPublicInfoThisParty();

            Assert.AreEqual(newDomainParameters.P, result.DomainParameters.P, nameof(newDomainParameters.P));
            Assert.AreEqual(newDomainParameters.Q, result.DomainParameters.Q, nameof(newDomainParameters.Q));
            Assert.AreEqual(newDomainParameters.G, result.DomainParameters.G, nameof(newDomainParameters.G));
        }

        [Test]
        [TestCase(FfcScheme.DhHybrid1, typeof(SchemeFfcDhHybrid1))]
        [TestCase(FfcScheme.Mqv2, typeof(SchemeFfcMqv2))]
        [TestCase(FfcScheme.DhEphem, typeof(SchemeFfcDiffieHellmanEphemeral))]
        [TestCase(FfcScheme.DhHybridOneFlow, typeof(SchemeFfcDhHybridOneFlow))]
        [TestCase(FfcScheme.Mqv1, typeof(SchemeFfcMqv1))]
        [TestCase(FfcScheme.DhOneFlow, typeof(SchemeFfcDhOneFlow))]
        public void ShouldReturnCorrectKasScheme(FfcScheme scheme, Type expectedType)
        {
            var result = _subject
                .WithHashFunction(new HashFunction(ModeValues.SHA2, DigestSizes.d256))
                .BuildScheme(
                    new SchemeParametersFfc(
                        new KasDsaAlgoAttributesFfc(scheme, FfcParameterSet.Fb),
                        KeyAgreementRole.InitiatorPartyU,
                        KasMode.NoKdfNoKc,
                        KeyConfirmationRole.None,
                        KeyConfirmationDirection.None,
                        KasAssurance.None,
                        new BitString(1)
                    ),
                    null,
                    null
                );

            Assert.IsInstanceOf(expectedType, result);
        }
    }
}
