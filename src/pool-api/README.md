# Pool Api

The pool API is an API used to get, add, and remove values from a "pool" of precomputed values related to a cryptographic algorithm.  The Pool can be run as either a service, or IIS hosted web api.

## Local testing

* Build the solution
* Navigate to the built exe (`/bin/Debug/.../)
* Run the web api project from terminal `NIST.CVP.PoolAPI.exe -c C:\workspace\gitLab\gen-val\src\pool-api\NIST.CVP.PoolAPI\Pools`
  * Note that the directory in the command above is the directory where the pools configuration json is location.