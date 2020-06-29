using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NIST.CVP.Libraries.Internal.ACVPCore.Providers;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Results;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
	public class AcvpUserService : IAcvpUserService
    {
        private readonly IAcvpUserProvider _acvpUserProvider;
		private readonly IPersonService _personService;

        public AcvpUserService(IAcvpUserProvider acvpUserProvider, IPersonService personService)
        {
            _acvpUserProvider = acvpUserProvider;
			_personService = personService;
        }

        public PagedEnumerable<AcvpUserLite> GetUserList(AcvpUserListParameters param)
        {
            return _acvpUserProvider.GetList(param);
        }

        public AcvpUser GetUserDetails(long userId)
        {
            return _acvpUserProvider.Get(userId);
        }

        public Result SetUserTotpSeed(long userId, AcvpUserSeedUpdateParameters param)
        {
            return _acvpUserProvider.UpdateSeed(userId, param.Seed);
        }

        public Result RefreshTotpSeed(long userId)
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

            return _acvpUserProvider.UpdateSeed(userId, base64Seed);
        }

        public Result SetUserCertificate(long userId, AcvpUserCertificateUpdateParameters param)
        {
            X509Certificate2 x509 = new X509Certificate2(param.Certificate);

            if (DateTime.TryParse(x509.GetExpirationDateString(), out var expiresOn))
            {
                if (x509 == null)
                {
                    return new Result("Failed to parse certificate");
                }
                else
                {
                    return _acvpUserProvider.UpdateCertificate(userId, x509.Subject, x509.RawData, expiresOn);
                }
            }
            else
            {
                return new Result("Error parsing expiration date");
            }

        }

        public Result CreateUser(AcvpUserCreateParameters param)
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
			X509Certificate2 x509 = new X509Certificate2(param.Certificate);

			if (x509 != null)
			{
				if (DateTime.TryParse(x509.GetExpirationDateString(), out var expiresOn))
				{
					PersonResult personResult = _personService.Create(param.Person);

					if (personResult.IsSuccess)
					{
                        //Create acvp user
                        return _acvpUserProvider.Insert(personResult.ID, x509.Subject, x509.RawData, base64Seed, expiresOn);
					}
                    else
					{
                        return personResult;
					}
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
            return _acvpUserProvider.Delete(userId);
        }
    }
}