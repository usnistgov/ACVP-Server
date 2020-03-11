namespace LCAVPCore
{
	public interface ILCAVPSubmissionProcessor
	{
		SubmissionProcessingResult Process(string filePath, string senderEmail);
	}
}