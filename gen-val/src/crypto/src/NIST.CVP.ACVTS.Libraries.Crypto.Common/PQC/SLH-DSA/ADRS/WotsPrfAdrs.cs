using System;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.ADRS;

public class WotsPrfAdrs: AdrsBase, IAdrs
{
    /// <summary>
    /// The WOTS_PRF address type defined in section 4.2 of FIPS 205 
    /// </summary>
    public byte[] KeyPairAddress { get; set; } = { 0x00, 0x00, 0x00, 0x00};
    public byte[] ChainAddress { get; set; } = { 0x00, 0x00, 0x00, 0x00};
    public byte[] HashAddress { get; set; } = { 0x00, 0x00, 0x00, 0x00};
    
    public WotsPrfAdrs()
    {
        AdrsType = AdrsTypes.WOTS_PRF;
    }
    
    public WotsPrfAdrs(byte[] layerAddress, byte[] treeAddress)
    {
        if (layerAddress.Length != LayerAddress.Length)
            throw new ArgumentException($"The length of {nameof(layerAddress)} must be {LayerAddress.Length}, but is {layerAddress.Length}.", nameof(layerAddress));
        if (treeAddress.Length != TreeAddress.Length)
            throw new ArgumentException($"The length of {nameof(treeAddress)} must be {TreeAddress.Length}, but is {treeAddress.Length}.", nameof(treeAddress));
        
        LayerAddress = layerAddress;
        TreeAddress = treeAddress;
        AdrsType = AdrsTypes.WOTS_PRF;
    }
    
    public byte[] GetAdrs()
    {
        var address = new byte[32];
        Array.Copy(LayerAddress, 0, address, 0, 4);
        Array.Copy(TreeAddress, 0, address, 4, 12);
        Array.Copy(AdrsType, 0, address, 16, 4);
        Array.Copy(KeyPairAddress, 0, address, 20, 4);
        Array.Copy(ChainAddress, 0, address, 24, 4);
        Array.Copy(HashAddress, 0, address, 28, 4);

        return address;
    }
    
    public byte[] GetCompressedAdrs()
    {
        var compressedAddress = new byte[22];
        Array.Copy(LayerAddress, 3, compressedAddress, 0, 1);
        Array.Copy(TreeAddress, 4, compressedAddress, 1, 8);
        Array.Copy(AdrsType, 3, compressedAddress, 9, 1);
        Array.Copy(KeyPairAddress, 0, compressedAddress, 10, 4);
        Array.Copy(ChainAddress, 0, compressedAddress, 14, 4);
        Array.Copy(HashAddress, 0, compressedAddress, 18, 4);

        return compressedAddress;
    }    
}
