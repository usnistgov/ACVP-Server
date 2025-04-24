using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.XOF128;

public class TestGroup : ITestGroup<TestGroup, TestCase>
{
    public int TestGroupId { get; set; }
    public string TestType { get; set; }
    public List<TestCase> Tests { get; set; } = new List<TestCase>();
    [JsonIgnore]
    public MathDomain MessageLength { get; set; }
    [JsonIgnore]
    public MathDomain DigestLength { get; set; }
}

