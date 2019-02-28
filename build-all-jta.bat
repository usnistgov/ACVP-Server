@echo off
set "zip=c:\Program Files\7-Zip\7z.exe"

For /f "tokens=2-4 delims=/ " %%a in ('date /t') do (set mydate=%%c%%a%%b)
For /f "tokens=1-2 delims=/: " %%a in ("%TIME%") do (if %%a LSS 10 (set mytime=0%%a%%b) else (set mytime=%%a%%b))
rem echo %mydate%-%mytime%

set git-branch="git branch | find "* " "
for /F "tokens=*" %%i in (' %git-branch% ') do set branch=%%i
set branch=%branch:* =%
rem echo %branch%

cd src\solutions\AllDeployable
dotnet restore
dotnet clean

cd ..\GenValAppRunner
dotnet restore
dotnet clean

cd ..\AllDeployable
dotnet build -c Release

rem GenVal Build
cd ..\GenValAppRunner
dotnet clean
dotnet publish -c Release -r win-x64

cd ..\..\common\src\NIST.CVP.Generation.GenValApp\bin\Release\netcoreapp2.1\win-x64\
"%zip%" a -tzip publish.zip publish\
move publish.zip "..\..\..\..\..\..\..\..\%mydate%_%mytime%_GenValsOrleans.zip"

cd ..\..\..\..\..\..\..\..\

rem Orleans build

cd src\orleans\src\NIST.CVP.Orleans.ServerHost
dotnet restore
dotnet clean
dotnet publish -c Release -r win-x64

rem cd ..\..\orleans\src\NIST.CVP.Orleans.ServerHost\bin\Release\netcoreapp2.1\win-x64\
cd bin\Release\netcoreapp2.1\win-x64\
"%zip%" a -tzip publish.zip publish\
move publish.zip "..\..\..\..\..\..\..\..\%mydate%_%mytime%_OrleansServer.zip"

cd ..\..\..\..\..\..\..\..\

rem Pool API build

cd src\pool-api\NIST.CVP.PoolAPI
dotnet clean
dotnet publish -c Release -r win-x64

cd bin\Release\netcoreapp2.1\win-x64\
"%zip%" a -tzip publish.zip publish\
move publish.zip "..\..\..\..\..\..\..\%mydate%_%mytime%_PoolService.zip"

cd ..\..\..\..\..\..\..\

rem Move builds to Latest_Builds on elwood (drive already mapped as X:)
move /Y *.zip X:\acvp\Latest_Builds\