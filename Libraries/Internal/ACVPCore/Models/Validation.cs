using System;

namespace ACVPCore.Models
{
    public class Validation : ValidationLite
    {
        public DateTime Updated { get; set; }
        public long VendorId { get; set; }
    }
}