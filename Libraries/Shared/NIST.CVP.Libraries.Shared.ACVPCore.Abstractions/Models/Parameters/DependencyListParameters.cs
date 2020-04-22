namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters
{
    /// <summary>
    /// Provides searching and paging capabilities when pulling a TestSession list.
    /// </summary>
    public class DependencyListParameters : PagedParametersBase
    {
        /// <summary>
        /// The dependency ID.
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// The name of the dependency to search for.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The type of dependency to search for.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// The description of the dependency to search for.
        /// </summary>
        public string Description { get; set; }
    }
}