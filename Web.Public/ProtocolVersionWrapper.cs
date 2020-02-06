using Newtonsoft.Json.Linq;

namespace Web.Public
{
    public class ProtocolVersionWrapper : IProtocolVersionWrapper
    {
        // TODO grab from config
        private readonly string _protocolVersion = "1.0";
        private readonly string _protocolVersionLabel = "acvVersion";
        
        public JArray WrapJson(JObject json)
        {
            var versionObject = new JObject {[_protocolVersionLabel] = _protocolVersion};
            var wrappedResponse = new JArray(versionObject, json);
            return wrappedResponse;
        }
    }
}