using System;
using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmResults.DSA;

namespace LCAVPCore.AlgorithmResultsExtractors.DSA
{
	public class DSASigVer_ResultsExtractor : DSA_ResultsExtractorBase, IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "DSA2_Summary.txt";
		private string _summaryFilePath;

		public DSASigVer_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public DSASigVer_Results Extract()
		{
			DSASigVer_Results results = new DSASigVer_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			results.Results = ExtractModResultsUnderHeader(lines, Array.IndexOf(lines, "[Signature Verification]"));

			return results;
		}
	}
}