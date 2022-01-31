using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigVer
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; } = "AFT";

        public List<LmsType> LmsTypes { get; set; } = new List<LmsType>();
        public List<LmotsType> LmotsTypes { get; set; } = new List<LmotsType>();

        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        [JsonIgnore] public ITestCaseExpectationProvider<LmsSignatureDisposition> TestCaseExpectationProvider { get; set; }

        public override int GetHashCode()
        {
            var result = "";
            foreach (var lmsType in LmsTypes)
            {
                result += EnumHelpers.GetEnumDescriptionFromEnum(lmsType);
            }
            foreach (var lmotsType in LmotsTypes)
            {
                result += EnumHelpers.GetEnumDescriptionFromEnum(lmotsType);
            }
            return result.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is TestGroup otherGroup)
            {
                return GetHashCode() == otherGroup.GetHashCode();
            }

            return false;
        }
    }
}
