using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Threading.Tasks;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        Task<KdfResult> GetDeferredKdfCaseAsync(KdfParameters param);
        Task<KdfResult> CompleteDeferredKdfCaseAsync(KdfParameters param, KdfResult fullParam);

        Task<AnsiX963KdfResult> GetAnsiX963KdfCaseAsync(AnsiX963Parameters param);
        Task<AnsiX942KdfResult> GetAnsiX942KdfCaseAsync(AnsiX942Parameters param);
        Task<IkeV1KdfResult> GetIkeV1KdfCaseAsync(IkeV1KdfParameters param);
        Task<IkeV2KdfResult> GetIkeV2KdfCaseAsync(IkeV2KdfParameters param);
        Task<SnmpKdfResult> GetSnmpKdfCaseAsync(SnmpKdfParameters param);
        Task<SrtpKdfResult> GetSrtpKdfCaseAsync(SrtpKdfParameters param);
        Task<SshKdfResult> GetSshKdfCaseAsync(SshKdfParameters param);
        Task<TlsKdfResult> GetTlsKdfCaseAsync(TlsKdfParameters param);
        Task<TpmKdfResult> GetTpmKdfCaseAsync();
        
        Task<PbKdfResult> GetPbKdfCaseAsync(PbKdfParameters param);
        Task<HkdfResult> GetHkdfCaseAsync(HkdfParameters param);
        Task<TlsKdfv13Result> GetTlsv13CaseAsync(TlsKdfv13Parameters param);
    }
}
