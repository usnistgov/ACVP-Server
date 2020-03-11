using System.Collections.Generic;

namespace LCAVPCore
{
	public class FileExtractionResult
	{
		public bool Success
		{
			get { return Errors.Count == 0; }
		}

		public List<string> Errors { get; set; } = new List<string>();

		public string ExtractionPath { get; set; }
	}
}
