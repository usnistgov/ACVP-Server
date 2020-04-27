using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models
{
    public class TestSessionRegisterPayload : IWorkflowItemPayload
    {
        [JsonPropertyName("tsID")]
        public long ID { get; set; }
        
        [JsonPropertyName("isSample")]
        public bool IsSample { get; set; }
        
        [JsonPropertyName("algorithms")]
        public List<VectorSetRegisterPayload> Algorithms { get; set; }
    }
}