using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kdf
{
    public class OracleObserverAnsiX942KdfCaseGrain : ObservableOracleGrainBase<AnsiX942KdfResult>, IOracleObserverAnsiX942KdfCaseGrain
    {
        private readonly IAnsiX942Factory _factory;
        private readonly IRandom800_90 _rand;

        private AnsiX942Parameters _param;

        public OracleObserverAnsiX942KdfCaseGrain(LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler, IAnsiX942Factory factory, IRandom800_90 rand) : base(nonOrleansScheduler)
        {
            _factory = factory;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(AnsiX942Parameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var kdf = _factory.GetInstance(_param.KdfMode, _param.HashAlg);
            var zz = _rand.GetRandomBitString(_param.ZzLen);

            IAns942Parameters param;
            AnsiX942KdfResult result;
            if (_param.KdfMode == AnsiX942Types.Concat)
            {
                var otherInfo = _rand.GetRandomBitString(_param.OtherInfoLen);

                param = new ConcatAns942Parameters
                {
                    KeyLen = _param.KeyLen,
                    Zz = zz,
                    OtherInfo = otherInfo
                };

                result = new AnsiX942KdfResult
                {
                    Zz = zz,
                    OtherInfo = otherInfo
                };
            }
            else
            {
                var u = _rand.GetRandomBitString(_param.SuppInfoLen);
                var v = _rand.GetRandomBitString(_param.SuppInfoLen);
                var pub = _rand.GetRandomBitString(_param.SuppInfoLen);
                var priv = _rand.GetRandomBitString(_param.SuppInfoLen);

                param = new DerAns942Parameters
                {
                    KeyLen = _param.KeyLen,
                    Oid = _param.Oid,
                    Zz = zz,
                    PartyUInfo = u,
                    PartyVInfo = v,
                    SuppPubInfo = pub,
                    SuppPrivInfo = priv
                };

                result = new AnsiX942KdfResult
                {
                    Zz = zz,
                    PartyUInfo = u,
                    PartyVInfo = v,
                    SuppPubInfo = pub,
                    SuppPrivInfo = priv
                };
            }

            var cryptoResult = kdf.DeriveKey(param);
            if (!cryptoResult.Success)
            {
                throw new Exception(cryptoResult.ErrorMessage);
            }

            result.DerivedKey = cryptoResult.DerivedKey;
            await Notify(result);
        }
    }
}
