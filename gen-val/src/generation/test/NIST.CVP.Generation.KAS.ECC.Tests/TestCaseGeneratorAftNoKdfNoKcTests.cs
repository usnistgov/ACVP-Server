﻿using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.ECC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorAftNoKdfNoKcTests
    {
        // TODO these tests need to be modified and moved to either shim testing, or cluster testing

        //private readonly EntropyProviderFactory _entropyProviderFactory = new EntropyProviderFactory();
        //private readonly MacParametersBuilder _macParametersBuilder = new MacParametersBuilder();
        //private readonly IEccCurveFactory _curveFactory = new EccCurveFactory();

        //private TestCaseGeneratorAftNoKdfNoKc _subject;
        //private Mock<IDsaEcc> _dsa;
        //private IEntropyProvider _entropyProvider;
        //private IKasBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> _kasBuilder;
        //private ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> _schemeBuilder;
        //private Mock<IDsaEccFactory> _dsaFactory;

        //[SetUp]
        //public void Setup()
        //{
        //    _dsa = new Mock<IDsaEcc>();
        //    _dsa
        //        .Setup(s => s.GenerateDomainParameters(It.IsAny<EccDomainParametersGenerateRequest>()))
        //        .Returns(
        //            new EccDomainParametersGenerateResult()
        //        );
        //    _dsa
        //        .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
        //        .Returns(
        //            new EccKeyPairGenerateResult(
        //                new EccKeyPair(new EccPoint(1, 2), 3)
        //            )
        //        );

        //    _dsaFactory = new Mock<IDsaEccFactory>();
        //    _dsaFactory
        //        .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
        //        .Returns(_dsa.Object);

        //    _entropyProvider = new EntropyProvider(new Random800_90());
        //    _schemeBuilder = new SchemeBuilderEcc(
        //        _dsaFactory.Object,
        //        new EccCurveFactory(),
        //        new KdfFactory(
        //            new ShaFactory()
        //        ),
        //        new KeyConfirmationFactory(),
        //        new NoKeyConfirmationFactory(),
        //        new OtherInfoFactory(_entropyProvider),
        //        _entropyProvider,
        //        new DiffieHellmanEcc(),
        //        new MqvEcc()
        //    );
        //    _kasBuilder = new KasBuilderEcc(_schemeBuilder);


        //    _subject = new TestCaseGeneratorAftNoKdfNoKc(
        //        _curveFactory, _kasBuilder, _schemeBuilder, _entropyProviderFactory, _macParametersBuilder
        //    );
        //}

        //[Test]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV)]
        //public void ShouldPopulateCorrectKeysNoncesForSchemeRole(EccScheme scheme, KeyAgreementRole testGroupIutRole)
        //{
        //    TestGroup tg = new TestGroup()
        //    {
        //        KasMode = KasMode.KdfNoKc,
        //        Scheme = scheme,
        //        KasRole = testGroupIutRole,
        //        Function = KasAssurance.None,
        //        HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
        //        KcRole = KeyConfirmationRole.None,
        //        KcType = KeyConfirmationDirection.None,
        //        MacType = KeyAgreementMacType.None,
        //        TestType = "VAL"
        //    };

        //    KeyAgreementRole serverRole =
        //        KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(testGroupIutRole);
        //    KeyConfirmationRole serverKeyConfRole =
        //        KeyGenerationRequirementsHelper.GetOtherPartyKeyConfirmationRole(tg.KcRole);

        //    var keyGenRequirements = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
        //        scheme,
        //        tg.KasMode,
        //        serverRole,
        //        serverKeyConfRole,
        //        tg.KcType
        //    );

        //    var result = _subject.Generate(tg, false);
        //    var resultTestCase = (TestCase) result.TestCase;

        //    Assert.IsTrue(resultTestCase.Deferred, nameof(resultTestCase.Deferred));
        //    if (keyGenRequirements.GeneratesStaticKeyPair)
        //    {
        //        Assert.IsTrue(resultTestCase.StaticPrivateKeyServer != 0,
        //            nameof(resultTestCase.StaticPrivateKeyServer));
        //        Assert.IsTrue(resultTestCase.StaticPublicKeyServerX != 0,
        //            nameof(resultTestCase.StaticPublicKeyServerX));
        //        Assert.IsTrue(resultTestCase.StaticPublicKeyServerY != 0,
        //            nameof(resultTestCase.StaticPublicKeyServerY));
        //    }
        //    else
        //    {
        //        Assert.IsTrue(resultTestCase.StaticPrivateKeyServer == 0,
        //            nameof(resultTestCase.StaticPrivateKeyServer));
        //        Assert.IsTrue(resultTestCase.StaticPublicKeyServerX == 0,
        //            nameof(resultTestCase.StaticPublicKeyServerX));
        //        Assert.IsTrue(resultTestCase.StaticPublicKeyServerY == 0,
        //            nameof(resultTestCase.StaticPublicKeyServerY));
        //    }

        //    if (keyGenRequirements.GeneratesEphemeralKeyPair)
        //    {
        //        Assert.IsTrue(resultTestCase.EphemeralPrivateKeyServer != 0,
        //            nameof(resultTestCase.EphemeralPrivateKeyServer));
        //        Assert.IsTrue(resultTestCase.EphemeralPublicKeyServerX != 0,
        //            nameof(resultTestCase.EphemeralPublicKeyServerX));
        //        Assert.IsTrue(resultTestCase.EphemeralPublicKeyServerY != 0,
        //            nameof(resultTestCase.EphemeralPublicKeyServerY));
        //    }
        //    else
        //    {
        //        Assert.IsTrue(resultTestCase.EphemeralPrivateKeyServer == 0,
        //            nameof(resultTestCase.EphemeralPrivateKeyServer));
        //        Assert.IsTrue(resultTestCase.EphemeralPublicKeyServerX == 0,
        //            nameof(resultTestCase.EphemeralPublicKeyServerX));
        //        Assert.IsTrue(resultTestCase.EphemeralPublicKeyServerY == 0,
        //            nameof(resultTestCase.EphemeralPublicKeyServerY));
        //    }
        //}
    }
}