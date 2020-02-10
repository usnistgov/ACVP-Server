using Newtonsoft.Json.Linq;

namespace Web.Public
{
    public class ProtocolVersionWrapper : IProtocolVersionWrapper
    {
        // TODO grab from config
        private readonly string _protocolVersion = "1.0";
        private readonly string _protocolVersionLabel = "acvVersion";

        private readonly JObject _versionObject;
        
        public ProtocolVersionWrapper()
        {
            _versionObject = new JObject {[_protocolVersionLabel] = _protocolVersion};
        }
        
        public string WrapJson(JObject json)
        {
            var wrappedResponse = new JArray(_versionObject, json);
            
            // ToString provides indentation
            return wrappedResponse.ToString();
        }
    }
}