using System;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Mighty;
using Serilog;

namespace Web.Public.Providers
{
    public class TotpProvider : ITotpProvider
    {
        private readonly string _acvpConnectionString;
        
        public TotpProvider(IConnectionStringFactory connectionStringFactory)
        {
            _acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
        }
        
        public byte[] GetSeedFromUserCertificateSubject(string userCertSubject)
        {
            var db = new MightyOrm(_acvpConnectionString);
            
            try
            {
                var data = db.SingleFromProcedure("acvp.AcvpUserSeedGetByCertificate", new
                {
                    Subject = userCertSubject
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
                Log.Error(ex.StackTrace);
                throw;
            }
        }

        public long GetUsedWindowFromUserCertificateSubject(string userCertSubject)
        {
            var db = new MightyOrm(_acvpConnectionString);

            try
            {
                var data = db.SingleFromProcedure("acvp.PreviousComputedWindowByUserGet",
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
                Log.Error(ex.StackTrace);
                throw;
            }
        }

        public void SetUsedWindowFromUserCertificateSubject(string userCertSubject, long usedWindow)
        {
            var db = new MightyOrm(_acvpConnectionString);

            try
            {
                // Everything is successful, record the window used
                db.ExecuteProcedure("acvp.PreviousComputedWindowByUserSet", new
                {
                    Subject = userCertSubject,
                    LastUsedWindow = usedWindow
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
                throw;
            }
        }
    }
}