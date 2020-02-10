using System;
using OtpNet;

namespace Web.Public
{
    public class TotpProvider : ITotpProvider
    {
        private readonly OtpHashMode _totpHmac = OtpHashMode.Sha256;
        private readonly int _step = 30;
        private readonly int _digits = 8;
        
        public TotpProvider()
        {
            // TODO grab values from config
        }
        
        public string GenerateTotp(byte[] seed)
        {
            var totp = new Totp(seed, _step, _totpHmac, _digits);
            return totp.ComputeTotp(DateTime.Now);
        }

        public (bool success, long computedWindow) ValidateTotp(byte[] seed, string password)
        {
            var totp = new Totp(seed, _step, _totpHmac, _digits);
            var success = totp.VerifyTotp(DateTime.Now, password, out var computedWindow);
            
            return (success, computedWindow);
        }
    }
}