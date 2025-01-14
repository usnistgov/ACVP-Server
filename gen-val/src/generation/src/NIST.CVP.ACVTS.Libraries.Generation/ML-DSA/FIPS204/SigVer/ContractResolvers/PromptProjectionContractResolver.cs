using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigVer.ContractResolvers;

public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
{
    protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
    {
        var includeProperties = new[]
        {
            nameof(TestGroup.TestGroupId), 
            nameof(TestGroup.TestType), 
            nameof(TestGroup.ParameterSet),
            nameof(TestGroup.SignatureInterface),
            nameof(TestGroup.Tests),
        };
        
        var includeIfInternalProperties = new[]
        {
            nameof(TestGroup.ExternalMu)
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
        
        if (includeIfInternalProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = instance =>
            {
                GetTestGroupFromTestGroupObject(instance, out var group);
                return group.SignatureInterface == SignatureInterface.Internal;
            };
        }
        
        return jsonProperty.ShouldSerialize = _ => false;
    }

    protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
    {
        var includeProperties = new[]
        {
            nameof(TestCase.TestCaseId), 
            nameof(TestCase.PublicKey),
            nameof(TestCase.Signature)
        };

        var includeExternalInterfaceProperties = new[]
        {
            nameof(TestCase.Context)
        };
        
        var includeExternalMuProperties = new []
        {
            nameof(TestCase.Mu)
        };
        
        var includeNotExternalMuProperties = new []
        {
            nameof(TestCase.Message)
        };
        
        var includePreHashProperties = new []
        {
            nameof(TestCase.HashAlg)
        };

        if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = _ => true;
        }
        
        if (includeExternalMuProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = instance =>
            {
                GetTestCaseFromTestCaseObject(instance, out var group, out _);
                return group.ExternalMu;
            };
        }
        
        if (includeNotExternalMuProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
        {
            return jsonProperty.ShouldSerialize = instance =>
            {
                GetTestCaseFromTestCaseObject(instance, out var group, out _);
                return !group.ExternalMu;
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
