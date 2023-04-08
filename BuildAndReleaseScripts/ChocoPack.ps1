# Adopted from
# - https://github.com/Ellerbach/docfx-companion-tools/blob/main/pack.ps1
# - https://github.com/Ellerbach/docfx-companion-tools/blob/main/tools/common.ps1
param(
    [string] $version = '0.1.5-alpha'
)

$nuspecPath = "Chocolatey/malimo.nuspec"

Write-Host "Update $nuspecPath to version $version"
$content = Get-Content $nuspecPath -Encoding UTF8 -Raw
$content = [Regex]::Replace($content, '(<version>.+<\/version>)', "<version>$version</version>")
$content | Set-Content $nuspecPath -Force -Encoding UTF8

Write-Host "Create Chocolatey package"
choco pack $nuspecPath
