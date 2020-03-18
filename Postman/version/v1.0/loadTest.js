// node --max-old-space-size=4096 loadTest.js "C:\workspace\gitLab\controller\Postman Tests\version\v1.0\formatted-registrations" "enumerate" "C:\Users\rlh4\.ssh\russHammett.crt" "C:\Users\rlh4\.ssh\id_rsa" "dev" "20" "500"
 
// const registration_path = "C:\\workspace\\gitLab\\controller\\Postman Tests\\version\\v0.5\\formatted-registrations",
//     registrationSubmissionStrategy = "enumerate",
//     crt_path = "C:\\Users\\rlh4\\.ssh\\russHammett.crt",
//     key_path = "C:\\Users\\rlh4\\.ssh\\id_rsa",
//     environment = "dev";
//     maxConcurrency = 1;
//     totalTestsToRun = 1;

// 2 must be directory containing registrations to test with
// 3 the method of submitting registrations to newman - "enumerate", "random"
// 4 must be crt file
// 5 must be key file
// 6 must be environment "dev", "test", "demo"
// 7 must be the max number of tests to run simultaneously.
// 8 must be the total number of tests to run.
const registration_path = process.argv[2],
    registrationSubmissionStrategy = process.argv[3],
    crt_path = process.argv[4],
    key_path = process.argv[5],
    environment = process.argv[6];
    maxConcurrency = parseInt(process.argv[7]);
    totalTestsToRun = parseInt(process.argv[8]);

const fs = require('fs'),
    moment = require('moment');
const LoadTestResult = require('./loadTestResult'),
    LoadTestSummary = require('./loadTestSummary');

let runCount = 0,
    currentlyRunning = 0,
    totalSuccess = 0,
    totalFailure = 0,
    results = [];

const loadTestStartTime = moment();

/**
 * 
 * Filenames to filter from the list of registrations.
 * 
 * @param {string} element - the file name to potentially filter 
 */
function filterRegistrations(element) {
    
    // Skips entire json files (or unwanted files in general)
    const registrationsToFilter = [
        ".DS_Store",
        "DSA-PQGGen-canonical.json", 
        "DSA-PQGGen-unverified.json", 
        "DSA-PQGVer-canonical.json", 
        "DSA-PQGVer-unverified.json"
    ];

    // Skips any json file that contains the substring
    const partialsToFilter = [
        "KAS"
    ]

    let shouldProcessRegistrationFile = !registrationsToFilter.includes(element);

    for (let i = 0; i < partialsToFilter.length; i++){
        if (element.includes(partialsToFilter[i])){
            shouldProcessRegistrationFile = false;
        }
    }

    if (shouldProcessRegistrationFile) {
        console.log(`including file: ${element}`);
    } else {
        console.log(`skipping file: ${element}`);
    }
    
    return shouldProcessRegistrationFile;
}

main = async function() {
    var registrationFiles = [];
    var unfilteredRegistration = fs.readdirSync(registration_path);

    for (let i = 0; i < unfilteredRegistration.length; i++) {
        if (filterRegistrations(unfilteredRegistration[i])) {
            registrationFiles.push(unfilteredRegistration[i]);
        }
    }

    const registrationFilesLength = registrationFiles.length;

    const snooze = ms => new Promise(resolve => setTimeout(resolve, ms));
    const example = async () => {
        //console.log('About to snooze without halting the event loop...');
        await snooze(1000);
        //console.log('done!');
      };

    for (let i = 0; i < totalTestsToRun; i++) {
        while (true) {
            if (currentlyRunning < maxConcurrency) {
                await runTestFile(registration_path, getRegistrationToSubmit(registrationFiles, registrationFilesLength, i));
                break;
            }
            else {
                //console.log('currently at max concurrency, waiting...');
            }

            await example();
        }
    }
};

