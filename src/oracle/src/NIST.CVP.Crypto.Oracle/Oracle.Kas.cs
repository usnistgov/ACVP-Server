using System;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.Oracle.KAS.Ecc;
using NIST.CVP.Crypto.Oracle.KAS.Ffc;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public KasValResultEcc GetKasValTestEcc(KasValParametersEcc param)
        {
            // TODO utilize oracle calls to ECDSA functions

            return new KasValEccTestGeneratorFactory()
                .GetInstance(param.KasMode)
                .GetTest(param);
        }

        public KasAftResultEcc GetKasAftTestEcc(KasAftParametersEcc param)
        {
            // TODO utilize oracle calls to ECDSA functions

            return new KasAftEccTestGeneratorFactory()
                .GetInstance(param.KasMode)
                .GetTest(param);
        }

        public KasAftDeferredResult CompleteDeferredKasTest(KasAftDeferredParametersEcc param)
        {
            return new KasAftEccDeferredTestResolverFactory()
                .GetInstance(param.KasMode)
                .CompleteTest(param);
        }

        public KasValResultFfc GetKasValTestFfc(KasValParametersFfc param)
        {
            // TODO utilize oracle calls to DSA functions

            return new KasValFfcTestGeneratorFactory()
                .GetInstance(param.KasMode)
                .GetTest(param);
        }

        public KasAftResultFfc GetKasAftTestFfc(KasAftParametersFfc param)
        {
            // TODO utilize oracle calls to DSA functions

            return new KasAftFfcTestGeneratorFactory()
                .GetInstance(param.KasMode)
                .GetTest(param);
        }

        public KasAftDeferredResult CompleteDeferredKasTest(KasAftDeferredParametersFfc param)
        {
            return new KasAftFfcDeferredTestResolverFactory()
                .GetInstance(param.KasMode)
                .CompleteTest(param);
        }

        public KasEccComponentResult GetKasEccComponentTest(KasEccComponentParameters param)
        {
            // TODO utilize oracle for ECDSA operations

            var curveFactory = new EccCurveFactory();
            var curve = curveFactory.GetCurve(param.Curve);
            var domainParameters = new EccDomainParameters(curve);

            // note hash function is used for signing/verifying - not relevant for use in this algo
            var ecdsa = new DsaEccFactory(new ShaFactory())
                .GetInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512));
            
            // Generate a server key pair
            var serverKeyPair = ecdsa.GenerateKeyPair(domainParameters);
            var result = new KasEccComponentResult()
            {
                PrivateKeyServer = serverKeyPair.KeyPair.PrivateD,
                PublicKeyServerX = serverKeyPair.KeyPair.PublicQ.X,
                PublicKeyServerY = serverKeyPair.KeyPair.PublicQ.Y
            };

            // Sample tests aren't deferred, calculate the "IUT" keypair and shared secret Z
            if (param.IsSample)
            {
                // Generate the IUT key pair
                var iutKeyPair = ecdsa.GenerateKeyPair(new EccDomainParameters(curve));
                result.PrivateKeyIut = iutKeyPair.KeyPair.PrivateD;
                result.PublicKeyIutX = iutKeyPair.KeyPair.PublicQ.X;
                result.PublicKeyIutY = iutKeyPair.KeyPair.PublicQ.Y;

                // Generate the shared secret
                result.Z = GenerateSharedSecretZ(
                    domainParameters, 
                    result.PrivateKeyServer,
                    new EccPoint(result.PublicKeyIutX, result.PublicKeyIutY)
                );
            }

            return result;
        }

        public KasEccComponentDeferredResult CompleteDeferredKasComponentTest(KasEccComponentDeferredParameters param)
        {
            var curveFactory = new EccCurveFactory();
            var curve = curveFactory.GetCurve(param.Curve);
            var domainParameters = new EccDomainParameters(curve);

            return new KasEccComponentDeferredResult()
            {
                Z = GenerateSharedSecretZ(
                    domainParameters, 
                    param.PrivateKeyServer,
                    new EccPoint(param.PublicKeyIutX, param.PublicKeyIutY)
                )
            };
        }

        public async Task<KasValResultEcc> GetKasValTestEccAsync(KasValParametersEcc param)
        {
            return await Task.Run(() => GetKasValTestEcc(param));
        }

        public async Task<KasAftResultEcc> GetKasAftTestEccAsync(KasAftParametersEcc param)
        {
            return await Task.Run(() => GetKasAftTestEcc(param));
        }

        public async Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersEcc param)
        {
            return await Task.Run(() => CompleteDeferredKasTest(param));
        }

        public async Task<KasValResultFfc> GetKasValTestFfcAsync(KasValParametersFfc param)
        {
            return await Task.Run(() => GetKasValTestFfc(param));
        }

        public async Task<KasAftResultFfc> GetKasAftTestFfcAsync(KasAftParametersFfc param)
        {
            return await Task.Run(() => GetKasAftTestFfc(param));
        }

        public async Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersFfc param)
        {
            return await Task.Run(() => CompleteDeferredKasTest(param));
        }

        public async Task<KasEccComponentResult> GetKasEccComponentTestAsync(KasEccComponentParameters param)
        {
            return await Task.Run(() => GetKasEccComponentTest(param));
        }

        public async Task<KasEccComponentDeferredResult> CompleteDeferredKasComponentTestAsync(KasEccComponentDeferredParameters param)
        {
            return await Task.Run(() => CompleteDeferredKasComponentTest(param));
        }

        private BitString GenerateSharedSecretZ(
            EccDomainParameters domainParameters, 
            BigInteger privateKeyPartyA,
            EccPoint publicKeyPartyB)
        {
            return new EccDhComponent(new DiffieHellmanEcc()).GenerateSharedSecret(
                domainParameters, 
                new EccKeyPair(privateKeyPartyA),
                new EccKeyPair(publicKeyPartyB)
            ).SharedSecretZ;
        }
    }
}
