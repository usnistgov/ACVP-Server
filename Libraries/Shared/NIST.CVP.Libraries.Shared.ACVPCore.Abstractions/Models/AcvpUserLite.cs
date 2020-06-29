using System;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models
{
    public class AcvpUserLite
    {
        public long ACVPUserID { get; set; }
        public long PersonID { get; set; } 
        public string FullName { get; set; }
        public long OrgnizationID { get; set; }
        public string OrganizationName { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}