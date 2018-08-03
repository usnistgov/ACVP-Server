using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Orleans.Grains.Interfaces;
using Orleans;

namespace NIST.CVP.Orleans.Grains
{
    public class OracleGrain : Grain, IOracleGrain
    {
        public AesResult CompleteDeferredAesCounterCase(CounterParameters<AesParameters> param)
        {
            throw new System.NotImplementedException();
        }

        public Task<AesResult> CompleteDeferredAesCounterCaseAsync(CounterParameters<AesParameters> param)
        {
            throw new System.NotImplementedException();
        }

        public AeadResult CompleteDeferredAesGcmCase(AeadParameters param, AeadResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<AeadResult> CompleteDeferredAesGcmCaseAsync(AeadParameters param, AeadResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public AeadResult CompleteDeferredAesXpnCase(AeadParameters param, AeadResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<AeadResult> CompleteDeferredAesXpnCaseAsync(AeadParameters param, AeadResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public VerifyResult<DsaKeyResult> CompleteDeferredDsaKey(DsaKeyParameters param, DsaKeyResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<VerifyResult<DsaKeyResult>> CompleteDeferredDsaKeyAsync(DsaKeyParameters param, DsaKeyResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public VerifyResult<DsaSignatureResult> CompleteDeferredDsaSignature(DsaSignatureParameters param, DsaSignatureResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<VerifyResult<DsaSignatureResult>> CompleteDeferredDsaSignatureAsync(DsaSignatureParameters param, DsaSignatureResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public EcdsaKeyResult CompleteDeferredEcdsaKey(EcdsaKeyParameters param, EcdsaKeyResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<EcdsaKeyResult> CompleteDeferredEcdsaKeyAsync(EcdsaKeyParameters param, EcdsaKeyResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public VerifyResult<EcdsaSignatureResult> CompleteDeferredEcdsaSignature(EcdsaSignatureParameters param, EcdsaSignatureResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<VerifyResult<EcdsaSignatureResult>> CompleteDeferredEcdsaSignatureAsync(EcdsaSignatureParameters param, EcdsaSignatureResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public KasEccComponentDeferredResult CompleteDeferredKasComponentTest(KasEccComponentDeferredParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KasEccComponentDeferredResult> CompleteDeferredKasComponentTestAsync(KasEccComponentDeferredParameters param)
        {
            throw new System.NotImplementedException();
        }

        public KasAftDeferredResult CompleteDeferredKasTest(KasAftDeferredParametersEcc param)
        {
            throw new System.NotImplementedException();
        }

        public KasAftDeferredResult CompleteDeferredKasTest(KasAftDeferredParametersFfc param)
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

        public KdfResult CompleteDeferredKdfCase(KdfParameters param, KdfResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<KdfResult> CompleteDeferredKdfCaseAsync(KdfParameters param, KdfResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public RsaDecryptionPrimitiveResult CompleteDeferredRsaDecryptionPrimitive(RsaDecryptionPrimitiveParameters param, RsaDecryptionPrimitiveResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<RsaDecryptionPrimitiveResult> CompleteDeferredRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param, RsaDecryptionPrimitiveResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public RsaKeyResult CompleteDeferredRsaKeyCase(RsaKeyParameters param, RsaKeyResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<RsaKeyResult> CompleteDeferredRsaKeyCaseAsync(RsaKeyParameters param, RsaKeyResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public VerifyResult<RsaSignatureResult> CompleteDeferredRsaSignature(RsaSignatureParameters param, RsaSignatureResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<VerifyResult<RsaSignatureResult>> CompleteDeferredRsaSignatureAsync(RsaSignatureParameters param, RsaSignatureResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public TdesResult CompleteDeferredTdesCounterCase(CounterParameters<TdesParameters> param)
        {
            throw new System.NotImplementedException();
        }

        public Task<TdesResult> CompleteDeferredTdesCounterCaseAsync(CounterParameters<TdesParameters> param)
        {
            throw new System.NotImplementedException();
        }

        public RsaKeyResult CompleteKey(RsaKeyResult param, PrivateKeyModes keyMode)
        {
            throw new System.NotImplementedException();
        }

        public Task<RsaKeyResult> CompleteKeyAsync(RsaKeyResult param, PrivateKeyModes keyMode)
        {
            throw new System.NotImplementedException();
        }

        public CounterResult ExtractIvs(TdesParameters param, TdesResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public CounterResult ExtractIvs(AesParameters param, AesResult fullParam)
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

        public AesResult GetAesCase(AesParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<AesResult> GetAesCaseAsync(AesParameters param)
        {
            throw new System.NotImplementedException();
        }

        public AeadResult GetAesCcmCase(AeadParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<AeadResult> GetAesCcmCaseAsync(AeadParameters param)
        {
            throw new System.NotImplementedException();
        }

        public AeadResult GetAesGcmCase(AeadParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<AeadResult> GetAesGcmCaseAsync(AeadParameters param)
        {
            throw new System.NotImplementedException();
        }

        public MctResult<AesResult> GetAesMctCase(AesParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<MctResult<AesResult>> GetAesMctCaseAsync(AesParameters param)
        {
            throw new System.NotImplementedException();
        }

        public AeadResult GetAesXpnCase(AeadParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<AeadResult> GetAesXpnCaseAsync(AeadParameters param)
        {
            throw new System.NotImplementedException();
        }

        public AesXtsResult GetAesXtsCase(AesXtsParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<AesXtsResult> GetAesXtsCaseAsync(AesXtsParameters param)
        {
            throw new System.NotImplementedException();
        }

        public AnsiX963KdfResult GetAnsiX963KdfCase(AnsiX963Parameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<AnsiX963KdfResult> GetAnsiX963KdfCaseAsync(AnsiX963Parameters param)
        {
            throw new System.NotImplementedException();
        }

        public MacResult GetCmacCase(CmacParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<MacResult> GetCmacCaseAsync(CmacParameters param)
        {
            throw new System.NotImplementedException();
        }

        public CShakeResult GetCShakeCase(CShakeParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<CShakeResult> GetCShakeCaseAsync(CShakeParameters param)
        {
            throw new System.NotImplementedException();
        }

        public MctResult<CShakeResult> GetCShakeMctCase(CShakeParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<MctResult<CShakeResult>> GetCShakeMctCaseAsync(CShakeParameters param)
        {
            throw new System.NotImplementedException();
        }

        public AesResult GetDeferredAesCounterCase(CounterParameters<AesParameters> param)
        {
            throw new System.NotImplementedException();
        }

        public Task<AesResult> GetDeferredAesCounterCaseAsync(CounterParameters<AesParameters> param)
        {
            throw new System.NotImplementedException();
        }

        public AeadResult GetDeferredAesGcmCase(AeadParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<AeadResult> GetDeferredAesGcmCaseAsync(AeadParameters param)
        {
            throw new System.NotImplementedException();
        }

        public AeadResult GetDeferredAesXpnCase(AeadParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<AeadResult> GetDeferredAesXpnCaseAsync(AeadParameters param)
        {
            throw new System.NotImplementedException();
        }

        public DsaSignatureResult GetDeferredDsaSignature(DsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<DsaSignatureResult> GetDeferredDsaSignatureAsync(DsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public EcdsaSignatureResult GetDeferredEcdsaSignature(EcdsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<EcdsaSignatureResult> GetDeferredEcdsaSignatureAsync(EcdsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public KdfResult GetDeferredKdfCase(KdfParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KdfResult> GetDeferredKdfCaseAsync(KdfParameters param)
        {
            throw new System.NotImplementedException();
        }

        public RsaDecryptionPrimitiveResult GetDeferredRsaDecryptionPrimitive(RsaDecryptionPrimitiveParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<RsaDecryptionPrimitiveResult> GetDeferredRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param)
        {
            throw new System.NotImplementedException();
        }

        public RsaSignatureResult GetDeferredRsaSignature(RsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<RsaSignatureResult> GetDeferredRsaSignatureAsync(RsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public TdesResult GetDeferredTdesCounterCase(CounterParameters<TdesParameters> param)
        {
            throw new System.NotImplementedException();
        }

        public Task<TdesResult> GetDeferredTdesCounterCaseAsync(CounterParameters<TdesParameters> param)
        {
            throw new System.NotImplementedException();
        }

        public Common.Oracle.ResultTypes.DrbgResult GetDrbgCase(DrbgParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<Common.Oracle.ResultTypes.DrbgResult> GetDrbgCaseAsync(DrbgParameters param)
        {
            throw new System.NotImplementedException();
        }

        public DsaDomainParametersResult GetDsaDomainParameters(DsaDomainParametersParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<DsaDomainParametersResult> GetDsaDomainParametersAsync(DsaDomainParametersParameters param)
        {
            throw new System.NotImplementedException();
        }

        public DsaDomainParametersResult GetDsaG(DsaDomainParametersParameters param, DsaDomainParametersResult pqParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<DsaDomainParametersResult> GetDsaGAsync(DsaDomainParametersParameters param, DsaDomainParametersResult pqParam)
        {
            throw new System.NotImplementedException();
        }

        public VerifyResult<DsaDomainParametersResult> GetDsaGVerify(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<VerifyResult<DsaDomainParametersResult>> GetDsaGVerifyAsync(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public DsaKeyResult GetDsaKey(DsaKeyParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<DsaKeyResult> GetDsaKeyAsync(DsaKeyParameters param)
        {
            throw new System.NotImplementedException();
        }

        public DsaDomainParametersResult GetDsaPQ(DsaDomainParametersParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<DsaDomainParametersResult> GetDsaPQAsync(DsaDomainParametersParameters param)
        {
            throw new System.NotImplementedException();
        }

        public VerifyResult<DsaDomainParametersResult> GetDsaPQVerify(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public Task<VerifyResult<DsaDomainParametersResult>> GetDsaPQVerifyAsync(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam)
        {
            throw new System.NotImplementedException();
        }

        public DsaSignatureResult GetDsaSignature(DsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<DsaSignatureResult> GetDsaSignatureAsync(DsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public EcdsaKeyResult GetEcdsaKey(EcdsaKeyParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<EcdsaKeyResult> GetEcdsaKeyAsync(EcdsaKeyParameters param)
        {
            throw new System.NotImplementedException();
        }

        public VerifyResult<EcdsaKeyResult> GetEcdsaKeyVerify(EcdsaKeyParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<VerifyResult<EcdsaKeyResult>> GetEcdsaKeyVerifyAsync(EcdsaKeyParameters param)
        {
            throw new System.NotImplementedException();
        }

        public EcdsaSignatureResult GetEcdsaSignature(EcdsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<EcdsaSignatureResult> GetEcdsaSignatureAsync(EcdsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public VerifyResult<EcdsaSignatureResult> GetEcdsaVerifyResult(EcdsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<VerifyResult<EcdsaSignatureResult>> GetEcdsaVerifyResultAsync(EcdsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public MacResult GetHmacCase(HmacParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<MacResult> GetHmacCaseAsync(HmacParameters param)
        {
            throw new System.NotImplementedException();
        }

        public IkeV1KdfResult GetIkeV1KdfCase(IkeV1KdfParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<IkeV1KdfResult> GetIkeV1KdfCaseAsync(IkeV1KdfParameters param)
        {
            throw new System.NotImplementedException();
        }

        public IkeV2KdfResult GetIkeV2KdfCase(IkeV2KdfParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<IkeV2KdfResult> GetIkeV2KdfCaseAsync(IkeV2KdfParameters param)
        {
            throw new System.NotImplementedException();
        }

        public KasAftResultEcc GetKasAftTestEcc(KasAftParametersEcc param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KasAftResultEcc> GetKasAftTestEccAsync(KasAftParametersEcc param)
        {
            throw new System.NotImplementedException();
        }

        public KasAftResultFfc GetKasAftTestFfc(KasAftParametersFfc param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KasAftResultFfc> GetKasAftTestFfcAsync(KasAftParametersFfc param)
        {
            throw new System.NotImplementedException();
        }

        public KasEccComponentResult GetKasEccComponentTest(KasEccComponentParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KasEccComponentResult> GetKasEccComponentTestAsync(KasEccComponentParameters param)
        {
            throw new System.NotImplementedException();
        }

        public KasValResultEcc GetKasValTestEcc(KasValParametersEcc param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KasValResultEcc> GetKasValTestEccAsync(KasValParametersEcc param)
        {
            throw new System.NotImplementedException();
        }

        public KasValResultFfc GetKasValTestFfc(KasValParametersFfc param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KasValResultFfc> GetKasValTestFfcAsync(KasValParametersFfc param)
        {
            throw new System.NotImplementedException();
        }

        public KeyWrapResult GetKeyWrapCase(KeyWrapParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KeyWrapResult> GetKeyWrapCaseAsync(KeyWrapParameters param)
        {
            throw new System.NotImplementedException();
        }

        public KmacResult GetKmacCase(KmacParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<KmacResult> GetKmacCaseAsync(KmacParameters param)
        {
            throw new System.NotImplementedException();
        }

        public ParallelHashResult GetParallelHashCase(ParallelHashParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<ParallelHashResult> GetParallelHashCaseAsync(ParallelHashParameters param)
        {
            throw new System.NotImplementedException();
        }

        public MctResult<ParallelHashResult> GetParallelHashMctCase(ParallelHashParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<MctResult<ParallelHashResult>> GetParallelHashMctCaseAsync(ParallelHashParameters param)
        {
            throw new System.NotImplementedException();
        }

        public RsaDecryptionPrimitiveResult GetRsaDecryptionPrimitive(RsaDecryptionPrimitiveParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<RsaDecryptionPrimitiveResult> GetRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param)
        {
            throw new System.NotImplementedException();
        }

        public RsaKeyResult GetRsaKey(RsaKeyParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<RsaKeyResult> GetRsaKeyAsync(RsaKeyParameters param)
        {
            throw new System.NotImplementedException();
        }

        public VerifyResult<RsaKeyResult> GetRsaKeyVerify(RsaKeyResult param)
        {
            throw new System.NotImplementedException();
        }

        public Task<VerifyResult<RsaKeyResult>> GetRsaKeyVerifyAsync(RsaKeyResult param)
        {
            throw new System.NotImplementedException();
        }

        public RsaSignatureResult GetRsaSignature(RsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<RsaSignatureResult> GetRsaSignatureAsync(RsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public RsaSignaturePrimitiveResult GetRsaSignaturePrimitive(RsaSignaturePrimitiveParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<RsaSignaturePrimitiveResult> GetRsaSignaturePrimitiveAsync(RsaSignaturePrimitiveParameters param)
        {
            throw new System.NotImplementedException();
        }

        public VerifyResult<RsaSignatureResult> GetRsaVerify(RsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<VerifyResult<RsaSignatureResult>> GetRsaVerifyAsync(RsaSignatureParameters param)
        {
            throw new System.NotImplementedException();
        }

        public HashResult GetSha3Case(Sha3Parameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<HashResult> GetSha3CaseAsync(Sha3Parameters param)
        {
            throw new System.NotImplementedException();
        }

        public MctResult<HashResult> GetSha3MctCase(Sha3Parameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<MctResult<HashResult>> GetSha3MctCaseAsync(Sha3Parameters param)
        {
            throw new System.NotImplementedException();
        }

        public HashResult GetShaCase(ShaParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<HashResult> GetShaCaseAsync(ShaParameters param)
        {
            throw new System.NotImplementedException();
        }

        public MctResult<HashResult> GetShaMctCase(ShaParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<MctResult<HashResult>> GetShaMctCaseAsync(ShaParameters param)
        {
            throw new System.NotImplementedException();
        }

        public SnmpKdfResult GetSnmpKdfCase(SnmpKdfParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<SnmpKdfResult> GetSnmpKdfCaseAsync(SnmpKdfParameters param)
        {
            throw new System.NotImplementedException();
        }

        public SrtpKdfResult GetSrtpKdfCase(SrtpKdfParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<SrtpKdfResult> GetSrtpKdfCaseAsync(SrtpKdfParameters param)
        {
            throw new System.NotImplementedException();
        }

        public SshKdfResult GetSshKdfCase(SshKdfParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<SshKdfResult> GetSshKdfCaseAsync(SshKdfParameters param)
        {
            throw new System.NotImplementedException();
        }

        public TdesResult GetTdesCase(TdesParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<TdesResult> GetTdesCaseAsync(TdesParameters param)
        {
            throw new System.NotImplementedException();
        }

        public MctResult<TdesResult> GetTdesMctCase(TdesParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<MctResult<TdesResult>> GetTdesMctCaseAsync(TdesParameters param)
        {
            throw new System.NotImplementedException();
        }

        public MctResult<TdesResultWithIvs> GetTdesMctWithIvsCase(TdesParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<MctResult<TdesResultWithIvs>> GetTdesMctWithIvsCaseAsync(TdesParameters param)
        {
            throw new System.NotImplementedException();
        }

        public TdesResultWithIvs GetTdesWithIvsCase(TdesParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<TdesResultWithIvs> GetTdesWithIvsCaseAsync(TdesParameters param)
        {
            throw new System.NotImplementedException();
        }

        public TlsKdfResult GetTlsKdfCase(TlsKdfParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<TlsKdfResult> GetTlsKdfCaseAsync(TlsKdfParameters param)
        {
            throw new System.NotImplementedException();
        }

        public TupleHashResult GetTupleHashCase(TupleHashParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<TupleHashResult> GetTupleHashCaseAsync(TupleHashParameters param)
        {
            throw new System.NotImplementedException();
        }

        public MctResult<TupleHashResult> GetTupleHashMctCase(TupleHashParameters param)
        {
            throw new System.NotImplementedException();
        }

        public Task<MctResult<TupleHashResult>> GetTupleHashMctCaseAsync(TupleHashParameters param)
        {
            throw new System.NotImplementedException();
        }
    }
}