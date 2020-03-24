using System;
using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmResults.RSA;

namespace LCAVPCore.AlgorithmResultsExtractors.RSA
{
	public class RSAKeyGen_ResultsExtractor : RSA_ResultsExtractorBase, IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "RSA2_Summary.txt";
		private string _summaryFilePath;

		public RSAKeyGen_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public RSAKeyGen_Results Extract()
		{
			RSAKeyGen_Results results = new RSAKeyGen_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			//Parse out the results
			results.Results = GetTestResults(lines, Array.IndexOf(lines, "[FIPS 186-4 RSA Key Generation]"), AlgorithmTestNames.RSA_KeyGen);

			return results;
		}
	}
}
