using LCAVPCore.AlgorithmChunkParsers;

namespace LCAVPCore
{
	public class AlgorithmChunkParserFactory : IAlgorithmChunkParserFactory
	{
		public IAlgorithmChunkParser GetParser(InfFileSection chunk)
		{
			//Some of these are no longer validated, so should return null. The rest need to return specific parsers. Use the section name to determine which chunk we're working with.
			switch (chunk.SectionName)
			{
				case "AES": return new AESChunkParser(chunk);
				case "CCM": return new CCMChunkParser(chunk);
				case "CMAC": return new CMACChunkParser(chunk);
				case "DES": return null;														//Never need to process DES
				case "DRBG 800-90A": return new DRBGChunkParser(chunk);
				case "DSA": return null;														//Never need to process DSA
				case "DSA2": return new DSA2ChunkParser(chunk);
				case "ECDSA": return null;														//Should never be called, as should be merged into ECDSAMerged
				case "ECDSA2": return new ECDSA2ChunkParser(chunk);
				//case "ECDSAMerged": return new ECDSAMergedChunkParser(chunk);					//Need to merge ECDSA and ECDSA2, as 186-2 things in ECDSA2 tab show up in ECDSA section
				case "ECDSA2SigGenComponent": return new ECDSA2SigGenComponentChunkParser(chunk);
				case "GCM": return new GCMChunkParser(chunk);
				case "HMAC": return new HMACChunkParser(chunk);
				case "KASECC": return new KASECCChunkParser(chunk);
				case "KASFFC": return new KASFFCChunkParser(chunk);
				case "KDF800_108": return new KDF_800_108ChunkParser(chunk);
				case "KDF_800_135": return new KDF_800_135ChunkParser(chunk);
				case "Key_Wrap_38F": return new KeyWrapChunkParser(chunk);
				case "RNG": return null;														//Never need to process RNG
				case "RSA": return null;
				case "RSA2": return new RSAChunkParser(chunk);
				case "RSAComponent_RSADP": return new RSAComponent_RSADPChunkParser(chunk);
				case "RSAComponent_RSASP1": return new RSAComponent_RSASP1ChunkParser(chunk);
				case "SHA": return new SHAChunkParser(chunk);
				case "SHA3": return new SHA3ChunkParser(chunk);
				case "TDES": return new TDESChunkParser(chunk);
				case "XTS": return new XTSChunkParser(chunk);
				default: return null;															//Would be pretty bizarre for there to be anything else...
			}
		}
	}
}