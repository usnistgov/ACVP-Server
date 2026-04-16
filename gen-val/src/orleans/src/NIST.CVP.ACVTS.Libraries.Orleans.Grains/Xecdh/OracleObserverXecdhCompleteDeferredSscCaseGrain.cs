using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Xecdh;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Xecdh
{
    public class OracleObserverXecdhCompleteDeferredSscCaseGrain : ObservableOracleGrainBase<XecdhSscDeferredResult>,
        IOracleObserverXecdhCompleteDeferredSscCaseGrain
    {
        private readonly IXecdhFactory _xecdhFactory;

        private XecdhSscDeferredParameters _param;

        public OracleObserverXecdhCompleteDeferredSscCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IXecdhFactory xecdhFactory
        ) : base(nonOrleansScheduler)
        {
            _xecdhFactory = xecdhFactory;
        }

        public async Task<bool> BeginWorkAsync(XecdhSscDeferredParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var xecdh = _xecdhFactory.GetXecdh(_param.Curve);

            // Notify observers of result
            await Notify(new XecdhSscDeferredResult()
            {
                Z = xecdh.XECDH(_param.PrivateKeyServer, _param.PublicKeyIut)
            });
        }
    }
}
