using System;
using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmResults.DSA;

namespace LCAVPCore.AlgorithmResultsExtractors.DSA
{
	public class DSAKeyPair_ResultsExtractor : DSA_ResultsExtractorBase, IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "DSA2_Summary.txt";
		private string _summaryFilePath;

		public DSAKeyPair_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public DSAKeyPair_Results Extract()
		{
			DSAKeyPair_Results results = new DSAKeyPair_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			//These sections fall under traditional section headers
			results.Results = ExtractModResultsUnderHeader(lines, Array.IndexOf(lines, "[Key Pair Generation]"));

			return results;
		}
	}
}