#Game store API

## Starting SQL Server

```powershell
$sa_possword = "[SA PASSWORD HERE]"
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=$sa_possword" -p 1433:1433 -v sqlvolume:/var/opt/mssql -d --rm --name mssql --platform linux/amd64 mcr.microsoft.com/mssql/server:2022-latest
```