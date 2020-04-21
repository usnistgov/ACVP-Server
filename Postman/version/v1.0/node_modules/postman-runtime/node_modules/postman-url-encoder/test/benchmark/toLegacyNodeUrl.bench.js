var fs = require('fs'),
    path = require('path'),
    toLegacyNodeUrl = require('../..').toLegacyNodeUrl,
    parseCsv = require('@postman/csv-parse/lib/sync');

suite('toLegacyNodeUrl()', function () {
    var testCases = fs.readFileSync(path.join(__dirname, '../fixtures/urlList.csv'));

    testCases = parseCsv(testCases, {
        columns: true,
        trim: false
    });

    testCases.forEach(function (testcase) {
        scenario(testcase.description, function () {
            toLegacyNodeUrl(testcase.url);
        });
    });
});
