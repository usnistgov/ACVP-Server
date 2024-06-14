namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.ADRS;

/// <summary>
/// Base ADRS/address class... because all of the ADRS types have some properties in common.  
/// </summary>
public abstract class AdrsBase
{
    /// <summary>
    /// Addresses are defined in section 4.2 of FIPS 205. Every address type has the following 3 fields in common.  
    /// </summary>
    public byte[] LayerAddress { get; set; } = { 0x00, 0x00, 0x00, 0x00}; 
    public byte[] TreeAddress { get; set; } = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
    public byte[] AdrsType { get; set; }
    
    // public abstract byte[] GetAdrs();
    //
    // public abstract byte[] GetCompressedAdrs();
}
