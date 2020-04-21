using System;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models
{
    public class Validation : ValidationLite
    {
        public DateTime Updated { get; set; }
        public long VendorId { get; set; }
    }
}