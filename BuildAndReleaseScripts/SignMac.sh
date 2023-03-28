#!/bin/sh
# This file is adopted from https://github.com/coding-flamingo/CodeSignConsoleApp/blob/main/BuildAndReleaseScripts/SignMac.sh
# and from https://www.kenmuse.com/blog/notarizing-dotnet-console-apps-for-macos/ 
## usage /bin/sh BuildAndReleaseScripts/SignMac.sh "markdown-linked-images-mover/MarkdownLinkedImagesMover" "markdown-linked-images-mover" "Developer ID Application: Stefan Boos" "BuildAndReleaseScripts/entitlements.plist"
## RunFile $1 "markdown-linked-images-mover/MarkdownLinkedImagesMover"
## Directory $2 "markdown-linked-images-mover"
## CertName $3 "Developer ID Application: YOURORG (YOURDEVID)" 
## Entitlements $4 "BuildAndReleaseScripts/entitlements.plist"

echo "======== INPUTS ========"
echo "RunFile: $1"
echo "Directory: $2"
echo "CertName: $3"
echo "Entitlements: $4"
echo "======== END INPUTS ========"

for f in $1 "$2"/createdump "$2"/*{.dll,.dylib}
do
  echo "Runtime Signing $f"
  codesign --force --verbose --timestamp --sign "$3" "$f" --options=runtime --no-strict --entitlements "$4"
done
