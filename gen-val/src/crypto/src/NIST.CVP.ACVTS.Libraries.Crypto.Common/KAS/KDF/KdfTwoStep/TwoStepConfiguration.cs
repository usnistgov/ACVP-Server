using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTwoStep
{
    public class TwoStepConfiguration : IKdfConfiguration
    {
        public Kda KdfType => Kda.TwoStep;
        public bool RequiresAdditionalNoncePair => false;
        public int L { get; set; }
        public int SaltLen { get; set; }
        public MacSaltMethod SaltMethod { get; set; }
        public string FixedInfoPattern { get; set; }
        public FixedInfoEncoding FixedInfoEncoding { get; set; }
        /// <summary>
        /// The TwoStep KDF mode.
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
        /// The length of the counter.
        /// </summary>
        public int CounterLen { get; set; }
        /// <summary>
        /// The IV length.
        /// </summary>
        public int IvLen { get; set; }

        public IKdfParameter GetKdfParameter(IKdfParameterVisitor visitor)
        {
            return visitor.CreateParameter(this);
        }
    }
}
