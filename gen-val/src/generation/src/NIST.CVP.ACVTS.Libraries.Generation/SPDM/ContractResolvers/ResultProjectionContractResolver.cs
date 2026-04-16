using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.SPDM.ContractResolvers
{
    public class ResultProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.TestType),
                nameof(TestGroup.Tests),
                nameof(TestGroup.KeyLength),
                nameof(TestGroup.HashFunction),
                nameof(TestGroup.Version),
                nameof(TestGroup.UsesPSK)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = _ => true;
            }

            return jsonProperty.ShouldSerialize = _ => false;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.RequestHandshakeSecret),
                nameof(TestCase.ResponseHandshakeSecret),
                nameof(TestCase.RequestDataSecret),
                nameof(TestCase.ResponseDataSecret),
                nameof(TestCase.ExportMasterSecret),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = _ => true;
            }

            return jsonProperty.ShouldSerialize = _ => false;
        }
    }
}
