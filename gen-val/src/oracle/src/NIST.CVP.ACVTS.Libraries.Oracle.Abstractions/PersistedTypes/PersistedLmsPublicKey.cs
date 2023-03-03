using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.PersistedTypes;

public class PersistedLmsPublicKey
{
    [JsonProperty("key")]
    public BitString FormattedKey { get; init; }
}
