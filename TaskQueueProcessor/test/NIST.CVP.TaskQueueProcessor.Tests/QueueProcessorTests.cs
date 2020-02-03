using Microsoft.Extensions.Options;
using Moq;
using NIST.CVP.Common.Config;
using NIST.CVP.Generation.Core;
using NIST.CVP.TaskQueueProcessor.Providers;
using NIST.CVP.TaskQueueProcessor.TaskModels;
using NUnit.Framework;

namespace NIST.CVP.TaskQueueProcessor.Tests
{
    [TestFixture]
    public class QueueProcessorTests
    {
        private Mock<IDbProvider> _dbProvider = new Mock<IDbProvider>();
        private Mock<IPoolProvider> _poolProvider = new Mock<IPoolProvider>();
        private Mock<ITaskRunner> _taskRunner = new Mock<ITaskRunner>();

        private IOptions<PoolConfig> _poolOptions;
        private IOptions<TaskQueueProcessorConfig> _taskOptions;
        
        private Mock<IGenValInvoker> _genValInvoker = new Mock<IGenValInvoker>();
        
        private QueueProcessor _queueProcessor;
        
        [SetUp]
        public void SetUp()
        {
            _poolOptions = new OptionsWrapper<PoolConfig>(new PoolConfig { AllowPoolSpawn = false });
            _taskOptions = new OptionsWrapper<TaskQueueProcessorConfig>(new TaskQueueProcessorConfig { MaxConcurrency = 2, PollDelay = 5 });

            _dbProvider.SetupSequence(s => s.GetNextTask())
                .Returns(new GenerationTask(_genValInvoker.Object, _dbProvider.Object){Capabilities = "capabilities1", DbId = 1, VsId = 2})
                .Returns(new GenerationTask(_genValInvoker.Object, _dbProvider.Object){Capabilities = "capabilities2", DbId = 3, VsId = 4});
            
            _queueProcessor = new QueueProcessor(_taskRunner.Object, _dbProvider.Object, _poolProvider.Object, _poolOptions, _taskOptions);
        }

        [Test]
        public void ShouldPollForNewTasks()
        {
            _queueProcessor.PollForTask();
        }
    }
}