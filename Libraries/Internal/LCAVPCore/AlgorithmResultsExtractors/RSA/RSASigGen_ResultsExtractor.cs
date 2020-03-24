using System;
using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmResults.RSA;

namespace LCAVPCore.AlgorithmResultsExtractors.RSA
{
	public class RSASigGen_ResultsExtractor : RSA_ResultsExtractorBase, IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "RSA2_Summary.txt";
		private string _summaryFilePath;

		public RSASigGen_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public RSASigGen_Results Extract()
		{
			RSASigGen_Results results = new RSASigGen_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			results.SigGen931 = GetTestResults(lines, Array.IndexOf(lines, "[FIPS 186-4 Signature Generation RSA-X9.31]"), AlgorithmTestNames.RSA_186_4_SigGenSigVer);
			results.SigGenPKCS15 = GetTestResults(lines, Array.IndexOf(lines, "[FIPS 186-4 Signature Generation PKCS#1 Ver 1.5]"), AlgorithmTestNames.RSA_186_4_SigGenSigVer);
			results.SigGenPSS = GetTestResults(lines, Array.IndexOf(lines, "[FIPS 186-4 Signature Generation PKCS#1 RSASSA-PSS]"), AlgorithmTestNames.RSA_186_4_SigGenSigVer);

			return results;
		}
	}
}