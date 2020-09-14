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
IFS=','
[ ! -f $file ] && { echo "$file file not found"; exit 99; }
while read teamname region subscriptionid teampassword gameengineuri
do
    echo "importing $teamname with $teampassword to $region in $subscriptionid for $gameengineuri..."

    TEAMID=$(curl -sL --header "Content-Type: application/json" \
      --header "Authorization: Bearer $TOKEN" \
      --request POST \
      --data "{'name': '$teamname', 'subscriptionId': '$subscriptionid', 'teampassword': '$teampassword', 'gameengineuri': '$gameengineuri'}" \
      $url/api/team/new | jq '.id' -r)
    
    echo "new teamid for $teamname is $TEAMID"

    if [ $TEAMID -eq 0 ]; then
      echo "failed to import $teamname"
    else
      echo "$teamname successfully imported with id $TEAMID"
    fi
    
done < $file
IFS=$OLDIFS