using System;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models
{
    public class AcvpUserLite
    {
        public long AcvpUserId { get; set; }
        public long PersonId { get; set; } 
        public string FullName { get; set; }
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}