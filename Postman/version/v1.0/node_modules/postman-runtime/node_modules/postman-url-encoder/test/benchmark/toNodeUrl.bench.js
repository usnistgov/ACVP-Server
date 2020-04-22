var fs = require('fs'),
    path = require('path'),
    toNodeUrl = require('../../').toNodeUrl,
    parseCsv = require('@postman/csv-parse/lib/sync');

suite('toNodeUrl()', function () {
    var testCases = fs.readFileSync(path.join(__dirname, '../fixtures/urlList.csv'));

    testCases = parseCsv(testCases, {
        columns: true,
        trim: false
    });

    testCases.forEach(function (testcase) {
        scenario(testcase.description, function () {
            toNodeUrl(testcase.url);
        });
    });
});
