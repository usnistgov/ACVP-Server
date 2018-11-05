using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.Orleans.Grains.Rsa
{
    public class OracleObserverRsaSignatureCaseGrain : ObservableOracleGrainBase<RsaSignatureResult>, 
        IOracleObserverRsaSignatureCaseGrain
    {
        private readonly IRsa _rsa;
        private readonly IShaFactory _shaFactory;
        private readonly IPaddingFactory _paddingFactory;
        private readonly IRandom800_90 _rand;

        private RsaSignatureParameters _param;

        public OracleObserverRsaSignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IRsa rsa,
            IShaFactory shaFactory,
            IPaddingFactory paddingFactory,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _rsa = rsa;
            _shaFactory = shaFactory;
            _paddingFactory = paddingFactory;
            _rand = rand;
        }
        
        public async Task<bool> BeginWorkAsync(RsaSignatureParameters param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var message = _rand.GetRandomBitString(_param.Modulo / 2);
            var sha = _shaFactory.GetShaInstance(_param.HashAlg);
            var salt = _rand.GetRandomBitString(_param.SaltLength * 8); // Comes in bytes, convert to bits
            var entropyProvider = new TestableEntropyProvider();
            entropyProvider.AddEntropy(salt);

            var paddingScheme = _paddingFactory.GetPaddingScheme(_param.PaddingScheme, sha, entropyProvider, _param.SaltLength);

            var result = new SignatureBuilder()
                .WithDecryptionScheme(_rsa)
                .WithMessage(message)
                .WithPaddingScheme(paddingScheme)
                .WithKey(_param.Key)
                .BuildSign();

            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new RsaSignatureResult
            {
                Message = message,
                Signature = new BitString(result.Signature),
                Salt = _param.PaddingScheme == SignatureSchemes.Pss ? salt : null
            });
        }
    }
}