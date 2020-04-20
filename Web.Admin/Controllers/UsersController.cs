using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;
using NIST.CVP.Enumerables;
using NIST.CVP.Results;
using System;

namespace Web.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IAcvpUserService _adminUserService;

        public UsersController(IAcvpUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }
        
        [HttpPost]
        public ActionResult<PagedEnumerable<AcvpUserLite>> Get(AcvpUserListParameters param)
        {
            if (param == null)
                return new BadRequestResult();
            
            return _adminUserService.GetUserList(param);
        }

        [HttpGet("{userId}")]
        public ActionResult<AcvpUser> GetUserDetails(long userId)
        {
            var user = _adminUserService.GetUserDetails(userId);

            if (user == null)
                return new NotFoundResult();

            return user;
        }

        [HttpPut]
        public Result CreateUser([FromBody] AcvpUserCreateParameters param)
        {
            return _adminUserService.CreateUser(param);
        }

        [HttpPost("{userId}/seed")]
        public Result SetUserTotpSeed(long userId, [FromBody] AcvpUserSeedUpdateParameters param)
        {
            return _adminUserService.SetUserTotpSeed(userId, param);
        }

        [HttpPost("{userId}/seed/refresh")]
        public Result RefreshTotpSeed(long userId)
        {
            return _adminUserService.RefreshTotpSeed(userId);
        }

        [HttpPost("{userId}/certificate")]
        public Result SetUserCertificate(long userId, [FromBody] AcvpUserCertificateUpdateParameters param)
        {
            return _adminUserService.SetUserCertificate(userId, param);
        }

        [HttpDelete("{userId}")]
        public Result DeleteUser(long userId)
        {
            return _adminUserService.DeleteUser(userId);
        }
    }
}