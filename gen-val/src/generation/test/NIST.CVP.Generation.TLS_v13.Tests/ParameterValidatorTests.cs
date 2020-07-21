using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Generation.TLSv13.RFC8446;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TLS_v13.Tests
{
	[TestFixture, UnitTest]
	public class ParameterValidatorTests
	{
		private readonly ParameterValidator _subject = new ParameterValidator();

		[Test]
		public void ShouldSucceedOnValidAlgorithm()
		{
			var p = new ParameterBuilder().Build();
			
			Assert.IsTrue(_subject.Validate(p).Success);
		}
		
		[Test]
		[TestCase("tlsv1.3", "", "1.0")]
		[TestCase("TLS-v1.0", "KDF", "1.0")]
		public void ShouldErrorOnInvalidAlgorithm(string algorithm, string mode, string revision)
		{
			var p = new ParameterBuilder()
				.WithAlgorithm(algorithm)
				.WithMode(mode)
				.WithRevision(revision)
				.Build();
			
			Assert.IsFalse(_subject.Validate(p).Success);
		}

		[Test]
		public void ShouldSucceedWIthAllHashAlgsNoDefault()
		{
			var p = new ParameterBuilder()
				.WithHashAlgs(EnumHelpers.GetEnumsWithoutDefault<HashFunctions>())
				.Build();
			
			Assert.IsTrue(_subject.Validate(p).Success);
		}
		
		[Test]
		public void ShouldFailWIthAllHashAlgsIncludingDefault()
		{
			var p = new ParameterBuilder()
				.WithHashAlgs(EnumHelpers.GetEnums<HashFunctions>())
				.Build();
			
			Assert.IsFalse(_subject.Validate(p).Success);
		}
		
		[Test]
		[TestCase(true, new[] {HashFunctions.Sha2_d512, HashFunctions.Sha3_d512})]
		[TestCase(false, new[] {HashFunctions.Sha1, HashFunctions.None})]
		[TestCase(false, new[] {HashFunctions.Sha1, HashFunctions.None, HashFunctions.None})]
		public void ShouldErrorOnInvalidHashAlgorithms(bool shouldPass, HashFunctions[] hashFunctions)
		{
			var p = new ParameterBuilder()
				.WithHashAlgs(hashFunctions)
				.Build();
			
			Assert.AreEqual(shouldPass, _subject.Validate(p).Success);
		}
	}
}