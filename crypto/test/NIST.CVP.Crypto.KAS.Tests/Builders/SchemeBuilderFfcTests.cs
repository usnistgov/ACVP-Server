using Moq;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Builders.Ffc;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KES;
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
    [TestFixture,  FastCryptoTest]
    public class SchemeBuilderFfcTests
    {
        private SchemeBuilderFfc _subject;
        private Mock<ISha> _sha;
        private Mock<IShaFactory> _shaFactory;
        private Mock<IDsaFfc> _dsa;
        private Mock<IDsaFfcFactory> _dsaFactory;
        private Mock<IKdfFactory> _kdfFactory;
        private Mock<IKeyConfirmationFactory> _keyConfirmationFactory;
        private Mock<INoKeyConfirmationFactory> _noKeyConfirmationFactory;
        private Mock<IOtherInfoFactory<OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair>> _otherInfoFactory;
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
            _kdfFactory = new Mock<IKdfFactory>();
            _keyConfirmationFactory = new Mock<IKeyConfirmationFactory>();
            _noKeyConfirmationFactory = new Mock<INoKeyConfirmationFactory>();
            _otherInfoFactory = new Mock<IOtherInfoFactory<OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair>>();
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
                .Setup(s => s.HashMessage(It.IsAny<BitString>()))
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

            Assert.IsInstanceOf(typeof(IScheme<
                ISchemeParameters<
                    FfcParameterSet, 
                    FfcScheme
                >, 
                FfcParameterSet, 
                FfcScheme, 
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
    }
}