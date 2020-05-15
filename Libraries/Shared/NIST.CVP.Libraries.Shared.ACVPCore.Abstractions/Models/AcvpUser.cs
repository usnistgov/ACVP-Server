using System;
using System.Security.Cryptography.X509Certificates;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models
{
    public class AcvpUser : AcvpUserLite
    {
        private string _certificateBase64;
        public string CertificateBase64
        {
            get => _certificateBase64;
            set
            {
                _certificateBase64 = value;
                var cert = new X509Certificate(Convert.FromBase64String(_certificateBase64));
                
                ExpiresOn = DateTime.UnixEpoch;
                if (DateTime.TryParse(cert.GetExpirationDateString(), out var expiresOn))
                {
                    ExpiresOn = expiresOn;
                }

                Issuer = cert.Issuer;
            }
        }
        public string CommonName { get; set; }
        public string Seed { get; set; }
        public string Issuer { get; private set; }
    }
}