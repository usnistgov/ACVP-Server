import subprocess
import multiprocessing
import os
import sys

from Classes.parsedArguments import ParsedArguments

# newman run Dev-Tests.postman_collection.json --bail -e ACVP-Dev.postman_environment.json -k --ssl-client-cert ..\cceli.crt --ssl-client-key ..\cceli.key -d <registration-file>
class Runner():

	_collection_file = 'ACVP-Testing.postman_collection.json'
	_environment_file = None
	
	_full_command = None
	_parsed_arguments = None
	_teamcity = " --reporters teamcity,cli,html --reporter-html-export newman/report.html"

	def __init__(self, parsed_arguments : ParsedArguments):
		self._parsed_arguments = parsed_arguments
		self._set_environment_file()

		fullCollectionFileName = '.\\version\\' + self._parsed_arguments.version + '\\' + self._collection_file
		fullEnvironmentFileName = '.\\version\\' + self._parsed_arguments.version + '\\' + self._environment_file

		self._full_command = "newman run " + fullCollectionFileName + " --bail --silent -e " + fullEnvironmentFileName + " -k --ssl-client-cert " + self._parsed_arguments.cert_file + " --ssl-client-key " + self._parsed_arguments.key_file + " -d " + self._parsed_arguments.registration_folder

	def _set_environment_file(self):
		env = self._parsed_arguments.environment

		if env == 'dev':
			self._environment_file = 'ACVP-Dev.postman_environment.json'
		elif env == 'test':
			self._environment_file = 'ACVP-Test.postman_environment.json'
		elif env == 'demo':
			self._environment_file = 'ACVP-Demo.postman_environment.json'

	def _run_file(self, filename):
		name = str(filename.split("\\")[:][0])

		if (self._parsed_arguments.teamcity):
			print("##teamcity[testStarted name='{name}' captureStandardOutput='true']".format(
				name = name
			))
		
		print("Running: " + name)
		ready_command = self._full_command + filename
		if (self._parsed_arguments.teamcity):
			ready_command += self._teamcity

		sts = subprocess.call(ready_command, shell=True)
		if (sts == 0):
			print("Passed: " + filename)
		else:
			print("Failed: " + filename)
		
		if (self._parsed_arguments.teamcity):
			print("##teamcity[testFinished name='{name}']".format(
				name = name
			))

		return (filename, sts)


	def run(self):
		if (self._parsed_arguments.teamcity):
			print("##teamcity[testStarted name='TestSummary' captureStandardOutput='true']")

		print("Starting tests...")
		p = multiprocessing.Pool(processes=self._parsed_arguments.max_threads)
		results = p.map(self._run_file, os.listdir(self._parsed_arguments.registration_folder))

		passed = []
		failed = []
		for pair in results:
			if (pair[1] == 0):
				passed.append(pair[0])
			else:
				failed.append(pair[0])

		print("-------")
		print()
		print("Passed:")
		print(passed)
		print("Failed:")
		print(failed)

		if (self._parsed_arguments.teamcity):
			print("##teamcity[testFinished name='TestSummary']")

		if len(failed) > 0 or len(passed) == 0:
			sys.exit(1)
		
		sys.exit(0)