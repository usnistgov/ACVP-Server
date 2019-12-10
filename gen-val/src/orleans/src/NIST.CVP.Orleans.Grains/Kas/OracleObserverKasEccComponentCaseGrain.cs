using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Kas;

namespace NIST.CVP.Orleans.Grains.Kas
{
    public class OracleObserverKasEccComponentCaseGrain : ObservableOracleGrainBase<KasEccComponentResult>, 
        IOracleObserverKasEccComponentCaseGrain
    {
        private readonly IEccCurveFactory _curveFactory;
        private readonly IDsaEccFactory _dsaFactory;
        private readonly IEccDhComponent _diffieHellman;

        private KasEccComponentParameters _param;

        public OracleObserverKasEccComponentCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEccCurveFactory curveFactory,
            IDsaEccFactory dsaFactory,
            IEccDhComponent diffieHellman
        ) : base (nonOrleansScheduler)
        {
            _curveFactory = curveFactory;
            _dsaFactory = dsaFactory;
            _diffieHellman = diffieHellman;
        }
        
        public async Task<bool> BeginWorkAsync(KasEccComponentParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var curve = _curveFactory.GetCurve(_param.Curve);
            var domainParameters = new EccDomainParameters(curve);

            var ecdsa = _dsaFactory.GetInstanceForKeys(new EntropyProvider(new Random800_90()));
            
            // Generate a server key pair
            var serverKeyPair = ecdsa.GenerateKeyPair(domainParameters);
            var result = new KasEccComponentResult()
            {
                PrivateKeyServer = serverKeyPair.KeyPair.PrivateD,
                PublicKeyServerX = serverKeyPair.KeyPair.PublicQ.X,
                PublicKeyServerY = serverKeyPair.KeyPair.PublicQ.Y
            };

            // Sample tests aren't deferred, calculate the "IUT" keypair and shared secret Z
            if (_param.IsSample)
            {
                // Generate the IUT key pair
                var iutKeyPair = ecdsa.GenerateKeyPair(new EccDomainParameters(curve));
                result.PrivateKeyIut = iutKeyPair.KeyPair.PrivateD;
                result.PublicKeyIutX = iutKeyPair.KeyPair.PublicQ.X;
                result.PublicKeyIutY = iutKeyPair.KeyPair.PublicQ.Y;

                // Generate the shared secret
                result.Z = _diffieHellman.GenerateSharedSecret(
                    domainParameters,
                    new EccKeyPair(result.PrivateKeyServer),
                    new EccKeyPair(new EccPoint(result.PublicKeyIutX, result.PublicKeyIutY))
                ).SharedSecretZ;
            }

            // Notify observers of result
            await Notify(result);
        }
    }
}