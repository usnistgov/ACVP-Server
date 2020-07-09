using System;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.DatabaseInterface;

namespace Web.Public.Providers
{
	public class UserProvider : IUserProvider
    {
        private readonly ILogger<UserProvider> _logger;
        private readonly string _connectionString;
        
        public UserProvider(ILogger<UserProvider> logger, IConnectionStringFactory connectionStringFactory)
        {
            _logger = logger;
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
        }
        
        public long GetUserIDFromCertificateSubject(string userCertSubject)
        {
            var db = new MightyOrm(_connectionString);
            
            try
            {
                var data = db.SingleFromProcedure("dbo.AcvpUserGetByCertificate", new
                {
                    Subject = userCertSubject
                });

                if (data == null)
                {
                    throw new Exception("Certificate not found");
                }

                return data.ACVPUserId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }
    }
}