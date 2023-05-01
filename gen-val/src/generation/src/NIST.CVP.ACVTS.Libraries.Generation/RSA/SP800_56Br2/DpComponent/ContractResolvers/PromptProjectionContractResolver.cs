using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Sp800_56Br2.DpComponent.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        /// <summary>
        /// Every group property included
        /// </summary>
        /// <param name="jsonProperty"></param>
        /// <returns></returns>

        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            return jsonProperty.ShouldSerialize = instance => true;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var stdIncludeProperties = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.CipherText),
                nameof(TestCase.N),
                nameof(TestCase.E),
                nameof(TestCase.P),
                nameof(TestCase.Q),
                nameof(TestCase.D)
            };

            var crtIncludeProperties = new[]
            {
                nameof(TestCase.Dmp1),
                nameof(TestCase.Dmq1),
                nameof(TestCase.Iqmp)
            };
            
            jsonProperty.ShouldSerialize = instance =>
            {
                GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                // Always include TestCaseId & CipherText
                if (stdIncludeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
                {
                    return true;
                }
                
                // Include based on keyformat
                if (testGroup.KeyMode == PrivateKeyModes.Crt)
                {
                    if (crtIncludeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    
                    return false;
                }

                return false;
            };

            return jsonProperty.ShouldSerialize;
        }

        protected override Predicate<object> ShouldSerialize(JsonProperty jsonProperty)
        {
            var type = jsonProperty.DeclaringType;

            if (typeof(TestGroup).IsAssignableFrom(type))
            {
                return TestGroupSerialization(jsonProperty);
            }

            if (typeof(TestCase).IsAssignableFrom(type))
            {
                return TestCaseSerialization(jsonProperty);
            }

            return jsonProperty.ShouldSerialize;
        }
    }    
}
