using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.Results;
using System;
using System.Security.Cryptography;
using NIST.CVP.Libraries.Internal.ACVPCore.Providers;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
    public class AcvpUserService : IAcvpUserService
    {
        private readonly IAcvpUserProvider _adminUserProvider;

        public AcvpUserService(IAcvpUserProvider adminUserProvider)
        {
            _adminUserProvider = adminUserProvider;
        }

        public PagedEnumerable<AcvpUserLite> GetUserList(AcvpUserListParameters param)
        {
            return _adminUserProvider.GetUserList(param);
        }

        public AcvpUser GetUserDetails(long userId)
        {
            return _adminUserProvider.GetUserDetails(userId);
        }

        public Result SetUserTotpSeed(long userId, AcvpUserSeedUpdateParameters param)
        {
            return _adminUserProvider.SetUserTotpSeed(userId, param.Seed);
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

            return _adminUserProvider.SetUserTotpSeed(userId, base64Seed);
        }

        public Result SetUserCertificate(long userId, AcvpUserCertificateUpdateParameters param)
        {
            return _adminUserProvider.SetUserCertificate(userId, param.Certificate);
        }

        public Result CreateUser(AcvpUserCreateParameters param)
        {
            return _adminUserProvider.CreateUser(param.Person.Name, param.Person.OrganizationID, param.Certificate, param.Person.EmailAddresses.ToArray());
        }
        public Result DeleteUser(long userId)
        {
            return _adminUserProvider.DeleteUser(userId);
        }
    }
}