# Postman Tests

Can be run in Postman or in newman (a terminal interface for Postman).

The command for running in newman is

```cmd
newman run Dev-Tests.postman_collection.json --bail --silent -e ACVP-Dev.postman_environment.json -k --ssl-client-cert ..\cceli.crt --ssl-client-key ..\cceli.key -d <registration file>
```

* --bail ejects when an error is reached
* -e specifies the environment variables used
* -k allows the self-signed certificate
* --silent reduces output
* --ssl-client-cert is for your client cert for the server
* --ssl-client-key is your private key for the cert

Command returns 0 on success and 1 on fail.

Can run via python with:

```cmd
python test-all.py -c "C:\\Users\rlh4\\.ssh\\russHammett.crt" -k "C:\\Users\\rlh4\\.ssh\\id_rsa" -r ".\\version\\v0.4\\formatted-registrations\\" -t 4 -v "v0.4"
```

For running on a specific environment (note dev is used by default when "e" flag not specified):

```cmd
python test-all.py -e "dev" -c "C:\\Users\rlh4\\.ssh\\russHammett.crt" -k "C:\\Users\\rlh4\\.ssh\\id_rsa" -r ".\\version\\v0.4\\formatted-registrations\\" -t 4 -v "v0.4"
python test-all.py -e "test" -c "C:\\Users\rlh4\\.ssh\\russHammett.crt" -k "C:\\Users\\rlh4\\.ssh\\id_rsa" -r ".\\version\\v0.4\\formatted-registrations\\" -t 4 -v "v0.4"
python test-all.py -e "demo" -c "C:\\Users\rlh4\\.ssh\\russHammett.crt" -k "C:\\Users\\rlh4\\.ssh\\id_rsa" -r ".\\version\\v0.4\\formatted-registrations\\" -t 4 -v "v0.4"
```

For running a specific version of ACVP protocol tests, specify the version with `-v`:

```cmd
python test-all.py -e "dev" -c "C:\\Users\rlh4\\.ssh\\russHammett.crt" -k "C:\\Users\\rlh4\\.ssh\\id_rsa" -r ".\\version\\v0.4\\formatted-registrations\\" -t 4 -v "v0.4"
```

For help on argument definitions, see:

```cmd
python test-all.py -h
```

uses node.js newman package.