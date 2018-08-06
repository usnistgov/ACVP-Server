using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces;
using Orleans;
using Orleans.Concurrency;

namespace NIST.CVP.Orleans.Grains
{
    [StatelessWorker, Reentrant]
    public class OracleGrain : Grain, IOracleGrain
    {
        #region not impl
        public Task<AesResult> CompleteDeferredAesCounterCaseAsync(CounterParameters<AesParameters> param)
        {
            throw new System.NotImplementedException();
        }

        public Task<AeadResult> CompleteDeferredAesGcmCaseAsync(AeadParameters param, AeadResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<AeadResult> CompleteDeferredAesXpnCaseAsync(AeadParameters param, AeadResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<VerifyResult<DsaKeyResult>> CompleteDeferredDsaKeyAsync(DsaKeyParameters param, DsaKeyResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<VerifyResult<DsaSignatureResult>> CompleteDeferredDsaSignatureAsync(DsaSignatureParameters param, DsaSignatureResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<EcdsaKeyResult> CompleteDeferredEcdsaKeyAsync(EcdsaKeyParameters param, EcdsaKeyResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<VerifyResult<EcdsaSignatureResult>> CompleteDeferredEcdsaSignatureAsync(EcdsaSignatureParameters param, EcdsaSignatureResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<KasEccComponentDeferredResult> CompleteDeferredKasComponentTestAsync(KasEccComponentDeferredParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersEcc param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersFfc param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KdfResult> CompleteDeferredKdfCaseAsync(KdfParameters param, KdfResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<RsaDecryptionPrimitiveResult> CompleteDeferredRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param, RsaDecryptionPrimitiveResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<RsaKeyResult> CompleteDeferredRsaKeyCaseAsync(RsaKeyParameters param, RsaKeyResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<VerifyResult<RsaSignatureResult>> CompleteDeferredRsaSignatureAsync(RsaSignatureParameters param, RsaSignatureResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<TdesResult> CompleteDeferredTdesCounterCaseAsync(CounterParameters<TdesParameters> param)
        {
            throw new System.NotImplementedException();
        }

        public Task<RsaKeyResult> CompleteKeyAsync(RsaKeyResult param, PrivateKeyModes keyMode)
        {
            throw new System.NotImplementedException();
        }

        public Task<CounterResult> ExtractIvsAsync(TdesParameters param, TdesResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<CounterResult> ExtractIvsAsync(AesParameters param, AesResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<AesResult> GetAesCaseAsync(AesParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<AeadResult> GetAesCcmCaseAsync(AeadParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<AeadResult> GetAesGcmCaseAsync(AeadParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<MctResult<AesResult>> GetAesMctCaseAsync(AesParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<AeadResult> GetAesXpnCaseAsync(AeadParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<AesXtsResult> GetAesXtsCaseAsync(AesXtsParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<AnsiX963KdfResult> GetAnsiX963KdfCaseAsync(AnsiX963Parameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<MacResult> GetCmacCaseAsync(CmacParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<CShakeResult> GetCShakeCaseAsync(CShakeParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<MctResult<CShakeResult>> GetCShakeMctCaseAsync(CShakeParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<AesResult> GetDeferredAesCounterCaseAsync(CounterParameters<AesParameters> param)
        {
            throw new System.NotImplementedException();
        }

        public Task<AeadResult> GetDeferredAesGcmCaseAsync(AeadParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<AeadResult> GetDeferredAesXpnCaseAsync(AeadParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<DsaSignatureResult> GetDeferredDsaSignatureAsync(DsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<EcdsaSignatureResult> GetDeferredEcdsaSignatureAsync(EcdsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KdfResult> GetDeferredKdfCaseAsync(KdfParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<RsaDecryptionPrimitiveResult> GetDeferredRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<RsaSignatureResult> GetDeferredRsaSignatureAsync(RsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<TdesResult> GetDeferredTdesCounterCaseAsync(CounterParameters<TdesParameters> param)
        {
            throw new System.NotImplementedException();
        }

        public Task<Common.Oracle.ResultTypes.DrbgResult> GetDrbgCaseAsync(DrbgParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<DsaDomainParametersResult> GetDsaDomainParametersAsync(DsaDomainParametersParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<DsaDomainParametersResult> GetDsaGAsync(DsaDomainParametersParameters param, DsaDomainParametersResult pqParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<VerifyResult<DsaDomainParametersResult>> GetDsaGVerifyAsync(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<DsaKeyResult> GetDsaKeyAsync(DsaKeyParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<DsaDomainParametersResult> GetDsaPQAsync(DsaDomainParametersParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<VerifyResult<DsaDomainParametersResult>> GetDsaPQVerifyAsync(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<DsaSignatureResult> GetDsaSignatureAsync(DsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<EcdsaKeyResult> GetEcdsaKeyAsync(EcdsaKeyParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<VerifyResult<EcdsaKeyResult>> GetEcdsaKeyVerifyAsync(EcdsaKeyParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<EcdsaSignatureResult> GetEcdsaSignatureAsync(EcdsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<VerifyResult<EcdsaSignatureResult>> GetEcdsaVerifyResultAsync(EcdsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<MacResult> GetHmacCaseAsync(HmacParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<IkeV1KdfResult> GetIkeV1KdfCaseAsync(IkeV1KdfParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<IkeV2KdfResult> GetIkeV2KdfCaseAsync(IkeV2KdfParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KasAftResultEcc> GetKasAftTestEccAsync(KasAftParametersEcc param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KasAftResultFfc> GetKasAftTestFfcAsync(KasAftParametersFfc param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KasEccComponentResult> GetKasEccComponentTestAsync(KasEccComponentParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KasValResultEcc> GetKasValTestEccAsync(KasValParametersEcc param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KasValResultFfc> GetKasValTestFfcAsync(KasValParametersFfc param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KeyWrapResult> GetKeyWrapCaseAsync(KeyWrapParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KmacResult> GetKmacCaseAsync(KmacParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<ParallelHashResult> GetParallelHashCaseAsync(ParallelHashParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<MctResult<ParallelHashResult>> GetParallelHashMctCaseAsync(ParallelHashParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<RsaDecryptionPrimitiveResult> GetRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<RsaKeyResult> GetRsaKeyAsync(RsaKeyParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<VerifyResult<RsaKeyResult>> GetRsaKeyVerifyAsync(RsaKeyResult param)
        {
            throw new System.NotImplementedException();
        }

        public Task<RsaSignatureResult> GetRsaSignatureAsync(RsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<RsaSignaturePrimitiveResult> GetRsaSignaturePrimitiveAsync(RsaSignaturePrimitiveParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<VerifyResult<RsaSignatureResult>> GetRsaVerifyAsync(RsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<HashResult> GetSha3CaseAsync(Sha3Parameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<MctResult<HashResult>> GetSha3MctCaseAsync(Sha3Parameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<HashResult> GetShaCaseAsync(ShaParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<MctResult<HashResult>> GetShaMctCaseAsync(ShaParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<SnmpKdfResult> GetSnmpKdfCaseAsync(SnmpKdfParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<SrtpKdfResult> GetSrtpKdfCaseAsync(SrtpKdfParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<SshKdfResult> GetSshKdfCaseAsync(SshKdfParameters param)
        {
            throw new System.NotImplementedException();
        }
        #endregion not impl

        private readonly BlockCipherEngineFactory _engineFactory = new BlockCipherEngineFactory();
        private readonly ModeBlockCipherFactory _modeFactory = new ModeBlockCipherFactory();
        private readonly TdesMonteCarloFactory _tdesMctFactory = new TdesMonteCarloFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory());
        private readonly Random800_90 _rand = new Random800_90();

        public async Task<TdesResult> GetTdesCaseAsync(TdesParameters param)
        {
            return await Task.Run(() =>
            {
                Console.WriteLine(param);
                var cipher = _modeFactory.GetStandardCipher(
                    _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes), 
                    param.Mode
                );
                var direction = BlockCipherDirections.Encrypt;
                if (param.Direction.ToLower() == "decrypt")
                {
                    direction = BlockCipherDirections.Decrypt;
                }

                var payload = _rand.GetRandomBitString(param.DataLength);
                var key = TdesHelpers.GenerateTdesKey(param.KeyingOption);
                var iv = _rand.GetRandomBitString(64);

                var blockCipherParams = new ModeBlockCipherParameters(direction, iv, key, payload);
                var result = cipher.ProcessPayload(blockCipherParams);

                if (!result.Success)
                {
                    // Log error somewhere
                    throw new Exception();
                }

                return new TdesResult
                {
                    PlainText = direction == BlockCipherDirections.Encrypt ? payload : result.Result,
                    CipherText = direction == BlockCipherDirections.Decrypt ? payload : result.Result,
                    Key = key,
                    Iv = iv
                };
            });
        }

        public async Task<MctResult<TdesResult>> GetTdesMctCaseAsync(TdesParameters param)
        {
            return await Task.Run(() =>
            {
                Console.WriteLine(param);
                var cipher = _tdesMctFactory.GetInstance(param.Mode);
                var direction = BlockCipherDirections.Encrypt;
                if (param.Direction.ToLower() == "decrypt")
                {
                    direction = BlockCipherDirections.Decrypt;
                }

                var payload = _rand.GetRandomBitString(param.DataLength);
                var key = TdesHelpers.GenerateTdesKey(param.KeyingOption);
                var iv = _rand.GetRandomBitString(64);

                var blockCipherParams = new ModeBlockCipherParameters(direction, iv, key, payload);
                var result = cipher.ProcessMonteCarloTest(blockCipherParams);

                if (!result.Success)
                {
                    // Log error somewhere
                    throw new Exception();
                }

                return new MctResult<TdesResult>
                {
                    Results = Array.ConvertAll(result.Response.ToArray(), element => new TdesResult
                    {
                        Key = element.Keys,
                        Iv = element.IV,
                        PlainText = element.PlainText,
                        CipherText = element.CipherText
                    }).ToList()
                };
            });
        }

        #region 
        public Task<MctResult<TdesResultWithIvs>> GetTdesMctWithIvsCaseAsync(TdesParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<TdesResultWithIvs> GetTdesWithIvsCaseAsync(TdesParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<TlsKdfResult> GetTlsKdfCaseAsync(TlsKdfParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<TupleHashResult> GetTupleHashCaseAsync(TupleHashParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<MctResult<TupleHashResult>> GetTupleHashMctCaseAsync(TupleHashParameters param)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}