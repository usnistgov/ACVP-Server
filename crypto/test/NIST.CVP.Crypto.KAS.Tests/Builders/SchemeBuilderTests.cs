using Moq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using DigestSizes = NIST.CVP.Crypto.SHAWrapper.DigestSizes;
using HashFunction = NIST.CVP.Crypto.SHAWrapper.HashFunction;
using HashResult = NIST.CVP.Crypto.SHAWrapper.HashResult;
using ModeValues = NIST.CVP.Crypto.SHAWrapper.ModeValues;

namespace NIST.CVP.Crypto.KAS.Tests.Builders
{
    [TestFixture, UnitTest]
    public class SchemeBuilderTests
    {
        private SchemeBuilder _subject;
        private Mock<ISha> _sha;
        private Mock<IDsaFfc> _dsa;
        private Mock<IKdfFactory> _kdfFactory;
        private Mock<IKeyConfirmationFactory> _keyConfirmationFactory;
        private Mock<INoKeyConfirmationFactory> _noKeyConfirmationFactory;
        private Mock<IOtherInfoFactory> _otherInfoFactory;
        private Mock<IEntropyProvider> _entropyProvider;

        private FfcDomainParameters _mockDomainParameters;

        [SetUp]
        public void Setup()
        {
            _sha = new Mock<ISha>();
            _dsa = new Mock<IDsaFfc>();
            _kdfFactory = new Mock<IKdfFactory>();
            _keyConfirmationFactory = new Mock<IKeyConfirmationFactory>();
            _noKeyConfirmationFactory = new Mock<INoKeyConfirmationFactory>();
            _otherInfoFactory = new Mock<IOtherInfoFactory>();
            _entropyProvider = new Mock<IEntropyProvider>();

            _subject = new SchemeBuilder(
                _dsa.Object, 
                _kdfFactory.Object, 
                _keyConfirmationFactory.Object, 
                _noKeyConfirmationFactory.Object, 
                _otherInfoFactory.Object, 
                _entropyProvider.Object
            );

            _mockDomainParameters = new FfcDomainParameters(1, 2, 3);

            _sha
                .Setup(s => s.HashFunction)
                .Returns(new HashFunction(ModeValues.SHA2, DigestSizes.d224));
            _sha
                .Setup(s => s.HashMessage(It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString(1)));
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
        }

        [Test]
        public void ShouldReturnScheme()
        {
            var result = _subject
                .BuildScheme(
                    new SchemeParameters(
                        KeyAgreementRole.InitiatorPartyU,
                        KasMode.NoKdfNoKc,
                        FfcScheme.DhEphem,
                        KeyConfirmationRole.None,
                        KeyConfirmationDirection.None,
                        FfcParameterSet.Fb,
                        KasAssurance.None,
                        new BitString(1)
                    ),
                    null,
                    null
                );

            Assert.IsInstanceOf(typeof(IScheme), result);
        }

