using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Generation.Core.ContractResolvers;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        /// <summary>
        /// Include tgId, l, n, hashAlg, test type.  include GGenMode or PrimeGenMode when not "none".
        /// </summary>
        /// <param name="jsonProperty">The property to check</param>
        /// <returns></returns>
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.Tests),
                nameof(TestGroup.L),
                nameof(TestGroup.N),
                nameof(TestGroup.HashAlgName),
                nameof(TestGroup.TestType)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            #region Conditional group level properties
            if (jsonProperty.UnderlyingName == nameof(TestGroup.PQGenMode))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestGroupFromTestGroupObject(instance, out var testGroup);

                        if (testGroup.PQGenMode != PrimeGenMode.None)
                        {
                            return true;
                        }

                        return false;
                    };
            }

            if (jsonProperty.UnderlyingName == nameof(TestGroup.GGenMode))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestGroupFromTestGroupObject(instance, out var testGroup);

                        if (testGroup.GGenMode != GeneratorGenMode.None)
                        {
                            return true;
                        }

                        return false;
                    };
            }
            #endregion Conditional group level properties

            return jsonProperty.ShouldSerialize = instance => false;
        }
        
        /// <summary>
        /// Include tcId, when GGen not "none" include P and Q.  
        /// When GGen is canonical include domainseed and index.
        /// </summary>
        /// <param name="jsonProperty"></param>
        /// <returns></returns>
        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.P),
                nameof(TestCase.Q)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            #region Conditional Test Case properties
            if (jsonProperty.UnderlyingName == nameof(TestCase.DomainSeed))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.PQGenMode == PrimeGenMode.Probable || testGroup.GGenMode == GeneratorGenMode.Unverifiable)
                        {
                            return true;
                        }

                        return false;
                    };
            }

            if (jsonProperty.UnderlyingName == nameof(TestCase.Index))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.PQGenMode == PrimeGenMode.Probable || testGroup.GGenMode == GeneratorGenMode.Canonical)
                        {
                            return true;
                        }

                        return false;
                    };
            }

            var includePQGenProvableProperties = new[]
            {
                nameof(TestCase.PCount),
                nameof(TestCase.QCount),
                nameof(TestCase.PSeed),
                nameof(TestCase.QSeed)
            };
            if (includePQGenProvableProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.PQGenMode == PrimeGenMode.Provable)
                        {
                            return true;
                        }

                        return false;
                    };
            }

            var includeGGenProperties = new[]
            {
                nameof(TestCase.G)
            };
            if (includeGGenProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.GGenMode != GeneratorGenMode.None)
                        {
                            return true;
                        }

                        return false;
                    };
            }

            var includeGGenUnverifiableProperties = new[]
            {
                nameof(TestCase.H)
            };
            if (includeGGenUnverifiableProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.GGenMode == GeneratorGenMode.Unverifiable)
                        {
                            return true;
                        }

                        return false;
                    };
            }
            #endregion Conditional Test Case properties

            return jsonProperty.ShouldSerialize = instance => false;
        }
    }
}