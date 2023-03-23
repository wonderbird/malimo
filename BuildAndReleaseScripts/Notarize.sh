#!/bin/zsh
# This file is adopted from https://github.com/coding-flamingo/CodeSignConsoleApp/blob/main/BuildAndReleaseScripts/Notarize.sh
## usage sh Notarize.sh "devemail" "Password1." "group.com.company" "DEVID" "./filename.zip"
## dev_account $1 "devemail"
## dev_Password $2 "Password1."
## GroupID $3 "group.com.company" 
## dev_team $4 DEVID
## FileName $5 ./filename.zip

responseJson=$(xcrun notarytool submit "$5" --wait --apple-id "$1" --password "$2" --team-id "$4" --output-format json)
status=$(echo "$responseJson" | jq '.status')
id=$(echo "$responseJson" | jq '.id')

xcrun notarytool log "$id" --apple-id "$1" --password "$2" notarize_log.json

echo "====="
echo "Notarize status: $status"
