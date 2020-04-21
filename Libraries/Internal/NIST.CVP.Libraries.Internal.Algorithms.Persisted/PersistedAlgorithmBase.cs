using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public abstract class PersistedAlgorithmBase : IPersistedAlgorithm
	{
		public string Name { get; set; }
		public string Mode { get; set; }
		public string Revision { get; set; }
	}
}
