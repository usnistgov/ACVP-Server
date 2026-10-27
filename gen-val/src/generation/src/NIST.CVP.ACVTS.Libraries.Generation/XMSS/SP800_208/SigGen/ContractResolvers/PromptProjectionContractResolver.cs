using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.SigGen.ContractResolvers;

public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
{
    protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
    {
        var includeProperties = new[]
        {
            nameof(TestGroup.TestGroupId),
            nameof(TestGroup.TestType),
            nameof(TestGroup.XmssMode),
            nameof(TestGroup.Tests),
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
            nameof(TestCase.Message),
            nameof(TestCase.MessageLength)
        };

        if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = _ => true;
        }

        return jsonProperty.ShouldSerialize = _ => false;
    }
}
