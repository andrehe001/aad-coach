#!/usr/bin/env bash
set -o errexit
set -o pipefail
set -o nounset

# ./import-teams.sh $TEAMFILE $APIURL $ADMIN_USERNAME $ADMIN_PASSWORT

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
while read teamname subscriptionid tenantid teampassword comment
do
    echo "importing $teamname with $teampassword to $tenantid in $subscriptionid..."
    TEAMID=$(curl -sL --header "Content-Type: application/json" \
      --header "Authorization: Bearer $TOKEN" \
      --request POST \
      --data "{'name': '$teamname', 'tenantid': '$tenantid', 'subscriptionId': '$subscriptionid', 'teampassword': '$teampassword'}" \
      $url/api/team/new | jq '.id' -r)
    
    echo "new teamid for $teamname is $TEAMID"

    if [ $TEAMID -eq 0 ]; then
      echo "failed to import $teamname"
    else
      echo "$teamname successfully imported with id $TEAMID"
    fi
    
done < $file
IFS=$OLDIFS