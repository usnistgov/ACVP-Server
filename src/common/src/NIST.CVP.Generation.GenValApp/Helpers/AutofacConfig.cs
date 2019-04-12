using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NIST.CVP.Common;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Generation.Core.Helpers;
using NLog;
using System;


namespace NIST.CVP.Generation.GenValApp.Helpers
{
    public static class AutofacConfig
    {
        private static IContainer _container;

        public static Action<ContainerBuilder> OverrideRegistrations;

        public static IContainer GetContainer()
        {
            return _container;
        }

        public static void IoCConfiguration(IServiceProvider serviceProvider, string algorithm, string mode, string revision, string dllLocation)
        {
            var builder = new ContainerBuilder();
            EntryPointConfigHelper.RegisterConfigurationInjections(serviceProvider, builder);

            var algoMode = AlgoModeLookupHelper.GetAlgoModeFromStrings(algorithm, mode, revision);

            // TODO this shouldn't be done here, fix with nuget maybe?
            // Crypto and Oracle Registration
            var crypto = new Crypto.RegisterInjections();
            crypto.RegisterTypes(builder, algoMode);
            var oracle = new Crypto.Oracle.RegisterInjections();
            oracle.RegisterTypes(builder, algoMode);

            if (!RegisterGenVals(builder, algoMode))
            { 
                // Fall through run time loading
                var iocRegisterables = GenValResolver.ResolveIocInjectables(
                    serviceProvider.GetService<IOptions<AlgorithmConfig>>().Value,
                    algorithm,
                    mode,
                    revision,
                    dllLocation
                );

                foreach (var iocRegisterable in iocRegisterables)
                {
                    iocRegisterable.RegisterTypes(builder, algoMode);
                }
            }

            OverrideRegistrations?.Invoke(builder);

            _container = builder.Build();
        }

