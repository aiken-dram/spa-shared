#read session env variable from machine
$ver = $env:SPA_SHARED_BUILD
$ver = [Environment]::GetEnvironmentVariable('SPA_SHARED_BUILD', 'User')
[int]$ver = [int]$ver + 1
[System.Environment]::SetEnvironmentVariable('SPA_SHARED_BUILD', $ver, 'User')
Write-Output "Build version incremented to:"
Write-Output $ver
$env:SPA_SHARED_BUILD = $ver

#build and pack
dotnet pack Shared.Domain --configuration Release
dotnet pack Shared.Application --configuration Release
dotnet pack Shared.Infrastructure --configuration Release

#push packages to local source
Set-Location Packages
Set-Location Domain
dotnet nuget push *.nupkg --source \\local\packages
Set-Location ..
Set-Location Application
dotnet nuget push *.nupkg --source \\local\packages
Set-Location ..
Set-Location Infrastructure
dotnet nuget push *.nupkg --source \\local\packages