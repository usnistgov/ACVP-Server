using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Ar3
{
    public abstract class TestCaseBase<TTestGroup, TTestCase, TKeyPair> : ITestCase<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>, new()
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>, new()
        where TKeyPair : IDsaKeyPair
    {
        public int TestCaseId { get; set; }
        public TTestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }

        [JsonIgnore] public abstract TKeyPair StaticKeyServer { get; set; }
        [JsonIgnore] public abstract TKeyPair EphemeralKeyServer { get; set; }
        [JsonIgnore] public abstract TKeyPair StaticKeyIut { get; set; }
        [JsonIgnore] public abstract TKeyPair EphemeralKeyIut { get; set; }

        public KasSscTestCaseExpectation TestCaseDisposition { get; set; }

        public BitString Z { get; set; }
        public BitString HashZ { get; set; }
    }
}
