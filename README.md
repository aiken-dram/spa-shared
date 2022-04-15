# backend-shared

Common library for dotnet core solutions

## Adding to project as nuget package

```sh
dotnet add package SPA.Shared.Domain
dotnet add package SPA.Shared.Application
dotnet add package SPA.Shared.Infrastructure
```

## Local nuget source

Check if local nuget source already exists:

```sh
dotnet nuget list source
```

Create local nuget source:

```sh
dotnet nuget add source c:\work\_nuget --name \\local\packages
```

Push nuget package into local source:

```sh
dotnet nuget push SPA.Shared.Domain.nupkg --source \\local\packages
```
