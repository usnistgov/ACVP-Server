using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Public.Exceptions;
using Web.Public.Services;

namespace Web.Public.Controllers
{
	[Authorize]
	[TypeFilter(typeof(ExceptionFilter))]
	[ApiController]
	public abstract class JwtAuthControllerBase : ControllerBase
	{
		protected readonly IJwtService _jwtService;

		protected JwtAuthControllerBase(IJwtService jwtService)
		{
			_jwtService = jwtService;
		}

		protected string GetJwt()
		{
			return Request.Headers["Authorization"];
		}
		
		protected string GetCertSubjectFromJwt()
		{
			return _jwtService
				.GetClaimsFromJwt(Request.Headers["Authorization"])
				.First(w => w.Key.Equals("sub", StringComparison.OrdinalIgnoreCase)).Value;
		}
	}
}