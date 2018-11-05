using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Dsa;

namespace NIST.CVP.Orleans.Grains.Dsa
{
    public class OracleObserverDsaSignatureCaseGrain : ObservableOracleGrainBase<DsaSignatureResult>, 
        IOracleObserverDsaSignatureCaseGrain
    {

        private readonly IDsaFfcFactory _dsaFfcFactory;
        private readonly IRandom800_90 _rand;

        private DsaSignatureParameters _param;

        public OracleObserverDsaSignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IDsaFfcFactory dsaFfcFactory,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _dsaFfcFactory = dsaFfcFactory;
            _rand = rand;
        }
        
        public async Task<bool> BeginWorkAsync(DsaSignatureParameters param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var message = _rand.GetRandomBitString(_param.MessageLength);

            var ffcDsa = _dsaFfcFactory.GetInstance(_param.HashAlg);
            var sigResult = ffcDsa.Sign(_param.DomainParameters, _param.Key, message);
            if (!sigResult.Success)
            {
                throw new Exception();
            }

            var result = new DsaSignatureResult
            {
                Message = message,
                Signature = sigResult.Signature,
                Key = _param.Key
            };

            if (_param.Disposition == DsaSignatureDisposition.None)
            {
                await Notify(result);
                return;
            }

            // Modify message
            if (_param.Disposition == DsaSignatureDisposition.ModifyMessage)
            {
                result.Message = _rand.GetDifferentBitStringOfSameSize(message);
            }
            // Modify public key
            else if (_param.Disposition == DsaSignatureDisposition.ModifyKey)
            {
                var x = result.Key.PrivateKeyX;
                var y = result.Key.PublicKeyY + 2;
                result.Key = new FfcKeyPair(x, y);
            }
            // Modify r
            else if (_param.Disposition == DsaSignatureDisposition.ModifyR)
            {
                var s = result.Signature.S;
                var r = result.Signature.R + 2;
                result.Signature = new FfcSignature(s, r);
            }
            // Modify s
            else if (_param.Disposition == DsaSignatureDisposition.ModifyS)
            {
                var s = result.Signature.S + 2;
                var r = result.Signature.R;
                result.Signature = new FfcSignature(s, r);
            }

            // Notify observers of result
            await Notify(result);
        }
    }
}