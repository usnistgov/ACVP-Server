@echo off
set "zip=c:\Program Files\7-Zip\7z.exe"

For /f "tokens=2-4 delims=/ " %%a in ('date /t') do (set mydate=%%c%%a%%b)
For /f "tokens=1-2 delims=/: " %%a in ("%TIME%") do (if %%a LSS 10 (set mytime=0%%a%%b) else (set mytime=%%a%%b))
rem echo %mydate%-%mytime%

cls
echo =============================================================
echo Preparing to build GenVals, PoolAPI and OrleansServer
echo Build %mydate%_%mytime%
echo =============================================================

echo Have you updated from the repo? If not, now's the time.
pause

rem This references the gitrepo env variable, which points to the root directory
rem where the repo is cloned down locally on your system
rem So, either set it in your environment, or define it here in the batch file
rem set gitrepo="c:\location_of_your_local_repo_clone_root"
rem If you define it locally per the above, please do not check it in to the repo - thanks :)
if exist %gitrepo% (
	set homedir="%gitrepo%\gen-val"
) else (
	echo You need to either define the "gitrepo" environment variable or define it locally in this batch file. Exiting.
	goto :eof
)

echo Checking to see if drive X: is mapped
if not exist X: (
	echo Drive X: not mapped, issuing net use command to map the drive now:
	net use X: \\elwood\773\internal\stvmdev\
) else (
	echo It is!
)

cd %homedir%

set git-branch="git branch | find "* " "
for /F "tokens=*" %%i in (' %git-branch% ') do set branch=%%i
set branch=%branch:* =%
rem echo %branch%

rem clear out existing build zip files?
if exist *.zip (
	echo =============================================================
	echo Checking for existing/older build zip files
	echo =============================================================
	dir *.zip
	set /P c1=Delete ALL of these existing build zip files? [Y/N]
	if /I "%c1%" EQU "Y" goto :delzips
	if /I "%c1%" EQU "N" goto :startbuild

	:delzips
	del *.zip
	echo Zip files cleared!
)

:startbuild
echo =============================================================
echo Building AllDeployable solution
echo =============================================================

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

echo =============================================================
echo Building GenValAppRunner
echo =============================================================

cd src\common\src\NIST.CVP.Generation.GenValApp
dotnet publish -c Release -r win-x64

cd %homedir%\src\common\src\NIST.CVP.Generation.GenValApp\bin\Release\netcoreapp2.1\win-x64\
"%zip%" a -tzip publish.zip publish\
move publish.zip "%homedir%\%mydate%_%mytime%_GenValsOrleans.zip"

cd %homedir%

rem Orleans build
echo =============================================================
echo Building OrleansServer
echo =============================================================
cd src\orleans\src\NIST.CVP.Orleans.ServerHost
dotnet publish -c Release -r win-x64

rem cd ..\..\orleans\src\NIST.CVP.Orleans.ServerHost\bin\Release\netcoreapp2.1\win-x64\
cd bin\Release\netcoreapp2.1\win-x64\
"%zip%" a -tzip publish.zip publish\
move publish.zip "%homedir%\%mydate%_%mytime%_OrleansServer.zip"

cd %homedir%

rem Pool API build
echo =============================================================
echo Building PoolAPI
echo =============================================================

cd src\pool-api\NIST.CVP.PoolAPI
dotnet publish -c Release -r win-x64

cd bin\Release\netcoreapp2.1\win-x64\
"%zip%" a -tzip publish.zip publish\
move publish.zip "%homedir%\%mydate%_%mytime%_PoolService.zip"

cd %homedir%
cls

dir *.zip

rem Move builds to Latest_Builds on elwood (drive already mapped as X:)
:choicemovefiles
echo =============================================================
set /P c=Do you want to move the above build files up to elwood [Y/N]?
echo =============================================================
if /I "%c%" EQU "Y" goto :movefilestoelwood
if /I "%c%" EQU "N" goto :aftermovefiles

:movefilestoelwood
echo Moving files...
move /Y *.zip X:\acvp\Latest_Builds\
goto :aftermovefiles

:aftermovefiles
echo =============================================================
echo Build %mydate%_%mytime% complete! Exiting.
echo =============================================================