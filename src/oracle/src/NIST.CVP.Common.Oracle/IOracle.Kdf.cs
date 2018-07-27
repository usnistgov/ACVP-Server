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
    }
}
