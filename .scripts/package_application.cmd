dotnet pack Shared.Application --configuration Release -p:NuspecFile=Shared.Application.nuspec
cd Packages
cd Application
dotnet nuget push *.nupkg --source \\local\packages