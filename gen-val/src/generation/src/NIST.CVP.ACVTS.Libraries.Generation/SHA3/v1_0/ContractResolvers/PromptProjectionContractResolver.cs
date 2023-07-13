using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.v1_0.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var shakeMctProperties = new[]
            {
                nameof(TestGroup.MinOutputLength),
                nameof(TestGroup.MaxOutputLength),
                nameof(TestGroup.MctVersion)
            };
            
            var mctProperties = new[]
            {
                nameof(TestGroup.MinOutputLength),
                nameof(TestGroup.MaxOutputLength),
                nameof(TestGroup.MctVersion)
            };

            #region Conditional Test Group properties
            if (shakeMctProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestGroupFromTestGroupObject(instance, out var testGroup);

                    if (testGroup.TestType.Equals("mct", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    return false;
                };
            }
            
            if (mctProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestGroupFromTestGroupObject(instance, out var testGroup);

                    if (testGroup.Function == ModeValues.SHA3 &&
                        testGroup.TestType.Equals("mct", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    return false;
                };
            }
            #endregion Conditional Test Group properties

            return jsonProperty.ShouldSerialize = instance => true;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var allProperties = new[]
            {
                nameof(TestCase.TestCaseId)
            };

            var aftMctVotProperties = new[]
            {
                nameof(TestCase.Message),
                nameof(TestCase.MessageLength)
            };

            var shakeVotAftProperties = new[]
            {
                nameof(TestCase.DigestLength)
            };

            var ldtProperties = new[]
            {
                nameof(TestCase.LargeMessage)
            };

            if (allProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance => true;
            }

            #region Conditional Test Group properties
            if (aftMctVotProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                    return testGroup.TestType.ToLower() == "mct" || testGroup.TestType.ToLower() == "aft" || testGroup.TestType.ToLower() == "vot";
                };
            }

            if (shakeVotAftProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                    if (testGroup.Function == ModeValues.SHAKE &&
                        (testGroup.TestType.Equals("vot", StringComparison.OrdinalIgnoreCase) || testGroup.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase)))
                    {
                        return true;
                    }

                    return false;
                };
            }

            if (ldtProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                    return testGroup.TestType.ToLower() == "ldt";
                };
            }
            #endregion Conditional Test Group properties

            return jsonProperty.ShouldSerialize = instance => false;
        }
    }
}
