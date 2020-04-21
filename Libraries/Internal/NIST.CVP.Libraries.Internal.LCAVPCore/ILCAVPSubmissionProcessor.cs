namespace NIST.CVP.Libraries.Internal.LCAVPCore
{
	public interface ILCAVPSubmissionProcessor
	{
		SubmissionProcessingResult Process(string filePath);
	}
}