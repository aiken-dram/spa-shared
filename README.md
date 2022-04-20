# backend-shared

Common library for backend dotnet core solution in [SPA template](https://github.com/aiken-dram/spa-template/) projects

## Building packages

Project uses environmental variable `SPA_SHARED_BUILD` for build version

VS Code tasks.json has two tasks:

- `build packages` - increments `SPA_SHARED_BUILD` machive variable by 1, then builds all packages with updated build number and pushes all packages from `/packages/..` directory to local storage `//local/packages`

- `reset build version` - resets `SPA_SHARED_BUILD` machine variable to 1, use this task after updating `.csproj` with new major version

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
