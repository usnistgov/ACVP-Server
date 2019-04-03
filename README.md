# ACVP - Gen/Vals

-----

Console application build in dotnet core used to generate (depending on a cryptographic algorithm) a set of random/static tests, used to help ensure the validity of a third parties crypto implementation.

-----

## Requirements

### To Run

* [Dotnet core runtime](https://www.microsoft.com/net/download/Windows/run), at least meeting netapp2.1.
* `ASPNETCORE_ENVIRONMENT` environment variable set, likely to "Local".
* That's it.

### To Develop

* [Dotnet core SDK](https://www.microsoft.com/net/download/), at least meeting netcoreapp2.1.
* `ASPNETCORE_ENVIRONMENT` environment variable set, likely to "Local".
* IDE such as [Visual Studio](https://www.visualstudio.com/), or [VSCode](https://code.visualstudio.com/?wt.mc_id=adw-brand&gclid=Cj0KCQjwibDXBRCyARIsAFHp4fojTxuEuNCbj-3iNK5DIGpPHUJeDkAzOkEkdCJjrZ42ijrzoi3sUxAaAu4rEALw_wcB)

## How it works

The console application works through communication of json files as input and output. The application is invoked with parameters such as the algorithm to be tested, as well as the capabilities/registration json file that describe the "functions" being used in the algorithm.

## Supported `ASPNETCORE_ENVIRONMENT` values

* Local
* Tc
* Dev
* Test
* Demo
* Prod

## Running the application

### Orleans

The ACVP Project uses [Orleans](https://github.com/dotnet/orleans) to distribute crypto across a (potential) cluster of nodes.  The genvals rely on this cluster being available, and configuration is provided via `appsettings.[env].json` files.

To host the Orleans Silo locally there are two options:

* Run as a console application
  * From the NIST.CVP.Orleans.ServerHost directory (where the csproj is located) `dotnet run --console`
  * From the compiled binary: `dotnet NIST.CVP.Orleans.ServerHost.dll --console`
* Run as a service
  * After publishing with `dotnet publish -c Release` run from an elevated command prompt: 

  ```cmd
  sc delete AcvpOrleans # if exists
  sc create AcvpOrleans binPath= "C:\workspace\gitLab\gen-val\src\orleans\src\NIST.CVP.Orleans.ServerHost\bin\Release\netcoreapp2.1\win7-x64\publish\NIST.CVP.Orleans.ServerHost.exe" # Note the above exe is a sample, should be replaced with the location of your built exe
  sc start AcvpOrleans
  ```

### Pools

The ACVP genvals are able (but not required) to make use of a "Pool" of precomputed values.  This Pool is served to the genvals through the use of a hosted webApi.  The webApi can be hosted either through a console app runner, or as a registered windows service.

To host:

* As a console application (local dev):
  * From NIST.CVP.PoolApi run `dotnet run --console`
  * From the compiled binary `dotnet NIST.CVP.PoolApi --console`
* As a windows service:

```cmd
sc create AcvpPoolApi binPath= "C:\workspace\gitLab\gen-val\src\pool-api\NIST.CVP.PoolAPI\bin\Release\netcoreapp2.1\win7-x64\NIST.CVP.PoolAPI.exe C:\workspace\gitLab\gen-val\src\pool-api\NIST.CVP.PoolAPI\Pools"
sc start AcvpPoolApi
```

### GenVals console application

```cmd
    # Generate test vectors with AES CBC testing revision 1.0, using the registration.json as the implementations capabilities
    dotnet NIST.CVP.Generation.GenValApp.dll -a aes -m cbc -R 1.0 -g registration.json

    # Validate test vectors with AES CBC testing revision 1.0, using the previously generated internalProjection along with the IUT's responses.
    dotnet NIST.CVP.Generation.GenValApp.dll -a aes -m cbc -R 1.0 -n internalProjection.json -r testResults.json
```



## See also

* [GitHub specification](https://github.com/usnistgov/ACVP)
* [ACVP front end API](https://gitlab.nist.gov/gitlab/ACVTS/Controller/controller)
* [JSON file examples](https://gitlab.nist.gov/gitlab/ACVTS/GenVals/gen-val/tree/master/json-files)
* [Wiki](https://gitlab.nist.gov/gitlab/ACVTS/GenVals/gen-val/wikis/home)