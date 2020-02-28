using System;
using System.IO;
using LCAVPCore.AlgorithmResults.HMAC;

namespace LCAVPCore.AlgorithmResultsExtractors.HMAC
{
	public class HMAC_SHA_1_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "HMAC_Summary.txt";
		private string _summaryFilePath;

		public HMAC_SHA_1_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public HMAC_Results Extract()
		{
			HMAC_Results results = new HMAC_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Really only care about 1 line
			results.Passed = Array.Exists(File.ReadAllLines(_summaryFilePath), l => l.Contains("[HMAC w/ SHA1  ]\t\tPassed"));

			return results;
		}
	}
}
