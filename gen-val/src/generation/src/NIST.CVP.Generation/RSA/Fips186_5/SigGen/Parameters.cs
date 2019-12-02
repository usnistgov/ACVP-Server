using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA.Fips186_5.SigGen
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        [JsonProperty(PropertyName = "capabilities")]
        public AlgSpecs[] Capabilities { get; set; }
    }
    
    public class AlgSpecs
    {
        public SignatureSchemes SigType { get; set; }
        
        [JsonProperty(PropertyName = "properties")]
        public CapSigType[] ModuloCapabilities { get; set; }
    }

    public class CapSigType
    {
        public int Modulo { get; set; }
        public PssMaskTypes[] MaskFunction { get; set; } = { PssMaskTypes.None };

        [JsonProperty(PropertyName = "hashPair")]
        public HashPair[] HashPairs { get; set; }
    }

    public class HashPair
    {
        public string HashAlg { get; set; }
        public int SaltLen { get; set; } = 0;
    }
}