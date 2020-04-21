namespace NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters
{
    /// <summary>
    /// Provides searching and paging capabilities when pulling a organization list.
    /// </summary>
    public class OrganizationListParameters : PagedParametersBase
    {
        /// <summary>
        /// The organization ID.
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// The name of the organization to search for.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The URL of organization to search for.
        /// </summary>
        public string URL { get; set; }
    }
}