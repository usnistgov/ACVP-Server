# ACVP - Gen/Vals

-----

Console application build in dotnet core used to generate (depending on a cryptographic algorithm) a set of random/static tests, used to help ensure the validity of a third parties crypto implementation.

-----

## Requirements

### To Run

* [Dotnet core runtime](https://www.microsoft.com/net/download/Windows/run), at least meeting netstandard2.1.
* That's it.

### To Develop

* [Dotnet core SDK](https://www.microsoft.com/net/download/), at least meeting netstandard2.1.
* IDE such as [Visual Studio](https://www.visualstudio.com/), or [VSCode](https://code.visualstudio.com/?wt.mc_id=adw-brand&gclid=Cj0KCQjwibDXBRCyARIsAFHp4fojTxuEuNCbj-3iNK5DIGpPHUJeDkAzOkEkdCJjrZ42ijrzoi3sUxAaAu4rEALw_wcB)

## How it works

The console application works through communication of json files as input and output. The application is invoked with parameters such as the algorithm to be tested, as well as the capabilities/registration json file that describe the "functions" being used in the algorithm.

## Running the application

```cmd
    # Generate test vectors with AES CBC, using the registration.json as the implementations capabilities
    dotnet NIST.CVP.Generation.GenValApp.dll -a aes -m cbc -g registration.json

    # Validate test vectors with AES CBC, using the previously generated internalProjection along with the IUT's responses.
    dotnet NIST.CVP.Generation.GenValApp.dll -a aes -m cbc -n internalProjection.json -r testResults.json
```

## See also

* [GitHub specification](https://github.com/usnistgov/ACVP)
* [ACVP front end API](https://gitlab.nist.gov/gitlab/ACVTS/Controller/controller)
* [JSON file examples](https://gitlab.nist.gov/gitlab/ACVTS/GenVals/gen-val/tree/master/json-files)
* [Wiki](https://gitlab.nist.gov/gitlab/ACVTS/GenVals/gen-val/wikis/home)