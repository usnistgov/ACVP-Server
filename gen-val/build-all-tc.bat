@echo on
rem version 0.9.3
set "zip=c:\Program Files\7-Zip\7z.exe"

For /f "tokens=2-4 delims=/ " %%a in ('date /t') do (set mydate=%%c%%a%%b)
For /f "tokens=1-2 delims=/: " %%a in ("%TIME%") do (if %%a LSS 10 (set mytime=0%%a%%b) else (set mytime=%%a%%b))
echo %mydate%-%mytime%

rem set git-branch="git branch | find "* " "
rem for /F "tokens=*" %%i in (' %git-branch% ') do set branch=%%i
rem set branch=%branch:* =%
rem echo %branch%
rem set branch=%BranchName%

rem set homedir="%gitrepo%\gen-val"
set homedir="%homedir%"
echo %homedir%
set netcoreappver="%netcorepath%"
echo %netcoreappver%

cd %homedir%

echo =============================================================
echo Building GenValApp
echo =============================================================

cd gen-val\src\generation\src\NIST.CVP.Generation.GenValApp
dotnet clean
dotnet restore
dotnet build -c Release
dotnet publish -c Release -r win-x64

rem cd %homedir%\src\generation\src\NIST.CVP.Generation.GenValApp\
cd bin\Release\%netcoreappver%\win-x64\
"%zip%" a -tzip publish.zip publish\
move publish.zip "%homedir%\%mydate%_%mytime%_GenValsOrleans.zip"

cd %homedir%

rem Orleans build
echo =============================================================
echo Building OrleansServer
echo =============================================================
cd gen-val\src\orleans\src\NIST.CVP.Orleans.ServerHost
dotnet clean
dotnet restore
dotnet build -c Release
dotnet publish -c Release -r win-x64

rem cd ..\..\orleans\src\NIST.CVP.Orleans.ServerHost\bin\Release\netcoreapp2.1\win-x64\
cd bin\Release\%netcoreappver%\win-x64\
"%zip%" a -tzip publish.zip publish\
move publish.zip "%homedir%\%mydate%_%mytime%_OrleansServer.zip"

cd %homedir%

rem Pool API build
echo =============================================================
echo Building PoolAPI
echo =============================================================

cd gen-val\src\pool-api\NIST.CVP.PoolAPI
dotnet clean
dotnet restore
dotnet build -c Release
dotnet publish -c Release -r win-x64

cd bin\Release\%netcoreappver%\win-x64\
"%zip%" a -tzip publish.zip publish\
move publish.zip "%homedir%\%mydate%_%mytime%_PoolService.zip"

cd %homedir%

dir *.zip

rem Move builds to Latest_Builds on elwood (drive already mapped as X:)
rem move /Y *.zip X:\acvp\Latest_Builds\