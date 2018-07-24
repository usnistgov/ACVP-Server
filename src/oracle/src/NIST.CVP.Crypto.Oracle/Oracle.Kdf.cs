using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.KDF;
using System;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly KdfFactory _kdfFactory = new KdfFactory();

        public KdfResult GetDeferredKdfCase(KdfParameters param)
        {
            return new KdfResult
            {
                KeyIn = _rand.GetRandomBitString(128),
                Iv = _rand.GetRandomBitString(param.ZeroLengthIv ? 0 : 128),
                FixedData = _rand.GetRandomBitString(128),
                BreakLocation = _rand.GetRandomInt(1, 128)
            };
        }

        public KdfResult CompleteDeferredKdfCase(KdfParameters param, KdfResult fullParam)
        {
            var kdf = _kdfFactory.GetKdfInstance(param.Mode, param.MacMode, param.CounterLocation, param.CounterLength);

            var result = kdf.DeriveKey(fullParam.KeyIn, fullParam.FixedData, param.KeyOutLength, fullParam.Iv, fullParam.BreakLocation);

            if (!result.Success)
            {
                throw new Exception();
            }

            return new KdfResult
            {
                KeyOut = result.DerivedKey
            };
        }

        // All the components individually probably, but those are straight-forward input/output
    }
}
