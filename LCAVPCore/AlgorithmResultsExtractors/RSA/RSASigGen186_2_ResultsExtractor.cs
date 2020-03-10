using System;
using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmResults.RSA;

namespace LCAVPCore.AlgorithmResultsExtractors.RSA
{
	public class RSASigGen186_2_ResultsExtractor : RSA_ResultsExtractorBase, IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "RSA2_Summary.txt";
		private string _summaryFilePath;

		public RSASigGen186_2_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public RSASigGen186_2_Results Extract()
		{
			RSASigGen186_2_Results results = new RSASigGen186_2_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			//186-2 SigGen is actually a subset of the 186-4 SigGen sections, so just use those, then filter to the Mod 4096 lines
			results.SigGen931_186_2 = GetTestResults(lines, Array.IndexOf(lines, "[FIPS 186-4 Signature Generation RSA-X9.31]"), AlgorithmTestNames.RSA_186_4_SigGenSigVer).Where(x => x.TestName.StartsWith("Mod 4096")).ToList();
			results.SigGenPKCS15_186_2 = GetTestResults(lines, Array.IndexOf(lines, "[FIPS 186-4 Signature Generation PKCS#1 Ver 1.5]"), AlgorithmTestNames.RSA_186_4_SigGenSigVer).Where(x => x.TestName.StartsWith("Mod 4096")).ToList();
			results.SigGenPSS_186_2 = GetTestResults(lines, Array.IndexOf(lines, "[FIPS 186-4 Signature Generation PKCS#1 RSASSA-PSS]"), AlgorithmTestNames.RSA_186_4_SigGenSigVer).Where(x => x.TestName.StartsWith("Mod 4096")).ToList();

			return results;
		}
	}
}