using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
    public class HKDF : PersistedAlgorithmBase
    {
        [AlgorithmProperty("capabilities")] 
        public List<Capability> Capabilities { get; set; } = new List<Capability>();

        public HKDF()
        {
            Name = "HKDF";
            Revision = "1.0";
        }

        public HKDF(External.HKDF external) : this()
        {
            foreach (var capability in external.Capabilities)
            {
                Capabilities.Add(new Capability
                {
                    SaltLength = capability.SaltLength,
                    InfoLength = capability.InfoLength,
                    KeyLength = capability.KeyLength,
                    InputKeyingMaterialLength = capability.InputKeyingMaterialLength,
                    HmacAlg = capability.HmacAlg
                });
            }
        }

        public class Capability
        {
            [AlgorithmProperty("saltLength")]
            public Domain SaltLength { get; set; }
            
            [AlgorithmProperty("infoLength")]
            public Domain InfoLength { get; set; }
            
            [AlgorithmProperty("keyLength")]
            public Domain KeyLength { get; set; }
            
            [AlgorithmProperty("inputKeyingMaterialLength")]
            public Domain InputKeyingMaterialLength { get; set; }
            
            [AlgorithmProperty("hmacAlg")]
            public string[] HmacAlg { get; set; }
        }
    }
}