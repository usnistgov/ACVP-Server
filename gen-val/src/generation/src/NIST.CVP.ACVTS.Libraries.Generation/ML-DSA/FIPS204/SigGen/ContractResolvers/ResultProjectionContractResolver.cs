using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen.ContractResolvers;

public class ResultProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
{
    protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
    {
        var includeProperties = new[]
        {
            nameof(TestGroup.TestGroupId),
            nameof(TestGroup.Tests)
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
            nameof(TestCase.Signature),
        };

        if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = _ => true;
        }
        
        return jsonProperty.ShouldSerialize = _ => false;
    }
}
