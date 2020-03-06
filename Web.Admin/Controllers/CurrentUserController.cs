using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrentUserController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
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