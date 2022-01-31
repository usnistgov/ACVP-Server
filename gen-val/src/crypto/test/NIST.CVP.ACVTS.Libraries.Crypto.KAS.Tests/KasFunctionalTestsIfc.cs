using System;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.CMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KTS;
using NIST.CVP.ACVTS.Libraries.Crypto.CSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.HKDF;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.IKEv1;
using NIST.CVP.ACVTS.Libraries.Crypto.IKEv2;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders.Ifc;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF.OneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.KES;
using NIST.CVP.ACVTS.Libraries.Crypto.KMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.KTS;
using NIST.CVP.ACVTS.Libraries.Crypto.RSA;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.TLS;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Tests
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
        private IRsaSve _rsaSve;

        private readonly IEntropyProvider _entropyProvider = new EntropyProvider(new Random800_90());

        [SetUp]
        public void Setup()
        {
            var shaFactory = new NativeShaFactory();
            var hmacFactory = new HmacFactory(shaFactory);
            var entropyFactory = new EntropyProviderFactory();
            var rsa = new Rsa(new RsaVisitor());

            var kdfVisitor = new KdfVisitor(
                new KdfOneStepFactory(shaFactory, new HmacFactory(shaFactory), new KmacFactory(new CSHAKEWrapper())),
                new Crypto.KDF.KdfFactory(new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory()),
                hmacFactory), hmacFactory,
                new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory()),
                new IkeV1Factory(hmacFactory, shaFactory),
                new IkeV2Factory(hmacFactory),
                new TlsKdfFactory(hmacFactory),
                new HkdfFactory(hmacFactory));
            _rsaSve = new RsaSve(rsa, _entropyProvider);

            _kasBuilderPartyU = new KasIfcBuilder();
            _schemeBuilderPartyU = new SchemeIfcBuilder(kdfVisitor);

            _kasBuilderPartyV = new KasIfcBuilder();
            _schemeBuilderPartyV = new SchemeIfcBuilder(kdfVisitor);

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
            };

            // KDF
            IKdfParameter kdfParam = null;
            var kdfConfiguration = new OneStepConfiguration()
            {
                L = l,
                SaltLen = 128,
                SaltMethod = MacSaltMethod.Default,
                FixedInfoEncoding = FixedInfoEncoding.Concatenation,
                FixedInfoPattern = "l|uPartyInfo|vPartyInfo",
                AuxFunction = KdaOneStepAuxFunction.HMAC_SHA2_D224
            };
            if (KeyGenerationRequirementsHelper.IfcKdfSchemes.Contains(scheme))
            {
                kdfParam = kdfConfiguration.GetKdfParameter(_kdfParameterVisitor);

                fixedInfoParameter.Encoding = kdfConfiguration.FixedInfoEncoding;
                fixedInfoParameter.FixedInfoPattern = kdfConfiguration.FixedInfoPattern;
                fixedInfoParameter.Salt = kdfParam.Salt;
            }

            // KTS
            KtsParameter ktsParam = null;
            var ktsConfiguration = new KtsConfiguration()
            {
                Encoding = FixedInfoEncoding.Concatenation,
                AssociatedDataPattern = "l|uPartyInfo|vPartyInfo",
                HashAlg = KasHashAlg.SHA2_D224
            };
            if (KeyGenerationRequirementsHelper.IfcKtsSchemes.Contains(scheme))
            {
                fixedInfoParameter.Encoding = ktsConfiguration.Encoding;
                fixedInfoParameter.FixedInfoPattern = ktsConfiguration.AssociatedDataPattern;

                ktsParam = new KtsParameter()
                {
                    Encoding = ktsConfiguration.Encoding,
                    AssociatedDataPattern = ktsConfiguration.AssociatedDataPattern,
                    KtsHashAlg = ktsConfiguration.HashAlg
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
                .WithEntropyProvider(_entropyProvider)
                .WithRsaSve(_rsaSve)
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
                .WithEntropyProvider(_entropyProvider)
                .WithRsaSve(_rsaSve)
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

        [Test]
        public void DkmIntermediateValues()
        {
            var l = 512;
            var algorithmId = new BitString("A0752AE1B4165A32C1387D1DA5E9F8AE");
            var fixedInfoPattern = "algorithmId||l||uPartyInfo||vPartyInfo";
            var fixedInfoEncoding = FixedInfoEncoding.Concatenation;
            var z = new BitString("08E11078180FBDA3C73183F116DA038243802E22F165C2EF4C6BD9AC82612F86FD4F8197AB0B4F5A5E14390BD8F01A55B443FEA55A05438978BC8368691CB8013D5BE2312FB7C64125882835EE3BACD0D442C83A636DE2DF81B04F714C75E2874A284784C1EE987D7E61123CE5C53AC82DD0DE4CA3022985DFA320C83444A338C50DFA2803D9A1B28E6C3D8CF2C283A27983B6F0AEA89444504A5D63A127E9BBEE05FE6867B34CE3C18CF438477ACF3D07C1F01D00913DC37661DC4FBD623E83A48407C2FF8A6369A1B682EC6C0BA62C9A1AC5DFAB8D3FF6C7FEF3CA1DED9943786043EEA76A58CCA19BAB408805C26A2CF93D57A5B6E4E872AF2D585704044C");

            var uPartyFixedInfo = new PartyFixedInfo(
                new BitString("CAFE01020304"),
                new BitString("DAC2B3C01810B48A206333183E4E873E139FE6E4EC274EDB44B27419BA96C1401B49670A44C1F89B996C235E242962CF4D39C9F9E99D4281B8D9D75C5129BDB4822B02C0DC8515C60F6C0337D5EFEAA1346EFE2F5A8475C17FCF85B1A536E13B2E6FBA0BA2737409DAECD4F4A36D104DD3BE465AD92CD081959489CCD1B26FE496B2090FB30EE3892C8090DF6EE74661D696F23A2A71F68FB7B395C97236F6B937639891AE1B6EAEC814928B325D9B2F3B7FFBEBE9A718F2964CBA01118EE9D769E769D24ED6553B79D9F82CA62619C3D20105DEF06618B6FAEA7FB358206F628B07AAA5B80AC97D99830D943C1AE78D8F9926CB3A1D004F650FCA0D40117D3C"));

            var vPartyFixedInfo = new PartyFixedInfo(
                new BitString("434156536964"),
                new BitString("121DCC2DD2F243398381290F0F1AE0648F7B1911606BCA5F1065D54578B40CCBCEB3CBC0EF5F7DACAA3567CEEB8B220AE3E257ED325380DDDDBB8C4F9FEE6FEB4F149A3225C447E7684B64E049A81A9CB57433E89F5CE068E6F0843646E6F80F16B1760628D87F17CAF74912BC66CE61F1D33697FD859CC51AC20AA4DB12DEB25D2147180A0C70EB0C316D3C22FA2BF0955F48259856305DC9801298C38B4838346B0DE85763C13AF8CEED2EE266462C5BCE30244A131D6B099E7CCB4668BAFA5F4FA3682786BFC5849238B6C13AFB464C58017D85A887D7D4346F162945CC3184AF68DDAC92845C4CB6CCD1EAF19F78B4A511129A3EA9C82D50FAACE30B7B50"));

            var fixedInfoParam = new FixedInfoParameter()
            {
                AlgorithmId = algorithmId,
                Encoding = fixedInfoEncoding,
                L = l,
                FixedInfoPattern = fixedInfoPattern
            };
            fixedInfoParam.SetFixedInfo(uPartyFixedInfo, vPartyFixedInfo);

            var fixedInfo = _fixedInfoFactory.Get().Get(fixedInfoParam);

            var kdf = _kdfFactory.GetKdf();
            var dkm = kdf.DeriveKey(new KdfParameterOneStep()
            {
                Z = z,
                L = l,
                AlgorithmId = algorithmId,
                AuxFunction = KdaOneStepAuxFunction.SHA2_D256,
                FixedInfoPattern = fixedInfoPattern,
                FixedInputEncoding = fixedInfoEncoding
            }, fixedInfo);

            Assert.AreEqual(
                new BitString("6D7783F2CB7D9F35A0B8809430A62B152D7C128F960B2827E46130D8955797601DB5263A135BC429D60568EDEB72B656F819CFC348CCC02EEEA7FE589FFA385B").ToHex(),
                dkm.DerivedKey.ToHex());
        }

        [Test]
        public void Sample()
        {
            var l = 512;
            var modulo = 2048;
            var scheme = IfcScheme.Kts_oaep_partyV_keyConfirmation;
            var kasMode = KasMode.NoKdfKc;
            var idPartyU = new BitString("434156536964");
            var idPartyV = new BitString("123456ABCD");

            // Set the key and party id for party u's builder
            _secretKeyingMaterialBuilderPartyU
                .WithKey(null)
                .WithC(new BitString("61489C8AE69AA4B55AE296C98656751883F1F27C6D861019E9AAE9198EEFE4B6105DB0C7009A320DA52AFF699B816124CF986F8426AFFB48397054176A22EC46E81145ADEAB0B2D9C12725281115578D6A6FD8F8E605EDCC4DB6DC4068CB77E504C8529D248422E91ECF70A5F7CEDBB16EE3C503FF6E58C026228CAFB4F0A55B4C358665EA60A95522BEC50FA1538FAAC758EF7DB72A20B1E38C87C009D9C8A7D1D3F9999F465FD32C8A486C72D9407A9D136A6E68615F4EB3C6D463596DB867FC05EAED52B700EF4B7BF7D0005DBB9D30F57AA6CFBAC46E134E5E31BF106F0B38752A443C73CEF7D7C6C0C0B6A52F5E837A028196460068A0967E53E02FDE69"))
                .WithPartyId(idPartyU);

            // Partial party v secret keying material is needed for party U to initialize
            var secretKeyingMaterialPartyV = _secretKeyingMaterialBuilderPartyV
                .WithKey(new KeyPair()
                {
                    PrivKey = new PrivateKey()
                    {
                        P = new BitString("DD03D104A0691B7CD884FE341A62283961814F082DA1725E76B08FF15530C110E42FD8CA4A663507F01DE2BEBCB321750C7E78C6CAE6C6B6CE1B6AD5710B9ABA43BAE73BD834B7DA4719EFF357417AE08601FDF0EB05CBCC6AB0FF01407C20DF113A003693A776096E64CB95B33166CE1DAB5292EBABF543F07BE34E3E253E13").ToPositiveBigInteger(),
                        Q = new BitString("C45280305C0A92347463D05A94CA75BA5145EF0417855E5574319ACC80C135FE5E86C16FA172039502689BBC568315C737A59C7FD46D2886C221A35FA0959C102026DDAA92AC5FEEC5C372400C207718903A611DD155253C31541C0061C85975A8A7DCCD9BA60298C26ED5FAA959DADE210427F90A45B7FCF479509544215C9B").ToPositiveBigInteger(),
                        D = new BitString("0720E1FD614A1A00B649FCC0147E3A88F0AE7AA3F5D7FD71A8A220DD83EBE88AB490CACB2F6BD071532D4F8C87C8C4C72A3B3298B7B6377344CCD14D32BC973434E4B63ED6F8690E9316B5CB73BBFB74143B85D66FA31A65EB913F8E2931AB173DA04F79EE5E97401D3C98BCACBA35CB2BE9EEBCD6A6DCD8B3C5322807F96D0AEA3C04AA3CD123735C1AF0E08F86D435912CF7188E435A7C224AE954329DEB4B77BE2A5C529EA8D20E059BD891F8732AA4AC279C555EF7F5709A7CD36CABEFB5B99E13D00B1A18CC2ED056E1BB6FB71B6AAEAD4CBE453EF04A412D92A6850EAC6765D091A3F9EF5844EB695E754512077D6C5D4ECDDEF80C4EC5A32CF8379541").ToPositiveBigInteger(),
                    },
                    PubKey = new PublicKey()
                    {
                        E = new BitString("3BDEA4775FE1").ToPositiveBigInteger(),
                        N = new BitString("A97E25EC26FBDB464F81CA5E1BBE412485961ECE7C0003E8EDFC63B6946F6B3E42B13235F7FC4EF52AD5719C67B8BF5F492CF3D7721AEAD0DEE44B867BF55C388BE3B2D4FD04737846032DA940B8596D423CE2D965A6FC724600411BCF2C5BA4931EE7E8943718366EC33D7079F58487161F9951CFD38CC7E3E29264677F744BD09C7D0CD1FC1B9B417D3C1CFF53A0B32354CEC86719379632BB1A4E9F3834159BF39FED2A7A2A16F8AA45524712AE6FCF31B87BCDB8C77434FCBEB061006D38F5C7FCB62CB44669A28EFAD975D31E30C64083168AC0F97922AA3C0FA214C2F1F492A16F6002A3CE003C64DBF1525C5EEFEEBE411E6721567858B70E0F4E6981").ToPositiveBigInteger()
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
            };

            // KTS
            KtsParameter ktsParam = null;
            var ktsConfiguration = new KtsConfiguration()
            {
                Encoding = FixedInfoEncoding.None,
                AssociatedDataPattern = "",
                HashAlg = KasHashAlg.SHA2_D224
            };
            if (KeyGenerationRequirementsHelper.IfcKtsSchemes.Contains(scheme))
            {
                fixedInfoParameter.Encoding = ktsConfiguration.Encoding;
                fixedInfoParameter.FixedInfoPattern = ktsConfiguration.AssociatedDataPattern;

                ktsParam = new KtsParameter()
                {
                    Encoding = ktsConfiguration.Encoding,
                    AssociatedDataPattern = ktsConfiguration.AssociatedDataPattern,
                    KtsHashAlg = ktsConfiguration.HashAlg
                };
            }

            // MAC
            MacParameters macParam = null;
            MacConfiguration macConfiguration = new MacConfiguration()
            {
                KeyLen = 128,
                MacLen = 224,
                MacType = KeyAgreementMacType.Kmac_128
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

            // Build keying material from party u
            var initializedKeyingMaterialPartyU = _secretKeyingMaterialBuilderPartyU.Build(
                scheme,
                KasMode.NoKdfKc,
                KeyAgreementRole.InitiatorPartyU,
                KeyConfirmationRole.Recipient,
                KeyConfirmationDirection.Unilateral);

            // Initialize Party V KAS
            _schemeBuilderPartyV
                .WithSchemeParameters(new SchemeParametersIfc(
                    new KasAlgoAttributesIfc(scheme, modulo, l),
                    KeyAgreementRole.ResponderPartyV,
                    kasMode,
                    KeyConfirmationRole.Provider,
                    KeyConfirmationDirection.Unilateral,
                    KasAssurance.None,
                    idPartyV
                ))
                .WithFixedInfo(_fixedInfoFactory, fixedInfoParameter)
                .WithKts(_ktsFactory, ktsParam)
                .WithKeyConfirmation(kcFactory, macParam)
                .WithEntropyProvider(_entropyProvider)
                .WithRsaSve(_rsaSve)
                .WithThisPartyKeyingMaterialBuilder(_secretKeyingMaterialBuilderPartyV);

            var kasPartyV = _kasBuilderPartyV
                .WithSchemeBuilder(_schemeBuilderPartyV)
                .Build();

            kasPartyV.InitializeThisPartyKeyingMaterial(initializedKeyingMaterialPartyU);
            kasPartyV.Scheme.ThisPartyKeyingMaterial = secretKeyingMaterialPartyV;

            var expectedDkm = new BitString("ACEC7886E4FD2CDC5763ACCBDAA5CBEBC1704CDD05B2606256535777CF1C8E6AD335559AB2CBC63551650CA8E0C7CE2895D53E36927B7C02862B52AA327E4164");

            var resultPartyV = kasPartyV.ComputeResult(initializedKeyingMaterialPartyU);

            Console.WriteLine(resultPartyV.Dkm.ToHex());
            Assert.AreEqual(expectedDkm.ToHex(), resultPartyV.Dkm.ToHex());
        }
    }
}
