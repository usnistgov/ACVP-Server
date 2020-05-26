using System;
using Microsoft.IdentityModel.Tokens;

namespace Web.Public.Configs
{
    public class JwtConfig
    {
        public string Issuer { get; set; }
        public int ValidWindowHours { get; set; }
        public int ValidWindowMinutes { get; set; }
        public int ValidWindowSeconds { get; set; }
        public string FriendlySignatureScheme { get; set; } = "";
        public string SignatureScheme => GetSignatureScheme(FriendlySignatureScheme);

        private string GetSignatureScheme(string signature)
        {
            return signature.ToLower() switch
            {
                "hmacsha256" => SecurityAlgorithms.HmacSha256Signature,
                _ => throw new Exception("Unable to identify signature algorithm")
            };
        }
    }
}