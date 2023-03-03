using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.PersistedTypes;

public class PersistedLmsKeyPair : IResult
{
    [JsonIgnore]
    public LmsAttribute LmsAttribute { get; set; }
    
    [JsonIgnore]
    public LmOtsAttribute LmOtsAttribute { get; set; }
    
    [JsonProperty("i")]
    public BitString I { get; set; }
    
    [JsonProperty("seed")]
    public BitString Seed { get; set; }
    
    [JsonProperty("privateKey")]
    public PersistedLmsPrivateKey PrivateKey { get; set; }

    [JsonProperty("publicKey")]
    public PersistedLmsPublicKey PublicKey { get; set; }
}
