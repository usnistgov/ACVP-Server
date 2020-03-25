using System.Collections.Generic;

namespace LCAVPCore
{
	public class InfFile
	{
		public string Folder { get; set; }
		public string FolderNumber
		{
			get => Folder.Substring(Folder.LastIndexOf('\\') + 1);
		}
		public InfFileSection VendorAndImplementationSection { get; set; }

		public List<InfFileSection> AlgorithmChunks { get; set; }

		public List<InfAlgorithm> Algorithms { get; set; } = new List<InfAlgorithm>();

		public bool Valid { get; set; }

		//public InfFile(string filePath)
		//{
		//	FilePath = filePath;
		//	AlgorithmChunks = new List<InfFileSection>();
		//	Algorithms = new List<InfAlgorithm>();
		//	Valid = Parse();
		//}


	}
}