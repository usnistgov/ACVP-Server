namespace Web.Public
{
    public class TotpConfig
    {
        public int Step { get; set; }
        public string Hmac { get; set; }
        public int Digits { get; set; }
        public bool EnforceUniqueness { get; set; }
    }
}