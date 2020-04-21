namespace NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters
{
    /// <summary>
    /// Base class used in paged result requests for retrieving lists of items.
    /// </summary>
    public abstract class PagedParametersBase
    {
        /// <summary>
        /// The (max) number of records to retrieve in a request.
        /// </summary>
        public int PageSize { get; set; }
        
        /// <summary>
        /// The page number to retrieve (starts at 1).
        /// </summary>
        public int Page { get; set; }
    }
}