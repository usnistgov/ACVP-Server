using Autofac;
using NIST.CVP.Common;
// vvv russ vvv
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_CBC;
using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Crypto.AES_CFB1;
using NIST.CVP.Crypto.AES_CFB128;
using NIST.CVP.Crypto.AES_CFB8;
using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Crypto.AES_OFB;
using NIST.CVP.Crypto.AES_XTS;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.DRBG;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Builders.Ecc;
using NIST.CVP.Crypto.KAS.Builders.Ffc;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.ANSIX963;
using NIST.CVP.Crypto.IKEv1;
using NIST.CVP.Crypto.IKEv2;
using NIST.CVP.Crypto.SNMP;
using NIST.CVP.Crypto.SRTP;
using NIST.CVP.Crypto.SSH;
using NIST.CVP.Crypto.TLS;
using NIST.CVP.Crypto.KeyWrap;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Crypto.TDES_CBC;
using NIST.CVP.Crypto.TDES_CBCI;
using NIST.CVP.Crypto.TDES_CFB;
using NIST.CVP.Crypto.TDES_CFBP;
using NIST.CVP.Crypto.TDES_CTR;
using NIST.CVP.Crypto.TDES_ECB;
using NIST.CVP.Crypto.TDES_OFB;
using NIST.CVP.Crypto.TDES_OFBI;
// ^^^ russ ^^^

// vvv chris vvv
using NIST.CVP.Crypto.RSA2;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Crypto.RSA2.PrimeGenerators;
using NIST.CVP.Crypto.RSA2.Signatures;
using NIST.CVP.Crypto.SHAWrapper;
// ^^^ chris ^^^
// fin

