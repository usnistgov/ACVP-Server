using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NIST.CVP.Generation.Core.JsonConverters
{
    /// <summary>
    /// Provides JSON converters to be utilized with DeSerialization
    /// </summary>
    public class JsonConverterProvider : IJsonConverterProvider
    {
        /// <summary>
        /// Get a set of <see cref="JsonConverter"/>s used in DeSerialization
        /// </summary>
        /// <returns></returns>
        public IEnumerable<JsonConverter> GetJsonConverters()
        {
            var converters = new HashSet<JsonConverter>
            {
                new BigIntegerConverter(),
                new BitstringConverter(),
                new DispositionConverter(),
                new DomainConverter(),
                new StringEnumConverter()
            };

            AddAdditionalConverters(converters);

            return converters;
        }

        /// <summary>
        /// Provides extension point of adding additional converters to the set of returned converters.
        /// </summary>
        /// <param name="registeredConverters">
        ///     The current set of converters to be 
        ///     returned from the <see cref="IJsonConverterProvider"/>
        /// </param>
        protected virtual void AddAdditionalConverters(HashSet<JsonConverter> registeredConverters)
        {

        }
    }
}