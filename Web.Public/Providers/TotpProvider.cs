using System;
using ACVPCore.Results;
using CVP.DatabaseInterface;
using Mighty;
using OtpNet;

namespace Web.Public.Providers
{
    public class TotpProvider : ITotpProvider
    {
        private readonly string _acvpConnectionString;

        // TODO grab values from config
        private readonly OtpHashMode _totpHmac = OtpHashMode.Sha256;
        private readonly int _step = 30;
        private readonly int _digits = 8;
        private readonly bool _uniquenessEnforced = true;
        
        public TotpProvider(IConnectionStringFactory connectionStringFactory)
        {
            _acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
        }
        
        public string GenerateTotp(byte[] certRawData)
        {
            var seed = GetSeedFromUserCertificate(certRawData);
            var totp = new Totp(seed, _step, _totpHmac, _digits);
            return totp.ComputeTotp(DateTime.Now);
        }

        public Result ValidateTotp(byte[] certRawData, string password)
        {
            var seed = GetSeedFromUserCertificate(certRawData);
            
            var totp = new Totp(seed, _step, _totpHmac, _digits);
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
                if (previousComputedWindow == computedWindow && _uniquenessEnforced)
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
    }
}