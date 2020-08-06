using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.Generation.Core.ContractResolvers;

namespace NIST.CVP.Generation.TLSv13.RFC8446.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.Tests),
                nameof(TestGroup.TestType),
                nameof(TestGroup.HmacAlg),
                nameof(TestGroup.RunningMode)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }
        
        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.HelloClientRandom),
                nameof(TestCase.HelloServerRandom),
                nameof(TestCase.FinishedClientRandom),
                nameof(TestCase.FinishedServerRandom),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            var includePropertiesRunningScenariosPsk = new[]
            {
                nameof(TestCase.Psk),
            };

            if (includePropertiesRunningScenariosPsk.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (new[] { TlsModes1_3.PSK, TlsModes1_3.PSK_DHE }.Contains(testGroup.RunningMode))
                        {
                            return true;
                        }

                        return false;
                    };
            }        
            
            var includePropertiesRunningScenariosDhe = new[]
            {
                nameof(TestCase.Dhe),
            };

            if (includePropertiesRunningScenariosDhe.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (new[] { TlsModes1_3.DHE, TlsModes1_3.PSK_DHE }.Contains(testGroup.RunningMode))
                        {
                            return true;
                        }

                        return false;
                    };
            }    
            
            return jsonProperty.ShouldSerialize = instance => false;
        }
    }
}