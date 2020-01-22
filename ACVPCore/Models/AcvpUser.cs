namespace ACVPCore.Models
{
    public class AcvpUser : AcvpUserLite
    {
        public string Certificate { get; set; }
        public string CommonName { get; set; }
        public string Seed { get; set; }
    }
}