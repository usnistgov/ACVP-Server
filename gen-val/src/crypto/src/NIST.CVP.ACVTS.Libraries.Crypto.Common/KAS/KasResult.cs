using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS
{
    public class KasResult : ICryptoResult
    {
        /// <summary>
        /// Constructor used to create result with shared secret Z and Hashed Z within tag.  
        /// Used for component only tests
        /// </summary>
        /// <param name="z">The shared secret</param>
        /// <param name="tag">The hashed secret</param>
        public KasResult(BitString z, BitString tag)
        {
            Z = z;
            Tag = tag;
        }

        /// <summary>
        /// Constructor used to create result for non component tests.
        /// </summary>
        /// <param name="z">The derived secret</param>
        /// <param name="oi">Other information</param>
        /// <param name="dkm">The derived keying material utilized in H(dkm, macData)</param>
        /// <param name="macData">the derived macData utilized in H(dkm, macData)</param>
        /// <param name="tag">The result of H(dkm, macData)</param>
        public KasResult(BitString z, BitString oi, BitString dkm, BitString macData, BitString tag)
        {
            Z = z;
            Oi = oi;
            Dkm = dkm;
            MacData = macData;
            Tag = tag;
        }

        /// <summary>
        /// The Shared Secret.
        /// </summary>
        /// <param name="z">The Shared secret.</param>
        public KasResult(BitString z)
        {
            Z = z;
            Dkm = z;
        }

        /// <summary>
        /// Constructor used to create result for (at the time of writing) KAS-IFC.
        /// </summary>
        /// <param name="dkm">The derived keying material minus any bits used for the macKey.</param>
        /// <param name="macKey">The macKey that is used for keyConfirmation, taken from most significant bits of the derivedKey.</param>
        /// <param name="macData">The data that is plugged into the message parameter of a mac function.</param>
        /// <param name="tag">The resulting tag of H(macKey, macData).</param>
        public KasResult(BitString dkm, BitString macKey, BitString macData, BitString tag)
        {
            Dkm = dkm;
            MacKey = macKey;
            MacData = macData;
            Tag = tag;
        }

        /// <summary>
        /// Kas error constructor
        /// </summary>
        /// <param name="errorMessage">Error information</param>
        public KasResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// The derived secret
        /// </summary>
        public BitString Z { get; }
        /// <summary>
        /// The result of hashing/macing the derived secret
        /// </summary>
        public BitString Tag { get; }
        /// <summary>
        /// The other information used in construction of <see cref="Dkm"/>
        /// </summary>
        public BitString Oi { get; }
        /// <summary>
        /// The negotiated key H(dkm, macData)
        /// </summary>
        public BitString Dkm { get; }
        /// <summary>
        /// The Key that is plugged into a MAC algorithm (taken from DKM) H(macKey, macData).
        /// </summary>
        public BitString MacKey { get; }
        /// <summary>
        /// The data portion of the mac function H(dkm, macData)
        /// </summary>
        public BitString MacData { get; }

        /// <summary>
        /// indicates success/failure of Kas operation
        /// </summary>
        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// The error message returned by the Kas operation
        /// </summary>
        public string ErrorMessage { get; }
    }
}
