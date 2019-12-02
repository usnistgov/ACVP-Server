using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.SHAWrapper;


namespace NIST.CVP.Orleans.Grains.Eddsa
{
    public class EddsaKeyGenRunner : IEddsaKeyGenRunner
    {
        private readonly IEdwardsCurveFactory _curveFactory;
        private readonly IDsaEdFactory _dsaFactory;
        private readonly IShaFactory _shaFactory;

        public EddsaKeyGenRunner(
            IEdwardsCurveFactory curveFactory, 
            IDsaEdFactory dsaFactory,
            IShaFactory shaFactory
        )
        {
            _curveFactory = curveFactory;
            _dsaFactory = dsaFactory;
            _shaFactory = shaFactory;
        }

        public EddsaKeyResult GenerateKey(EddsaKeyParameters param)
        {
            var curve = _curveFactory.GetCurve(param.Curve);
            var domainParams = new EdDomainParameters(curve, _shaFactory);

            // Hash function is not used, but the factory requires it
            var edDsa = _dsaFactory.GetInstance(null);

            var result = edDsa.GenerateKeyPair(domainParams);
            if (!result.Success)
            {
                throw new Exception();
            }

            return new EddsaKeyResult
            {
                Key = result.KeyPair
            };
        }
    }
}