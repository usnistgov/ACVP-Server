using System;
using Microsoft.Extensions.Logging;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;

namespace CertificateCommonNameUpdatinator.Services
{
	public class AcvpUserSubjectService : IAcvpUserSubjectService
	{
		private readonly ILogger<AcvpUserSubjectService> _logger;
		private readonly IAcvpUserService _acvpUserService;
		
		public AcvpUserSubjectService(ILogger<AcvpUserSubjectService> logger, IAcvpUserService acvpUserService)
		{
			_logger = logger;
			_acvpUserService = acvpUserService;
		}
		
		public void UpdateUserSubjectsFromCertBytes()
		{
			_logger.LogInformation("Getting user list...");
			var userLites =
				_acvpUserService.GetUserList(new AcvpUserListParameters()
				{
					Page = 1,
					PageSize = 2048 // /shrug
				});

			if (userLites.TotalRecords == 0)
			{
				throw new Exception("Unable to retrieve users to update.");
			}
			
			_logger.LogInformation($"User list count: {userLites.TotalRecords}");
			
			foreach (var userLite in userLites.Data)
			{
				_logger.LogInformation($"Processing user: {userLite.FullName} ({userLite.AcvpUserId})");
				var user = _acvpUserService.GetUserDetails(userLite.AcvpUserId);

				if (user == null)
				{
					throw new Exception($"Unable to retrieve user details for {userLite.FullName} ({userLite.AcvpUserId})");
				}
				
				// This updates the cert to itself effectively, but also updates the common name to be the subject from the cert
				var result = _acvpUserService.SetUserCertificate(user.AcvpUserId, new AcvpUserCertificateUpdateParameters()
				{
					Certificate = Convert.FromBase64String(user.CertificateBase64)
				});

				if (!result.IsSuccess)
				{
					throw new Exception($"Unable to update user common name. Reason: {result.ErrorMessage}");
				}
			}
			
			_logger.LogInformation("Done updating user cert subjects.");
		}
	}
}