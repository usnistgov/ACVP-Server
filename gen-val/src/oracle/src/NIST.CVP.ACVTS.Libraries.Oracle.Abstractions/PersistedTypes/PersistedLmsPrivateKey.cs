using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.PersistedTypes;

public class PersistedLmsPrivateKey
{
    [JsonProperty("x")]
    public int X { get; init; }

    [JsonProperty("tree")]
    public List<BitString> Tree { get; init; }
}
