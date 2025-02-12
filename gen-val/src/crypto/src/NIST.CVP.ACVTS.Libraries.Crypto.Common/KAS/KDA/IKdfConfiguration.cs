﻿using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA
{
    /// <summary>
    /// Used to describe the valid ranges, primitives, etc in use for a KDF as described in:
    ///
    /// https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-56Cr1.pdf
    /// https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication800-108.pdf
    /// https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication800-135r1.pdf
    /// </summary>
    public interface IKdfConfiguration
    {
        /// <summary>
        /// The enum type of KDF
        /// </summary>
        Kda KdfType { get; }
        /// <summary>
        /// Some KDFs require a second set of nonces outside the generation scope of the KAS scheme.
        /// This flag is used to indicate when an additional pair of nonces is required.
        /// </summary>
        /// <remarks>This flag should be true for all SP800-135 KDFs, and false for the SP800-56C KDFs.</remarks>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        bool RequiresAdditionalNoncePair { get; }
        /// <summary>
        /// The length of the key to derive.
        /// </summary>
        int L { get; set; }
        /// <summary>
        /// The length of the salt used.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        int SaltLen { get; set; }
        /// <summary>
        /// How the salt is constructed.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        MacSaltMethod SaltMethod { get; set; }
        /// <summary>
        /// The pattern used for FixedInputConstruction.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        string FixedInfoPattern { get; set; }
        /// <summary>
        /// The encoding type of the fixedInput
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        FixedInfoEncoding FixedInfoEncoding { get; set; }

        /// <summary>
        /// Utilize a <see cref="IKdfParameterVisitor"/> to create a <see cref="IKdfParameter"/> for use in a <see cref="IKdf"/>
        /// </summary>
        /// <param name="visitor">The visitor for creating <see cref="IKdfParameter"/>s.</param>
        /// <returns></returns>
        IKdfParameter GetKdfParameter(IKdfParameterVisitor visitor);
    }
}
