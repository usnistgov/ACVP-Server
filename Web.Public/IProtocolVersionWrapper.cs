using Newtonsoft.Json.Linq;

namespace Web.Public
{
    public interface IProtocolVersionWrapper
    {
        JArray WrapJson(JObject json);
    }
}