using Moq;
using NIST.CVP.TaskQueueProcessor.Providers;
using NIST.CVP.TaskQueueProcessor.TaskModels;
using NUnit.Framework;

namespace NIST.CVP.TaskQueueProcessor.Tests
{
    [TestFixture]
    public class PoolTaskTests
    {
        private Mock<IPoolProvider> _poolProviderMock;
        
        [SetUp]
        public void SetUp()
        {
            _poolProviderMock = new Mock<IPoolProvider>();
        }

        [Test]
        public void ShouldCallPoolApiWhenSpawnIsCalled()
        {
            _poolProviderMock.Setup(s => s.SpawnPoolData()).Verifiable();
            var subject = new PoolTask(_poolProviderMock.Object);
            subject.Run();
            _poolProviderMock.Verify(s => s.SpawnPoolData(), Times.Once);
        }
    }
}