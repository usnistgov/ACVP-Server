using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Xecdh
{
    public class XecdhKeyGenRunner : IXecdhKeyGenRunner
    {
        private readonly IXecdhFactory _xecdhFactory;

        public XecdhKeyGenRunner(
            IXecdhFactory xecdhFactory
        )
        {
            _xecdhFactory = xecdhFactory;
        }

        public XecdhKeyResult GenerateKey(XecdhKeyParameters param)
        {
            var xecdh = _xecdhFactory.GetXecdh(param.Curve);

            var result = xecdh.GenerateKeyPair();
            if (!result.Success)
            {
                throw new Exception();
            }

            return new XecdhKeyResult
            {
                Key = result.KeyPair
            };
        }
    }
}
