using ACVPCore.ExtensionMethods;
using ACVPCore.Models;
using ACVPCore.Results;
using ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;

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
        
        [HttpGet]
        public WrappedEnumerable<AcvpUserLite> Get()
        {
            return _adminUserService.GetUserList().WrapEnumerable();
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