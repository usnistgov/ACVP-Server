using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF.SP800_108r1.KMAC
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public List<TestCase> Tests { get; set; } = new();
        
        public MacModes MacMode { get; set; }
        
        [JsonIgnore]
        public MathDomain KeyDerivationKeyLength { get; set; }
        
        [JsonIgnore]
        public MathDomain ContextLength { get; set; }
        
        [JsonIgnore]
        public MathDomain LabelLength { get; set; }
        
        [JsonIgnore]
        public MathDomain DerivedKeyLength { get; set; }
    }
}
