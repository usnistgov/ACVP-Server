using System;
using System.Collections.Generic;
using System.Linq;
using ACVPCore.ExtensionMethods;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
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
        
        public PagedEnumerable<AcvpUserLite> GetUserList(AcvpUserListParameters param)
        {
            List<AcvpUserLite> result = new List<AcvpUserLite>();
            long totalRecords = 0;
            var db = new MightyOrm<AcvpUserLite>(_acvpConnectionString);

            try
            {
                var dbData = db.QueryWithExpando("acvp.AcvpUsersGet",
                    inParams: new 
                    {
                        PageSize = param.PageSize,
                        PageNumber = param.Page,
                        AcvpUserId = param.AcvpUserId,
                        PersonId = param.PersonId,
                        CompanyName = param.CompanyName,
                        PersonName = param.PersonName
                    },
                    new
                    {
                        totalRecords = (long)0
                    });
                
                result = dbData.Data;
                totalRecords = dbData.ResultsExpando.totalRecords;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
			
            return result.WrapPagedEnumerable(param.PageSize, param.PageSize, totalRecords);
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
                    CertificateBase64 = Convert.ToBase64String((byte[]) data.certificate),
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