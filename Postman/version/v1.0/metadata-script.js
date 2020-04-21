// node metadata-script.js "collection file" "C:\Users\rlh4\.ssh\russHammett.crt" "C:\Users\rlh4\.ssh\id_rsa" "dev"

// 2 must be collection file
// 3 must be crt file
// 4 must be key file
// 5 must be environment "dev", "test", "demo", "tc-dev"
var collection_path = process.argv[2],
    crt_path = process.argv[3],
    key_path = process.argv[4],
    environment = process.argv[5];

var newmanRun = require('./newmanRun-metadata');
newmanRun.runNewmanTest(collection_path, crt_path, key_path, environment, ['teamcity', 'json']);
