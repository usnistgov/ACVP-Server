using Newtonsoft.Json;

namespace NIST.CVP.Crypto.Common.Hash
{
    public class AlgoArrayResponseWithCustomization : AlgoArrayResponse
    {
        [JsonIgnore]
        public string Customization { get; set; }
        [JsonIgnore]
        public string FunctionName { get; set; }
    }
}
