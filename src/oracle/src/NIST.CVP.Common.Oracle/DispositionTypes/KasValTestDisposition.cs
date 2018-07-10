namespace NIST.CVP.Common.Oracle.DispositionTypes
{
    public enum KasValTestDisposition
    {
        Success,
        SuccessLeadingZeroNibbleZ,
        SuccessLeadingZeroNibbleDkm,
        FailAssuranceIutStaticPrivateKey,
        FailAssuranceIutStaticPublicKey,
        FailAssuranceServerStaticPublicKey,
        FailAssuranceServerEphemeralPublicKey,
        FailChangedZ,
        FailChangedDkm,
        FailChangedOi,
        FailChangedMacData,
        FailChangedTag
    }
}
