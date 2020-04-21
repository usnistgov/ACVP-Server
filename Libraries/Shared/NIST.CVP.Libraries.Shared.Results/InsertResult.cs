namespace NIST.CVP.Libraries.Shared.Results
{
	public class InsertResult : Result
	{
		public long ID { get; set; }

		public InsertResult(long id)
		{
			ID = id;
		}

		public InsertResult(string errorMessage) : base(errorMessage) { }
	}
}
