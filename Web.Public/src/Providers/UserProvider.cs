using System;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Mighty;
using Serilog;

namespace Web.Public.Providers
{
    public class UserProvider : IUserProvider
    {
        private readonly string _connectionString;

        public UserProvider(IConnectionStringFactory connectionStringFactory)
        {
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
        }
        
        public long GetUserIDFromCertificate(byte[] certRawData)
        {
            var db = new MightyOrm(_connectionString);
            
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

                return data.UserID;
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
                throw;
            }
        }
    }
}