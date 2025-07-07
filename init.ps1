# Create solution
dotnet new sln -n CloudNotesApp

# Create individual projects
dotnet new blazorwasm -n CloudNotes.Client
dotnet new webapi -n CloudNotes.Server
dotnet new classlib -n CloudNotes.Shared

# Add projects to solution
dotnet sln add CloudNotes.Client/CloudNotes.Client.csproj
dotnet sln add CloudNotes.Server/CloudNotes.Server.csproj
dotnet sln add CloudNotes.Shared/CloudNotes.Shared.csproj

# Reference Shared project in both Client and Server
dotnet add CloudNotes.Client/CloudNotes.Client.csproj reference CloudNotes.Shared/CloudNotes.Shared.csproj
dotnet add CloudNotes.Server/CloudNotes.Server.csproj reference CloudNotes.Shared/CloudNotes.Shared.csproj
