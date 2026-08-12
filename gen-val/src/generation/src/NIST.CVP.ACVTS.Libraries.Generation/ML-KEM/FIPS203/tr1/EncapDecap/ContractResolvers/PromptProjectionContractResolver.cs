using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.MLKEM;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.tr1.EncapDecap.ContractResolvers;

public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
{
    protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
    {
        // Always include
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
        
        // Only include for function = Decapsulation, and function = DecapsulationKeyCheck
        // Note, this value will only ever be "expanded" for function = DecapsulationKeyCheck, that is OK, we want to be clear to the user.
        var includeDecapProperties = new []
        {
            nameof(TestGroup.KeyFormat)
        };
        
        if (includeDecapProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = instance =>
            {
                GetTestGroupFromTestGroupObject(instance, out var group);
                return group.Function is MLKEMFunction.Decapsulation or MLKEMFunction.DecapsulationKeyCheck;
            };
        }

        return jsonProperty.ShouldSerialize = _ => false;
    }

    protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
    {
        // Applies to all test cases
        var alwaysIncludeProperties = new[]
        {
            nameof(TestCase.TestCaseId)
        };
        
        if (alwaysIncludeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = _ => true;
        }
        
        // Applies to function = Encapsulation and EncapsulationKeyChecks
        var alwaysIncludeEncapProperties = new []
        {
            nameof(TestCase.EncapsulationKey)
        };
        
        if (alwaysIncludeEncapProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = instance =>
            {
                GetTestCaseFromTestCaseObject(instance, out var group, out _);
                return group.Function is MLKEMFunction.Encapsulation or MLKEMFunction.EncapsulationKeyCheck;
            };
        }
        
        // Applies to function = Encapsulation
        var includeEncapProperties = new[]
        {
            nameof(TestCase.SeedM)
        };
        
        if (includeEncapProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = instance =>
            {
                GetTestCaseFromTestCaseObject(instance, out var group, out _);
                return group.Function == MLKEMFunction.Encapsulation;
            };
        }
        
        // Applies to function = Decapsulation and KeyFormat = Seed
        // Note that function = DecapsulationKeyCheck and KeyFormat = Seed is not a valid combination.
        var includeDecapSeedKeyProperties = new []
        {
            nameof(TestCase.SeedD),
            nameof(TestCase.SeedZ)
        };
        
        if (includeDecapSeedKeyProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = instance =>
            {
                GetTestCaseFromTestCaseObject(instance, out var group, out _);
                return group.Function is MLKEMFunction.Decapsulation &&
                       group.KeyFormat is PrivateKeyFormat.Seed;
            };
        }
        
        // Applies to function = Decapsulation and DecapsulationKeyCheck and KeyFormat = Expanded
        var includeDecapExpandedKeyProperties = new []
        {
            nameof(TestCase.DecapsulationKey)
        };
        
        if (includeDecapExpandedKeyProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = instance =>
            {
                GetTestCaseFromTestCaseObject(instance, out var group, out _);
                return group.Function is MLKEMFunction.Decapsulation or MLKEMFunction.DecapsulationKeyCheck &&
                       group.KeyFormat is PrivateKeyFormat.Expanded;
            };
        }
        
        // Applies to function = Decapsulation
        var includeDecapProperties = new[]
        {
            nameof(TestCase.Ciphertext)
        };

        if (includeDecapProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = instance =>
            {
                GetTestCaseFromTestCaseObject(instance, out var group, out _);
                return group.Function == MLKEMFunction.Decapsulation;
            };
        }

        // Otherwise property should not be serialized
        return jsonProperty.ShouldSerialize = _ => false;
    }
}
