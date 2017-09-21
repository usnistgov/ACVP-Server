using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.KAS
{
    public class Parameters : IParameters, ISimpleValidatable
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
        public string[] Function { get; set; }
        /// <summary>
        /// The KAS schemes to test
        /// </summary>
        public Schemes Scheme { get; set; }

        public IEnumerable<string> Validate()
        {
            List<string> validationErrors = new List<string>();
            
            // Algorithm provided
            if (string.IsNullOrEmpty(Algorithm))
            {
                validationErrors.Add($"{nameof(Algorithm)} may not be null");
                return validationErrors;
            }

            // Scheme provided
            if (Scheme == null)
            {
                validationErrors.Add("At least one scheme must be provided.");
                return validationErrors;
            }

            // Validate scheme
            validationErrors.AddRangeIfNotNullOrEmpty(Scheme.Validate());

            return validationErrors;
        }
    }

    /// <summary>
    /// Wraps the possible KAS schemes
    /// </summary>
    public class Schemes : ISimpleValidatable
    {
        /// <summary>
        /// Diffie Hellman Ephemeral
        /// </summary>
        public DhEphem DhEphem { get; set; }

        public IEnumerable<string> Validate()
        {
            List<string> errorList = new List<string>();

            // At least one non-null Scheme
            if (DhEphem == null)
            {
                errorList.Add("No valid schemes provided");
                return errorList;
            }

            errorList.AddRangeIfNotNullOrEmpty(DhEphem?.Validate());

            return errorList;
        }
    }

    /// <summary>
    /// Describes information needed for a scheme
    /// </summary>
    public abstract class SchemeBase : ISimpleValidatable
    {
        /// <summary>
        /// The Key Agreement ROles
        /// </summary>
        public string[] Role { get; set; }
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

        public IEnumerable<string> Validate()
        {
            List<string> errorList = new List<string>();

            // Role provided
            if (Role == null || Role.Length == 0)
            {
                errorList.Add($"{Role} not provided.");
            }

            // At least one KAS method is required.
            if (NoKdfNoKc == null && KdfNoKc == null && KdfKc == null)
            {
                errorList.Add("No valid KAS methods were provided");
            }

            // Validate the non null KAS methods
            errorList.AddRangeIfNotNullOrEmpty(NoKdfNoKc?.Validate());
            errorList.AddRangeIfNotNullOrEmpty(KdfNoKc?.Validate());
            errorList.AddRangeIfNotNullOrEmpty(KdfKc?.Validate());

            return errorList;
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for DiffieHellman ephemeral
    /// </summary>
    public class DhEphem : SchemeBase { }

    /// <summary>
    /// Registration requirements for NoKdfNoKc
    /// </summary>
    public class NoKdfNoKc : ISimpleValidatable
    {
        protected List<string> ErrorList = new List<string>();

        /// <summary>
        /// The parameter sets used by the scheme
        /// </summary>
        public ParameterSets ParameterSet { get; set; }

        public virtual IEnumerable<string> Validate()
        {
            if (ParameterSet == null)
            {
                ErrorList.Add($"{ParameterSet} must not be null");
            }

            ErrorList.AddRangeIfNotNullOrEmpty(ParameterSet?.Validate());

            return ErrorList;
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration requirements for KdfNoKc
    /// </summary>
    public class KdfNoKc : NoKdfNoKc
    {
        /// <summary>
        /// KDF specific options
        /// </summary>
        public KdfOptions KdfOption { get; set; }

        public override IEnumerable<string> Validate()
        {
            base.Validate();

            if (KdfOption == null)
            {
                ErrorList.Add($"{KdfOption} cannot be null.");
            }

            ErrorList.AddRangeIfNotNullOrEmpty(KdfOption?.Validate());

            return ErrorList;
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration requirements for Kc
    /// </summary>
    public class KdfKc : KdfNoKc
    {
        /// <summary>
        /// KC specific options
        /// </summary>
        public KcOptions KcOption { get; set; }

        public override IEnumerable<string> Validate()
        {
            base.Validate();

            if (KcOption == null)
            {
                ErrorList.Add($"{KcOption} cannot be null.");
            }

            ErrorList.AddRangeIfNotNullOrEmpty(KcOption?.Validate());

            return ErrorList;
        }
    }

    /// <summary>
    /// The possible parameter sets for the scheme
    /// </summary>
    public class ParameterSets : ISimpleValidatable
    {
        /// <summary>
        /// The FB parameter set
        /// </summary>
        public Fb Fb { get; set; }
        /// <summary>
        /// The FC parameter set
        /// </summary>
        public Fc Fc { get; set; }

        public IEnumerable<string> Validate()
        {
            List<string> errorList = new List<string>();

            // At least one parameter set
            if (Fb == null && Fc == null)
            {
                errorList.Add("At least one ParameterSet is required.");
            }

            errorList.AddRangeIfNotNullOrEmpty(Fb?.Validate());
            errorList.AddRangeIfNotNullOrEmpty(Fc?.Validate());

            return errorList;
        }
    }

    /// <summary>
    /// The options for a parameter set
    /// </summary>
    public abstract class ParameterSetBase : ISimpleValidatable
    {
        /// <summary>
        /// Array of SHA modes used for DSA
        /// </summary>
        public string[] HashAlg { get; set; }
        /// <summary>
        /// The MAC options for NoKc and Kc
        /// </summary>
        public MacOptions MacOption { get; set; }

        public virtual IEnumerable<string> Validate()
        {
            List<string> errorList = new List<string>();

            // at least one hashAlg
            if (HashAlg == null || HashAlg.Length == 0)
            {
                errorList.Add($"At least one {HashAlg} is required.");
            }

            errorList.AddRangeIfNotNullOrEmpty(MacOption?.Validate());

            return errorList;
        }
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
    public class KdfOptions : ISimpleValidatable
    {
        public string Concatenation { get; set; }
        public string Asn1 { get; set; }
        public IEnumerable<string> Validate()
        {
            List<string> errorList = new List<string>();

            if (string.IsNullOrEmpty(Concatenation) && string.IsNullOrEmpty(Asn1))
            {
                errorList.Add($"At least one KDF method must be supplied.");
            }

            return errorList;
        }
    }

    /// <summary>
    /// Options for a MAC
    /// </summary>
    public class MacOptions : ISimpleValidatable
    {
        [JsonProperty(PropertyName = "AES-CCM")]
        public MacOptionAesCcm AesCcm { get; set; }
        [JsonProperty(PropertyName = "CMAC")]
        public MacOptionCmac Cmac { get; set; }
        [JsonProperty(PropertyName = "HMAC-SHA2-224")]
        public MacOptionHmacSha2_d224 HmacSha2_D224 { get; set; }
        [JsonProperty(PropertyName = "HMAC-SHA2-256")]
        public MacOptionHmacSha2_d256 HmacSha2_D256 { get; set; }
        [JsonProperty(PropertyName = "HMAC-SHA2-384")]
        public MacOptionHmacSha2_d384 HmacSha2_D384 { get; set; }
        [JsonProperty(PropertyName = "HMAC-SHA2-512")]
        public MacOptionHmacSha2_d512 HmacSha2_D512 { get; set; }

        public IEnumerable<string> Validate()
        {
            List<string> errorList = new List<string>();

            if (AesCcm == null && Cmac == null && HmacSha2_D224 == null && HmacSha2_D256 == null &&
                HmacSha2_D384 == null && HmacSha2_D512 == null)
            {
                errorList.Add("At least one MAC is required.");
            }

            errorList.AddRangeIfNotNullOrEmpty(AesCcm?.Validate());
            errorList.AddRangeIfNotNullOrEmpty(Cmac?.Validate());
            errorList.AddRangeIfNotNullOrEmpty(HmacSha2_D224?.Validate());
            errorList.AddRangeIfNotNullOrEmpty(HmacSha2_D256?.Validate());
            errorList.AddRangeIfNotNullOrEmpty(HmacSha2_D384?.Validate());
            errorList.AddRangeIfNotNullOrEmpty(HmacSha2_D512?.Validate());

            return errorList;
        }
    }

    /// <summary>
    /// Mac Options shared between MAC types
    /// </summary>
    public abstract class MacOptionsBase : ISimpleValidatable
    {
        protected List<string> ErrorList = new List<string>();

        public int[] KeyLen { get; set; }
        public int MacLen { get; set; }
        public virtual IEnumerable<string> Validate()
        {
            if (KeyLen == null || KeyLen.Length == 0)
            {
                ErrorList.Add($"At least one {KeyLen} is required.");
            }

            if (MacLen == 0)
            {
                ErrorList.Add($"{MacLen} cannot be 0.");
            }

            return ErrorList;
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Aes-Ccm
    /// </summary>
    public class MacOptionAesCcm : MacOptionsBase
    {
        public int NonceLen { get; set; }

        public override IEnumerable<string> Validate()
        {
            base.Validate();

            if (NonceLen == 0)
            {
                ErrorList.Add($"{NonceLen} cannot be 0.");
            }

            return ErrorList;
        }
    }
    /// <inheritdoc />
    /// <summary>
    /// Cmac
    /// </summary>
    public class MacOptionCmac : MacOptionsBase { }
    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha2-224
    /// </summary>
    public class MacOptionHmacSha2_d224 : MacOptionsBase { }
    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha2-256
    /// </summary>
    public class MacOptionHmacSha2_d256 : MacOptionsBase { }
    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha2-384
    /// </summary>
    public class MacOptionHmacSha2_d384 : MacOptionsBase { }
    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha2 512
    /// </summary>
    public class MacOptionHmacSha2_d512 : MacOptionsBase { }

    /// <summary>
    /// The key confirmation options
    /// </summary>
    public class KcOptions : ISimpleValidatable
    {
        /// <summary>
        /// The key confirmation roles supported
        /// </summary>
        public string[] KcRole { get; set; }
        /// <summary>
        /// The Key confirmation types supported
        /// </summary>
        public string[] KcType { get; set; }
        /// <summary>
        /// The nonce types supported
        /// </summary>
        public string[] NonceType { get; set; }

        public IEnumerable<string> Validate()
        {
            List<string> errorList = new List<string>();

            if (KcRole == null || KcRole.Length == 0)
            {
                errorList.Add($"At least one {KcRole} must be provided.");
            }

            if (KcType == null || KcType.Length == 0)
            {
                errorList.Add($"At least one {KcType} must be provided.");
            }

            if (NonceType == null || NonceType.Length == 0)
            {
                errorList.Add($"At least one {KcType} must be provided.");
            }

            return errorList;
        }
    }
}