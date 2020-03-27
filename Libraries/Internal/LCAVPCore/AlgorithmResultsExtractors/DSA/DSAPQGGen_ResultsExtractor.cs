using System;
using System.IO;
using System.Linq;
using LCAVPCore.AlgorithmResults.DSA;

namespace LCAVPCore.AlgorithmResultsExtractors.DSA
{
	public class DSAPQGGen_ResultsExtractor : DSA_ResultsExtractorBase, IAlgorithmResultsExtractor
	{
		private const string SUMMARY_FILE_NAME = "DSA2_Summary.txt";
		private string _summaryFilePath;

		public DSAPQGGen_ResultsExtractor(string submissionPath)
		{
			_summaryFilePath = Path.Combine(submissionPath, SUMMARY_FILE_NAME);
		}

		public DSAPQGGen_Results Extract()
		{
			DSAPQGGen_Results results = new DSAPQGGen_Results();

			//Verify that summary file exists
			if (!File.Exists(_summaryFilePath))
			{
				results.InvalidReasons.Add($"{SUMMARY_FILE_NAME} not found");
				return results;
			}

			//Read the summary file content
			string[] lines = File.ReadAllLines(_summaryFilePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			//The rest are subheaders under the parent header. Fortunately the subheaders are unique in the file, we can work with them just like regular headers
			results.PQGGen_ProbablePrime = ExtractModResultsUnderHeader(lines, Array.IndexOf(lines, "\tA.1.1.2 Generation of the Probable Primes p and q Using an Approved Hash Function:"));
			results.PQGGen_ProvablePrime = ExtractModResultsUnderHeader(lines, Array.IndexOf(lines, "\tA.1.2.1 Construction of the Primes p and q Using the Shawe-Taylor Algorithm:"));
			results.PQGGen_UnverifiableG = ExtractModResultsUnderHeader(lines, Array.IndexOf(lines, "\tA.2.1   Unverifiable Generation of the Generator g:"));
			results.PQGGen_CanonicalG = ExtractModResultsUnderHeader(lines, Array.IndexOf(lines, "\tA.2.3   Verifiable Canonical Generation of the Generator g:"));

			return results;
		}
	}
}