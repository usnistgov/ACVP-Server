using System;

namespace ACVPCore.Models
{
    public class ValidationLite
    {
        public long ValidationId { get; set; }
        public string ValidationLabel { get; set; }
        public string ProductName { get; set; }
        public DateTime Created { get; set; }
    }
}