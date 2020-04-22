using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.AES;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.AES
{
	public class AES_GCM_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "GCM_Summary.txt";
		private string _summaryFilePath;

		public AES_GCM_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public AES_GCM_Results Extract()
		{
			AES_GCM_Results results = new AES_GCM_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			//Break out the GCM sections
			int i = 0;

			//Skip everything that isn't a GCM mode header
			while (i < lines.Length && !lines[i].StartsWith("[GCM/GMAC]"))
			{
				i++;
			}

			//Make sure we're not at the end, and that there may be enough lines left
			if (i == lines.Length || i > lines.Length - 8)
			{
				results.InvalidReasons.Add("Improper GCM results");
				return results;
			}


			i++;	//Skip over the GCM line


			//Verify that the encrypt and decrypt lines are where they should be
			if (lines[i] != "[Encrypt]" || lines[i+4] != "[Decrypt]")
			{
				results.InvalidReasons.Add("Improper GCM results");
				return results;
			}

			//Get the 3 encrypt lines
			IEnumerable<string> relevantLines = lines.Skip(i + 1).Take(3);
			results.Encrypt128Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "[AES-128]") == "Passed";
			results.Encrypt192Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "[AES-192]") == "Passed";
			results.Encrypt256Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "[AES-256]") == "Passed";

			//Get the 3 decrypt lines
			relevantLines = lines.Skip(i + 5).Take(3);
			results.Decrypt128Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "[AES-128]") == "Passed";
			results.Decrypt192Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "[AES-192]") == "Passed";
			results.Decrypt256Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "[AES-256]") == "Passed";

			return results;
		}
	}
}