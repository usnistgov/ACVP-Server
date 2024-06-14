namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.ADRS;

/// <summary>
/// The seven address types as defined in section 4.2 of FIPS 205
/// </summary>
public static class AdrsTypes
{
    public static readonly byte[] WOTS_HASH = new byte[] { 0x00, 0x00, 0x00, 0x00};
    public static readonly byte[] WOTS_PK = new byte[] { 0x00, 0x00, 0x00, 0x01};
    public static readonly byte[] TREE = new byte[] { 0x00, 0x00, 0x00, 0x02};
    public static readonly byte[] FORS_TREE = new byte[] { 0x00, 0x00, 0x00, 0x03};
    public static readonly byte[] FORS_ROOTS = new byte[] { 0x00, 0x00, 0x00, 0x04};
    public static readonly byte[] WOTS_PRF = new byte[] { 0x00, 0x00, 0x00, 0x05};
    public static readonly byte[] FORS_PRF = new byte[] { 0x00, 0x00, 0x00, 0x06};
}
