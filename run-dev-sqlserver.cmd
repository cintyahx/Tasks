docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD =2Secure*Password2" -p 5533:1433 --name TasksDB -d mcr.microsoft.com/mssql/server:2019-latest
