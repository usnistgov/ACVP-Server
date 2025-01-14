using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.SigGen.ContractResolvers;

public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
{
    protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
    {
        var includeProperties = new[]
        {
            nameof(TestGroup.TestGroupId), 
            nameof(TestGroup.TestType), 
            nameof(TestGroup.ParameterSet), 
            nameof(TestGroup.Deterministic), 
            nameof(TestGroup.SignatureInterface),
            nameof(TestGroup.Tests)
        };
        
        var includeIfExternalProperties = new[]
        {
            nameof(TestGroup.PreHash)
        };

        if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = _ => true;
        }

        if (includeIfExternalProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = instance =>
            {
                GetTestGroupFromTestGroupObject(instance, out var group);
                return group.SignatureInterface == SignatureInterface.External;
            };
        }
        
        return jsonProperty.ShouldSerialize = _ => false;
    }
    
    protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
    {
        var includeProperties = new[]
        {
            nameof(TestCase.TestCaseId), 
            nameof(TestCase.PrivateKey), 
            nameof(TestCase.Message)
        };
        
        var includeNotDeterministicProperties = new[]
        {
            nameof(TestCase.AdditionalRandomness)
        };
        
        var includeExternalInterfaceProperties = new[]
        {
            nameof(TestCase.Context)
        };
        
        var includePreHashProperties = new []
        {
            nameof(TestCase.HashAlg)
        };
        
        if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = _ => true;
        }

        if (includeNotDeterministicProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = instance =>
            {
                GetTestCaseFromTestCaseObject(instance, out var group, out _);
                return !group.Deterministic;
            };
        }
        
        if (includeExternalInterfaceProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = instance =>
            {
                GetTestCaseFromTestCaseObject(instance, out var group, out _);
                return group.SignatureInterface == SignatureInterface.External;
            };
        }
        
        if (includePreHashProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = instance =>
            {
                GetTestCaseFromTestCaseObject(instance, out var group, out _);
                return group.PreHash == PreHash.PreHash;
            };
        }
        
        return jsonProperty.ShouldSerialize = _ => false;
    }
}
