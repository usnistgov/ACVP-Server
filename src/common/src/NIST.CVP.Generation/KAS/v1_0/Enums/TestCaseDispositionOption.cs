namespace NIST.CVP.Generation.KAS.v1_0.Enums
{
    public enum TestCaseDispositionOption
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