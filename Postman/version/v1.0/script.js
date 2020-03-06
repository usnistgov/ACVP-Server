// node script.js "C:\workspace\gitLab\controller\Postman Tests\version\v0.5\formatted-registrations\aes-cbc.json" "C:\Users\rlh4\.ssh\russHammett.crt" "C:\Users\rlh4\.ssh\id_rsa" "dev"

// 2 must be registration file
// 3 must be crt file
// 4 must be key file
// 5 must be environment "dev", "test", "demo", "tc-dev"
var registration_path = process.argv[2],
    crt_path = process.argv[3],
    key_path = process.argv[4],
    environment = process.argv[5];

var newmanRun = require('./newmanRun');
newmanRun.runNewmanTest(registration_path, crt_path, key_path, environment, ['teamcity', 'json']);
