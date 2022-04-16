dotnet pack Shared.Domain --configuration Release
cd packages
cd Domain
dotnet nuget push *.nupkg --source \\local\packages