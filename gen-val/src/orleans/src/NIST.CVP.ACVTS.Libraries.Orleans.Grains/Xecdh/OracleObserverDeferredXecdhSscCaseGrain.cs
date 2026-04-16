using System.Net.Mime;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Xecdh;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Xecdh
{
    public class OracleObserverDeferredXecdhSscCaseGrain : ObservableOracleGrainBase<XecdhSscResult>,
        IOracleObserverDeferredXecdhSscCaseGrain
    {
        private readonly IXecdhFactory _xecdhFactory;

        private XecdhSscParameters _param;

        public OracleObserverDeferredXecdhSscCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IXecdhFactory xecdhFactory
        ) : base(nonOrleansScheduler)
        {
            _xecdhFactory = xecdhFactory;
        }

        public async Task<bool> BeginWorkAsync(XecdhSscParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var xecdh = _xecdhFactory.GetXecdh(_param.Curve);

            var param = new XecdhKeyParameters()
            {
                Curve = _param.Curve,
                Disposition = XecdhKeyDisposition.None,
            };

            // Generate a server key pair
            var serverKeyPair = xecdh.GenerateKeyPair();
            var result = new XecdhSscResult()
            {
                PrivateKeyServer = serverKeyPair.KeyPair?.PrivateKey,
                PublicKeyServer = serverKeyPair.KeyPair?.PublicKey,
            };

            // Notify observers of result
            await Notify(result);
        }
    }
}
