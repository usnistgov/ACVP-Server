using System;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models
{
    public class ValidationLite
    {
        public long ValidationId { get; set; }
        public string ValidationLabel { get; set; }
        public string ImplementationName { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}