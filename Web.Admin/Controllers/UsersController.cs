using ACVPCore.ExtensionMethods;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseAuthorizedController
    {
        private readonly IAcvpUserService _adminUserService;

        public UsersController(IAcvpUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }
        
        [HttpPost]
        public ActionResult<WrappedEnumerable<AcvpUserLite>> Get(AcvpUserListParameters param)
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

        [HttpPost("{userId}/seed")]
        public Result SetUserTotpSeed(long userId, [FromBody] string seed)
        {
            return _adminUserService.SetUserTotpSeed(userId, seed);
        }
    }
}