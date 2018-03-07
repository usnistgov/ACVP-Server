using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NIST.CVP.Generation.Core.ContractResolvers
{
    /// <summary>
    /// Server JSON projection ContractResolver
    /// 
    /// This projection generally includes every property available in the object.
    /// </summary>
    public class ServerProjectionContractResolver<TTestGroup, TTestCase> : ProjectionContractResolverBase<TTestGroup, TTestCase>
        where TTestGroup : class, ITestGroup<TTestGroup, TTestCase>
        where TTestCase : class, ITestCase<TTestGroup, TTestCase>
    {
        /// <summary>
        /// The server projection returns every property available to it.
        /// </summary>
        /// <param name="jsonProperty">The current property to be serialized</param>
        /// <returns></returns>
        protected override Predicate<object> ShouldSerialize(JsonProperty jsonProperty)
        {
            return jsonProperty.ShouldSerialize;
        }

        /// <summary>
        /// The server projection returns every property available to it.
        /// </summary>
        /// <param name="jsonProperty">The current property to be serialized</param>
        /// <returns></returns>
        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            return jsonProperty.ShouldSerialize;
        }

        /// <summary>
        /// The server projection returns every property available to it.
        /// </summary>
        /// <param name="jsonProperty">The current property to be serialized</param>
        /// <returns></returns>
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            return jsonProperty.ShouldSerialize;
        }
    }
}