using System;
using Microsoft.Extensions.Logging;
using NIST.CVP.Libraries.Shared.Results;
using Microsoft.Extensions.Options;
using OtpNet;
using Web.Public.Configs;
using Web.Public.Providers;

namespace Web.Public.Services
{
    public class TotpService : ITotpService
    {
        private readonly ILogger<TotpService> _logger;
        private readonly ITotpProvider _totpProvider;
        private readonly TotpConfig _totpConfig;
        
        public TotpService(ILogger<TotpService> logger, ITotpProvider totpProvider, IOptions<TotpConfig> totpConfig)
        {
            _logger = logger;
            _totpProvider = totpProvider;
            _totpConfig = totpConfig.Value;
        }
        
        public string GenerateTotp(string userCertSubject)
        {
            _logger.LogInformation($@"Attempting to generate TOTP for cert subject: ""{userCertSubject}""");
            
            var seed = _totpProvider.GetSeedFromUserCertificateSubject(userCertSubject);
            var totp = new Totp(seed, _totpConfig.Step, StringToHmacMode(_totpConfig.Hmac), _totpConfig.Digits);
            return totp.ComputeTotp(DateTimeOffset.FromUnixTimeSeconds(DateTime.Now.Ticks).DateTime);
        }

        public Result ValidateTotp(string userCertSubject, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return new Result("TOTP not provided.");
            }

            var seed = _totpProvider.GetSeedFromUserCertificateSubject(userCertSubject);

            if (seed == null)
            {
                _logger.LogWarning($@"Failed retrieving seed for cert subject: ""{userCertSubject}""");
                return new Result("TOTP failed to verify");
            }
            
            var totp = new Totp(seed, _totpConfig.Step, StringToHmacMode(_totpConfig.Hmac), _totpConfig.Digits);
            var success = totp.VerifyTotp(DateTimeOffset.FromUnixTimeSeconds(DateTime.Now.Ticks).DateTime, password, out var computedWindow, VerificationWindow.RfcSpecifiedNetworkDelay);

            // If they failed authentication, don't bother with anything else
            if (!success)
            {
                _logger.LogWarning($@"TOTP failed to verify for cert subject: ""{userCertSubject}""");
                return new Result("TOTP failed to verify");
            }
            
            // If authentication was successful, we need to grab the latest window used and compare for uniqueness
            var previousWindow = _totpProvider.GetUsedWindowFromUserCertificateSubject(userCertSubject);

            if (previousWindow == computedWindow && _totpConfig.EnforceUniqueness)
            {
                _logger.LogWarning($@"TOTP Window has already been used for cert subject: ""{userCertSubject}""");
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