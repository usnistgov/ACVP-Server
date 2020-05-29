namespace Web.Public.Configs
{
    public class TotpConfig
    {
        /// <summary>
        /// Should the TOTP Controller allow the creation of TOTP passwords based on the provided user cert?
        /// </summary>
        public bool IncludeTotpControllerAccess { get; set; }
        /// <summary>
        /// The time shift allowed +/- in seconds.
        /// </summary>
        public int Step { get; set; }
        /// <summary>
        /// The hmac used to calculate the TOTP
        /// </summary>
        public string Hmac { get; set; }
        /// <summary>
        /// Number of digits to get from the TOTP generation
        /// </summary>
        public int Digits { get; set; }
        /// <summary>
        /// When true validates that a TOTP can only be used once for the given certificate.
        /// </summary>
        public bool EnforceUniqueness { get; set; }
    }
}