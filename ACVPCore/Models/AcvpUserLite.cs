namespace ACVPCore.Models
{
    public class AcvpUserLite
    {
        public long AcvpUserId { get; set; }
        public long PersonId { get; set; } 
        public string FullName { get; set; }
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}