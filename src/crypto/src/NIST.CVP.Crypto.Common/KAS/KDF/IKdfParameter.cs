using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.KDF
{
    /// <summary>
    /// Represents information needed for invoking a KDF function
    /// </summary>
    public interface IKdfParameter
    {
        /// <summary>
        /// The type of KDF supported.
        /// </summary>
        KasKdf KdfType { get; }
        /// <summary>
        /// The Salt used with MAC based KDFs.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        BitString Salt { get; set; }
        /// <summary>
        /// The IV used with some KDFs.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        BitString Iv { get; set; }
        /// <summary>
        /// The shared secret for use in deriving a key.
        /// </summary>
        [JsonIgnore]
        BitString Z { get; set; }
        /// <summary>
        /// The length of the key to derive.
        /// </summary>
        int L { get; set; }
        /// <summary>
        /// The pattern to use when constructing fixed info.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        string FixedInfoPattern { get; set; }
        /// <summary>
        /// The encoding type of the fixedInput
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        FixedInfoEncoding FixedInputEncoding { get; set; }
        /// <summary>
        /// The algorithm ID indicator.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        BitString AlgorithmId { get; set; }
        /// <summary>
        /// The Label for the transaction.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        BitString Label { get; set; }
        /// <summary>
        /// The Context for the transaction.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        BitString Context { get; set; }
        /// <summary>
        /// Accepts a <see cref="IKdfVisitor"/> this will in turn dispatch a call to a supported KDF.
        /// </summary>
        /// <param name="visitor">Describes how to invoke a KDF for implementors.</param>
        /// <param name="fixedInfo">The contextual fixed information to be plugged into a kdf.</param>
        /// <returns>A derived key.</returns>
        KdfResult AcceptKdf(IKdfVisitor visitor, BitString fixedInfo);
    }
}