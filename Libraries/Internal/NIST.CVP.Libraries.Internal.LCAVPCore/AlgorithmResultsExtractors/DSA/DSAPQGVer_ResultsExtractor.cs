using System;
using System.IO;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResults.DSA;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmResultsExtractors.DSA
{
	public class DSAPQGVer_ResultsExtractor : DSA_ResultsExtractorBase, IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "DSA2_Summary.txt";
		private string _summaryFilePath;

		public DSAPQGVer_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public DSAPQGVer_Results Extract()
		{
			DSAPQGVer_Results results = new DSAPQGVer_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			results.PQGVer_ProbablePrime = ExtractModResultsUnderHeader(lines, Array.IndexOf(lines, "\tA.1.1.3 Validation of the Probable Primes p and q that were Generated Using an Approved Hash Function:"));
			results.PQGVer_ProvablePrime = ExtractModResultsUnderHeader(lines, Array.IndexOf(lines, "\tA.1.2.2 Validation of the DSA Primes p and q that were Constructed Using the Shawe-Taylor Algorithm:"));
			results.PQGVer_UnverifiableG = ExtractModResultsUnderHeader(lines, Array.IndexOf(lines, "\tA.2.2   Assurance of the Validity of the Generator g:"));
			results.PQGVer_CanonicalG = ExtractModResultsUnderHeader(lines, Array.IndexOf(lines, "\tA.2.4   Validation Routine when the Canonical Generation of the Generator g Routine Was Used:"));
			results.PQGVer_186_2 = ExtractModResultsUnderHeader(lines, Array.IndexOf(lines, "\tA.1.1.1 Validation of the Probable Primes p and q that were Generated Using SHA-1 as Specified in Prior Versions of this Standard:"));

			return results;
		}
	}
}