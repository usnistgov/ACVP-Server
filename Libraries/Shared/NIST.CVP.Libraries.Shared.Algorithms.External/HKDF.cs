using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
    public class HKDF : AlgorithmBase, IExternalAlgorithm
    {
        [JsonPropertyName("capabilities")]
        public List<Capability> Capabilities { get; set; }

        public HKDF()
        {
            Name = "HKDF";
            Revision = "1.0";
        }
        
        public class Capability
        {
            [JsonPropertyName("saltLength")]
            public Domain SaltLength { get; set; }
            
            [JsonPropertyName("infoLength")]
            public Domain InfoLength { get; set; }
            
            [JsonPropertyName("keyLength")]
            public Domain KeyLength { get; set; }
            
            [JsonPropertyName("inputKeyingMaterialLength")]
            public Domain InputKeyingMaterialLength { get; set; }
            
            [JsonPropertyName("hmacAlg")]
            public string[] HmacAlg { get; set; }
        }
    }
}