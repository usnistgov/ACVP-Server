using System;
using NIST.CVP.Libraries.Shared.Results;
using Microsoft.Extensions.Options;
using OtpNet;
using Web.Public.Configs;
using Web.Public.Providers;

namespace Web.Public.Services
{
    public class TotpService : ITotpService
    {
        private readonly ITotpProvider _totpProvider;
        private readonly TotpConfig _totpConfig;
        
        public TotpService(ITotpProvider totpProvider, IOptions<TotpConfig> totpConfig)
        {
            _totpProvider = totpProvider;
            _totpConfig = totpConfig.Value;
        }
        
        public string GenerateTotp(string userCertSubject)
        {
            var seed = _totpProvider.GetSeedFromUserCertificateSubject(userCertSubject);
            var totp = new Totp(seed, _totpConfig.Step, StringToHmacMode(_totpConfig.Hmac), _totpConfig.Digits);
            return totp.ComputeTotp(DateTime.Now);
        }

        public Result ValidateTotp(string userCertSubject, string password)
        {
            var seed = _totpProvider.GetSeedFromUserCertificateSubject(userCertSubject);
            
            var totp = new Totp(seed, _totpConfig.Step, StringToHmacMode(_totpConfig.Hmac), _totpConfig.Digits);
            var success = totp.VerifyTotp(DateTime.Now, password, out var computedWindow, VerificationWindow.RfcSpecifiedNetworkDelay);

            // If they failed authentication, don't bother with anything else
            if (!success)
            {
                return new Result("TOTP failed to verify");
            }
            
            // If authentication was successful, we need to grab the latest window used and compare for uniqueness
            var previousWindow = _totpProvider.GetUsedWindowFromUserCertificateSubject(userCertSubject);

            if (previousWindow == computedWindow && _totpConfig.EnforceUniqueness)
            {
                return new Result("TOTP Window has already been used");
            }
            
            _totpProvider.SetUsedWindowFromUserCertificateSubject(userCertSubject, previousWindow);
            return new Result();
        }

        private OtpHashMode StringToHmacMode(string hmac)
        {
            return hmac.ToLower() switch
            {
                "sha1" => OtpHashMode.Sha1,
                "sha256" => OtpHashMode.Sha256,
                "sha512" => OtpHashMode.Sha512,
                _ => throw new Exception("Hmac provided via config is not supported for TOTP.")
            };
        }
    }
}