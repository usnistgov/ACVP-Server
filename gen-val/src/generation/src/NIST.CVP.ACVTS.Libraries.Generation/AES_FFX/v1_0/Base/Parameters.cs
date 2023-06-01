using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_FFX.v1_0.Base
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; } = "1.0";
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };
        public string[] Direction { get; set; }
        public int[] KeyLen { get; set; }
        public MathDomain TweakLen { get; set; }
        public Capability[] Capabilities { get; set; }
    }

    public class Capability
    {
        /// <summary>
        /// The alphabet supported by the IUT.  The first character represents a "0" as a numeral string.
        /// Should not have any repeating characters.
        /// </summary>
        public string Alphabet { get; set; }
        /// <summary>
        /// The base should match the number of characters from the alphabet.
        /// </summary>
        public int Radix { get; set; }
        /// <summary>
        /// The minimum length of messages to encrypt with the provided alphabet.
        /// </summary>
        public int MinLen { get; set; }
        /// <summary>
        /// The maximum length of messages to encrypt with the provided alphabet.
        /// </summary>
        public int MaxLen { get; set; }
    }
}
