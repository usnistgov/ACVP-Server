using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;

namespace Web.Public.JsonObjects
{
    public class MessageObject
    {
        public long RequestID { get; set; }
        public long UserID { get; set; }
        public APIAction ApiActionID { get; set; }
        public object Json { get; set; }
    }
}