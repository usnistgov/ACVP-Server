/**
 * 
 * Represents the result of a single iteration of a newman invoke.
 * 
 * @param {number} runNumber - the unique ID for this test.
 * @param {number} vsID - the vectorSetId as reported by the driver - not grabbed until after registration.
 * @param {number} currentlyRunningTests - the number of other running tests at that START of the current newman test.
 * @param {string} registrationFile - the name of the registration file that was submitted with the newman test.
 * @param {date} startDate - the start time of the newman test.
 * @param {date} endDate - the end time of the newman test.
 * @param {bool} isSuccessful - the outcome of the test.
 * @param {[]} errors - the errors that were present in the test run (if any).
 * @param {[]]} responses - The start/end/status code of each individual request of the run.
*/
function LoadTestResult(runNumber, vsId, currentlyRunningTests, registrationFile, startDate, endDate, isSuccessful, errors, responses) {
    const moment = require('moment');
    
    this.runNumber = runNumber;
    this.vsId = vsId;
    this.currentlyRunningTests = currentlyRunningTests;
    this.registrationFile = registrationFile;
    this.startDate = moment(startDate);
    this.endDate = moment(endDate);
    this.isSuccessful = isSuccessful;
    this.errors = errors;
    this.responses = responses;

    this.testDurationSeconds = endDate.diff(startDate, 'seconds');
}

module.exports = LoadTestResult;