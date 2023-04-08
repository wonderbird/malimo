$ErrorActionPreference = 'Stop'
$toolsDir   = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$url64      = 'https://github.com/wonderbird/malimo/releases/download/v0.1.5-alpha/malimo.win-x64.zip'

$packageArgs = @{
  packageName   = $env:ChocolateyPackageName
  unzipLocation = $toolsDir
  url64bit      = $url64

  softwareName  = 'malimo*'

  checksum64    = '6e852a984e4118fb30943d54f3dfda52040b8aa8174bb6df9ceac1e4257d0fbf'
  checksumType64= 'sha256'
}

Install-ChocolateyZipPackage @packageArgs
