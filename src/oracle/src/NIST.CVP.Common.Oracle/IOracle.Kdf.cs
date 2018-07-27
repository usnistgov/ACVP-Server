using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        KdfResult GetDeferredKdfCase(KdfParameters param);
        KdfResult CompleteDeferredKdfCase(KdfParameters param, KdfResult fullParam);

        AnsiX963KdfResult GetAnsiX963KdfCase(AnsiX963Parameters param);
        IkeV1KdfResult GetIkeV1KdfCase(IkeV1KdfParameters param);
        IkeV2KdfResult GetIkeV2KdfCase(IkeV2KdfParameters param);
        SnmpKdfResult GetSnmpKdfCase(SnmpKdfParameters param);
        SrtpKdfResult GetSrtpKdfCase(SrtpKdfParameters param);
        SshKdfResult GetSshKdfCase(SshKdfParameters param);
        TlsKdfResult GetTlsKdfCase(TlsKdfParameters param);


        Task<KdfResult> GetDeferredKdfCaseAsync(KdfParameters param);
        Task<KdfResult> CompleteDeferredKdfCaseAsync(KdfParameters param, KdfResult fullParam);

        Task<AnsiX963KdfResult> GetAnsiX963KdfCaseAsync(AnsiX963Parameters param);
        Task<IkeV1KdfResult> GetIkeV1KdfCaseAsync(IkeV1KdfParameters param);
        Task<IkeV2KdfResult> GetIkeV2KdfCaseAsync(IkeV2KdfParameters param);
        Task<SnmpKdfResult> GetSnmpKdfCaseAsync(SnmpKdfParameters param);
        Task<SrtpKdfResult> GetSrtpKdfCaseAsync(SrtpKdfParameters param);
        Task<SshKdfResult> GetSshKdfCaseAsync(SshKdfParameters param);
        Task<TlsKdfResult> GetTlsKdfCaseAsync(TlsKdfParameters param);
    }
}
