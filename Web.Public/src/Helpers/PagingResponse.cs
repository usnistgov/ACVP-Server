namespace Web.Public.Helpers
{
    public class PagingResponse
    {
        public int TotalCount { get; set; }
        public bool Incomplete { get; set; }
        public PagingLinks Links { get; set; }
        public object Data { get; set; }
    }
}