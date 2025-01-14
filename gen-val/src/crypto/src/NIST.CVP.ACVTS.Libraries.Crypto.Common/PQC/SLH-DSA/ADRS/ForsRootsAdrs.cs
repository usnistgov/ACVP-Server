using System;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.ADRS;

public class ForsRootsAdrs: AdrsBase, IAdrs
{
    /// <summary>
    /// The FORS_ROOTS address type defined in section 4.2 of FIPS 205 
    /// </summary>
    public byte[] KeyPairAddress { get; set; } = { 0x00, 0x00, 0x00, 0x00};
    private readonly byte[] _padding  = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
    
    public ForsRootsAdrs()
    {
        AdrsType = AdrsTypes.FORS_ROOTS;
    }
    
    public ForsRootsAdrs(byte[] treeAddress)
    {
        if (treeAddress.Length != TreeAddress.Length)
            throw new ArgumentException($"The length of {nameof(treeAddress)} must be {TreeAddress.Length}, but is {treeAddress.Length}.", nameof(treeAddress));
        
        TreeAddress = treeAddress;
        AdrsType = AdrsTypes.FORS_ROOTS;
    }
    
    public byte[] GetAdrs()
    {
        var address = new byte[32];
        Array.Copy(LayerAddress, 0, address, 0, 4);
        Array.Copy(TreeAddress, 0, address, 4, 12);
        Array.Copy(AdrsType, 0, address, 16, 4);
        Array.Copy(KeyPairAddress, 0, address, 20, 4);
        Array.Copy(_padding, 0, address, 24, 8);

        return address;
    }
    
    public byte[] GetCompressedAdrs()
    {
        var compressedAddress = new byte[22];
        Array.Copy(LayerAddress, 3, compressedAddress, 0, 1);
        Array.Copy(TreeAddress, 4, compressedAddress, 1, 8);
        Array.Copy(AdrsType, 3, compressedAddress, 9, 1);
        Array.Copy(KeyPairAddress, 0, compressedAddress, 10, 4);
        Array.Copy(_padding, 0, compressedAddress, 14, 8);

        return compressedAddress;
    }
}
