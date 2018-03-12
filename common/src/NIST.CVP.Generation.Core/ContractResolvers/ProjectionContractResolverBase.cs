using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NIST.CVP.Generation.Core.ContractResolvers
{
    /// <summary>
    /// Base class for defining separate serializations,
    /// depending on a projection's intended audience.
    /// </summary>
    public abstract class ProjectionContractResolverBase<TTestGroup, TTestCase> : CamelCasePropertyNamesContractResolver
        where TTestGroup : class, ITestGroup<TTestGroup, TTestCase>
        where TTestCase : class, ITestCase<TTestGroup, TTestCase>
    {
        /// <summary>
        /// Override the base <see cref="DefaultContractResolver.CreateProperty"/>.
        /// Continues to call the base method, as well as an additional method to check
        /// if a property should be serialized.
        /// </summary>
        /// <param name="member">The member being considered for serialization.</param>
        /// <param name="memberSerialization">Member serialization options</param>
        /// <returns></returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);

            jsonProperty.ShouldSerialize = ShouldSerialize(jsonProperty);

            return jsonProperty;
        }

        /// <summary>
        /// Additional checks on if <see cref="jsonProperty"/> should be serialized.
        /// </summary>
        /// <param name="jsonProperty">The property being considered for serialization.</param>
        /// <returns></returns>
        protected virtual Predicate<object> ShouldSerialize(JsonProperty jsonProperty)
        {
            var type = jsonProperty.DeclaringType;

            if (typeof(ITestGroup<TTestGroup, TTestCase>).IsAssignableFrom(type))
            {
                return TestGroupSerialization(jsonProperty);
            }

            if (typeof(ITestCase<TTestGroup, TTestCase>).IsAssignableFrom(type))
            {
                return TestCaseSerialization(jsonProperty);
            }

            return jsonProperty.ShouldSerialize;
        }
        
        protected abstract Predicate<object> TestGroupSerialization(JsonProperty jsonProperty);
        protected abstract Predicate<object> TestCaseSerialization(JsonProperty jsonProperty);

        protected virtual void GetTestCaseFromTestCaseObject(object instance, out TTestGroup testGroup, out TTestCase testCase)
        {
            testCase = instance as TTestCase;
            testGroup = testCase?.ParentGroup;
        }

        protected virtual void GetTestGroupFromTestGroupObject(object instance, out TTestGroup testGroup)
        {
            testGroup = instance as TTestGroup;
        }
    }
}