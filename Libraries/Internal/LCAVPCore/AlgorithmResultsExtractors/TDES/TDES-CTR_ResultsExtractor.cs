using System;
using System.IO;
using LCAVPCore.AlgorithmResults.TDES;

namespace LCAVPCore.AlgorithmResultsExtractors.TDES
{
	public class TDES_CTR_ResultsExtractor : IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "TDES_Summary.txt";
		private string _summaryFilePath;

		public TDES_CTR_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public TDES_CTR_Results Extract()
		{
			TDES_CTR_Results results = new TDES_CTR_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Really only care about 1 line, and it may not even be there - in the unlikely even that they said the mode was true but the state was not Encrypt (CTR is encrypt only)
			results.ForwardCipherFunctionTestPassed = Array.Exists(File.ReadAllLines(_summaryFilePath), l => l.Contains("\tAt least one Forward Cipher Function Test\t\t\t\tPASSED"));

			return results;
		}
	}
}
