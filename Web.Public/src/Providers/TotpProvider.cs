using System;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using NIST.CVP.Libraries.Shared.ExtensionMethods;

namespace Web.Public.Providers
{
	public class TotpProvider : ITotpProvider
    {
        private ILogger<TotpProvider> _logger;
        private readonly string _acvpConnectionString;

        public TotpProvider(ILogger<TotpProvider> logger, IConnectionStringFactory connectionStringFactory)
        {
            _logger = logger;
            _acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
        }
        
        public byte[] GetSeedFromUserCertificateSubject(string userCertSubject)
        {
            var db = new MightyOrm(_acvpConnectionString);
            
            try
            {
                var data = db.SingleFromProcedure("dbo.AcvpUserSeedGetByCertificate", new
                {
                    Subject = userCertSubject
                });

                if (data == null)
                {
                    throw new Exception("Certificate not found");
                }

                string base64Seed = data.Seed;
                return Convert.FromBase64String(base64Seed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        public long GetUsedWindowFromUserCertificateSubject(string userCertSubject)
        {
            var db = new MightyOrm(_acvpConnectionString);

            try
            {
                var data = db.SingleFromProcedure("dbo.PreviousComputedWindowByUserGet",
                    new
                    {
                        Subject = userCertSubject
                    });

                long previousComputedWindow = -1;
                if (data != null)
                {
                    previousComputedWindow = data.LastUsedWindow;
                }

                return previousComputedWindow;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        public void SetUsedWindowFromUserCertificateSubject(string userCertSubject, long usedWindow)
        {
            var db = new MightyOrm(_acvpConnectionString);

            try
            {
                // Everything is successful, record the window used
                db.ExecuteProcedure("dbo.PreviousComputedWindowByUserSet", new
                {
                    Subject = userCertSubject,
                    LastUsedWindow = usedWindow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
    }
}