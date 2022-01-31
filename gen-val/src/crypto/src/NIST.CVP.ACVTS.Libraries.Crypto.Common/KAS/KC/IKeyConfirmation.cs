namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC
{
    /// <summary>
    /// Interface for describing Key Confirmation
    /// 
    /// U = key agreement initiator
    /// V = key agreement responder
    /// 
    /// http://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-56Ar2.pdf
    /// Section 5.9
    /// </summary>
    public interface IKeyConfirmation
    {
        /// <summary>
        /// Computes a MAC for KAS
        /// </summary>
        /// <returns></returns>
        ComputeKeyMacResult ComputeMac();
    }

}
