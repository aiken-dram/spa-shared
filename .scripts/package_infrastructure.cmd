dotnet pack Shared.Infrastructure --configuration Release -p:NuspecFile=Shared.Infrastructure.nuspec
cd Packages
cd Infrastructure
dotnet nuget push *.nupkg --source \\local\packages