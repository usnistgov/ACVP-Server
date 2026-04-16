using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH;
using NIST.CVP.ACVTS.Libraries.Crypto.XECDH;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Xecdh;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Xecdh
{
    public class OracleObserverXecdhKeyCaseCaseGrain : ObservableOracleGrainBase<XecdhKeyResult>,
        IOracleObserverXecdhKeyCaseGrain
    {

        private XecdhKeyParameters _param;
        private readonly IXecdhFactory _xecdhFactory;

        public OracleObserverXecdhKeyCaseCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler
        ) : base(nonOrleansScheduler)
        {
            _xecdhFactory = new XecdhFactory();
        }

        public async Task<bool> BeginWorkAsync(XecdhKeyParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            // Notify observers of result
            var keyResult = new XecdhKeyResult
            {
                Key = _xecdhFactory.GetXecdh(_param.Curve).GenerateKeyPair().KeyPair
            };
            await Notify(keyResult);
        }
    }
}
