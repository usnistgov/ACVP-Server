namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters
{
    public class AcvpUserListParameters : PagedParametersBase
    {
        public long? AcvpUserId { get; set; }
        public long? PersonId { get; set; }
        public string CompanyName { get; set; }
        public string PersonName { get; set; }
    }
}