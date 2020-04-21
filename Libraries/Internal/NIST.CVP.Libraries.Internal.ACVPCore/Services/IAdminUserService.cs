using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
    /// <summary>
    /// Interface for working with users that are authorized to access the Web.Admin site.
    /// </summary>
    public interface IAdminUserService
    {
        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>The users that are allowed to access Web.Admin.</returns>
        List<AdminUser> GetUsers();
        /// <summary>
        /// Determine if a user is authorized to access Web.Admin.
        /// </summary>
        /// <param name="email">The email address to check for access.</param>
        /// <returns></returns>
        Task<bool> IsUserAuthorized(string email);
        /// <summary>
        /// Add a user's email address as a authorized user to Web.Admin.
        /// </summary>
        /// <param name="email">The email address to add as an authorized user.</param>
        void AddUser(string email);
    }
}