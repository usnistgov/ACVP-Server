using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
using NIST.CVP.Math;
using Web.Public.Configs;

namespace Web.Public.Services
{
	public class MtlsCertValidatorService : IMtlsCertValidatorService
	{
		private const string AuthorityKeyIdentifier = "Authority Key Identifier";
		
		private readonly ILogger<MtlsCertValidatorService> _logger;
		private readonly BitString _caCertSubjectKeyIdentifier;
		
		public MtlsCertValidatorService(ILogger<MtlsCertValidatorService> logger, IOptions<MtlsConfig> mtlsConfig)
		{
			_logger = logger;
			
			if (mtlsConfig?.Value == null)
			{
				var missingConfigErrorMessage = $"{nameof(mtlsConfig)} was not constructed properly.";
				_logger.LogCritical(missingConfigErrorMessage);
				throw new ArgumentException(missingConfigErrorMessage);
			}

			_caCertSubjectKeyIdentifier = new BitString(mtlsConfig.Value.CaSubjectKeyId);
		}
		
		public bool IsValid(X509Certificate2 clientCert)
		{
			if (clientCert == null)
				return false;

			foreach (var extension in clientCert.Extensions)
			{
				if (extension.Oid.FriendlyName.Equals(AuthorityKeyIdentifier, StringComparison.OrdinalIgnoreCase))
				{
					try
					{
						/*
							There is some additional data at the beginning of the raw data, i'm guessing "KeyID=",
							though I've not been able to get that information out of the first few bytes.
							
							Taking the least significant bytes from the raw data to match the length of 
							the CA public key seems to work well enough					 
						*/
						var authorityKeyIdentifier = new BitString(extension.RawData)
							.GetLeastSignificantBits(_caCertSubjectKeyIdentifier.BitLength);
					
						if (_caCertSubjectKeyIdentifier.Equals(authorityKeyIdentifier))
						{
							return true;
						}
					}
					catch (Exception e)
					{
						_logger.LogError(e);
						return false;
					}
				}
			}
			
			_logger.LogError($"Certificate with subject {clientCert.Subject} was not signed by our CA.");
			return false;
		}
	}
}