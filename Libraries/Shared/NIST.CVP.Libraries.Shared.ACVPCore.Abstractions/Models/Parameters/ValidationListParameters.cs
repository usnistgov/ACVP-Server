namespace NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters
{
    public class ValidationListParameters : PagedParametersBase
    {
        public long? ValidationId { get; set; }
        public string ValidationLabel { get; set; }
        public string ProductName { get; set; }
    }
}