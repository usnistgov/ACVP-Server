using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Admin.Controllers
{
    [Authorize]
    public class BaseAuthorizedController : ControllerBase
    {
        
    }
}