using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Enumerables;
using NIST.CVP.ExtensionMethods;
using NIST.CVP.Results;

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
			
            return result.ToPagedEnumerable(param.PageSize, param.PageSize, totalRecords);
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

        public Result CreateUser(string personName, long organizationID, byte[] certificate, string seed)
        {

            X509Certificate2 x509 = new X509Certificate2(certificate);

            if (x509 == null)
            {
                return new InsertResult("Failed to parse certificate");
            }
            else
            {
                var db = new MightyOrm(_acvpConnectionString);
                try
                {
                    var personQueryData = db.SingleFromProcedure("val.PersonInsert", new
                    {
                        Name = personName,
                        OrganizationID = organizationID
                    });
                    if (personQueryData == null)
                    {
                        return new InsertResult("Failed to insert Person");
                    }
                    else
                    {
                        var acvpUserQueryData = db.SingleFromProcedure("acvp.AcvpUserInsert", new
                        {
                            PersonID = personQueryData.PersonID,
                            CommonName = x509.Subject,
                            Certificate = certificate,
                            Seed = seed
                        });

                        return new InsertResult((long)acvpUserQueryData.UserID);
                    }
                }
                catch (Exception orgQueryEx)
                {
                    Console.WriteLine(orgQueryEx);
                }
                return new InsertResult("Unspecified error in ACVP User creation");
            }
        }
    }
}