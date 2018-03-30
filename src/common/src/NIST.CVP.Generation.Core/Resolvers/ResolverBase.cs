using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NIST.CVP.Generation.Core.Resolvers
{
    public abstract class ResolverBase : CamelCasePropertyNamesContractResolver
    {
        protected abstract string[] IgnoreProperties { get; }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            bool ignoredProperty = IgnoreProperties.Contains(property.PropertyName.ToLower());
            if (ignoredProperty)
            { 
                property.ShouldSerialize =
                    instance =>
                    {
                        return false;
                    };
            }
            else
            {
                JsonProperty prop = base.CreateProperty(member, memberSerialization);
                return prop;
            }
            return property;
        }
    }
}