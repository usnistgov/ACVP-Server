namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models
{
	public class PropertyLookup
	{
		public long AlgorithmID { get; set; }
		public long PropertyID { get; set; }
		public string Name { get; set; }
		public int? OrderIndex { get; set; }
	}
}
