using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Orleans.Grains.Ecdsa
{
    public class EcdsaKeyGenRunner : IEcdsaKeyGenRunner
    {
        private readonly IEccCurveFactory _curveFactory;
        private readonly IDsaEccFactory _dsaFactory;

        public EcdsaKeyGenRunner(IEccCurveFactory curveFactory, IDsaEccFactory dsaFactory)
        {
            _curveFactory = curveFactory;
            _dsaFactory = dsaFactory;
        }

        public EcdsaKeyResult GenerateKey(EcdsaKeyParameters param)
        {
            var curve = _curveFactory.GetCurve(param.Curve);
            var domainParams = new EccDomainParameters(curve);

            // Hash function is not used, but the factory requires it
            var eccDsa = _dsaFactory.GetInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));

            var result = eccDsa.GenerateKeyPair(domainParams);
            if (!result.Success)
            {
                throw new Exception();
            }

            return new EcdsaKeyResult
            {
                Key = result.KeyPair
            };
        }
    }
}