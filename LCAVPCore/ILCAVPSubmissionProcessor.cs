namespace LCAVPCore
{
	public interface ILCAVPSubmissionProcessor
	{
		SubmissionProcessingResult Process(string filePath, string senderEmail, string processedFileName, string extractedFilesRoot);
	}
}