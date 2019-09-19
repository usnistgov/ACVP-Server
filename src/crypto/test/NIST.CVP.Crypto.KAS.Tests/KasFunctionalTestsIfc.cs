using System.Linq;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KTS;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.KAS.Builders.Ifc;
using NIST.CVP.Crypto.KAS.FixedInfo;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.KDF.OneStep;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.KTS;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests
{
    [TestFixture, FastCryptoTest]
    public class KasFunctionalTestsIfc
    {
        private IKasIfcBuilder _kasBuilderPartyU;
        private ISchemeIfcBuilder _schemeBuilderPartyU;
        private IKasIfcBuilder _kasBuilderPartyV;
        private ISchemeIfcBuilder _schemeBuilderPartyV;
        private IIfcSecretKeyingMaterialBuilder _secretKeyingMaterialBuilderPartyU;
        private IIfcSecretKeyingMaterialBuilder _secretKeyingMaterialBuilderPartyV;
        private IKdfFactory _kdfFactory;
        private IKdfParameterVisitor _kdfParameterVisitor;
        private IKtsFactory _ktsFactory;
        private IKeyConfirmationFactory _keyConfirmationFactory;
        private IFixedInfoFactory _fixedInfoFactory;

        [SetUp]
        public void Setup()
        {
            var shaFactory = new ShaFactory();
            var entropyFactory = new EntropyProviderFactory();
            var rsa = new Rsa(new RsaVisitor());
            
            var kdfVisitor = new KdfVisitor(new KdfOneStepFactory(shaFactory, new HmacFactory(shaFactory)));
            var rsaSve = new RsaSve(rsa, entropyFactory);
            
            _kasBuilderPartyU = new KasIfcBuilder();
            _schemeBuilderPartyU = new SchemeIfcBuilder(kdfVisitor, rsaSve, entropyFactory.GetEntropyProvider(EntropyProviderTypes.Random));
            
            _kasBuilderPartyV = new KasIfcBuilder();
            _schemeBuilderPartyV = new SchemeIfcBuilder(kdfVisitor, rsaSve, entropyFactory.GetEntropyProvider(EntropyProviderTypes.Random));
            
            _secretKeyingMaterialBuilderPartyU = new IfcSecretKeyingMaterialBuilder();
            _secretKeyingMaterialBuilderPartyV = new IfcSecretKeyingMaterialBuilder();
            
            _kdfFactory = new KdfFactory(kdfVisitor);
            _kdfParameterVisitor = new KdfParameterVisitor(entropyFactory.GetEntropyProvider(EntropyProviderTypes.Random));
            _ktsFactory = new KtsFactory(shaFactory, rsa, entropyFactory);
            _keyConfirmationFactory = new KeyConfirmationFactory(new KeyConfirmationMacDataCreator());
            _fixedInfoFactory = new FixedInfoFactory(new FixedInfoStrategyFactory());
        }

        [Test]
        public void BothPartiesShouldKas1()
        {
            var l = 512;
            var modulo = 2048;
            var scheme = IfcScheme.Kas1_basic;
            var kasMode = KasMode.KdfNoKc;
            var idPartyU = new BitString("1010101010");
            var idPartyV = new BitString("BEEFFACE");
            
            // Set the key and party id for party u's builder
            _secretKeyingMaterialBuilderPartyU
                .WithKey(null)
                .WithPartyId(idPartyU);
                
            // Partial party v secret keying material is needed for party U to initialize
            var secretKeyingMaterialPartyV = _secretKeyingMaterialBuilderPartyV
                .WithKey(new KeyPair()
                {
                    PrivKey = new CrtPrivateKey()
                    {
                        P = new BitString("F79CE837FBD16A6DCF35CBD83F856BF2D4F33F58BF726E82EBB880B55ACEAE6C3A2170788624913968739703D9D3F7BEF2AD9346E8A44568156E5C8D0BD2E16639A4AB36445EA0683927FD04DA83F147458B873B34E710E558B4D6268DCB6165787287CA1A3D497AE9C82EF98A9E1CEB95E9596C3859D9C06FC6748ED88A3169").ToPositiveBigInteger(),
                        Q = new BitString("CEAF12D64C14E32E4633A597135349A7554952122F9628213E22D96223A22D8C8535C4FB8B705FEC38D3FA839C43D54B3DCDEA1199211993AB9E47AC011B0240B741DAE89E8675405BEC344929D3BF85CEDA7E25E3E5CE601E638101B1BBD96C1D24565BC8A425BBF2CDF7688237F72C9FF867DA881F650E9DA38DC442680E53").ToPositiveBigInteger(),
                        DMP1 = new BitString("0D3261685C9F0203ECF00155929DAF33983251BF04AB9D8ECC261AA2015E86FDA04353E85FC2860804ABE747E910430840E1172A2E860D0FCE473B896BD8758CE9767507B2906DE83788F37A404BEBE40D905DA3C2F3F8984035842293B3FC54D1095014287E2FA32BBE5979C3EF6DE4ED495E2F20B883008055488E5B24E173").ToPositiveBigInteger(),
                        DMQ1 = new BitString("43B8DB9D93D81B46C0950BA256873235C33A4F6399181186EA9B8D180C6B41BA59FCF3764A08E63CB4284E74749D8BE0494BF9F1C41AA00D7DDE658DD8C7308A3925E2800CEB316174F5FE75D162A286488E6E9C2070EF9F6EEE0C73AB171B8D6750E2CD388B45FE6D68AB1C25B270EFC248DA3A40E2E0F920A59882717EFBB9").ToPositiveBigInteger(),
                        IQMP = new BitString("E018E420369A94A74663AF749EF114137AB5A7F01D885043243F328C1BE84EA82347BEDA0DA7407B77F01931FAF527A9EF73385FAFA2C7787682EEB05E0E1D6580F81355205A983F867BAE116CBBB4D2968BEB3D955C5AA13F22D40ACDD921F8ADD87DF5134A06037E1EF08FE93A103C8F0B6571309A59234B2511E93D1F2C48").ToPositiveBigInteger()
                    },
                    PubKey = new PublicKey()
                    {
                        E = new BitString("3BCF32AB").ToPositiveBigInteger(),
                        N = new BitString("C7E9995819FC7F3CFF7CB2660A43BBFD8336C50203ED07FDDBC17325D96AA2BE21BCFC218CBB8CBF6947210C06C298B58AB52BAF88841A34DE5A7425920E86B2064D92A8F4ABB356A9A7E8D21138CB5BB0A47D53CE01895AB2FCAEF27E8DC2AD00DA0DDD7E7681C6214CABD8D8324A0258E625D3D807595B364690DAFEF5935947EAE69E5184E42E7C8DB4121CF8A4B89E92A4096790B6C7437FB4AC126FAF6F5D9F81292042D29A510E8C6795F06F8E8EDCBFF5FBE37664510CBC003C1C5BDA3A5548D38DBCE5E6C48ABA1A395D2983B843E653663587AC4514E437C828E66D66009063930CDE90DBADE3A6F7D54E07614B3E3116B15DD80772DC6CE829C30B").ToPositiveBigInteger()
                    }
                })
                .WithPartyId(idPartyV)
                .Build(
                    scheme, 
                    kasMode, 
                    KeyAgreementRole.ResponderPartyV, 
                    KeyConfirmationRole.None,
                    KeyConfirmationDirection.None, 
                    false);
            
            var fixedInfoParameter = new FixedInfoParameter()
            {
                L = l,
                // vvv These are set internally to the kas instance vvv
                FixedInfoPartyU = null,
                FixedInfoPartyV = null
                // ^^^ These are set internally to the kas instance ^^^
            };
            
            // KDF
            IKdfParameter kdfParam = null;
            var kdfConfiguration = new OneStepConfiguration()
            {
                L = l,
                SaltLen = 128,
                SaltMethod = MacSaltMethod.Default,
                FixedInputEncoding = FixedInfoEncoding.Concatenation,
                FixedInputPattern = "l|uPartyInfo|vPartyInfo",
                AuxFunction = KasKdfOneStepAuxFunction.HMAC_SHA2_D224
            };
            if (KeyGenerationRequirementsHelper.IfcKdfSchemes.Contains(scheme))
            {
                kdfParam = kdfConfiguration.GetKdfParameter(_kdfParameterVisitor);
                
                fixedInfoParameter.Encoding = kdfConfiguration.FixedInputEncoding;
                fixedInfoParameter.FixedInfoPattern = kdfConfiguration.FixedInputPattern;
                fixedInfoParameter.Salt = kdfParam.Salt;
            }
            
            // KTS
            KtsParameter ktsParam = null;
            var ktsConfiguration = new KtsConfiguration()
            {
                Encoding = FixedInfoEncoding.Concatenation,
                AssociatedDataPattern = "l|uPartyInfo|vPartyInfo",
                KtsHashAlg = KasHashAlg.SHA2_D224
            };
            if (KeyGenerationRequirementsHelper.IfcKtsSchemes.Contains(scheme))
            {
                fixedInfoParameter.Encoding = ktsConfiguration.Encoding;
                fixedInfoParameter.FixedInfoPattern = ktsConfiguration.AssociatedDataPattern;
                
                ktsParam = new KtsParameter()
                {
                    Encoding = ktsConfiguration.Encoding,
                    AssociatedDataPattern = ktsConfiguration.AssociatedDataPattern,
                    KtsHashAlg = ktsConfiguration.KtsHashAlg
                };
            }
            
            // MAC
            MacParameters macParam = null;
            MacConfiguration macConfiguration = new MacConfiguration()
            {
                KeyLen = 128,
                MacLen = 128,
                MacType = KeyAgreementMacType.HmacSha2D224
            };
            IKeyConfirmationFactory kcFactory = null;
            if (KeyGenerationRequirementsHelper.IfcKcSchemes.Contains(scheme))
            {
                macParam = new MacParameters(
                    macConfiguration.MacType, 
                    macConfiguration.KeyLen, 
                    macConfiguration.MacLen);

                kcFactory = _keyConfirmationFactory;
            }
            
            
            // Initialize Party U KAS
            _schemeBuilderPartyU
                .WithSchemeParameters(new SchemeParametersIfc(
                    new KasAlgoAttributesIfc(scheme, modulo, l),
                    KeyAgreementRole.InitiatorPartyU,
                    kasMode,
                    KeyConfirmationRole.None,
                    KeyConfirmationDirection.None,
                    KasAssurance.None,
                    idPartyU
                ))
                .WithFixedInfo(_fixedInfoFactory, fixedInfoParameter)
                .WithKdf(_kdfFactory, kdfParam)
                .WithKts(_ktsFactory, ktsParam)
                .WithKeyConfirmation(kcFactory, macParam)
                .WithThisPartyKeyingMaterialBuilder(_secretKeyingMaterialBuilderPartyU);

            var kasPartyU = _kasBuilderPartyU
                .WithSchemeBuilder(_schemeBuilderPartyU)
                .Build();
                    
            kasPartyU.InitializeThisPartyKeyingMaterial(secretKeyingMaterialPartyV);
            var initializedKeyingMaterialPartyU = kasPartyU.Scheme.ThisPartyKeyingMaterial;
            
            
            // Initialize Party V KAS
            _schemeBuilderPartyV
                .WithSchemeParameters(new SchemeParametersIfc(
                    new KasAlgoAttributesIfc(scheme, modulo, l),
                    KeyAgreementRole.ResponderPartyV,
                    kasMode,
                    KeyConfirmationRole.None,
                    KeyConfirmationDirection.None,
                    KasAssurance.None,
                    idPartyV
                ))
                .WithFixedInfo(_fixedInfoFactory, fixedInfoParameter)
                .WithKdf(_kdfFactory, kdfParam)
                .WithKts(_ktsFactory, ktsParam)
                .WithKeyConfirmation(kcFactory, macParam)
                .WithThisPartyKeyingMaterialBuilder(_secretKeyingMaterialBuilderPartyV);

            var kasPartyV = _kasBuilderPartyV
                .WithSchemeBuilder(_schemeBuilderPartyV)
                .Build();
                    
            kasPartyV.InitializeThisPartyKeyingMaterial(initializedKeyingMaterialPartyU);

            var initializedKeyingMaterialPartyV = kasPartyV.Scheme.ThisPartyKeyingMaterial;
            
            var resultPartyU = kasPartyU.ComputeResult(initializedKeyingMaterialPartyV);
            var resultPartyV = kasPartyV.ComputeResult(resultPartyU.KeyingMaterialPartyU);
            
            Assert.AreEqual(resultPartyU.Dkm.ToHex(), resultPartyV.Dkm.ToHex());
        }
    }
}