runTestFile = async function(testDirectory, testFile) {
    const newmanRun = require('./newmanRun');

    const fullFile = testDirectory + '\\' + testFile;
    const uniqueId = ++runCount;
    const startDate = moment();
    const runningAtStart = currentlyRunning++;

    let vsId = 0;
    let requestsForRun = [];

    let requestGuid, 
    name, 
    startRequest, 
    endRequest;
    // use this for tracking the amount of time between the end of a request, and the start of the next.
    let previousRequestEnd = moment();
    
    //console.log(`uniqueId: ${uniqueId}`);
    console.log(`Starting test ${runCount} of ${totalTestsToRun}, ${runningAtStart} other tests are running currently. Running file ${testFile}`);

    // TODO note to self for when i pick this back up - "args.item.id" is NOT a unique identifier for the request/response, it appears to be a unique
    // identifier for the ***STEP***, so that steps that run in a loop all have the same ID.  Need to find if there's a way to uniquely identify a request/response.
    newmanRun.runNewmanTest(fullFile, crt_path, key_path, environment, ['json'])
        .on('beforeRequest', function(err, args){
            requestGuid = uuidv4();
            name = args.item.name;
            startRequest = moment();
        })
        .on('request', function (err, args) {
            endRequest = moment();

            // create an item in array that represents the request
            requestsForRun.push({
                id: requestGuid,
                name: name,
                startRequest: startRequest,
                endRequest: endRequest,
                requestDurationMilliseconds: endRequest.diff(startRequest, 'milliseconds'),
                endToStartDiffMilliseconds: startRequest.diff(previousRequestEnd, 'milliseconds')
            });

            // update the previousRequestEnd for the next request to track against.
            previousRequestEnd = endRequest;

            // We only care about the response stream until we grab the vsId
            if (vsId == 0 && args && args.response && args.response.stream) {
                let rawBody = args.response.stream, // this is a buffer
                body = JSON.parse(rawBody.toString()); // stringified JSON

                if (body[1] && body[1].vsId) {
                    vsId = body[1].vsId;

                    rawBody = null;
                    body = null;
                }
            }
        })
        .on('done', function (err, summary) {
            const endDate = moment();
            currentlyRunning--;
            
            let isSuccessful = false;

            if (err || summary.error || summary.run.failures.length > 0) {
                totalFailure++;
            }
            else {
                totalSuccess++;
                isSuccessful = true;
            }

            console.log(`Remaining tests to complete: ${totalTestsToRun - totalFailure - totalSuccess}. ` + 
                `So far ${totalSuccess} tests have passed, ${totalFailure} tests have failed. ` +
                `Test ${vsId}'s outcome was ${isSuccessful? "SUCCESSFUL" : "NOT SUCCESSFUL"}`
            );

            const iterationResult = new LoadTestResult(
                uniqueId, 
                vsId,
                runningAtStart, 
                testFile, 
                startDate, 
                endDate, 
                isSuccessful,
                summary.run.failures,
                requestsForRun
            );

            results.push(iterationResult);
        });
};

getRegistrationToSubmit = function(registrationFiles, registrationFilesLength, testRunNumber) {
    if (registrationSubmissionStrategy == "random") {
        return registrationFiles[Math.floor(Math.random() * registrationFilesLength)];
    }
    
    return registrationFiles[testRunNumber % registrationFilesLength];
};

uuidv4 = function() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
};

function exitHandler(options, exitCode) {
    if (options.cleanup) {
        const testResultsFileName = moment().format("YYYY-MM-DD_HHmmss") + 
            `_${environment}_loadTest_${uuidv4()}`;
        const dir = "loadTestRuns";
        if (!fs.existsSync(dir)){
            fs.mkdirSync(dir);
        }
        
        console.log(testResultsFileName);
        fs.writeFileSync(`${dir}/${testResultsFileName}.json`, JSON.stringify(results, null, 4));

        // Write Summary file
        const summary = new LoadTestSummary(
            environment, 
            loadTestStartTime,
            totalTestsToRun, 
            runCount,
            totalSuccess,
            totalFailure,
            maxConcurrency,
            results
        );
        fs.writeFileSync(`${dir}/${testResultsFileName}_summary.json`, JSON.stringify(summary, null, 4));

        console.log(`Total Successes: ${totalSuccess}.`);
        console.log(`Total Failures: ${totalFailure}.`);
    }

    if (options.exit) process.exit();
}

//do something when app is closing
process.on('exit', exitHandler.bind(null,{cleanup:true}));

//catches ctrl+c event
process.on('SIGINT', exitHandler.bind(null, {exit:true}));

main();
