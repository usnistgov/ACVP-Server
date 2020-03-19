using System.ComponentModel.DataAnnotations;

namespace Web.Admin.Auth.Models
{
    /// <summary>
    /// Single Sign On configuration options
    /// </summary>
    public class SsoConfig
    {
        /// <summary>
        /// Should SSO be used?
        /// </summary>
        /// <remarks>Should only be false for local development.</remarks>
        public bool UseSso { get; set; }
        /// <summary>
        /// The endpoint to get AD-FS meta data. 
        /// </summary>
        public string AdfsMetadata { get; set; }
        /// <summary>
        /// The application root url.
        /// </summary>
        public string WtRealm { get; set; }
    }
}