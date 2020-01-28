using System;
using System.Collections.Generic;
using System.Linq;
using ACVPCore.Models;
using ACVPCore.Results;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

namespace ACVPCore.Providers
{
    public class AcvpUserProvider : IAcvpUserProvider
    {
        private readonly string _acvpConnectionString;
        private readonly ILogger<AcvpUserProvider> _logger;

        public AcvpUserProvider(IConnectionStringFactory connectionStringFactory, ILogger<AcvpUserProvider> logger)
        {
            _acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
            _logger = logger;
        }
        
        public List<AcvpUserLite> GetUserList()
        {
            List<AcvpUserLite> result = new List<AcvpUserLite>();
            var db = new MightyOrm(_acvpConnectionString);

            try
            {
                var data = db.QueryFromProcedure("acvp.AcvpUsersGet");

                result.AddRange(data.Select(item => new AcvpUserLite()
                {
                    CompanyId = item.companyId,
                    CompanyName = item.companyName,
                    FullName = item.fullName,
                    PersonId = item.personId,
                    AcvpUserId = item.acvpUserId
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
			
            return result;
        }

        public AcvpUser GetUserDetails(long userId)
        {
            var db = new MightyOrm(_acvpConnectionString);

            try
            {
                var data = db.SingleFromProcedure("acvp.AcvpUserGetById", new
                {
                    acvpUserId = userId
                });

                if (data == null)
                    return null;
                
                return new AcvpUser()
                {
                    CompanyId = data.companyId,
                    CompanyName = data.companyName,
                    FullName = data.fullName,
                    PersonId = data.personId,
                    AcvpUserId = data.acvpUserId,
                    Certificate = Convert.ToBase64String((byte[]) data.certificate),
                    Seed = data.seed,
                    CommonName = data.commonName
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public Result SetUserTotpSeed(long userId, string seed)
        {
            var db = new MightyOrm(_acvpConnectionString);

            try
            {
                db.ExecuteProcedure("acvp.AcvpUserSetSeed", new
                {
                    acvpUserId = userId,
                    seed = seed
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new Result("Failed updating seed.");
            }

            return new Result();
        }
    }
}