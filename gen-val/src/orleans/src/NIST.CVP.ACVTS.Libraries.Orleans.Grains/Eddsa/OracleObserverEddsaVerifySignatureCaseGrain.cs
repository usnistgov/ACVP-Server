using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Eddsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Eddsa
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
        ) : base(nonOrleansScheduler)
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
