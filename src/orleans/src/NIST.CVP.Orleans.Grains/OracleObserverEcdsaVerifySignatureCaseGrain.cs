using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Ecdsa;
using NIST.CVP.Orleans.Grains.Interfaces;

namespace NIST.CVP.Orleans.Grains
{
    public class OracleObserverEcdsaVerifySignatureCaseCaseGrain : ObservableOracleGrainBase<VerifyResult<EcdsaSignatureResult>>, 
        IOracleObserverEcdsaVerifySignatureCaseGrain
    {
        private readonly IEcdsaKeyGenRunner _runner;
        private readonly IEccCurveFactory _curveFactory;
        private readonly IDsaEccFactory _dsaFactory;
        private readonly IRandom800_90 _rand;

        private EcdsaSignatureParameters _param;

        public OracleObserverEcdsaVerifySignatureCaseCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEcdsaKeyGenRunner runner,
            IEccCurveFactory curveFactory,
            IDsaEccFactory dsaFactory,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _runner = runner;
            _curveFactory = curveFactory;
            _dsaFactory = dsaFactory;
            _rand = rand;
        }
        
        public async Task<bool> BeginWorkAsync(EcdsaSignatureParameters param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var keyParam = new EcdsaKeyParameters
            {
                Curve = _param.Curve
            };
            var key = _runner.GenerateKey(keyParam).Key;
            var curve = _curveFactory.GetCurve(_param.Curve);
            var domainParams = new EccDomainParameters(curve);
            var eccDsa = _dsaFactory.GetInstance(_param.HashAlg);

            var message = _rand.GetRandomBitString(1024);

            var result = eccDsa.Sign(domainParams, key, message);
            if (!result.Success)
            {
                throw new Exception();
            }

            var sigResult = new EcdsaSignatureResult
            {
                Message = message,
                Key = key,
                Signature = result.Signature
            };

            if (_param.Disposition == EcdsaSignatureDisposition.ModifyMessage)
            {
                // Generate a different random message
                sigResult.Message = _rand.GetDifferentBitStringOfSameSize(message);
            }
            else if (_param.Disposition == EcdsaSignatureDisposition.ModifyKey)
            {
                // Generate a different key pair for the test case
                var keyResult = _runner.GenerateKey(keyParam).Key;
                sigResult.Key = keyResult;
            }
            else if (_param.Disposition == EcdsaSignatureDisposition.ModifyR)
            {
                var modifiedRSignature = new EccSignature(sigResult.Signature.R + 1, sigResult.Signature.S);
                sigResult.Signature = modifiedRSignature;
            }
            else if (_param.Disposition == EcdsaSignatureDisposition.ModifyS)
            {
                var modifiedSSignature = new EccSignature(sigResult.Signature.R, sigResult.Signature.S + 1);
                sigResult.Signature = modifiedSSignature;
            }

            // Notify observers of result
            await Notify(new VerifyResult<EcdsaSignatureResult>
            {
                Result = _param.Disposition == EcdsaSignatureDisposition.None,
                VerifiedValue = sigResult
            });
        }
    }
}