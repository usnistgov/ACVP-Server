using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrentUserController : BaseAuthorizedController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("email")]
        public string GetCurrentUserEmail()
        {
            return _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        }
        
        [HttpGet]
        public Dictionary<string, string> GetClaims()
        {
            var results = new Dictionary<string, string>();

            foreach (var claim in _httpContextAccessor.HttpContext.User.Claims)
            {
                results.Add(claim.Type, claim.Value);
            }

            return results;
        }
    }
}