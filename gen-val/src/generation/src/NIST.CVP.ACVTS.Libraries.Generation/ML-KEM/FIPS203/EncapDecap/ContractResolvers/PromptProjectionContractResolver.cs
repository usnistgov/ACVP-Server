using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap.ContractResolvers;

public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
{
    protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
    {
        var includeProperties = new[]
        {
            nameof(TestGroup.TestGroupId), 
            nameof(TestGroup.TestType), 
            nameof(TestGroup.ParameterSet),
            nameof(TestGroup.Function),
            nameof(TestGroup.DecapsulationKey),
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
            nameof(TestCase.TestCaseId)
        };
        
        var includeAftProperties = new[]
        {
            nameof(TestCase.EncapsulationKey),
            nameof(TestCase.SeedM)
        };
        
        var includeValProperties = new[]
        {
            nameof(TestCase.Ciphertext)
        };

        if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = _ => true;
        }

        if (includeAftProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = instance =>
            {
                GetTestCaseFromTestCaseObject(instance, out var group, out var testCase);
                return group.TestType.Equals("AFT", StringComparison.OrdinalIgnoreCase);
            };
        }
        
        if (includeValProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = instance =>
            {
                GetTestCaseFromTestCaseObject(instance, out var group, out var testCase);
                return group.TestType.Equals("VAL", StringComparison.OrdinalIgnoreCase);
            };
        }
        
        return jsonProperty.ShouldSerialize = _ => false;
    }
}
