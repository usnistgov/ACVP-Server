using System;
using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmResults.ECDSA;

namespace LCAVPCore.AlgorithmResultsExtractors.ECDSA
{
	public class ECDSAPKV_ResultsExtractor : ECDSA_ResultsExtractorBase, IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "ECDSA2_Summary.txt";
		private string _summaryFilePath;

		public ECDSAPKV_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public ECDSAPKV_Results Extract()
		{
			ECDSAPKV_Results results = new ECDSAPKV_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			results.Results = ExtractLinesInSection(lines, Array.IndexOf(lines, "[PKV]"));

			return results;
		}
	}
}