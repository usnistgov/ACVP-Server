using System;
using System.Threading;
using System.Threading.Tasks;
using MessageQueueProcessor.MessageProcessors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MessageQueueProcessor
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly IMessageProvider _messageProvider;
		private readonly IMessageProcessorFactory _messageProcessorFactory;
		private readonly int _sleepDuration;

		public Worker(ILogger<Worker> logger, IMessageProvider messageProvider, IMessageProcessorFactory messageProcessorFactory, IConfiguration configuration)
		{
			_logger = logger;
			_messageProvider = messageProvider;
			_messageProcessorFactory = messageProcessorFactory;
			_sleepDuration = configuration.GetValue<int>("MessageQueueProcessor:SleepDuration");
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				//Do work here
				_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

				//Get the next message from the queue 
				Message message = _messageProvider.GetNextMessage();

				//Loop while we have messages to process
				while (message != null)
				{
					//Get the processor for this message - might not have one
					IMessageProcessor messageProcessor = _messageProcessorFactory.GetMessageProcessor(message.Action);

					//Process message or flag it for Java processing
					if (messageProcessor == null)
					{
						_logger.LogInformation($"Passing {message.Action.ToString()} message {message.ID} to the Java processor");

						//Cannot process the message, so flag it for the Java processor to pick up
						_messageProvider.MarkForJavaProcessor(message.ID);
					}
					else
					{
						//Process the message
						messageProcessor.Process(message);

						//Done with the the message, so delete it
						_messageProvider.DeleteMessage(message.ID);

						//Log it
						_logger.LogInformation($"Processed {message.Action.ToString()} message {message.ID}");
					}

					//Get the next message
					message = _messageProvider.GetNextMessage();
				}

				//Go to sleep for a while before checking for new messages
				await Task.Delay(_sleepDuration, stoppingToken);
			}
		}
	}
}
