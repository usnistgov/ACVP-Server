using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DataMaintainer
{
	public class Worker : IHostedService
	{
		private readonly ILogger<Worker> _logger;
		private readonly IVectorSetService _vectorSetService;
		private readonly ITestSessionService _testSessionService;
		private readonly IHostApplicationLifetime _hostApplicationLifetime;
		private readonly string _destinationFolder;
		private readonly int _ageInDays;
		private readonly bool _createArchiveFile;
		private readonly bool _expirationEnabled;

		public Worker(ILogger<Worker> logger, IVectorSetService vectorSetService, ITestSessionService testSessionService, IHostApplicationLifetime hostApplicationLifetime, IConfiguration configuration)
		{
			_logger = logger;
			_vectorSetService = vectorSetService;
			_testSessionService = testSessionService;
			_hostApplicationLifetime = hostApplicationLifetime;
			_destinationFolder = configuration.GetValue<string>("DataMaintainer:DestinationFolder");
			_ageInDays = configuration.GetValue<int>("DataMaintainer:AgeInDays");
			_createArchiveFile = configuration.GetValue<bool>("DataMaintainer:CreateArchiveFile");
			_expirationEnabled = configuration.GetValue<bool>("DataMaintainer:ExpirationEnabled");
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Starting");
			Maintain();
			_hostApplicationLifetime.StopApplication();
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Stopping");
			return Task.CompletedTask;
		}

		public void Maintain()
		{
			//Expire test sessions older than the configured age
			//TODO - Do a more complex version of expiration, based on vector set activity and keep-alives, in conjunction with public rewrite
			if (_expirationEnabled)
			{
				_testSessionService.Expire(_ageInDays);
			}

			//If want to produce archive files, make sure the destination can be reached or exit
			if (_createArchiveFile && !Directory.Exists(_destinationFolder))
			{
				_logger.LogError($"Destination folder {_destinationFolder} could not be accessed");
				return;
			}

			//Get the vector sets to be archived
			List<long> vectorSetIDs = _vectorSetService.GetVectorSetsToArchive();

			//Loop through and archive them
			foreach (long vectorSetID in vectorSetIDs)
			{
				//Get all the VectorSetJson data for this vector set
				List<VectorSetJsonEntry> archiveData = new List<VectorSetJsonEntry>();

				var vectorSetJsonData = _vectorSetService.GetVectorFileJson(vectorSetID);

				foreach (var (FileType, Content, CreatedOn) in vectorSetJsonData)
				{
					archiveData.Add(new VectorSetJsonEntry
					{
						VectorSetID = vectorSetID,
						FileType = FileType,
						Content = JsonSerializer.Deserialize<JsonElement>(Content),
						CreatedOn = CreatedOn
					});
				}

				//Write the data to the archive file
				if (archiveData.Count > 0 && _createArchiveFile)
				{
					WriteArchiveFile(vectorSetID, archiveData);
				}

				//Clean up the database
				_vectorSetService.Archive(vectorSetID);

				_logger.LogInformation($"Archived vector {vectorSetID}");
			}

			_logger.LogInformation("Done");
		}

		private void WriteArchiveFile(long vectorSetID, List<VectorSetJsonEntry> vectorSetData)
		{
			//Create a zip file containing the serialized data related to the vector set
			using FileStream archiveFile = new FileStream(Path.Combine(_destinationFolder, $"{vectorSetID}.zip"), FileMode.Create);
			using ZipArchive archive = new ZipArchive(archiveFile, ZipArchiveMode.Create);
			ZipArchiveEntry entry = archive.CreateEntry($"{vectorSetID}.json");

			using StreamWriter writer = new StreamWriter(entry.Open());
			writer.Write(JsonSerializer.Serialize(vectorSetData));
		}
	}
}
