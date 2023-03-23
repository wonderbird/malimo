#!/bin/zsh
# This file is adopted from https://github.com/coding-flamingo/CodeSignConsoleApp/blob/main/BuildAndReleaseScripts/Notarize.sh
## usage sh Notarize.sh "devemail" "Password1." "group.com.company" "DEVID" "./filename.zip"
## dev_account $1 "devemail"
## dev_Password $2 "Password1."
## dev_team $3 DEVID
## FileName $4 ./filename.zip

responseJson=$(xcrun notarytool submit "$4" --wait --apple-id "$1" --password "$2" --team-id "$3" --output-format json)
status=$(echo "$responseJson" | jq '.status')
id=$(echo "$responseJson" | jq --raw-output '.id')

xcrun notarytool log "$id" --apple-id "$1" --password "$2" notarize_log.json

echo "====="
echo "Notarize status: $status"
