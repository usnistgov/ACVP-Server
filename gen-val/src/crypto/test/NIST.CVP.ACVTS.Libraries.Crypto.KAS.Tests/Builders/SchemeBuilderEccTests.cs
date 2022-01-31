using System;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
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
using NIST.CVP.ACVTS.Libraries.Crypto.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders.Ecc;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Scheme.Ecc;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Tests.Builders
{
    [TestFixture, FastCryptoTest]
    public class SchemeBuilderEccTests
    {
        private readonly IEccCurveFactory _curveFactory = new EccCurveFactory();

        private SchemeBuilderEcc _subject;

        private Curve _curve;
        private Mock<ISha> _sha;
        private Mock<IShaFactory> _shaFactory;
        private Mock<IDsaEcc> _dsa;
        private Mock<IDsaEccFactory> _dsaFactory;
        private Mock<IKdfOneStepFactory> _kdfFactory;
        private Mock<IKeyConfirmationFactory> _keyConfirmationFactory;
        private Mock<INoKeyConfirmationFactory> _noKeyConfirmationFactory;
        private Mock<IOtherInfoFactory> _otherInfoFactory;
        private Mock<IEntropyProvider> _entropyProvider;
        private Mock<IDiffieHellman<EccDomainParameters, EccKeyPair>> _diffieHellmanEcc;
        private Mock<IMqv<EccDomainParameters, EccKeyPair>> _mqv;

        private EccDomainParameters _mockDomainParameters;

        [SetUp]
        public void Setup()
        {
            _sha = new Mock<ISha>();
            _shaFactory = new Mock<IShaFactory>();
            _dsa = new Mock<IDsaEcc>();
            _dsaFactory = new Mock<IDsaEccFactory>();
            _kdfFactory = new Mock<IKdfOneStepFactory>();
            _keyConfirmationFactory = new Mock<IKeyConfirmationFactory>();
            _noKeyConfirmationFactory = new Mock<INoKeyConfirmationFactory>();
            _otherInfoFactory = new Mock<IOtherInfoFactory>();
            _entropyProvider = new Mock<IEntropyProvider>();
            _diffieHellmanEcc = new Mock<IDiffieHellman<EccDomainParameters, EccKeyPair>>();
            _mqv = new Mock<IMqv<EccDomainParameters, EccKeyPair>>();

            _subject = new SchemeBuilderEcc(
                _dsaFactory.Object,
                _curveFactory,
                _kdfFactory.Object,
                _keyConfirmationFactory.Object,
                _noKeyConfirmationFactory.Object,
                _otherInfoFactory.Object,
                _entropyProvider.Object,
                _diffieHellmanEcc.Object,
                _mqv.Object
            );

            _curve = Curve.B163;
            _mockDomainParameters = new EccDomainParameters(_curveFactory.GetCurve(_curve));

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
                .Setup(s => s.GenerateDomainParameters(It.IsAny<EccDomainParametersGenerateRequest>()))
                .Returns(
                    new EccDomainParametersGenerateResult()
                );
            _dsa
                .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
                .Returns(new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 2), 2)));
            _dsaFactory
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(_dsa.Object);
        }

        [Test]
        public void ShouldReturnScheme()
        {
            var result = _subject
                .BuildScheme(
                    new SchemeParametersEcc(
                        new KasDsaAlgoAttributesEcc(EccScheme.EphemeralUnified, EccParameterSet.Eb, _curve),
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
                ISchemeParameters<KasDsaAlgoAttributesEcc>,
                KasDsaAlgoAttributesEcc,
                OtherPartySharedInformation<
                    EccDomainParameters,
                    EccKeyPair
                >,
                EccDomainParameters,
                EccKeyPair
            >), result);
        }

        [Test]
        [TestCase(EccScheme.EphemeralUnified, typeof(SchemeEccEphemeralUnified))]
        [TestCase(EccScheme.OnePassMqv, typeof(SchemeEccOnePassMqv))]
        public void ShouldReturnCorrectKasScheme(EccScheme scheme, Type expectedType)
        {
            var result = _subject
                .WithHashFunction(new HashFunction(ModeValues.SHA2, DigestSizes.d256))
                .BuildScheme(
                    new SchemeParametersEcc(
                        new KasDsaAlgoAttributesEcc(scheme, EccParameterSet.Eb, _curve),
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
