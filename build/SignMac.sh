#!/bin/sh
# This file is adopted from https://github.com/coding-flamingo/CodeSignConsoleApp/blob/main/BuildAndReleaseScripts/SignMac.sh
# and from https://www.kenmuse.com/blog/notarizing-dotnet-console-apps-for-macos/ 
#
# usage /bin/sh ./build/SignMac.sh "markdown-linked-images-mover/MarkdownLinkedImagesMover" "Developer ID Application: Stefan Boos" "./build/entitlements.plist"
#
# RunFile      $1 "markdown-linked-images-mover/MarkdownLinkedImagesMover"
# CertName     $2 "Developer ID Application: YOURORG (YOURDEVID)" 
# Entitlements $3 "./build/entitlements.plist"

echo "======== INPUTS ========"
echo "RunFile: $1"
echo "CertName: $2"
echo "Entitlements: $3"
echo "======== END INPUTS ========"

echo "Runtime Signing $1"
codesign --force --verbose --timestamp --sign "$2" "$1" --options=runtime --no-strict --entitlements "$3"
