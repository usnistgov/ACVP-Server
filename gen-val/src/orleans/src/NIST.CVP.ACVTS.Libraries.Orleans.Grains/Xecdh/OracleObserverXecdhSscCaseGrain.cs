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
    public class OracleObserverXecdhSscCaseGrain : ObservableOracleGrainBase<XecdhSscResult>,
        IOracleObserverXecdhSscCaseGrain
    {
        private readonly IXecdhFactory _xecdhFactory;
        private readonly IXecdhKeyGenRunner _runner;

        private XecdhSscParameters _param;

        public OracleObserverXecdhSscCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IXecdhFactory xecdhFactory,
            IXecdhKeyGenRunner runner
        ) : base(nonOrleansScheduler)
        {
            _xecdhFactory = xecdhFactory;
            _runner = runner;
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
            var serverKeyPair = _runner.GenerateKey(param);
            var result = new XecdhSscResult()
            {
                PrivateKeyServer = serverKeyPair.Key?.PrivateKey,
                PublicKeyServer = serverKeyPair.Key?.PublicKey,
            };

            // Sample tests aren't deferred, calculate the "IUT" keypair and shared secret Z
            if (_param.IsSample)
            {
                // Generate the IUT key pair
                var iutKeyPair = _runner.GenerateKey(param);
                result.PrivateKeyIut = iutKeyPair.Key?.PrivateKey;
                result.PublicKeyIut = iutKeyPair.Key?.PublicKey;

                // Generate the shared secret
                result.Z = xecdh.XECDH(result.PrivateKeyServer, result.PublicKeyIut);
            }

            // Notify observers of result
            await Notify(result);
        }
    }
}
