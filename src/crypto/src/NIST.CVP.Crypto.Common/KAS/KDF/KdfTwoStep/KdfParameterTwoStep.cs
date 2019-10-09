using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.KDF.KdfTwoStep
{
    public class KdfParameterTwoStep : IKdfParameter
    {
        public KasKdf KdfType => KasKdf.TwoStep;
        public BitString Salt { get; set; }
        public BitString Z { get; set; }
        public int L { get; set; }
        public string FixedInfoPattern { get; set; }
        public FixedInfoEncoding FixedInputEncoding { get; set; }
        /// <summary>
        /// The IV in use in the KDF.
        /// </summary>
        public BitString Iv { get; set; }
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
        /// <summary>
        /// The TwoStep KDF mode
        /// </summary>
        public KdfModes KdfMode { get; set; }
        /// <summary>
        /// The MAC used for the KDF.
        /// </summary>
        public MacModes MacMode { get; set; }
        /// <summary>
        /// Where the counter is located within the data fed into the KDF.
        /// </summary>
        public CounterLocations CounterLocation { get; set; }
        /// <summary>
        /// The length of the counter
        /// </summary>
        public int CounterLen { get; set; }
        public KdfResult AcceptKdf(IKdfVisitor visitor, BitString fixedInfo)
        {
            return visitor.Kdf(this, fixedInfo);
        }

        public void SetEphemeralData(BitString initiatorData, BitString responderData)
        {
            // not used for this kdf
        }
    }
}
