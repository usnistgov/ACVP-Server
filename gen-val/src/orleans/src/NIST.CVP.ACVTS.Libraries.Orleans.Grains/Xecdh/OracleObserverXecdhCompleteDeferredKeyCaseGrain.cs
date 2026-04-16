using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Xecdh;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Xecdh
{
    public class OracleObserverXecdhCompleteDeferredKeyCaseGrain : ObservableOracleGrainBase<XecdhKeyResult>,
        IOracleObserverXecdhCompleteDeferredKeyCaseGrain
    {
        private readonly IXecdhFactory _xecdhFactory;

        private XecdhKeyParameters _param;
        private XecdhKeyResult _fullParam;

        public OracleObserverXecdhCompleteDeferredKeyCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IXecdhFactory xecdhFactory
        ) : base(nonOrleansScheduler)
        {
            _xecdhFactory = xecdhFactory;
        }

        public async Task<bool> BeginWorkAsync(XecdhKeyParameters param, XecdhKeyResult fullParam)
        {
            _param = param;
            _fullParam = fullParam;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var xecdh = _xecdhFactory.GetXecdh(_param.Curve);

            var privateKey = _fullParam.Key?.PrivateKey;

            var result = xecdh.DeriveKeyPair(privateKey);

            // Check this before Generating, validate their provided values
            if (!result.Success)
            {
                throw new ArgumentException(result.ErrorMessage);
            }

            // Notify observers of result
            await Notify(new XecdhKeyResult
            {
                Key = result.KeyPair
            });
        }
    }
}
