using System;
using System.IO;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.RSA;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.RSA
{
	public class RSALegacySigVer_ResultsExtractor : RSA_ResultsExtractorBase, IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "RSA2_Summary.txt";
		private string _summaryFilePath;

		public RSALegacySigVer_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public RSALegacySigVer_Results Extract()
		{
			RSALegacySigVer_Results results = new RSALegacySigVer_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			results.SigVer931_186_2 = GetTestResults(lines, Array.IndexOf(lines, "[FIPS 186-2 Signature Verification RSA-X9.31 (FOR LEGACY USE)]"), AlgorithmTestNames.RSA_186_2_SigVer);
			results.SigVerPKCS15_186_2 = GetTestResults(lines, Array.IndexOf(lines, "[FIPS 186-2 Signature Verification PKCS#1 Ver 1.5 (FOR LEGACY USE)]"), AlgorithmTestNames.RSA_186_2_SigVer);
			results.SigVerPSS_186_2 = GetTestResults(lines, Array.IndexOf(lines, "[FIPS 186-2 Signature Verification PKCS#1 RSASSA-PSS (FOR LEGACY USE)]"), AlgorithmTestNames.RSA_186_2_SigVer);

			return results;
		}
	}
}