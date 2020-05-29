using System.Collections.Generic;
using System.IO;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.AES;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.AES
{
	public class AES_XPN_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "GCM_Summary.txt";
		private string _summaryFilePath;

		public AES_XPN_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public AES_XPN_Results Extract()
		{
			AES_XPN_Results results = new AES_XPN_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			//Break out the XPN sections
			int i = 0;

			//Skip everything that isn't a XPN mode header
			while (i < lines.Length && !lines[i].StartsWith("[XPN]"))
			{
				i++;
			}

			//Make sure we're not at the end, and that there may be enough lines left
			if (i == lines.Length || i > lines.Length - 6)
			{
				results.InvalidReasons.Add("Improper XPN results");
				return results;
			}


			i++;	//Skip over the XPN line


			//Verify that the encrypt and decrypt lines are where they should be
			if (lines[i] != "[Encrypt]" || lines[i+3] != "[Decrypt]")
			{
				results.InvalidReasons.Add("Improper XPN results");
				return results;
			}

			//Get the 2 encrypt lines
			IEnumerable<string> relevantLines = lines.Skip(i + 1).Take(2);
			results.Encrypt128Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "[AES-128]") == "Passed";
			results.Encrypt256Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "[AES-256]") == "Passed";

			//Get the 2 decrypt lines
			relevantLines = lines.Skip(i + 4).Take(2);
			results.Decrypt128Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "[AES-128]") == "Passed";
			results.Decrypt256Passed = ParsingHelper.GetValueStringFromSummaryLinesByKey(relevantLines, "[AES-256]") == "Passed";

			return results;
		}
	}
}