using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Kyber;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap.ContractResolvers;

public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
{
    protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
    {
        var includeProperties = new []
        {
            nameof(TestGroup.TestGroupId), 
            nameof(TestGroup.TestType), 
            nameof(TestGroup.ParameterSet),
            nameof(TestGroup.Function),
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
            nameof(TestCase.TestCaseId)
        };
        
        var includeAllEncapProperties = new []
        {
            nameof(TestCase.EncapsulationKey)
        };
        
        var includeAlLDecapProperties = new []
        {
            nameof(TestCase.DecapsulationKey)
        };
        
        var includeEncapProperties = new[]
        {
            nameof(TestCase.SeedM)
        };
        
        var includeDecapProperties = new[]
        {
            nameof(TestCase.Ciphertext)
        };

        if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = _ => true;
        }

        if (includeEncapProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = instance =>
            {
                GetTestCaseFromTestCaseObject(instance, out var group, out var testCase);
                return group.Function == KyberFunction.Encapsulation;
            };
        }
        
        if (includeDecapProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = instance =>
            {
                GetTestCaseFromTestCaseObject(instance, out var group, out var testCase);
                return group.Function == KyberFunction.Decapsulation;
            };
        }
        
        if (includeAllEncapProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = instance =>
            {
                GetTestCaseFromTestCaseObject(instance, out var group, out var testCase);
                return group.Function is KyberFunction.Encapsulation or KyberFunction.EncapsulationKeyCheck;
            };
        }
        
        if (includeAlLDecapProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = instance =>
            {
                GetTestCaseFromTestCaseObject(instance, out var group, out var testCase);
                return group.Function is KyberFunction.Decapsulation or KyberFunction.DecapsulationKeyCheck;
            };
        }
        
        return jsonProperty.ShouldSerialize = _ => false;
    }
}
