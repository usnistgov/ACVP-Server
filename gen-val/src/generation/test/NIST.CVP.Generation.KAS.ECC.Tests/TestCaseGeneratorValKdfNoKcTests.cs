﻿using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.ECC.Tests
{
    /// <summary>
    /// Note using sha2-256 in tests using real DSA in order to get valid behavior out of key generators.
    /// Real DSA behavior is needed for ensuring assurance failures proceed as expected.
    /// </summary>
    [TestFixture, UnitTest]
    public class TestCaseGeneratorValKdfNoKcTests
    {
        // TODO these tests need to be modified and moved to either shim testing, or cluster testing

        //private readonly HashFunction _hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d256);
        //private readonly IEccCurveFactory _curveFactory = new EccCurveFactory();
        //private readonly Mock<IDsaEcc> _dsa = new Mock<IDsaEcc>();
        //private readonly Mock<IDsaEccFactory> _dsaFactory = new Mock<IDsaEccFactory>();

        //private TestCaseGeneratorValKdfNoKc _subject;
        //private IEntropyProvider _entropyProvider;
        //private IEntropyProviderFactory _entropyProviderFactory;
        //private IKasBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> _kasBuilder;
        //private ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> _schemeBuilder;
        //private IMacParametersBuilder _macParametersBuilder;
        //private INoKeyConfirmationFactory _noKeyConfirmationFactory;
        //private IShaFactory _shaFactory;
        //private IKdfFactory _kdfFactory;
        //private IKeyConfirmationFactory _keyConfirmationFactory;
        
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
        //    _kdfFactory = new FakeKdfFactory_BadDkm(new KdfFactory(_shaFactory));

        //    _subject = new TestCaseGeneratorValKdfNoKc(
        //        _curveFactory,
        //        _kasBuilder, 
        //        _schemeBuilder, 
        //        _shaFactory, 
        //        _entropyProviderFactory, 
        //        _macParametersBuilder, 
        //        _kdfFactory, 
        //        _keyConfirmationFactory,
        //        _noKeyConfirmationFactory, 
        //        new List<TestCaseDispositionOption>() {TestCaseDispositionOption.Success}
        //    );
        //}

        //[Test]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //public void ShouldPopulateCorrectKeysNoncesForSchemeRole(EccScheme scheme, KeyAgreementRole testGroupIutRole, KeyAgreementMacType macType)
        //{
        //    BuildTestGroup(scheme, testGroupIutRole, macType, out var iutKeyGenRequirements, out var serverKeyGenRequirements, out var resultTestCase);

        //    Assert.IsFalse(resultTestCase.Deferred, nameof(resultTestCase.Deferred));
        //    Assert.IsTrue(resultTestCase.NonceNoKc.BitLength != 0, nameof(resultTestCase.NonceNoKc));

        //    if (macType == KeyAgreementMacType.AesCcm)
        //    {
        //        Assert.IsTrue(resultTestCase.NonceAesCcm.BitLength != 0, nameof(resultTestCase.NonceAesCcm));
        //    }
        //    else
        //    {
        //        Assert.IsNull(resultTestCase.NonceAesCcm, nameof(resultTestCase.NonceAesCcm));
        //    }

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

        //    if (serverKeyGenRequirements.GeneratesDkmNonce)
        //    {
        //        Assert.IsTrue(resultTestCase.DkmNonceServer != null,
        //            nameof(resultTestCase.DkmNonceServer));
        //    }
        //    else
        //    {
        //        Assert.IsTrue(resultTestCase.DkmNonceServer == null,
        //            nameof(resultTestCase.DkmNonceServer));
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

        //    if (iutKeyGenRequirements.GeneratesDkmNonce)
        //    {
        //        Assert.IsTrue(resultTestCase.DkmNonceIut != null,
        //            nameof(resultTestCase.DkmNonceIut));
        //    }
        //    else
        //    {
        //        Assert.IsTrue(resultTestCase.DkmNonceIut == null,
        //            nameof(resultTestCase.DkmNonceIut));
        //    }
        //    #endregion KeyCheck
        //}
        
        //[Test]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        //public void ShouldSetProperTestCaseFailureTestProperty(EccScheme scheme, KeyAgreementRole testGroupIutRole,
        //    KeyAgreementMacType macType, TestCaseDispositionOption option, bool isFailure)
        //{
        //    _subject = new TestCaseGeneratorValKdfNoKc(
        //        _curveFactory,
        //        _kasBuilder,
        //        _schemeBuilder,
        //        _shaFactory,
        //        _entropyProviderFactory,
        //        _macParametersBuilder,
        //        _kdfFactory,
        //        _keyConfirmationFactory,
        //        _noKeyConfirmationFactory,
        //        new List<TestCaseDispositionOption>() {option}
        //    );

        //    BuildTestGroup(scheme, testGroupIutRole, macType, out var iutKeyGenRequirements, out var serverKeyGenRequirements, out var resultTestCase);

        //    Assert.AreEqual(isFailure, !resultTestCase.TestPassed);
        //}

        //private void BuildTestGroup(
        //    EccScheme scheme, 
        //    KeyAgreementRole testGroupIutRole, 
        //    KeyAgreementMacType macType, 
        //    out SchemeKeyNonceGenRequirement<EccScheme> iutKeyGenRequirements, 
        //    out SchemeKeyNonceGenRequirement<EccScheme> serverKeyGenRequirements, 
        //    out TestCase resultTestCase)
        //{
        //    TestGroup tg = new TestGroup()
        //    {
        //        KasMode = KasMode.KdfNoKc,
        //        Scheme = scheme,
        //        KasRole = testGroupIutRole,
        //        Function = KasAssurance.None,
        //        HashAlg = _hashFunction,
        //        KcRole = KeyConfirmationRole.None,
        //        KcType = KeyConfirmationDirection.None,
        //        MacType = macType,
        //        TestType = "VAL",
        //        Curve = Curve.P224,
        //        KeyLen = 128,
        //        MacLen = 128,
        //        KdfType = "concatenation",
        //        OiPattern = "uPartyInfo||vPartyInfo",
        //        IdIut = new BitString("a1b2c3d4e5"),
        //        IdServer = new BitString("434156536964")
        //    };

        //    tg.IdIutLen = tg.IdIut.BitLength;
        //    tg.IdServerLen = tg.IdServer.BitLength;

        //    if (macType == KeyAgreementMacType.AesCcm)
        //    {
        //        tg.AesCcmNonceLen = 104;
        //    }

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