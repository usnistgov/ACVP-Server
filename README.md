To set up a database on docker use:
```
docker run --name acvplocal -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=<password>' -e 'MSSQL_PID=Express' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-CU1-ubuntu-16.04 
```

Run the instance
```
docker start acvplocal
```

Grab a back-up of the database (.bak) and do a restore via Azure Data Studio or some other SQL Client

Copy it into the docker container with this

```
docker cp database_file.bak acvplocal:/var/opt/mssql/data/
```

You may have to chmod the backup file to allow your SQL account to use the file you place into docker. That would look something like this

```
docker exec --user root acvplocal chmod +rx /var/opt/mssql/data/database_file.bak
```

Change `appsettings.local.json` with your specific password.

Having trouble w/ docker on windows? if `docker pull hello-world` is returning "error during connect", the daemon may not be running properly, this command for some reason fixes it:

```cmd
MOFCOMP %SYSTEMROOT%\System32\WindowsVirtualization.V2.mof
```

Sym Links
Symbolic links are used to mirror the `Directory.build.props` file from `/_config` to -> `/`.  It's contained with `/config` for team city build purposes, but needs to be at `/` for local purposes.

The following bash commands from the project root will create the needed sym links:

```
rm Directory.Build.props
ln -s ./_config/Directory.Build.props
```