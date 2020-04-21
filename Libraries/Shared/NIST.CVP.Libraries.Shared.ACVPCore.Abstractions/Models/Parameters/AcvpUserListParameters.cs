namespace NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters
{
    public class AcvpUserListParameters : PagedParametersBase
    {
        public long? AcvpUserId { get; set; }
        public long? PersonId { get; set; }
        public string CompanyName { get; set; }
        public string PersonName { get; set; }
    }
}