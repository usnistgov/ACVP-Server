using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;

namespace Web.Public.JsonObjects
{
    public class MessageObject
    {
        public long RequestID { get; set; }
        public object Json { get; set; }
    }
}