using Autofac;
using NIST.CVP.Common;
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
using NIST.CVP.Crypto.RSA2;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Crypto.RSA2.PrimeGenerators;
using NIST.CVP.Crypto.RSA2.Signatures;
using NIST.CVP.Crypto.SHAWrapper;

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

            // Chris Algos
            builder.RegisterType<KeyBuilder>().AsImplementedInterfaces();
            builder.RegisterType<KeyComposerFactory>().AsImplementedInterfaces();
            builder.RegisterType<PrimeGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<Rsa>().AsImplementedInterfaces();
            builder.RegisterType<RsaVisitor>().AsImplementedInterfaces();

            builder.RegisterType<SignatureBuilder>().AsImplementedInterfaces();
            builder.RegisterType<PaddingFactory>().AsImplementedInterfaces();
            builder.RegisterType<ShaFactory>().AsImplementedInterfaces();
        }
    }
}