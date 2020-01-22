var exports = module.exports = {};

/**
 * 
 * Runs a newman/postman test with the provided arguments.
 * 
 * @param {string} registration_path - the registration file that will be provided in the newman test.
 * @param {string} crt_path - the public certificate file used for authenticating.
 * @param {string} key_path - the private key file used for authenticating.
 * @param {string} environment  - the environment "dev", "test", "demo".
 * @param {string[]} reporters - the newman reporters to utilize.
 * 
 * @returns {bool} true if successful, false if failure encountered.
 */
exports.runNewmanTest = function (registration_path, crt_path, key_path, environment, reporters) {
    const fs = require('fs'),
        newman = require('newman');
    let results = [];

    return newman.run({
        reporters: reporters,
        collection: 'ACVP-Testing.postman_collection.json',
        environment: 'ACVP.' + environment + '.postman_environment.json',
        iterationData: registration_path,
        insecure: true,
        sslClientCert: crt_path,
        sslClientKey: key_path,
        bail: true
    })
    .on('request', function (err, args) {
        // // here, args.response represents the entire response object
        // var rawBody = args.response.stream, // this is a buffer
        //     body = rawBody.toString(); // stringified JSON

        // results.push(JSON.parse(body)); // this is just to aggregate all responses into one object
    })
    // a second argument is also passed to this handler, if more details are needed.
    .on('done', function (err, summary) {
        // write the details to any file of your choice. The format may vary depending on your use case
        // fs.writeFileSync('totp.json', JSON.stringify(results[0], null, 4));
        // fs.writeFileSync('accessToken.json', JSON.stringify(results[1], null, 4));
        // fs.writeFileSync('registration-response.json', JSON.stringify(results[2], null, 4));

        // fs.writeFileSync('output.json', JSON.stringify(results.slice(3), null, 4));

        if (err || summary.error || summary.run.failures.length > 0) {
            // do something on error
            return;
        }

        // success
        return;
    });
};
