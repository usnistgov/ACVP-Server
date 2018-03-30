using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;
using NIST.CVP.Generation.Core.JsonConverters;

namespace NIST.CVP.Generation.Core.Resolvers
{
    public class ValidationResolver : CamelCasePropertyNamesContractResolver
    {
        private string[] EnumProperties = new[] { "disposition", "result" };

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            bool enumProperty = EnumProperties.Contains(property.PropertyName.ToLower());
            if (enumProperty)
            {
                property.Converter = new DispositionConverter();
            }

            return property;
        }
    }
}