namespace NIST.CVP.Crypto
{
    public class RegisterInjections : IRegisterInjections
    {
        public void RegisterTypes(ContainerBuilder builder, AlgoMode algoMode)
        {
            builder.RegisterType<RijndaelInternals>().AsImplementedInterfaces();
            builder.RegisterType<RijndaelFactory>().AsImplementedInterfaces();
            builder.RegisterType<AES_CBC.AES_CBC>().AsImplementedInterfaces();
            builder.RegisterType<AES_CBC_MCT>().AsImplementedInterfaces();

            builder.RegisterType<AES_CCM.AES_CCM>().AsImplementedInterfaces();
            builder.RegisterType<AES_CCMInternals>().AsImplementedInterfaces();

            builder.RegisterType<AES_CFB1.AES_CFB1>().AsImplementedInterfaces();
            builder.RegisterType<AES_CFB1_MCT>().AsImplementedInterfaces();

            builder.RegisterType<AES_CFB8.AES_CFB8>().AsImplementedInterfaces();
            builder.RegisterType<AES_CFB8_MCT>().AsImplementedInterfaces();

            builder.RegisterType<AES_CFB128.AES_CFB128>().AsImplementedInterfaces();
            builder.RegisterType<AES_CFB128_MCT>().AsImplementedInterfaces();

            builder.RegisterType<AES_CTR.AesCtr>().AsImplementedInterfaces();

            builder.RegisterType<AES_ECB.AES_ECB>().AsImplementedInterfaces();
            builder.RegisterType<AES_ECB_MCT>().AsImplementedInterfaces();

            builder.RegisterType<AES_GCM.AES_GCM>().AsImplementedInterfaces();
            builder.RegisterType<AES_GCM.AES_GCMInternals>().AsImplementedInterfaces();

            builder.RegisterType<AES_OFB.AES_OFB>().AsImplementedInterfaces();
            builder.RegisterType<AES_OFB_MCT>().AsImplementedInterfaces();

            builder.RegisterType<AesXts>().AsImplementedInterfaces();

            // Russ Algos
            builder.RegisterType<CmacFactory>().AsImplementedInterfaces();

            builder.RegisterType<DrbgFactory>().AsImplementedInterfaces();

            builder.RegisterType<HmacFactory>().AsImplementedInterfaces();

            builder.RegisterType<MacParametersBuilder>().AsImplementedInterfaces();
            builder.RegisterType<KeyConfirmationFactory>().AsImplementedInterfaces();
            builder.RegisterType<NoKeyConfirmationFactory>().AsImplementedInterfaces();
            builder.RegisterType<Crypto.KAS.KDF.KdfFactory>().AsImplementedInterfaces();

            builder.RegisterType<DiffieHellmanFfc>().AsImplementedInterfaces();
            builder.RegisterType<MqvFfc>().AsImplementedInterfaces();
            builder.RegisterType<SchemeBuilderFfc>().AsImplementedInterfaces();
            builder.RegisterType<KasBuilderFfc>().AsImplementedInterfaces();
            builder.RegisterType<OtherInfoFactory>().AsImplementedInterfaces();
            builder.RegisterType<DsaFfcFactory>().AsImplementedInterfaces();

            builder.RegisterType<DiffieHellmanEcc>().AsImplementedInterfaces();
            builder.RegisterType<MqvEcc>().AsImplementedInterfaces();
            builder.RegisterType<SchemeBuilderEcc>().AsImplementedInterfaces();
            builder.RegisterType<KasBuilderEcc>().AsImplementedInterfaces();
            builder.RegisterType<OtherInfoFactory>().AsImplementedInterfaces();
            builder.RegisterType<DsaEccFactory>().AsImplementedInterfaces();
            builder.RegisterType<EccCurveFactory>().AsImplementedInterfaces();

            builder.RegisterType<EccDhComponent>().AsImplementedInterfaces();

            builder.RegisterType<Crypto.KDF.KdfFactory>().AsImplementedInterfaces();

            builder.RegisterType<AnsiX963Factory>().AsImplementedInterfaces();
            builder.RegisterType<IkeV1Factory>().AsImplementedInterfaces();
            builder.RegisterType<IkeV2Factory>().AsImplementedInterfaces();
            builder.RegisterType<SnmpFactory>().AsImplementedInterfaces();
            builder.RegisterType<SrtpFactory>().AsImplementedInterfaces();
            builder.RegisterType<SshFactory>().AsImplementedInterfaces();
            builder.RegisterType<TlsKdfFactory>().AsImplementedInterfaces();

            builder.RegisterType<KeyWrapFactory>().AsImplementedInterfaces();

            builder.RegisterType<SHA>().AsImplementedInterfaces();
            builder.RegisterType<SHA_MCT>().AsImplementedInterfaces();

            builder.RegisterType<SHA3.SHA3>().AsImplementedInterfaces();
            builder.RegisterType<SHA3_MCT>().AsImplementedInterfaces();

            builder.RegisterType<TdesCbc>().AsImplementedInterfaces();
            builder.RegisterType<TDES_CBC_MCT>().AsImplementedInterfaces();
            builder.RegisterType<TDES_CBC.MonteCarloKeyMaker>().AsImplementedInterfaces();

            builder.RegisterType<TdesCbci>().AsImplementedInterfaces();
            builder.RegisterType<TdesCbciMCT>().AsImplementedInterfaces();
            builder.RegisterType<TDES_CBCI.MonteCarloKeyMaker>().AsImplementedInterfaces();

            builder.RegisterType<TDES_CFB.MonteCarloKeyMaker>().AsImplementedInterfaces();
            builder.Register(c => TDES_CFB.ModeFactory.GetMode(algoMode)).AsImplementedInterfaces();
            builder.Register(c => TDES_CFB.ModeFactoryMCT.GetMode(algoMode)).AsImplementedInterfaces();

            builder.RegisterType<TDES_CFBP.MonteCarloKeyMaker>().AsImplementedInterfaces();
            builder.Register(c => TDES_CFBP.ModeFactory.GetMode(algoMode)).AsImplementedInterfaces();
            builder.Register(c => TDES_CFBP.ModeFactoryMCT.GetMode(algoMode)).AsImplementedInterfaces();

            builder.RegisterType<TdesCtr>().AsImplementedInterfaces();

            builder.RegisterType<TDES_ECB.TDES_ECB>().AsImplementedInterfaces();
            builder.RegisterType<TDES_ECB_MCT>().AsImplementedInterfaces();
            builder.RegisterType<TDES_ECB.MonteCarloKeyMaker>().AsImplementedInterfaces();

            builder.RegisterType<TdesOfb>().AsImplementedInterfaces();
            builder.RegisterType<TDES_OFB_MCT>().AsImplementedInterfaces();
            builder.RegisterType<TDES_OFB.MonteCarloKeyMaker>().AsImplementedInterfaces();

            builder.RegisterType<TdesOfbi>().AsImplementedInterfaces();
            builder.RegisterType<TdesOfbiMCT>().AsImplementedInterfaces();
            builder.RegisterType<TDES_OFBI.MonteCarloKeyMaker>().AsImplementedInterfaces();

            // Chris Algos
            builder.RegisterType<KeyBuilder>().AsImplementedInterfaces();
            builder.RegisterType<KeyComposerFactory>().AsImplementedInterfaces();
            builder.RegisterType<PrimeGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<Rsa>().AsImplementedInterfaces();
            builder.RegisterType<RsaVisitor>().AsImplementedInterfaces();

            builder.RegisterType<SignatureBuilder>().AsImplementedInterfaces();
            builder.RegisterType<PaddingFactory>().AsImplementedInterfaces();
            builder.RegisterType<ShaFactory>().AsImplementedInterfaces();

            builder.RegisterType<EccCurveFactory>().AsImplementedInterfaces();
            builder.RegisterType<EccDsa>().AsImplementedInterfaces();
            builder.RegisterType<DsaEccFactory>().AsImplementedInterfaces();
        }
    }
}