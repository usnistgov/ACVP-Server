using Newtonsoft.Json.Linq;

namespace Web.Public
{
    public interface IProtocolVersionWrapper
    {
        string WrapJson(JObject json);
    }
}