        [Test]
        public void ShouldUseDefaultImplementationOfDependencies()
        {
            var scheme = _subject
                .BuildScheme(
                    new SchemeParameters(
                        KeyAgreementRole.InitiatorPartyU,
                        KasMode.NoKdfNoKc,
                        FfcScheme.DhEphem,
                        KeyConfirmationRole.None,
                        KeyConfirmationDirection.None,
                        FfcParameterSet.Fb,
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

            Mock<IDsaFfc> overrideDsa = new Mock<IDsaFfc>();
            overrideDsa
                .Setup(s => s.Sha)
                .Returns(_sha.Object);
            overrideDsa
                .Setup(s => s.GenerateDomainParameters(It.IsAny<FfcDomainParametersGenerateRequest>()))
                .Returns(
                    new FfcDomainParametersGenerateResult(
                        newDomainParameters,
                        new DomainSeed(0, 1, 2),
                        new Counter(0)
                    )
                );
            overrideDsa
                .Setup(s => s.GenerateKeyPair(It.IsAny<FfcDomainParameters>()))
                .Returns(new FfcKeyPairGenerateResult(new FfcKeyPair(1, 2)));

            var scheme = _subject
                .WithDsa(overrideDsa.Object)
                .BuildScheme(
                    new SchemeParameters(
                        KeyAgreementRole.InitiatorPartyU,
                        KasMode.NoKdfNoKc,
                        FfcScheme.DhEphem,
                        KeyConfirmationRole.None,
                        KeyConfirmationDirection.None,
                        FfcParameterSet.Fb,
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
        public void ShouldRevertToDefaultDependenciesAfterBuild()
        {
            FfcDomainParameters newDomainParameters = new FfcDomainParameters(42, 43, 44);

            Mock<IDsaFfc> overrideDsa = new Mock<IDsaFfc>();
            overrideDsa
                .Setup(s => s.Sha)
                .Returns(_sha.Object);
            overrideDsa
                .Setup(s => s.GenerateDomainParameters(It.IsAny<FfcDomainParametersGenerateRequest>()))
                .Returns(
                    new FfcDomainParametersGenerateResult(
                        newDomainParameters,
                        new DomainSeed(0, 1, 2),
                        new Counter(0)
                    )
                );
            overrideDsa
                .Setup(s => s.GenerateKeyPair(It.IsAny<FfcDomainParameters>()))
                .Returns(new FfcKeyPairGenerateResult(new FfcKeyPair(1, 2)));

            SchemeParameters sp = new SchemeParameters(
                KeyAgreementRole.InitiatorPartyU,
                KasMode.NoKdfNoKc,
                FfcScheme.DhEphem,
                KeyConfirmationRole.None,
                KeyConfirmationDirection.None,
                FfcParameterSet.Fb,
                KasAssurance.None,
                new BitString(1)
            );

            var scheme = _subject
                .WithDsa(overrideDsa.Object)
                .BuildScheme(
                    sp,
                    null,
                    null
                );
            scheme.ReturnPublicInfoThisParty();

            var secondScheme = _subject
                .BuildScheme(
                    sp,
                    null,
                    null
                );
            var result = secondScheme.ReturnPublicInfoThisParty();

            Assert.AreEqual(_mockDomainParameters.P, result.DomainParameters.P, nameof(_mockDomainParameters.P));
            Assert.AreEqual(_mockDomainParameters.Q, result.DomainParameters.Q, nameof(_mockDomainParameters.Q));
            Assert.AreEqual(_mockDomainParameters.G, result.DomainParameters.G, nameof(_mockDomainParameters.G));
        }

        [Test]
        public void ShouldKeepOverridenDependenciesAfterBuildWhenSpecified()
        {
            FfcDomainParameters newDomainParameters = new FfcDomainParameters(42, 43, 44);

            Mock<IDsaFfc> overrideDsa = new Mock<IDsaFfc>();
            overrideDsa
                .Setup(s => s.Sha)
                .Returns(_sha.Object);
            overrideDsa
                .Setup(s => s.GenerateDomainParameters(It.IsAny<FfcDomainParametersGenerateRequest>()))
                .Returns(
                    new FfcDomainParametersGenerateResult(
                        newDomainParameters,
                        new DomainSeed(0, 1, 2),
                        new Counter(0)
                    )
                );
            overrideDsa
                .Setup(s => s.GenerateKeyPair(It.IsAny<FfcDomainParameters>()))
                .Returns(new FfcKeyPairGenerateResult(new FfcKeyPair(1, 2)));

            SchemeParameters sp = new SchemeParameters(
                KeyAgreementRole.InitiatorPartyU,
                KasMode.NoKdfNoKc,
                FfcScheme.DhEphem,
                KeyConfirmationRole.None,
                KeyConfirmationDirection.None,
                FfcParameterSet.Fb,
                KasAssurance.None,
                new BitString(1)
            );

            var scheme = _subject
                .WithDsa(overrideDsa.Object)
                .BuildScheme(
                    sp,
                    null,
                    null,
                    false
                );
            scheme.ReturnPublicInfoThisParty();

            var secondScheme = _subject
                .BuildScheme(
                    sp,
                    null,
                    null,
                    false
                );
            var result = secondScheme.ReturnPublicInfoThisParty();

            Assert.AreEqual(newDomainParameters.P, result.DomainParameters.P, nameof(newDomainParameters.P));
            Assert.AreEqual(newDomainParameters.Q, result.DomainParameters.Q, nameof(newDomainParameters.Q));
            Assert.AreEqual(newDomainParameters.G, result.DomainParameters.G, nameof(newDomainParameters.G));
        }
    }
}