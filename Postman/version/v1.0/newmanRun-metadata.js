var exports = module.exports = {};

/**
 * 
 * Runs a newman/postman test with the provided arguments.
 * 
 * @param {string} collection_path - the collection file that will be provided in the newman test.
 * @param {string} crt_path - the public certificate file used for authenticating.
 * @param {string} key_path - the private key file used for authenticating.
 * @param {string} environment  - the environment "dev", "test", "demo", "tc-dev"
 * @param {string[]} reporters - the newman reporters to utilize.
 * 
 * @returns {bool} true if successful, false if failure encountered.
 */
exports.runNewmanTest = function (collection_path, crt_path, key_path, environment, reporters) {
    const fs = require('fs'),
        newman = require('newman');
    let results = [];

    return newman.run({
        reporters: reporters,
        collection: collection_path,
        environment: 'ACVP.' + environment + '.postman_environment.json',
        insecure: true,
        sslClientCert: crt_path,
        sslClientKey: key_path,
        bail: true
    })
    .on('request', function (err, args) {
        // here, args.response represents the entire response object
        var rawBody = args.response.stream, // this is a buffer
             body = rawBody.toString(); // stringified JSON

        results.push(JSON.parse(body)); // this is just to aggregate all responses into one object
    });
};
