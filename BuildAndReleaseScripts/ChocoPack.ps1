# Adopted from
# - https://github.com/Ellerbach/docfx-companion-tools/blob/main/pack.ps1
# - https://github.com/Ellerbach/docfx-companion-tools/blob/main/tools/common.ps1
param(
    [string] $version,
    [string] $hash
)

$ErrorActionPreference = 'stop'

$nuspecPath = "Chocolate\malimo.nuspec"

Write-Host "Update $nuspecPath to version $version"
$content = Get-Content $nuspecPath -Encoding UTF8 -Raw
$content = [Regex]::Replace($content, '(<version>.+<\/version>)', "<version>$version</version>")
$content | Set-Content $nuspecPath -Force -Encoding UTF8

$installScriptPath = "Chocolatey\tools\chocolateyinstall.ps1"

Write-Host "Update $installScriptPath to version $version with hash $hash"
$content = Get-Content $installScriptPath -Encoding UTF8 -Raw
$content = [Regex]::Replace($content, "(\`$version\s+=\s+'.+')", "`$version = '$version'")
$content = [Regex]::Replace($content, "(\`$hash\s+=\s+'.+')", "`$hash = '$hash'")
$content | Set-Content $installScriptPath -Force -Encoding UTF8

Write-Host "Create Chocolatey package"
choco pack $nuspecPath
