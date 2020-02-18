using System;
using ACVPCore.Results;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Options;
using Mighty;
using OtpNet;

namespace Web.Public.Providers
{
    public class TotpProvider : ITotpProvider
    {
        private readonly string _acvpConnectionString;
        private readonly TotpConfig _totpConfig;

        public TotpProvider(IConnectionStringFactory connectionStringFactory, IOptions<TotpConfig> totpConfig)
        {
            _acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
            _totpConfig = totpConfig.Value;
        }
        
        public string GenerateTotp(byte[] certRawData)
        {
            var seed = GetSeedFromUserCertificate(certRawData);
            var totp = new Totp(seed, _totpConfig.Step, StringToHmacMode(_totpConfig.Hmac), _totpConfig.Digits);
            return totp.ComputeTotp(DateTime.Now);
        }

        public Result ValidateTotp(byte[] certRawData, string password)
        {
            var seed = GetSeedFromUserCertificate(certRawData);
            
            var totp = new Totp(seed, _totpConfig.Step, StringToHmacMode(_totpConfig.Hmac), _totpConfig.Digits);
            var success = totp.VerifyTotp(DateTime.Now, password, out var computedWindow);

            // If they failed authentication, don't bother with anything else
            if (!success)
            {
                return new Result("TOTP failed to verify");
            }
            
            // If authentication was successful, we need to grab the latest window used and compare for uniqueness
            var db = new MightyOrm(_acvpConnectionString);

            try
            {
                var data = db.SingleFromProcedure("acvp.PreviousComputedWindowByUserGet", new
                {
                    CertificateRawData = certRawData
                });

                long previousComputedWindow = -1;
                if (data != null)
                {
                    previousComputedWindow = data.LastUsedWindow;
                }

                // Compare the last used window to the one just computed
                if (previousComputedWindow == computedWindow && _totpConfig.EnforceUniqueness)
                {
                    return new Result("TOTP Window has already been used");
                }
                
                // Everything is successful, record the window used
                db.ExecuteProcedure("acvp.PreviousComputedWindowByUserSet", new
                {
                    CertificateRawData = certRawData,
                    LastUsedWindow = computedWindow
                });
                
                return new Result();
            }
            catch (Exception ex)
            {
                // No logging right now
                throw ex;
            }
        }

        private byte[] GetSeedFromUserCertificate(byte[] certRawData)
        {
            var db = new MightyOrm(_acvpConnectionString);
            
            try
            {
                var data = db.SingleFromProcedure("acvp.AcvpUserGetByCertificate", new
                {
                    CertificateRawData = certRawData
                });

                if (data == null)
                {
                    throw new Exception("Certificate not found");
                }

                string base64Seed = data.seed;
                return Convert.FromBase64String(base64Seed);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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