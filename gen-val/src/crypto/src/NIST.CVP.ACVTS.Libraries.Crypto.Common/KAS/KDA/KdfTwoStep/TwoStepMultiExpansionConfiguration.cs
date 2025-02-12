﻿using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTwoStep
{
    public class TwoStepMultiExpansionConfiguration : IKdfMultiExpansionConfiguration
    {
        public Kda KdfType => Kda.TwoStep;
        public int L { get; set; }
        public int SaltLen { get; set; }
        public MacSaltMethod SaltMethod { get; set; }
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
        public IKdfMultiExpansionParameter GetKdfParameter(IKdfMultiExpansionParameterVisitor visitor)
        {
            return visitor.CreateParameter(this);
        }
    }
}
