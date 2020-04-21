using System;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models
{
    public class PersonNote
    {
        public long ID { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime? Time { get; set; }
    }
}
