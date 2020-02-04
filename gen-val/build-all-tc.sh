#!/bin/bash

# set datetimestamp format YYYYMMDD_HHMM
datetimestamp=$(date +%Y%m%d_%H%m)
echo datetimestamp is ${datetimestamp}

# set homedir var from TC %homedir%
homedir=$(pwd)
echo homedir is ${homedir}

# set netcorappver var from TC %netcorepath%
netcoreappver="netcoreapp3.1"
echo netcoreappver is ${netcoreappver}

cd ${homedir}

cd gen-val/src/generation/src/NIST.CVP.Generation.GenValApp
dotnet clean
dotnet restore
dotnet build -c Release
dotnet publish -c Release -r win-x64

cd bin/Release/${netcoreappver}/win-x64
/usr/bin/zip -q -r publish.zip publish/
cp -v ./publish.zip ${homedir}/${datetimestamp}_GenValsOrleans.zip

cd ${homedir}

cd gen-val/src/orleans/src/NIST.CVP.Orleans.ServerHost
dotnet clean
dotnet restore
dotnet build -c Release
dotnet publish -c Release -r win-x64

cd bin/Release/${netcoreappver}/win-x64
/usr/bin/zip -q -r publish.zip publish/
cp -v ./publish.zip ${homedir}/${datetimestamp}_OrleansServer.zip

cd ${homedir}

cd gen-val/src/pool-api/NIST.CVP.PoolAPI
dotnet clean
dotnet restore
dotnet build -c Release
dotnet publish -c Release -r win-x64

cd bin/Release/${netcoreappver}/win-x64/
/usr/bin/zip -q -r publish.zip publish/
cp -v ./publish.zip ${homedir}/${datetimestamp}_PoolService.zip

cd ${homedir}
ls -l *.zip
