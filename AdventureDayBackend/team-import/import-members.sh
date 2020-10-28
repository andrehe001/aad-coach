#!/usr/bin/env bash
set -o errexit
set -o pipefail
set -o nounset

# ./import-teams.sh csvfile.csv apirul

export file="$1"
export url="$2"
export adminusername="$3"
export adminpassword="$4"

echo "creating admin auth token"
echo "posting $url/api/team/login"

TOKEN=$(curl -sL --header "Content-Type: application/json" \
  --request POST \
  --data "{'teamname':'$adminusername','password':'$adminpassword'}" \
  $url/api/team/login | jq '.token' -r)

echo "received auth token $TOKEN"

OLDIFS=$IFS
IFS=';'
[ ! -f $file ] && { echo "$file file not found"; exit 99; }
while read teamname username password
do
    echo "importing $username to $teamname with $password..."

    MEMBERID=$(curl -sL --header "Content-Type: application/json" \
      --header "Authorization: Bearer $TOKEN" \
      --request POST \
      --data "{'username': '$username', 'password': '$password'}" \
      $url/api/team/addmembertoteamname/$teamname | jq '.id' -r)
    
    echo "new memberid for $teamname is $MEMBERID"

    if [ $MEMBERID -eq 0 ]; then
      echo "failed to import $member $username"
    else
      echo "$username successfully imported with id $MEMBERID"
    fi
    
done < $file
IFS=$OLDIFS