namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters
{
    /// <summary>
    /// Provides searching and paging capabilities when pulling a TestSession list.
    /// </summary>
    public class TestSessionListParameters : PagedParametersBase
    {
        /// <summary>
        /// The Test Session to search for.
        /// </summary>
        public long? TestSessionId { get; set; }
        /// <summary>
        /// The Vector Set to search for.
        /// </summary>
        public long? VectorSetId { get; set; }
        /// <summary>
        /// The status value to search for.
        /// </summary>
        public TestSessionStatus? TestSessionStatus { get; set; }
    }
}