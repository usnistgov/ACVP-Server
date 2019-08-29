using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.KDF
{
    /// <summary>
    /// Note this class is not exactly obsolete, but should be removed once the 1.0 testing revision of KAS is retired.
    ///
    /// This version of OtherInfo does not conform to the specification (though it does conform to how CAVS did the testing).
    /// OtherInfo is now known as FixedInfo, and should be constructed similarly,
    /// though needs either Concatenation wrapping or asn.1 wrapping, which this implementation does not utilize.
    ///
    /// May be able to remove this immediately upon making updates to the original KAS, but leaving this note here as a reminder.
    /// </summary>
    public interface IOtherInfo
    {
        BitString GetOtherInfo();
    }
}