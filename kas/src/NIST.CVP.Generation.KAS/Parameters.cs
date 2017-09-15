using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS
{
    public class Parameters : IParameters
    {
        /// <inheritdoc />
        public string Algorithm { get; set; }
        /// <inheritdoc />
        public string Mode { get; set; }
        /// <inheritdoc />
        public bool IsSample { get; set; }
        /// <summary>
        /// KAS Assurances
        /// </summary>
        public string[] Functions { get; set; }
        /// <summary>
        /// The KAS schemes to test
        /// </summary>
        public Schemes Schemes { get; set; }
    }
    
    /// <summary>
    /// Wraps the possible KAS schemes
    /// </summary>
    public class Schemes
    {
        /// <summary>
        /// Diffie Hellman Ephemeral
        /// </summary>
        public DhEphem DhEphem { get; set; }
    }

    /// <summary>
    /// Describes information needed for a scheme
    /// </summary>
    public abstract class SchemeBase
    {
        /// <summary>
        /// The Key Agreement ROles
        /// </summary>
        public string[] Roles { get; set; }
        /// <summary>
        /// Registration options for NoKdfNoKc (Component only test)
        /// </summary>
        public NoKdfNoKc NoKdfNoKc { get; set; }
        /// <summary>
        /// Registration options for KdfNoKc
        /// </summary>
        public KdfNoKc KdfNoKc { get; set; }
        /// <summary>
        /// Registration options for KdfKc
        /// </summary>
        public KdfKc KdfKc { get; set; }
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for DiffieHellman ephemeral
    /// </summary>
    public class DhEphem : SchemeBase { }

    /// <summary>
    /// Registration requirements for NoKdfNoKc
    /// </summary>
    public class NoKdfNoKc
    {
        /// <summary>
        /// The parameter sets used by the scheme
        /// </summary>
        public ParameterSets ParameterSets { get; set; }
    }

    /// <summary>
    /// Registration requirements for KdfNoKc
    /// </summary>
    public class KdfNoKc : NoKdfNoKc
    {
        /// <summary>
        /// KDF specific options
        /// </summary>
        public KdfOptions KdfOptions { get; set; }
    }

    /// <summary>
    /// Registration requirements for Kc
    /// </summary>
    public class KdfKc : KdfNoKc
    {
        /// <summary>
        /// KC specific options
        /// </summary>
        public KcOptions KcOptions { get; set; }
    }

    /// <summary>
    /// The possible parameter sets for the scheme
    /// </summary>
    public class ParameterSets
    {
        /// <summary>
        /// The FB parameter set
        /// </summary>
        public Fb Fb { get; set; }
        /// <summary>
        /// The FC parameter set
        /// </summary>
        public Fc Fc { get; set; }
    }

    /// <summary>
    /// The options for a parameter set
    /// </summary>
    public abstract class ParameterSetBase
    {
        /// <summary>
        /// Array of SHA modes used for DSA
        /// </summary>
        public string[] Shas { get; set; }
        /// <summary>
        /// The MAC options for NoKc and Kc
        /// </summary>
        public MacOptions[] MacOptions { get; set; }
    }

    /// <inheritdoc />
    /// <summary>Fb</summary>
    public class Fb : ParameterSetBase { }

    /// <inheritdoc />
    /// <summary>Fc</summary>
    public class Fc : ParameterSetBase { }

    /// <summary>
    /// The options for a Kdf function
    /// </summary>
    /// TODO - this will limit OI pattern to the KDF, rather than the KDF type
    public class KdfOptions
    {
        /// <summary>
        /// The types of KDFs to test
        /// </summary>
        public string[] Types { get; set; }
        /// <summary>
        /// The other info pattern to apply
        /// </summary>
        public string OiPattern { get; set; }
    }
    
    /// <summary>
    /// Options for a MAC
    /// </summary>
    public class MacOptions
    {
        /// <summary>
        /// The type of MAC (AES-CCM, CMAC, HMAC-Sha2-254, etc)
        /// </summary>
        public string MacType { get; set; }
        /// <summary>
        /// Supported key lengths for the HMAC
        /// </summary>
        public int[] KeyLens { get; set; }
        /// <summary>
        /// The nonce length to use (AES-CCM only)
        /// </summary>
        public int NonceLen { get; set; }
        /// <summary>
        /// The length of the mac to output
        /// </summary>
        public int MacLen { get; set; }
    }
    
    /// <summary>
    /// The key confirmation options
    /// </summary>
    public class KcOptions
    {
        /// <summary>
        /// The key confirmation roles supported
        /// </summary>
        public string[] KcRoles { get; set; }
        /// <summary>
        /// The Key confirmation types supported
        /// </summary>
        public string[] KcTypes { get; set; }
        /// <summary>
        /// The nonce types supported
        /// </summary>
        public string[] NonceTypes { get; set; }
    }
}