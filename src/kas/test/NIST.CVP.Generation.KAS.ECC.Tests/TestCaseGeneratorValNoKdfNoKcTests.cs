using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.ECC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorValNoKdfNoKcTests
    {
        // TODO these tests need to be modified and moved to either shim testing, or cluster testing

        //// Note using sha2-256 in tests
        //private readonly HashFunction _hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d256);
        //private readonly IEccCurveFactory _curveFactory = new EccCurveFactory();
        //private readonly Mock<IDsaEcc> _dsa = new Mock<IDsaEcc>();
        //private readonly Mock<IDsaEccFactory> _dsaFactory = new Mock<IDsaEccFactory>();

        //private TestCaseGeneratorValNoKdfNoKc _subject;
        //private IEntropyProvider _entropyProvider;
        //private IEntropyProviderFactory _entropyProviderFactory;
        //private IKasBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> _kasBuilder;
        //private ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> _schemeBuilder;
        //private IShaFactory _shaFactory;
        //private IMacParametersBuilder _macParametersBuilder;
        //private IKeyConfirmationFactory _keyConfirmationFactory;
        //private INoKeyConfirmationFactory _noKeyConfirmationFactory;
        //private IKdfFactory _kdfFactory;

        //[SetUp]
        //public void Setup()
        //{
        //    _shaFactory = new ShaFactory();

        //    _dsaFactory
        //        .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), EntropyProviderTypes.Random))
        //        .Returns(_dsa.Object);

        //    _dsa
        //        .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
        //        .Returns(
        //            new EccKeyPairGenerateResult(
        //                new EccKeyPair(
        //                    new EccPoint(
        //                        new BitString("603713dbf1f28d8b2a8d4550b27a49e95e7d206a29a428623d97486d").ToPositiveBigInteger(),
        //                        new BitString("45d671536d32b85da3510b87873ed881eddc7c3573f603d9e71d9319").ToPositiveBigInteger()
        //                    ),
        //                    new BitString("4962ed36f9ec3f873a11274a8e75e126bbba5a08f82ba1fb516aed7d").ToPositiveBigInteger()
        //                )
        //            )
        //        );
        //    _dsa
        //        .SetupGet(s => s.Sha)
        //        .Returns(
        //            _shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256))
        //        );

        //    _entropyProvider = new EntropyProvider(new Random800_90());
        //    _entropyProviderFactory = new EntropyProviderFactory();
        //    _schemeBuilder = new SchemeBuilderEcc(
        //            _dsaFactory.Object,
        //            _curveFactory,
        //            new KdfFactory(
        //                new ShaFactory()
        //            ),
        //            new KeyConfirmationFactory(),
        //            new NoKeyConfirmationFactory(),
        //            new OtherInfoFactory(_entropyProvider),
        //            _entropyProvider,
        //            new DiffieHellmanEcc(),
        //            new MqvEcc()
        //    );
        //    _kasBuilder = new KasBuilderEcc(_schemeBuilder);

        //    _macParametersBuilder = new MacParametersBuilder();
        //    _keyConfirmationFactory = new KeyConfirmationFactory();
        //    _noKeyConfirmationFactory = new NoKeyConfirmationFactory();
        //    _kdfFactory = new KdfFactory(_shaFactory);

        //    _subject = new TestCaseGeneratorValNoKdfNoKc(
        //        _curveFactory, _kasBuilder, _schemeBuilder, _shaFactory, _entropyProviderFactory, _macParametersBuilder, _kdfFactory, _keyConfirmationFactory, _noKeyConfirmationFactory, new List<TestCaseDispositionOption>() {TestCaseDispositionOption.Success}
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
        //    BuildTestGroup(scheme, testGroupIutRole, out var iutKeyGenRequirements, out var serverKeyGenRequirements, out var resultTestCase);

        //    Assert.IsFalse(resultTestCase.Deferred, nameof(resultTestCase.Deferred));
        //    #region KeyCheck
        //    // Server checks
        //    if (serverKeyGenRequirements.GeneratesStaticKeyPair)
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

        //    if (serverKeyGenRequirements.GeneratesEphemeralKeyPair)
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

        //    // Iut checks
        //    if (iutKeyGenRequirements.GeneratesStaticKeyPair)
        //    {
        //        Assert.IsTrue(resultTestCase.StaticPrivateKeyIut != 0,
        //            nameof(resultTestCase.StaticPrivateKeyIut));
        //        Assert.IsTrue(resultTestCase.StaticPublicKeyIutX != 0,
        //            nameof(resultTestCase.StaticPublicKeyIutX));
        //        Assert.IsTrue(resultTestCase.StaticPublicKeyIutY != 0,
        //            nameof(resultTestCase.StaticPublicKeyIutY));
        //    }
        //    else
        //    {
        //        Assert.IsTrue(resultTestCase.StaticPrivateKeyIut == 0,
        //            nameof(resultTestCase.StaticPrivateKeyIut));
        //        Assert.IsTrue(resultTestCase.StaticPublicKeyIutX == 0,
        //            nameof(resultTestCase.StaticPublicKeyIutX));
        //        Assert.IsTrue(resultTestCase.StaticPublicKeyIutY == 0,
        //            nameof(resultTestCase.StaticPublicKeyIutY));
        //    }

        //    if (iutKeyGenRequirements.GeneratesEphemeralKeyPair)
        //    {
        //        Assert.IsTrue(resultTestCase.EphemeralPrivateKeyIut != 0,
        //            nameof(resultTestCase.EphemeralPrivateKeyIut));
        //        Assert.IsTrue(resultTestCase.EphemeralPublicKeyIutX != 0,
        //            nameof(resultTestCase.EphemeralPublicKeyIutX));
        //        Assert.IsTrue(resultTestCase.EphemeralPublicKeyIutY != 0,
        //            nameof(resultTestCase.EphemeralPublicKeyIutY));
        //    }
        //    else
        //    {
        //        Assert.IsTrue(resultTestCase.EphemeralPrivateKeyIut == 0,
        //            nameof(resultTestCase.EphemeralPrivateKeyIut));
        //        Assert.IsTrue(resultTestCase.EphemeralPublicKeyIutX == 0,
        //            nameof(resultTestCase.EphemeralPublicKeyIutX));
        //        Assert.IsTrue(resultTestCase.EphemeralPublicKeyIutY == 0,
        //            nameof(resultTestCase.EphemeralPublicKeyIutY));
        //    }
        //    #endregion KeyCheck
        //}

        //[Test]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailChangedZ, true)]

        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailChangedZ, true)]

        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailChangedZ, true)]

        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // server doesn't generate ephem key when party v
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailChangedZ, true)]

        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // server doesn't generate ephem key when party v
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailChangedZ, true)]

        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // server doesn't generate ephem key when party v
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailChangedZ, true)]

        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // server doesn't generate ephem key when party v
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailChangedZ, true)]
        //public void ShouldSetProperTestCaseFailureTestProperty(EccScheme scheme, KeyAgreementRole testGroupIutRole,
        //    TestCaseDispositionOption option, bool isFailure)
        //{
        //    _subject = new TestCaseGeneratorValNoKdfNoKc(_curveFactory, _kasBuilder, _schemeBuilder, _shaFactory, _entropyProviderFactory, _macParametersBuilder, _kdfFactory, _keyConfirmationFactory, _noKeyConfirmationFactory, new List<TestCaseDispositionOption>() { option });

        //    BuildTestGroup(scheme, testGroupIutRole, out var iutKeyGenRequirements, out var serverKeyGenRequirements, out var resultTestCase);

        //    Assert.AreEqual(isFailure, !resultTestCase.TestPassed);
        //}

        //private void BuildTestGroup(
        //    EccScheme scheme, 
        //    KeyAgreementRole testGroupIutRole, 
        //    out SchemeKeyNonceGenRequirement<EccScheme> iutKeyGenRequirements, 
        //    out SchemeKeyNonceGenRequirement<EccScheme> serverKeyGenRequirements, 
        //    out TestCase resultTestCase
        //)
        //{
        //    TestGroup tg = new TestGroup()
        //    {
        //        KasMode = KasMode.NoKdfNoKc,
        //        Scheme = scheme,
        //        KasRole = testGroupIutRole,
        //        Function = KasAssurance.None,
        //        HashAlg = _hashFunction,
        //        KcRole = KeyConfirmationRole.None,
        //        KcType = KeyConfirmationDirection.None,
        //        MacType = KeyAgreementMacType.None,
        //        TestType = "VAL",
        //        Curve = Curve.P224
        //    };

        //    KeyAgreementRole serverRole =
        //        KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(testGroupIutRole);
        //    KeyConfirmationRole serverKeyConfRole =
        //        KeyGenerationRequirementsHelper.GetOtherPartyKeyConfirmationRole(tg.KcRole);

        //    iutKeyGenRequirements = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
        //        scheme,
        //        tg.KasMode,
        //        tg.KasRole,
        //        tg.KcRole,
        //        tg.KcType
        //    );
        //    serverKeyGenRequirements = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
        //        scheme,
        //        tg.KasMode,
        //        serverRole,
        //        serverKeyConfRole,
        //        tg.KcType
        //    );
        //    var result = _subject.Generate(tg, false);
        //    resultTestCase = (TestCase)result.TestCase;
        //}
    }
}