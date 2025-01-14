namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.ADRS;

/// <summary>
/// Provides an ADRS implementation (see FIPS 205 Section 4.2). There are 7 types.
/// </summary>
public interface IAdrs
{
    byte[] LayerAddress { get; set; }
    byte[] TreeAddress { get; set; }
    byte[] AdrsType { get; set; }

    byte[] GetAdrs();
    byte[] GetCompressedAdrs();
}
