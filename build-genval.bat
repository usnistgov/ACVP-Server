set "zip=c:\Program Files\7-Zip\7z.exe"

for /F "tokens=2" %%i in ('date /t') do set mydate=%%i
set mydate=%mydate:/=-%
set mytime=%time::=-%
set mytime=%mytime:.=-%

set git-branch="git branch | find "* " "
for /F "tokens=*" %%i in (' %git-branch% ') do set branch=%%i
set branch=%branch:* =%

cd src\solutions\AllDeployable
dotnet clean

cd ..\GenValAppRunner
dotnet clean

cd ..\AllDeployable
dotnet build -c Release

cd ..\GenValAppRunner
dotnet build -c Release 
dotnet publish -c Release

cd ..\..\common\src\NIST.CVP.Generation.GenValApp\bin\Release\netcoreapp2.1
"%zip%" a -tzip publish.zip publish\
mv publish.zip ..\..\..\..\..\..\..\gen-val-%branch%-%mydate%-%mytime%.zip

cd ..\..\..\..\..\..\..\
