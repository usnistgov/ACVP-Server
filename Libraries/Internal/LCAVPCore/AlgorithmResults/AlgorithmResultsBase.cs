using System.Collections.Generic;

namespace LCAVPCore.AlgorithmResults
{
	public class AlgorithmResultsBase
	{
		public bool Valid
		{
			get
			{
				return InvalidReasons.Count == 0;
			}
		}

		public List<string> InvalidReasons { get; set; }

		public AlgorithmResultsBase()
		{
			InvalidReasons = new List<string>();
		}
	}
}