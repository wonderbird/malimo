# Produce Chocolatey .nupkg with substituted version and hash.
#
# The version number is written to both the Chocolatey .nuspec file and to the install script.
# In addition the hash of the release zip is written to the install script.
#
# This script was adopted from Martin Tirion: Publish a .NET Console App to Chocolatey using GitHub Actions
#
# - https://mtirion.medium.com/publish-a-net-console-app-to-chocolatey-using-github-actions-29eaa60a8668
# - https://github.com/Ellerbach/docfx-companion-tools/blob/main/pack.ps1
# - https://github.com/Ellerbach/docfx-companion-tools/blob/main/tools/common.ps1
#
param(
    [Parameter(Mandatory, HelpMessage="Release version number, e.g. 0.1.0-alpha")]
    [string] $version,
    
    [Parameter(Mandatory, HelpMessage="Relative path to the ZIP referred to by the Chocolatey package")]
    [string] $zipFile,
    
    [Parameter(Mandatory, HelpMessage="Relative path to the output directory for the Chocolatey .nupkg package")]
    [string] $outputDirectory
)

$ErrorActionPreference = 'stop'

Write-Host "Update $nuspecPath to version $version"

$nuspecPath = "Chocolatey\malimo.nuspec"
$content = Get-Content $nuspecPath -Encoding UTF8 -Raw
$content = [Regex]::Replace($content, '(<version>.+<\/version>)', "<version>$version</version>")
$content | Set-Content $nuspecPath -Force -Encoding UTF8


Write-Host "Update $installScriptPath to version $version with hash $hash"

$installScriptPath = "Chocolatey\tools\chocolateyinstall.ps1"
$hash = (Get-FileHash -Algorithm SHA256 -Path $zipFile).Hash.ToUpper()
$content = Get-Content $installScriptPath -Encoding UTF8 -Raw
$content = [Regex]::Replace($content, "(\`$version\s+=\s+'.+')", "`$version = '$version'")
$content = [Regex]::Replace($content, "(\`$hash\s+=\s+'.+')", "`$hash = '$hash'")
$content | Set-Content $installScriptPath -Force -Encoding UTF8

Write-Host "Create Chocolatey package"

choco pack --outputdirectory $outputDirectory $nuspecPath
