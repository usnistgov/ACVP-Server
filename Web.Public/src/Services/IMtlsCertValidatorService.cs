using System.Security.Cryptography.X509Certificates;

namespace Web.Public.Services
{
	/// <summary>
	/// Provides a means of validating a client certificate.
	/// </summary>
	public interface IMtlsCertValidatorService
	{
		/// <summary>
		/// Is the provided <see cref="X509Certificate2"/> valid?
		/// </summary>
		/// <param name="clientCert">The client certificate.</param>
		/// <returns>true when the certificate is valid, false otherwise.</returns>
		bool IsValid(X509Certificate2 clientCert);
	}
}