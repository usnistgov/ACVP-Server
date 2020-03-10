namespace LCAVPCore.AlgorithmResults
{
	public class PassFailResult
	{
		public string TestName { get; set; }
		public bool Pass { get; set; }

		public PassFailResult(string testName)
		{
			TestName = testName;
		}

		public PassFailResult(string testName, bool pass)
		{
			TestName = testName;
			Pass = pass;
		}
	}
}