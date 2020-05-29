namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models
{
	public class DependencyAttribute
	{
		public long ID { get; set; }
		public long DependencyID { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
	}
}
