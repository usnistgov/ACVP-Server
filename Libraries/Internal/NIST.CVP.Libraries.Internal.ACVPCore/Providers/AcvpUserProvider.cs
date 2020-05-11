using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
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

        public Result SetUserCertificate(long userId, byte[] certificate)
        {
            X509Certificate2 x509 = new X509Certificate2(certificate);

            if (DateTime.TryParse(x509.GetExpirationDateString(), out var expiresOn))
            {
                if (x509 == null)
                {
                    return new InsertResult("Failed to parse certificate");
                }
                else
                {
                    var db = new MightyOrm(_acvpConnectionString);

                    try
                    {
                        db.ExecuteProcedure("acvp.AcvpUserSetCertificate", new
                        {
                            AcvpUserId = userId,
                            CommonName = x509.Subject,
                            Certificate = x509.RawData,
                            ExpiresOn = expiresOn
                        });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                        return new Result("Failed updating certificate.");
                    }
                }
            }
            else
            {
                return new Result("Error parsing expiration date");
            }
            return new Result();
        }

        public Result CreateUser(string personName, long organizationID, byte[] certificate, string[] personEmails)
        {
            string base64Seed;
            // Based on the note about IDisposable interface in the docs, this is the recommended usage to ensure the
            // CSP is disposed of properly
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                byte[] seed = new byte[48];
                rngCsp.GetBytes(seed); 
                base64Seed = Convert.ToBase64String(seed, 0, seed.Length);
            }

            // Parse out the certificate provided
            X509Certificate2 x509 = new X509Certificate2(certificate);
            
            if (x509 != null)
            {
                if (DateTime.TryParse(x509.GetExpirationDateString(), out var expiresOn)) 
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
                            for (int i = 0; i < personEmails.Length; i++) {
                                //There is no ID on the record, so don't return anything
                                db.ExecuteProcedure("val.PersonEmailInsert", inParams: new
                                {
                                    PersonID = personQueryData.PersonID,
                                    EmailAddress = personEmails[i],
                                    OrderIndex = i
                                });
                            }

                            var acvpUserQueryData = db.SingleFromProcedure("acvp.AcvpUserInsert", new
                            {
                                PersonID = personQueryData.PersonID,
                                CommonName = x509.Subject,
                                Certificate = x509.RawData,
                                Seed = base64Seed,
                                ExpiresOn = expiresOn
                            });

                            return new InsertResult((long)acvpUserQueryData.UserID);
                        }
                    }
                    catch (Exception personQueryEx)
                    {
                        Console.WriteLine(personQueryEx);
                    }
                    return new InsertResult("Unspecified error in ACVP User creation");
                }
                else
                {
                    return new InsertResult("Error in extracting certificate expiration date");
                }
            }
            else
            {
                return new InsertResult("Failed to parse certificate"); 
            }
            
        }
        public Result DeleteUser(long userId)
        {
            var db = new MightyOrm(_acvpConnectionString);

            try
            {
                var data = db.ExecuteProcedure("acvp.AcvpUserDelete", new
                {
                    acvpUserId = userId
                });

                return new Result();
            }
            catch (Exception ex)
            {
                return new DeleteResult(ex.Message);
            }
        }
    }
}