namespace Web.Public.Models
{
    public class Request
    {
        public long RequestID { get; set; }
        public long ApprovedID { get; set; }
        public RequestStatus Status { get; set; }
        public APIAction APIAction { get; set; }
        
        public string ApprovedURL { get; set; }
    }
}