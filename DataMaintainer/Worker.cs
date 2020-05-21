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
		private readonly bool _testSessionExpirationEnabled;
		private readonly int _testSessionExpirationAgeInDays;
		private readonly bool _testSessionExpirationWarningEmailsEnabled;
		private readonly int _testSessionDaysBeforeExpirationToWarn;
		private readonly bool _vectorSetCleanupEnabled;
		private readonly bool _vectorSetArchiveEnabled;
		private readonly string _vectorSetArchiveFolder;

		public Worker(ILogger<Worker> logger, IVectorSetService vectorSetService, ITestSessionService testSessionService, IPersonService personService, IHostApplicationLifetime hostApplicationLifetime, IConfiguration configuration, IMailer mailer)
		{
			_logger = logger;
			_vectorSetService = vectorSetService;
			_testSessionService = testSessionService;
			_personService = personService;
			_hostApplicationLifetime = hostApplicationLifetime;
			_mailer = mailer;

			_testSessionExpirationEnabled = configuration.GetValue<bool>("DataMaintainer:TestSessionExpirationEnabled");
			_testSessionExpirationAgeInDays = configuration.GetValue<int>("DataMaintainer:TestSessionExpirationAgeInDays");
			_testSessionExpirationWarningEmailsEnabled = configuration.GetValue<bool>("DataMaintainer:TestSessionExpirationWarningEmailsEnabled");
			_testSessionDaysBeforeExpirationToWarn = configuration.GetValue<int>("DataMaintainer:TestSessionDaysBeforeExpirationToWarn");
			_vectorSetCleanupEnabled = configuration.GetValue<bool>("DataMaintainer:VectorSetCleanupEnabled");
			_vectorSetArchiveEnabled = configuration.GetValue<bool>("DataMaintainer:VectorSetArchiveEnabled");
			_vectorSetArchiveFolder = configuration.GetValue<string>("DataMaintainer:VectorSetArchiveFolder");
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
			//Send expiration warning emails
			if (_testSessionExpirationWarningEmailsEnabled)
			{
				SendExpirationWarningEmails();
			}

			//Expire test sessions that haven't been touched in the configured number of days
			if (_testSessionExpirationEnabled)
			{
				_testSessionService.Expire(_testSessionExpirationAgeInDays);
			}

			//Clean up the VectorSetJson data, potentially writing archive file
			if (_vectorSetCleanupEnabled)
			{
				//If want to produce archive files, make sure the destination can be reached, or else don't do the cleanup - don't want to delete the data without having the archive
				if (_vectorSetArchiveEnabled && !Directory.Exists(_vectorSetArchiveFolder))
				{
					_logger.LogError($"Destination folder {_vectorSetArchiveFolder} could not be accessed, no vector set cleanup or archival performed");
				}
				else
				{
					//Get the vector sets to be cleaned up/archived
					List<long> vectorSetIDs = _vectorSetService.GetVectorSetsToArchive();

					foreach (long vectorSetID in vectorSetIDs)
					{
						//If configured to write archive files, do so
						if (_vectorSetArchiveEnabled)
						{
							CreateArchiveFile(vectorSetID);
						}

						//Clean up the database
						_vectorSetService.Archive(vectorSetID);

						_logger.LogInformation($"Archived vector {vectorSetID}");
					}
				}
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
			using FileStream archiveFile = new FileStream(Path.Combine(_vectorSetArchiveFolder, $"{vectorSetID}.zip"), FileMode.Create);
			using ZipArchive archive = new ZipArchive(archiveFile, ZipArchiveMode.Create);
			ZipArchiveEntry entry = archive.CreateEntry($"{vectorSetID}.json");

			using StreamWriter writer = new StreamWriter(entry.Open());
			writer.Write(JsonSerializer.Serialize(vectorSetData));
		}

		private void SendExpirationWarningEmails()
		{
			//Get the IDs of the test sessions expiring in however many days, and the ID of the person
			var testSessions = _testSessionService.GetTestSessionsForExpirationWarning(_testSessionExpirationAgeInDays - _testSessionDaysBeforeExpirationToWarn);

			//Group them by the person so we only send each person 1 email
			var groupedByPerson = testSessions.GroupBy(x => x.PersonID);

			foreach (var person in groupedByPerson)
			{
				SendExpirationWarningEmail(person.Key, person.Select(x => x.TestSessionID));
			}
		}

		private void SendExpirationWarningEmail(long personID, IEnumerable<long> testSessionIDs)
		{
			string subject = "ACVTS Test Session Expiration Warning";
			string body = $"The following ACVTS Test Sessions will expire in {_testSessionDaysBeforeExpirationToWarn} days if there is no activity: {string.Join(", ", testSessionIDs)}";
			_mailer.Send(subject, body, _personService.GetEmailAddresses(personID));
		}
	}
}
