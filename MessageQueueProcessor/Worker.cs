using System;
using System.Threading;
using System.Threading.Tasks;
using MessageQueueProcessor.MessageProcessors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NIST.CVP.MessageQueue;
using NIST.CVP.MessageQueue.Providers;

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
			_logger.LogInformation("Starting Message Queue Processor");

			while (!stoppingToken.IsCancellationRequested)
			{
				_logger.LogDebug($"Worker running at: {DateTimeOffset.Now}");

				//Get the next message from the queue 
				Message message = _messageProvider.GetNextMessage();

				//Loop while we have messages to process and we haven't tried to stop the service
				while (message != null && !stoppingToken.IsCancellationRequested)
				{
					//Update the message status to show we're working on it
					_messageProvider.UpdateStatus(message.ID, MessageStatus.Processing);

					//Get the processor for this message - might not have one
					IMessageProcessor messageProcessor = _messageProcessorFactory.GetMessageProcessor(message.Action);

					//Process message or error
					if (messageProcessor == null)
					{
						//Mark it as an error
						_messageProvider.UpdateStatus(message.ID, MessageStatus.Error);

						//Log it
						_logger.LogError($"Unable to find processor for message {message.ID}");
					}
					else
					{
						//Process the message
						var messageProcessingResult = messageProcessor.Process(message);

						if (messageProcessingResult.IsSuccess)
						{
							//Done with the the message, so delete it
							_messageProvider.DeleteMessage(message.ID);

							//Log it
							_logger.LogInformation($"Processed {message.Action.ToString()} message {message.ID}");
						}
						else
						{
							//Mark it as an error
							_messageProvider.UpdateStatus(message.ID, MessageStatus.Error);

							//Log it
							_logger.LogError($"Error processing {message.Action.ToString()} message {message.ID} : {messageProcessingResult.ErrorMessage}");
						}

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
