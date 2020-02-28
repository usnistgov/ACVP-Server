using System.Collections.Generic;
using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmResults.AES;

namespace LCAVPCore.AlgorithmResultsExtractors.AES
{
	public class AES_XTS_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "XTS_Summary.txt";
		private string _summaryFilePath;

		public AES_XTS_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public AES_XTS_Results Extract()
		{
			AES_XTS_Results results = new AES_XTS_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			//Break out the XTS sections
			int i = 0;

			//Skip everything until the AESKeySize = 128 header
			while (i < lines.Length && !lines[i].StartsWith("[AESKeySize = 128]"))
			{
				i++;
			}

			//Skip the header line and the useless blocksize line
			i += 2;

			//Next 2 lines are encrypt/decrypt
			results.Encrypt128Passed = ParsingHelper.GetValueStringFromSummaryLine(lines[i]) == "Passed";
			results.Decrypt128Passed = ParsingHelper.GetValueStringFromSummaryLine(lines[i + 1]) == "Passed";

			//Skip everything until the AESKeySize = 256 header
			while (i < lines.Length && !lines[i].StartsWith("[AESKeySize = 256]"))
			{
				i++;
			}

			//Skip the header line and the useless blocksize line
			i += 2;

			//Next 2 lines are encrypt/decrypt
			results.Encrypt256Passed = ParsingHelper.GetValueStringFromSummaryLine(lines[i]) == "Passed";
			results.Decrypt256Passed = ParsingHelper.GetValueStringFromSummaryLine(lines[i + 1]) == "Passed";

			return results;
		}
	}
}