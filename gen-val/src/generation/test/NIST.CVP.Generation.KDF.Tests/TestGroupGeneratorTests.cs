using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Generation.KDF.v1_0;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KDF.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        [Test]
        public async Task ShouldOnlyHaveEmptyIvWhenFeedbackAndEmptyIvRequired()
        {
            var capabilities = new CapabilityBuilder()
                    .WithKdfMode(KdfModes.Feedback)
                    .WithRequiresEmptyIv(true)
                    .Build();
            
            var param = new ParameterBuilder().WithCapabilities(new [] {capabilities}).Build();

            var subject = new TestGroupGenerator();
            var result = await subject.BuildTestGroupsAsync(param);

            foreach (var group in result)
            {
                Assert.IsTrue(group.ZeroLengthIv);
            }
        }

        [Test]
        public async Task ShouldIgnoreIvWhenNotFeedback()
        {
            var capabilities = new CapabilityBuilder()
                .WithKdfMode(KdfModes.Counter)
                .WithRequiresEmptyIv(true)
                .Build();

            var capabilities2 = new CapabilityBuilder()
                .WithKdfMode(KdfModes.Pipeline)
                .WithRequiresEmptyIv(true)
                .Build();
            
            var param = new ParameterBuilder().WithCapabilities(new [] {capabilities, capabilities2}).Build();

            var subject = new TestGroupGenerator();
            var result = await subject.BuildTestGroupsAsync(param);

            foreach (var group in result)
            {
                Assert.IsFalse(group.ZeroLengthIv);
            }
        }

        [Test]
        public async Task ShouldHaveBothEmptyAndFullIvWhenFeedbackAndEmptyIvNotRequiredButSupported()
        {
            var capabilities = new CapabilityBuilder()
                .WithKdfMode(KdfModes.Feedback)
                .WithSupportsEmptyIv(true)
                .Build();

            var capabilities2 = new CapabilityBuilder()
                .WithKdfMode(KdfModes.Pipeline)
                .WithRequiresEmptyIv(true)
                .Build();
            
            var param = new ParameterBuilder().WithCapabilities(new [] {capabilities, capabilities2}).Build();

            var subject = new TestGroupGenerator();
            var result = await subject.BuildTestGroupsAsync(param);

            var emptyIvPresent = false;
            var fullIvPresent = false;
            foreach (var group in result)
            {
                if (group.KdfMode == KdfModes.Feedback)
                {
                    if (group.ZeroLengthIv)
                    {
                        emptyIvPresent = true;
                    }

                    if (!group.ZeroLengthIv)
                    {
                        fullIvPresent = true;
                    }
                }
            }
            
            Assert.IsTrue(emptyIvPresent && fullIvPresent);
        }
    }
}