To set up a database on docker use:
    docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=<password>' -e 'MSSQL_PID=Express' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-CU1-ubuntu-16.04 --name acvplocal
    
Run the instance
    docker start acvplocal
    
Grab a back-up of the database (.bak) and do a restore via Azure Data Studio or some other SQL Client

Change `appsettings.local.json` with your specific password.
