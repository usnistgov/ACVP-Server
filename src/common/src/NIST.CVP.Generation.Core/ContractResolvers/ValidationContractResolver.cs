using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Generation.Core.JsonConverters;

namespace NIST.CVP.Generation.Core.ContractResolvers
{
    public class ValidationContractResolver : CamelCasePropertyNamesContractResolver
    {
        private readonly string[] _enumProperties = new[] { "disposition", "result" };

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            bool enumProperty = _enumProperties.Contains(property.PropertyName.ToLower());
            if (enumProperty)
            {
                property.Converter = new DispositionConverter();
            }

            return property;
        }
    }
}
