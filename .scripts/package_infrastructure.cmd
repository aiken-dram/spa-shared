cd shared.infrastructure
dotnet build
cd ..
cd Packages
cd infrastructure
dotnet nuget push *.nupkg --source \\local\packages