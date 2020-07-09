using System;
using System.Collections.Generic;
using System.Text.Json;
using JwtCreatinator.Models;
using Microsoft.Extensions.Logging;
using Web.Public.Services;

namespace JwtCreatinator.Services
{
	public class JwtCreatinator : IJwtCreatinator
	{
		private readonly ILogger<JwtCreatinator> _logger;
		private readonly IJwtService _jwtService;
		private readonly ITestSessionService _testSessionService;
		
		public JwtCreatinator(ILogger<JwtCreatinator> logger, IJwtService jwtService, ITestSessionService testSessionService)
		{
			_logger = logger;
			_jwtService = jwtService;
			_testSessionService = testSessionService;
		}
		
		public List<JwtRenewResponse> CreateJwts(List<JwtRenewRequest> requests)
		{
			var list = new List<JwtRenewResponse>();
			
			foreach (var request in requests)
			{
				// Get the test session
				var testSession = _testSessionService.GetTestSession(request.TestSessionId);

				if (testSession == null)
				{
					_logger.LogError($"Unable to retrieve test session information for ID: {request.TestSessionId}");
					continue;
				}
				
				var claims = new Dictionary<string, string>
				{
					{"tsId", JsonSerializer.Serialize(testSession.ID)},
					{"vsId", JsonSerializer.Serialize(testSession.VectorSetIDs)}
				};
				var jwt = _jwtService.Create(request.ClientCertSubject, claims);
				
				if (!_jwtService.IsTokenValid(request.ClientCertSubject, jwt.Token, true))
				{
					_logger.LogError($"The token that we literally just created for {testSession.ID} didn't validate.");
				}
				else
				{
					_logger.LogInformation($"Token validated for {testSession.ID}.");
				}
				
				list.Add(new JwtRenewResponse()
				{
					Jwt = jwt.Token,
					TestSessionId = testSession.ID
				});
				
				_logger.LogInformation($"Create new JWT for test session ID: {request.TestSessionId}");
			}

			return list;
		}
	}
}