using System;
using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmResults.ECDSA;

namespace LCAVPCore.AlgorithmResultsExtractors.ECDSA
{
	public class ECDSAKeyPair_ResultsExtractor : ECDSA_ResultsExtractorBase, IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "ECDSA2_Summary.txt";
		private string _summaryFilePath;

		public ECDSAKeyPair_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public ECDSAKeyPair_Results Extract()
		{
			ECDSAKeyPair_Results results = new ECDSAKeyPair_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			results.Results = ExtractLinesInSection(lines, Array.IndexOf(lines, "[Key Pair Generation]"));

			return results;
		}
	}
}