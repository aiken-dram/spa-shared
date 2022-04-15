cd shared.application
dotnet build
cd ..
cd Packages
cd Application
dotnet nuget push *.nupkg --source \\local\packages