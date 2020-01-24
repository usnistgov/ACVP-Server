namespace ACVPCore.Models.Capabilities
{
	public interface IDomainSegment
	{
		public long NumericSortValue { get; }
		public string AsDomainSegmentString();
	}
}
