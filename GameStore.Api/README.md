#Game store API

## Starting SQL Server


in the environment set the password by running:
export sa_password="my_password_goes_here"

```powershell
$sa_password = "[SA PASSWORD HERE]"
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=$sa_password" -p 1433:1433 -v sqlvolume:/var/opt/mssql -d --rm --name mssql --platform linux/amd64 mcr.microsoft.com/mssql/server:2022-latest
```

## Setting the connection string to secret manager
```powershell
$sa_password = "[SA PASSWORD HERE]"
dotnet user-secrets set "ConnectionStrings:GameStoreContext" "Server=localhost; Database=GameStore; User Id=sa; Password=$sa_password; TrustServerCertificate=True"
```

# To check if the password has been stored in dotnet user-secres, then:
```powershell
dotnet user-secrets list
```

## Add a Entity Framework into project
```powershell
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```


## Update migrations
```powershell
dotnet ef database update
```

## Generate access token for local development
```powershell
dotnet user-jwts create
dotnet user-jwts print {token-id}

dotnet user-jwts create --role "Admin"
dotnet user-jwts create --scope "games:read"
dotnet user-jwts create --role "Admin" --scop "games:read"
```

## JWT payload
Field | Sample Values | Description
iss | dotnet-user-jwts https://myautherserver.com | Issuer : Who created and signed the token
aud | http://localhost:5124 https//gamestore | Audience: Who the token is intended for
jti | 128b95ab | The unique identifier of the JWT
sub | julio auth0 64141af968011b.. | Subject. THe principal that is the subject of the JWT. The Id of the signed in user
scope | games: read, games:write, openid, profile | The type of access granted to the client
nbf | 161150973311 | not valid before. THe time before which the JWT must not be accepted for porcessing.
exp | 1615100911 | Exporation time after which the JWT must not be accepted for processing
iat | 1617140901 | The time at which the JWT was issued