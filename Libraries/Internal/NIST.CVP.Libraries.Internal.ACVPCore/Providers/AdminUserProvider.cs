using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Mighty;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
    public class AdminUserProvider : IAdminUserProvider
    {
        private readonly string _acvpConnectionString;

        public AdminUserProvider(IConnectionStringFactory connectionStringFactory)
        {
            _acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
        }
        
        public List<AdminUser> GetUsers()
        {
            var db = new MightyOrm<AdminUser>(_acvpConnectionString);

            return db.QueryFromProcedure("common.AdminUserListGet").ToList();
        }

        public async Task<bool> IsUserAuthorized(string email)
        {
            var db = new MightyOrm(_acvpConnectionString);

            var result = await db.ExecuteProcedureAsync("common.AdminUserIsAuthorized",
                inParams: new
                {
                    email
                },
                outParams: new
                {
                    isValid = false
                });

            return result.isValid;
        }

        public void AddUser(string email)
        {
            var db = new MightyOrm(_acvpConnectionString);
            db.ExecuteProcedure("common.AdminUserAdd", new {email});
        }
    }
}