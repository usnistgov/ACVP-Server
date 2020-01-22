/**
 * 
 * Summarize the load test
 * 
 * @param {string} environment - the environment the tests were run on.
 * @param {number} totalTests - the total (intended) to be run amount of tests.
 * @param {number} totalTestsStarted  - the number of tests that were started.
 * @param {number} totalSuccess - the number of tests that were successful.
 * @param {number} totalFailure - the number of tests that failed.
 * @param {number} maxConcurrency - the total number of tests that can run concurrently.
 * @param {[]]} loadTestResults - the individual test results.
 */
function LoadTestSummary(environment, loadTestStartTime, totalTests, totalTestsStarted, totalSuccess, totalFailure, maxConcurrency, loadTestResults) {
    var moment = require('moment');

    this.environment = environment;
    this.loadTestStartTime = loadTestStartTime;
    this.loadTestEndTime = moment();

    this.totalTests = totalTests;
    this.totalTestsStarted = totalTestsStarted;
    this.totalSuccess = totalSuccess;
    this.totalFailure = totalFailure;

    this.successPercent = this.totalSuccess / this.totalTestsStarted * 100;
    this.failurePercent = this.totalFailure / this.totalTestsStarted * 100;

    this.maxConcurrency = maxConcurrency;

    this.loadTestDurationSeconds = this.loadTestEndTime.diff(loadTestStartTime, 'seconds');
    
    if (loadTestResults.length > 0) {
        const totalTestSeconds = loadTestResults.reduce((total, current) => total + current.testDurationSeconds, 0);
        this.averageTestSeconds = totalTestSeconds / totalTestsStarted;

        // Summarize the load test on a per file type basis
        this.summaryPerRegistrationType = [];
        for (let i = 0; i < loadTestResults.length; i++) {
            if (!this.summaryPerRegistrationType.includes(loadTestResults[i])) {
                const testFile = loadTestResults[i].registrationFile;

                const filteredResults = loadTestResults.filter(w => w.registrationFile === testFile);
                const filteredResultsLength = filteredResults.length;
                const totalSuccess = filteredResults.filter(w => w.isSuccessful).length;
                const totalFailure = filteredResults.filter(w => !w.isSuccessful).length;

                this.summaryPerRegistrationType.push({
                    file: testFile,
                    totalTests: filteredResultsLength,
                    totalSuccess: totalSuccess,
                    totalFailure: totalFailure,
                    successPercent: totalSuccess / filteredResultsLength * 100,
                    failurePercent: totalFailure / filteredResultsLength * 100,
                    minDurationSeconds: Math.min(...filteredResults.map(i => i.testDurationSeconds)),
                    maxDurationSeconds: Math.max(...filteredResults.map(i => i.testDurationSeconds)),
                    avgDurationSeconds: filteredResults.reduce((total, current) => total + current.testDurationSeconds, 0) / filteredResultsLength
                });
            }
        }
    }
}

module.exports = LoadTestSummary;