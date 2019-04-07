@echo on
set "zip=c:\Program Files\7-Zip\7z.exe"

For /f "tokens=2-4 delims=/ " %%a in ('date /t') do (set mydate=%%c%%a%%b)
For /f "tokens=1-2 delims=/: " %%a in ("%%TIME%%") do (if %%a LSS 10 (set mytime=0%%a%%b) else (set mytime=%%a%%b))
rem echo %mydate%-%mytime%

rem set git-branch="git branch | find "* " "
rem for /F "tokens=*" %%i in (' %git-branch% ') do set branch=%%i
rem set branch=%branch:* =%
rem echo %branch%
set branch=develop

rem set homedir="%gitrepo%\gen-val"
set homedir="%env.homedir%"

cd %homedir%

cd src\solutions\AllDeployable
dotnet clean
dotnet restore

cd ..\GenValAppRunner
dotnet restore

cd ..\AllDeployable
dotnet build -c Release

rem GenVal Build
rem cd ..\GenValAppRunner
cd %homedir%

cd src\common\src\NIST.CVP.Generation.GenValApp
dotnet publish -c Release -r win-x64

cd %homedir%\src\common\src\NIST.CVP.Generation.GenValApp\bin\Release\netcoreapp2.1\win-x64\
"%zip%" a -tzip publish.zip publish\
move publish.zip "%homedir%\%mydate%_%mytime%_GenValsOrleans.zip"

cd %homedir%

rem Orleans build

cd src\orleans\src\NIST.CVP.Orleans.ServerHost
dotnet publish -c Release -r win-x64

rem cd ..\..\orleans\src\NIST.CVP.Orleans.ServerHost\bin\Release\netcoreapp2.1\win-x64\
cd bin\Release\netcoreapp2.1\win-x64\
"%zip%" a -tzip publish.zip publish\
move publish.zip "%homedir%\%mydate%_%mytime%_OrleansServer.zip"

cd %homedir%

rem Pool API build

cd src\pool-api\NIST.CVP.PoolAPI
dotnet publish -c Release -r win-x64

cd bin\Release\netcoreapp2.1\win-x64\
"%zip%" a -tzip publish.zip publish\
move publish.zip "%homedir%\%mydate%_%mytime%_PoolService.zip"

cd %homedir%

rem Move builds to Latest_Builds on elwood (drive already mapped as X:)
rem move /Y *.zip X:\acvp\Latest_Builds\