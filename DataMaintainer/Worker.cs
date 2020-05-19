using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DataMaintainer
{
	public class Worker : IHostedService
	{
		private readonly ILogger<Worker> _logger;
		private readonly IVectorSetService _vectorSetService;
		private readonly ITestSessionService _testSessionService;
		private readonly IPersonService _personService;
		private readonly IHostApplicationLifetime _hostApplicationLifetime;
		private readonly IMailer _mailer;
		private readonly string _destinationFolder;
		private readonly int _ageInDays;
		private readonly int _daysBeforeExpirationToWarn;
		private readonly bool _createArchiveFile;
		private readonly bool _expirationEnabled;

		public Worker(ILogger<Worker> logger, IVectorSetService vectorSetService, ITestSessionService testSessionService, IPersonService personService, IHostApplicationLifetime hostApplicationLifetime, IConfiguration configuration, IMailer mailer)
		{
			_logger = logger;
			_vectorSetService = vectorSetService;
			_testSessionService = testSessionService;
			_personService = personService;
			_hostApplicationLifetime = hostApplicationLifetime;
			_mailer = mailer;
			_destinationFolder = configuration.GetValue<string>("DataMaintainer:DestinationFolder");
			_ageInDays = configuration.GetValue<int>("DataMaintainer:AgeInDays");
			_daysBeforeExpirationToWarn = configuration.GetValue<int>("DataMaintainer:DaysBeforeExpirationToWarn");
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
			//Expire test sessions that haven't been touched in the configured number of days
			if (_expirationEnabled)
			{
				//Send warning emails
				SendWarningEmails();

				//Do expiration
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
				//If configured to write archive files, do so
				if (_createArchiveFile)
				{
					CreateArchiveFile(vectorSetID);
				}

				//Clean up the database
				_vectorSetService.Archive(vectorSetID);

				_logger.LogInformation($"Archived vector {vectorSetID}");
			}

			_logger.LogInformation("Done");
		}

		private void CreateArchiveFile(long vectorSetID)
		{
			List<VectorSetJsonEntry> archiveData = new List<VectorSetJsonEntry>();

			//Get all the VectorSetJson data for this vector set, turn them into VectorSetJsonEntry records
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
			if (archiveData.Count > 0)
			{
				WriteArchiveFile(vectorSetID, archiveData);
			}
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

		private void SendWarningEmails()
		{
			//Get the IDs of the test sessions expiring in however many days, and the ID of the person
			var testSessions = _testSessionService.GetTestSessionsForExpirationWarning(_daysBeforeExpirationToWarn);

			//Group them by the person so we only send each person 1 email
			var groupedByPerson = testSessions.GroupBy(x => x.PersonID);

			foreach (var person in groupedByPerson)
			{
				SendWarningEmailToPerson(person.Key, person.Select(x => x.TestSessionID));
			}
		}

		private void SendWarningEmailToPerson(long personID, IEnumerable<long> testSessionIDs)
		{
			string subject = "ACVTS Test Session Expiration Warning";
			string body = $"The following ACVTS Test Sessions will expire in {_daysBeforeExpirationToWarn} days: {string.Join(", ", testSessionIDs)}";
			_mailer.Send(subject, body, _personService.GetEmailAddresses(personID));
		}
	}
}
