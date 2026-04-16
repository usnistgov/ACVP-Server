using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.SPDM;

public class TestGroup : ITestGroup<TestGroup, TestCase>
{
    public int TestGroupId { get; set; }
    public string TestType { get; set; }
    
    [JsonIgnore]
    public MathDomain KeyLength { get; set; }

    [JsonIgnore]
    public MathDomain THLength { get; set; }

    public HashFunctions HashFunction { get; set; }
    public SPDMVersions Version { get; set; }
    public bool UsesPSK { get; set; }
    
    public List<TestCase> Tests { get; set; } = [];
}
