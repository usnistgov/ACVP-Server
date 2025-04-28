using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Xecdh;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Xecdh
{
    public class OracleObserverXecdhVerifyKeyCaseCaseGrain : ObservableOracleGrainBase<VerifyResult<XecdhKeyResult>>,
        IOracleObserverXecdhVerifyKeyCaseGrain
    {
        private readonly IXecdhFactory _xecdhFactory;
        private readonly IXecdhKeyGenRunner _runner;

        private XecdhKeyParameters _param;

        public OracleObserverXecdhVerifyKeyCaseCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IXecdhFactory xecdhFactory,
            IXecdhKeyGenRunner runner) : base(nonOrleansScheduler)
        {
            _xecdhFactory = xecdhFactory;
            _runner = runner;
        }

        public async Task<bool> BeginWorkAsync(XecdhKeyParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var xecdh = _xecdhFactory.GetXecdh(_param.Curve);

            var result = true;
            var key = _runner.GenerateKey(_param).Key;

            if (_param.Disposition == XecdhKeyDisposition.MsbSet)
            {
                // Note the public key is in little endian byte format.
                var modifiedPublicKey = key.PublicKey.ToBytes();
                // The most significant byte is the last byte.
                modifiedPublicKey[^1] |= 0x80;
                key = new XecdhKeyPair(new BitString(modifiedPublicKey), key.PrivateKey);

                // Public keys with the most significant bit set are still valid.
                result = true;
            }

            if (_param.Disposition == XecdhKeyDisposition.TooShort)
            {
                // Modify the public key value until it is too short
                var modifiedPublicKey = key.PublicKey;

                do
                {
                    modifiedPublicKey = modifiedPublicKey.Substring(0, modifiedPublicKey.BitLength - 8);
                    key = new XecdhKeyPair(modifiedPublicKey, key.PrivateKey);
                } while (xecdh.ValidateKeyPair(key).Success);
                result = false;
            }

            if (_param.Disposition == XecdhKeyDisposition.TooLong)
            {
                // Modify the public key value until it is too long
                var modifiedPublicKey = key.PublicKey;

                do
                {
                    modifiedPublicKey = modifiedPublicKey.ConcatenateBits(BitString.Ones(8));
                    key = new XecdhKeyPair(modifiedPublicKey, key.PrivateKey);
                } while (xecdh.ValidateKeyPair(key).Success);
                result = false;
            }

            // Notify observers of result
            await Notify(new VerifyResult<XecdhKeyResult>
            {
                Result = result,
                VerifiedValue = new XecdhKeyResult
                {
                    Key = key
                }
            });
        }
    }
}
