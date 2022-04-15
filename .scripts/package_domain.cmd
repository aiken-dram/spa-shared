cd shared.domain
dotnet build
cd ..
cd Packages
cd Domain
dotnet nuget push *.nupkg --source \\local\packages

cd ..
cd ..

cd shared.application
dotnet add package SPA.Shared.Domain
dotnet build
cd ..
cd Packages
dotnet nuget push *.nupkg --source \\local\packages