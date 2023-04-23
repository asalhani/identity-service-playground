```
███████╗██╗     ███╗   ███╗
██╔════╝██║     ████╗ ████║
█████╗  ██║     ██╔████╔██║
██╔══╝  ██║     ██║╚██╔╝██║
███████╗███████╗██║ ╚═╝ ██║
╚══════╝╚══════╝╚═╝     ╚═╝
```
# Identity Server

# Add migration commands:
Make sure first the connection string is pointing to the right intance of SQL Server
Delete the folders in `Migrations` folder and keep the ApplicationDbContext.cs
```
dotnet ef migrations add Configuration -c ConfigurationDbContext -o "Migrations/ConfigurationDb" -v
dotnet ef migrations add PersistedGrant -c PersistedGrantDbContext -o "Migrations/PersistedGrantDb" -v
dotnet ef migrations add Application -c ApplicationDbContext -o "Migrations/ApplicationDb" -v
```