        /// <summary>
        /// Register the GenVals for the specified <see cref="AlgoMode"/>.
        /// </summary>
        /// <param name="builder">The IOC builder</param>
        /// <param name="algoMode">The algoMode to register</param>
        /// <returns></returns>
        private static bool RegisterGenVals(ContainerBuilder builder, AlgoMode algoMode)
        {
            IRegisterInjections genVals = null;

            // TODO fix this up so that we can hopefully set a IRegisterInjections to the AlgoMode definition itself, to avoid having to change this code whenever a new algorithm is added.
            switch (algoMode)
            {
                case AlgoMode.AES_CBC_v1_0:
                    genVals = new AES_CBC.v1_0.RegisterInjections();
                    break;
                case AlgoMode.AES_CCM_v1_0:
                    genVals = new AES_CCM.v1_0.RegisterInjections();
                    break;
                case AlgoMode.AES_CFB1_v1_0:
                    genVals = new AES_CFB1.v1_0.RegisterInjections();
                    break;
                // vvv -- Russ Algos -- vvv

                case AlgoMode.AES_CFB8_v1_0:
                    genVals = new AES_CFB8.v1_0.RegisterInjections();
                    break;
                case AlgoMode.AES_CFB128_v1_0:
                    genVals = new AES_CFB128.v1_0.RegisterInjections();
                    break;
                case AlgoMode.AES_CTR_v1_0:
                    genVals = new AES_CTR.v1_0.RegisterInjections();
                    break;
                case AlgoMode.AES_ECB_v1_0:
                    genVals = new AES_ECB.v1_0.RegisterInjections();
                    break;
                case AlgoMode.AES_GCM_v1_0:
                    genVals = new AES_GCM.v1_0.RegisterInjections();
                    break;
                case AlgoMode.AES_GCM_SIV_v1_0:
                    genVals = new AES_GCM_SIV.v1_0.RegisterInjections();
                    break;
                case AlgoMode.AES_OFB_v1_0:
                    genVals = new AES_OFB.v1_0.RegisterInjections();
                    break;
                case AlgoMode.AES_XPN_v1_0:
                    genVals = new AES_XPN.v1_0.RegisterInjections();
                    break;
                case AlgoMode.AES_XTS_v1_0:
                    genVals = new AES_XTS.v1_0.RegisterInjections();
                    break;
                case AlgoMode.CMAC_AES_v1_0:
                case AlgoMode.CMAC_TDES_v1_0:
                    genVals = new CMAC.v1_0.RegisterInjections();
                    break;
                case AlgoMode.DRBG_CTR_v1_0:
                case AlgoMode.DRBG_Hash_v1_0:
                case AlgoMode.DRBG_HMAC_v1_0:
                    genVals = new CMAC.v1_0.RegisterInjections();
                    break;
                case AlgoMode.DSA_KeyGen_v1_0:
                    genVals = new DSA.v1_0.KeyGen.RegisterInjections();
                    break;
                case AlgoMode.DSA_PQGGen_v1_0:
                    genVals = new DSA.v1_0.PqgGen.RegisterInjections();
                    break;
                case AlgoMode.DSA_PQGVer_v1_0:
                    genVals = new DSA.v1_0.PqgVer.RegisterInjections();
                    break;
                case AlgoMode.DSA_SigGen_v1_0:
                    genVals = new DSA.v1_0.SigGen.RegisterInjections();
                    break;
                case AlgoMode.DSA_SigVer_v1_0:
                    genVals = new DSA.v1_0.SigVer.RegisterInjections();
                    break;
                case AlgoMode.ECDSA_KeyGen_v1_0:
                    genVals = new ECDSA.v1_0.KeyGen.RegisterInjections();
                    break;
                case AlgoMode.ECDSA_KeyVer_v1_0:
                    genVals = new ECDSA.v1_0.KeyVer.RegisterInjections();
                    break;
                case AlgoMode.ECDSA_SigGen_v1_0:
                    genVals = new ECDSA.v1_0.SigGen.RegisterInjections();
                    break;
                case AlgoMode.ECDSA_SigVer_v1_0:
                    genVals = new ECDSA.v1_0.SigVer.RegisterInjections();
                    break;
                case AlgoMode.EDDSA_KeyGen_v1_0:
                    genVals = new EDDSA.v1_0.KeyGen.RegisterInjections();
                    break;
                case AlgoMode.EDDSA_KeyVer_v1_0:
                    genVals = new EDDSA.v1_0.KeyVer.RegisterInjections();
                    break;
                case AlgoMode.EDDSA_SigGen_v1_0:
                    genVals = new EDDSA.v1_0.SigGen.RegisterInjections();
                    break;
                case AlgoMode.EDDSA_SigVer_v1_0:
                    genVals = new EDDSA.v1_0.SigVer.RegisterInjections();
                    break;

                // ^^^ -- Russ Algos -- ^^^



                // vvv -- Chris Algos -- vvv
                case AlgoMode.SHA1_v1_0:
                case AlgoMode.SHA2_v1_0:
                    genVals = new SHA2.v1_0.RegisterInjections();
                    break;
                case AlgoMode.SHA3_v1_0:
                case AlgoMode.SHAKE_v1_0:
                    genVals = new SHA3.v1_0.RegisterInjections();
                    break;
                case AlgoMode.TDES_CBC_v1_0:
                    genVals = new TDES_CBC.v1_0.RegisterInjections();
                    break;
                case AlgoMode.TDES_CBCI_v1_0:
                    genVals = new TDES_CBCI.v1_0.RegisterInjections();
                    break;
                case AlgoMode.TDES_CFB1_v1_0:
                case AlgoMode.TDES_CFB8_v1_0:
                case AlgoMode.TDES_CFB64_v1_0:
                    genVals = new TDES_CFB.v1_0.RegisterInjections();
                    break;
                case AlgoMode.TDES_CFBP1_v1_0:
                case AlgoMode.TDES_CFBP8_v1_0:
                case AlgoMode.TDES_CFBP64_v1_0:
                    genVals = new TDES_CFBP.v1_0.RegisterInjections();
                    break;
                case AlgoMode.TDES_CTR_v1_0:
                    genVals = new TDES_CTR.v1_0.RegisterInjections();
                    break;
                case AlgoMode.TDES_ECB_v1_0:
                    genVals = new TDES_ECB.v1_0.RegisterInjections();
                    break;
                case AlgoMode.TDES_OFB_v1_0:
                    genVals = new TDES_OFB.v1_0.RegisterInjections();
                    break;
                case AlgoMode.TDES_OFBI_v1_0:
                    genVals = new TDES_OFBI.v1_0.RegisterInjections();
                    break;
                case AlgoMode.TupleHash_v1_0:
                    genVals = new TupleHash.v1_0.RegisterInjections();
                    break;
                // ^^^ -- Chris Algos -- ^^^

                default:
                    LogManager.GetCurrentClassLogger().Warn($"{nameof(algoMode)} ({algoMode}) cannot be attributed to the Single GenVals assembly, falling back to runtime loading.");
                    return false;
            }

            genVals.RegisterTypes(builder, algoMode);
            return true;
        }
    }
}