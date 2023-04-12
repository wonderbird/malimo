$ErrorActionPreference = 'Stop'
$toolsDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$version = 'Will be replaced by the CI pipeline'
$hash = 'Will be replaced by the CI pipeline'
$url64 = "https://github.com/wonderbird/malimo/releases/download/v$version/malimo.win-x64.zip"

$packageArgs = @{
  packageName   = $env:ChocolateyPackageName
  unzipLocation = $toolsDir
  url64bit      = $url64

  softwareName  = 'malimo*'

  checksum64    = $hash
  checksumType64= 'sha256'
}

Install-ChocolateyZipPackage @packageArgs
