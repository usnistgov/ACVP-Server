using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep
{
    public class KdfParameterOneStep : IKdfParameter
    {
        /// <inheritdoc />
        public KasKdf KdfType => KasKdf.OneStep;
        public bool RequiresAdditionalNoncePair => false;
        /// <summary>
        /// The AuxFunction (hash or mac) to use with the KDF.
        /// </summary>
        public KasKdfOneStepAuxFunction AuxFunction { get; set; }
        /// <summary>
        /// A salt value for use when the AuxFunction used is a MAC algorithm.
        /// </summary>
        public BitString Salt { get; set; }
        /// <summary>
        /// The Iv used in some KDFs.
        /// </summary>
        public BitString Iv { get; set; }
        /// <summary>
        /// The shared secret for use in deriving a key.
        /// </summary>
        public BitString Z { get; set; }
        /// <summary>
        /// The length of the key to derive.
        /// </summary>
        public int L { get; set; }
        /// <summary>
        /// The algorithm ID indicator.
        /// </summary>
        public BitString AlgorithmId { get; set; }
        /// <summary>
        /// The Label for the transaction.
        /// </summary>
        public BitString Label { get; set; }
        /// <summary>
        /// The Context for the transaction.
        /// </summary>
        public BitString Context { get; set; }
        public BitString AdditionalInitiatorNonce { get; set; }
        public BitString AdditionalResponderNonce { get; set; }
        /// <summary>
        /// The pattern to use when constructing fixed info.
        /// </summary>
        public string FixedInfoPattern { get; set; }
        /// <summary>
        /// The encoding type of the fixedInput
        /// </summary>
        public FixedInfoEncoding FixedInputEncoding { get; set; }

        public KdfResult AcceptKdf(IKdfVisitor visitor, BitString fixedInfo)
        {
            return visitor.Kdf(this, fixedInfo);
        }

        public void SetEphemeralData(BitString initiatorData, BitString responderData)
        {
            // Not used for this kdf
        }
    }
}