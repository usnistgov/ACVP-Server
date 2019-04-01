using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Eddsa;
using System;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Helpers;

namespace NIST.CVP.Orleans.Grains.Eddsa
{
    public class OracleObserverEddsaVerifySignatureCaseCaseGrain : ObservableOracleGrainBase<VerifyResult<EddsaSignatureResult>>, 
        IOracleObserverEddsaVerifySignatureCaseGrain
    {
        private readonly IEddsaKeyGenRunner _runner;
        private readonly IEdwardsCurveFactory _curveFactory;
        private readonly IDsaEdFactory _dsaFactory;
        private readonly IShaFactory _shaFactory;
        private readonly IRandom800_90 _rand;

        private EddsaSignatureParameters _param;

        public OracleObserverEddsaVerifySignatureCaseCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEddsaKeyGenRunner runner,
            IEdwardsCurveFactory curveFactory,
            IDsaEdFactory dsaFactory,
            IShaFactory shaFactory,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _runner = runner;
            _curveFactory = curveFactory;
            _dsaFactory = dsaFactory;
            _shaFactory = shaFactory;
            _rand = rand;
        }
        
        public async Task<bool> BeginWorkAsync(EddsaSignatureParameters param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var keyParam = new EddsaKeyParameters
            {
                Curve = _param.Curve
            };
            var key = _runner.GenerateKey(keyParam).Key;
            var curve = _curveFactory.GetCurve(_param.Curve);
            var domainParams = new EdDomainParameters(curve, _shaFactory);
            var edDsa = _dsaFactory.GetInstance(null);

            var message = _rand.GetRandomBitString(1024);

            var result = edDsa.Sign(domainParams, key, message);
            if (!result.Success)
            {
                throw new Exception();
            }

            var sigResult = new EddsaSignatureResult
            {
                Message = message,
                Key = key,
                Signature = result.Signature
            };

            if (_param.Disposition == EddsaSignatureDisposition.ModifyMessage)
            {
                // Generate a different random message
                sigResult.Message = _rand.GetDifferentBitStringOfSameSize(message);
            }
            else if (_param.Disposition == EddsaSignatureDisposition.ModifyKey)
            {
                // Generate a different key pair for the test case
                var keyResult = _runner.GenerateKey(keyParam).Key;
                sigResult.Key = keyResult;
            }
            else if (_param.Disposition == EddsaSignatureDisposition.ModifyR)
            {
                var decodedSig = SignatureDecoderHelper.DecodeSig(domainParams, sigResult.Signature);

                var modifiedRSignature = new EdSignature(
                    domainParams.CurveE.Encode(decodedSig.R).BitStringAddition(BitString.One()), 
                    new BitString(decodedSig.s)
                );
                sigResult.Signature = modifiedRSignature;
            }
            else if (_param.Disposition == EddsaSignatureDisposition.ModifyS)
            {
                var decodedSig = SignatureDecoderHelper.DecodeSig(domainParams, sigResult.Signature);

                var modifiedSSignature = new EdSignature(
                    domainParams.CurveE.Encode(decodedSig.R), 
                    new BitString(decodedSig.s).BitStringAddition(BitString.One())
                );
                sigResult.Signature = modifiedSSignature;
            }

            // Notify observers of result
            await Notify(new VerifyResult<EddsaSignatureResult>
            {
                Result = _param.Disposition == EddsaSignatureDisposition.None,
                VerifiedValue = sigResult
            });
        }
    }
}