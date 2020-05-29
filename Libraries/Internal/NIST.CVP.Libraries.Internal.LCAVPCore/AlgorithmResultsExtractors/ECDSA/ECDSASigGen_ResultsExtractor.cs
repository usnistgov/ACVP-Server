using System;
using System.IO;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.ECDSA;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.ECDSA
{
	public class ECDSASigGen_ResultsExtractor : ECDSA_ResultsExtractorBase, IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "ECDSA2_Summary.txt";
		private string _summaryFilePath;

		public ECDSASigGen_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public ECDSASigGen_Results Extract()
		{
			ECDSASigGen_Results results = new ECDSASigGen_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			results.Results = ExtractLinesInSection(lines, Array.IndexOf(lines, "[Signature Generation]"));

			return results;
		}
	}
}