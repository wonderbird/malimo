#!/bin/bash
# This file is adopted from https://github.com/coding-flamingo/CodeSignConsoleApp/blob/main/BuildAndReleaseScripts/SignMac.sh
## usage sh sign.sh "./consoleapp/ConsoleApp1" "./consoleapp/*" "Developer ID Application: YOURORG (YOURDEVID)" "./entitlements.plist"
## RunFile $1 "./consoleapp/ConsoleApp1"
## Directory $2 ./consoleapp/*
## CertName $3 "Developer ID Application: YOURORG (YOURDEVID)" 
## Entitlements $4 ./entitlements.plist 

echo "======== INPUTS ========"
echo "RunFile: $1"
echo "Directory: $2"
echo "CertName: $3"
echo "Entitlements: $4"
echo "======== END INPUTS ========"

for f in $2
do 
    if [ "$f" = "$1" ]; 
    then 
        echo "Runtime Signing $f"
        # Adopted from https://www.kenmuse.com/blog/notarizing-dotnet-console-apps-for-macos/ 
        codesign --force --verbose --timestamp --sign "$3" "$f" --options=runtime --no-strict --entitlements "$4"
    else 
        echo "NOT Signing $f" 
        # Adopted from https://www.kenmuse.com/blog/notarizing-dotnet-console-apps-for-macos/
        # Not needed?
        #codesign --force --verbose --timestamp --sign "$3" "$f" --no-strict 
    fi
done

