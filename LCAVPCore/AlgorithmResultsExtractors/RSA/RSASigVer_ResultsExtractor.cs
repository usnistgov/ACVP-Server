using System;
using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmResults.RSA;

namespace LCAVPCore.AlgorithmResultsExtractors.RSA
{
	public class RSASigVer_ResultsExtractor : RSA_ResultsExtractorBase, IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "RSA2_Summary.txt";
		private string _summaryFilePath;

		public RSASigVer_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public RSASigVer_Results Extract()
		{
			RSASigVer_Results results = new RSASigVer_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			results.SigVer931 = GetTestResults(lines, Array.IndexOf(lines, "[FIPS 186-4 Signature Verification RSA-X9.31]"), AlgorithmTestNames.RSA_186_4_SigGenSigVer);
			results.SigVerPKCS15 = GetTestResults(lines, Array.IndexOf(lines, "[FIPS 186-4 Signature Verification PKCS#1 Ver 1.5]"), AlgorithmTestNames.RSA_186_4_SigGenSigVer);
			results.SigVerPSS = GetTestResults(lines, Array.IndexOf(lines, "[FIPS 186-4 Signature Verification PKCS#1 RSASSA-PSS]"), AlgorithmTestNames.RSA_186_4_SigGenSigVer);

			return results;
		}

	}
}