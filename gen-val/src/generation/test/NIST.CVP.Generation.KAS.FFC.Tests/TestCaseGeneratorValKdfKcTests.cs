﻿using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.FFC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorValKdfKcTests
    {
        // TODO update tests to go along with DI shim, or cluster

        //private readonly HashFunction _hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d256);
        //private readonly FfcDomainParameters _domainParameters = new FfcDomainParameters(
        //    new BitString("d5cf9ba288ff8438650904a5fe2eeb8bf6b52b691a455b21bd3b37998f82544036ae61fe436039c66feab83ba21a5ba13e7b4c1692ac82c65309eed54e593efe9831cc82bfd11e9552d4eb7d8f2c233024bc10819c57093890ec19aa9ee915e524d81c285928a5b87acc7f496f93689ed59b15183689ec5e487fdc9fb994c4bf7e1dba57f8f12c17e404f68b5506d69f4aa98146f6ba3be1f3397e5d41c4235a18b38c75417bb9091a487db1fc89eb2f73b4ac6f6c12dc5634a342cd9d10605090bd544e1c0813aaaa3814166df84a302b8df38babe9c31e4a2c64317046bdb841fa8717e22dcf1496f9ad94c17f1ff9175567d02ab5e30601e2e5b0884e9a77").ToPositiveBigInteger(),
        //    new BitString("ceb9916bbc14cdc9dda80481135bee68ee94f4ecadc2921261a316d1c9cf9283").ToPositiveBigInteger(),
        //    new BitString("37a7b2a094ed6253784d51c71a4a7407f0727df15e480a02b0ff2f9befdb0e92c0d63e482c6909e5337373abe347ceaec25a9f2a23f1770c2447bfb5c35dbda80d3f00d8b6569a0165a3d52f1cff03e02b67be47b2f26ebd0fc13299fd0317719c3bcbbcbba0a982915b5d68fb4c5c483c3df12052b56ceacd16176c783d56422b1366cd5ce65922b734a1780acf35b4a658cfe8166469ace04b87a33e1e8d4603ae3f9607d1e708137d581aaf2cd86608ca06e52cddbe475f8bdb4597cd5b55f47380276c86d08d8ddbbef4ee3ca76cefe6ff9c74d4d5d1be3407e30720be34cf41f482be8bc6ed6e89ea7cc2d164da1dc1c9d0e2762eef983dc9e96f3bfd50").ToPositiveBigInteger()
        //);

        //private TestCaseGeneratorValKdfKc _subject;
        //private IEntropyProvider _entropyProvider;
        //private IEntropyProviderFactory _entropyProviderFactory;
        //private IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> _kasBuilder;
        //private ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> _schemeBuilder;
        //private IMacParametersBuilder _macParametersBuilder;
        //private IKeyConfirmationFactory _keyConfirmationFactory;
        //private INoKeyConfirmationFactory _noKeyConfirmationFactory;
        //private IShaFactory _shaFactory;
        //private Mock<IDsaFfc> _dsa;
        //private IKdfFactory _kdfFactory;
        //private Mock<IDsaFfcFactory> _dsaFactory;

        //[SetUp]
        //public void Setup()
        //{
        //    _shaFactory = new ShaFactory();

        //    _dsa = new Mock<IDsaFfc>();
        //    _dsa
        //        .SetupGet(s => s.Sha)
        //        .Returns(_shaFactory.GetShaInstance(_hashFunction));
        //    _dsa
        //        .Setup(s => s.GenerateDomainParameters(It.IsAny<FfcDomainParametersGenerateRequest>()))
        //        .Returns(
        //            new FfcDomainParametersGenerateResult(
        //                new FfcDomainParameters(1, 2, 3),
        //                new DomainSeed(1),
        //                new Counter(1)
        //            )
        //        );
        //    _dsa
        //        .Setup(s => s.GenerateKeyPair(It.IsAny<FfcDomainParameters>()))
        //        .Returns(
        //            new FfcKeyPairGenerateResult(
        //                new FfcKeyPair(4, 5)
        //            )
        //        );
        //    _dsaFactory = new Mock<IDsaFfcFactory>();
        //    _dsaFactory
        //        .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
        //        .Returns(_dsa.Object);

        //    _entropyProvider = new EntropyProvider(new Random800_90());
        //    _entropyProviderFactory = new EntropyProviderFactory();
        //    _schemeBuilder = new SchemeBuilderFfc(
        //        _dsaFactory.Object, 
        //        new KdfFactory(
        //            new ShaFactory()
        //        ),
        //        new KeyConfirmationFactory(),
        //        new NoKeyConfirmationFactory(),
        //        new OtherInfoFactory(_entropyProvider),
        //        _entropyProvider,
        //        new DiffieHellmanFfc(),
        //        new MqvFfc()
        //    );
        //    _kasBuilder = new KasBuilderFfc(_schemeBuilder);

        //    _macParametersBuilder = new MacParametersBuilder();
        //    _keyConfirmationFactory = new KeyConfirmationFactory();
        //    _noKeyConfirmationFactory = new NoKeyConfirmationFactory();
        //    _kdfFactory = new FakeKdfFactory_BadDkm(new KdfFactory(_shaFactory));

        //    _subject = new TestCaseGeneratorValKdfKc(
        //        _kasBuilder,
        //        _schemeBuilder,
        //        _shaFactory,
        //        _entropyProviderFactory,
        //        _macParametersBuilder,
        //        _kdfFactory,
        //        _keyConfirmationFactory,
        //        _noKeyConfirmationFactory,
        //        new List<KasValTestDisposition>() {KasValTestDisposition.Success}
        //    );
        //}

        //[Test]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        //public void ShouldPopulateCorrectKeysNoncesForSchemeRole(
        //    FfcScheme scheme,
        //    KeyConfirmationRole kcRole,
        //    KeyConfirmationDirection kcType,
        //    KeyAgreementRole testGroupIutRole,
        //    KeyAgreementMacType macType
        //)
        //{
        //    BuildTestGroup(scheme, testGroupIutRole, kcRole, kcType, macType, out var iutKeyGenRequirements, out var serverKeyGenRequirements, out var resultTestCase);

        //    if (macType == KeyAgreementMacType.AesCcm)
        //    {
        //        Assert.IsTrue(resultTestCase.NonceAesCcm.BitLength != 0, nameof(resultTestCase.NonceAesCcm));
        //    }
        //    else
        //    {
        //        Assert.IsNull(resultTestCase.NonceAesCcm, nameof(resultTestCase.NonceAesCcm));
        //    }

        //    #region key/nonce checks
        //    // server checks
        //    if (serverKeyGenRequirements.GeneratesStaticKeyPair)
        //    {
        //        Assert.IsTrue(resultTestCase.StaticPrivateKeyServer != 0,
        //            nameof(resultTestCase.StaticPrivateKeyServer));
        //        Assert.IsTrue(resultTestCase.StaticPublicKeyServer != 0,
        //            nameof(resultTestCase.StaticPublicKeyServer));
        //    }
        //    else
        //    {
        //        Assert.IsTrue(resultTestCase.StaticPrivateKeyServer == 0,
        //            nameof(resultTestCase.StaticPrivateKeyServer));
        //        Assert.IsTrue(resultTestCase.StaticPublicKeyServer == 0,
        //            nameof(resultTestCase.StaticPublicKeyServer));
        //    }

        //    if (serverKeyGenRequirements.GeneratesEphemeralKeyPair)
        //    {
        //        Assert.IsTrue(resultTestCase.EphemeralPrivateKeyServer != 0,
        //            nameof(resultTestCase.EphemeralPrivateKeyServer));
        //        Assert.IsTrue(resultTestCase.EphemeralPublicKeyServer != 0,
        //            nameof(resultTestCase.EphemeralPublicKeyServer));
        //    }
        //    else
        //    {
        //        Assert.IsTrue(resultTestCase.EphemeralPrivateKeyServer == 0,
        //            nameof(resultTestCase.EphemeralPrivateKeyServer));
        //        Assert.IsTrue(resultTestCase.EphemeralPublicKeyServer == 0,
        //            nameof(resultTestCase.EphemeralPublicKeyServer));
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

        //    if (serverKeyGenRequirements.GeneratesEphemeralNonce)
        //    {
        //        Assert.IsTrue(resultTestCase.EphemeralNonceServer != null,
        //            nameof(resultTestCase.EphemeralNonceServer));
        //    }
        //    else
        //    {
        //        Assert.IsTrue(resultTestCase.EphemeralNonceServer == null,
        //            nameof(resultTestCase.EphemeralNonceServer));
        //    }

        //    // Iut checks
        //    if (iutKeyGenRequirements.GeneratesStaticKeyPair)
        //    {
        //        Assert.IsTrue(resultTestCase.StaticPrivateKeyIut != 0,
        //            nameof(resultTestCase.StaticPrivateKeyIut));
        //        Assert.IsTrue(resultTestCase.StaticPublicKeyIut != 0,
        //            nameof(resultTestCase.StaticPublicKeyIut));
        //    }
        //    else
        //    {
        //        Assert.IsTrue(resultTestCase.StaticPrivateKeyIut == 0,
        //            nameof(resultTestCase.StaticPrivateKeyIut));
        //        Assert.IsTrue(resultTestCase.StaticPublicKeyIut == 0,
        //            nameof(resultTestCase.StaticPublicKeyIut));
        //    }

        //    if (iutKeyGenRequirements.GeneratesEphemeralKeyPair)
        //    {
        //        Assert.IsTrue(resultTestCase.EphemeralPrivateKeyIut != 0,
        //            nameof(resultTestCase.EphemeralPrivateKeyIut));
        //        Assert.IsTrue(resultTestCase.EphemeralPublicKeyIut != 0,
        //            nameof(resultTestCase.EphemeralPublicKeyIut));
        //    }
        //    else
        //    {
        //        Assert.IsTrue(resultTestCase.EphemeralPrivateKeyIut == 0,
        //            nameof(resultTestCase.EphemeralPrivateKeyIut));
        //        Assert.IsTrue(resultTestCase.EphemeralPublicKeyIut == 0,
        //            nameof(resultTestCase.EphemeralPublicKeyIut));
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

        //    if (iutKeyGenRequirements.GeneratesEphemeralNonce)
        //    {
        //        Assert.IsTrue(resultTestCase.EphemeralNonceIut != null,
        //            nameof(resultTestCase.EphemeralNonceIut));
        //    }
        //    else
        //    {
        //        Assert.IsTrue(resultTestCase.EphemeralNonceIut == null,
        //            nameof(resultTestCase.EphemeralNonceIut));
        //    }
        //    #endregion key/nonce checks
        //}

        //[Test]
        //#region dhHybrid1
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]

        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]

        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]

        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybrid1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //#endregion dhHybrid1

        //#region Mqv2
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]

        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]

        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]

        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv2, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //#endregion Mqv2

        //#region DhHybridOneFlow
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]

        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]

        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]

        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //#endregion DhHybridOneFlow

        //#region mqv1
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]

        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]

        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]

        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.Mqv1, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //#endregion mqv1

        //#region DhOneFlow
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]

        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]

        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]

        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        ////[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhOneFlow, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //#endregion DhOneFlow

        //#region DhStatic
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]

        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]

        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]

        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.Success, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, false)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedZ, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedOi, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedDkm, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedMacData, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //[TestCase(FfcScheme.DhStatic, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, KasValTestDisposition.FailChangedTag, true)]
        //#endregion DhStatic

        //public void ShouldSetProperTestCaseFailureTestProperty(
        //    FfcScheme scheme, 
        //    KeyConfirmationRole kcRole,
        //    KeyConfirmationDirection kcType,
        //    KeyAgreementRole testGroupIutRole,
        //    KeyAgreementMacType macType, 
        //    KasValTestDisposition option, 
        //    bool isFailure
        //)
        //{
        //    _subject = new TestCaseGeneratorValKdfKc(
        //        _kasBuilder,
        //        _schemeBuilder,
        //        _shaFactory,
        //        _entropyProviderFactory,
        //        _macParametersBuilder,
        //        _kdfFactory,
        //        _keyConfirmationFactory,
        //        _noKeyConfirmationFactory,
        //        new List<KasValTestDisposition> {option}
        //    );

        //    BuildTestGroup(scheme, testGroupIutRole, kcRole, kcType, macType, out var iutKeyGenRequirements, out var serverKeyGenRequirements, out var resultTestCase);

        //    Assert.AreEqual(isFailure, !resultTestCase.TestPassed);
        //}

        //private void BuildTestGroup(
        //    FfcScheme scheme,
        //    KeyAgreementRole testGroupIutRole,
        //    KeyConfirmationRole kcRole,
        //    KeyConfirmationDirection kcType,
        //    KeyAgreementMacType macType,
        //    out SchemeKeyNonceGenRequirement<FfcScheme> iutKeyGenRequirements,
        //    out SchemeKeyNonceGenRequirement<FfcScheme> serverKeyGenRequirements,
        //    out TestCase resultTestCase
        //)
        //{
        //    TestGroup tg = new TestGroup()
        //    {
        //        KasMode = KasMode.KdfKc,
        //        Scheme = scheme,
        //        KasRole = testGroupIutRole,
        //        Function = KasAssurance.None,
        //        HashAlg = _hashFunction,
        //        KcRole = kcRole,
        //        KcType = kcType,
        //        MacType = macType,
        //        TestType = "VAL",
        //        P = _domainParameters.P,
        //        Q = _domainParameters.Q,
        //        G = _domainParameters.G,
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