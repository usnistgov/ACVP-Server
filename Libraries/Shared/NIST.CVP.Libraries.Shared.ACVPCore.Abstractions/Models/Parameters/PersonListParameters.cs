namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters
{
    public class PersonListParameters : PagedParametersBase
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string OrganizationName { get; set; }
    }
}