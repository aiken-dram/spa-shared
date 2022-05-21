cd shared.application
dotnet add package FluentValidation
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Newtonsoft.Json
cd ..

cd shared.infrastructure
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Relational
dotnet add package NPOI
cd ..
