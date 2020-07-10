#!/usr/bin/env bash
set -o errexit
set -o pipefail
set -o nounset

# ./deploy-teams.sh csvfile.csv terraformpath

export INPUT_FILE="$1"
export TERRAFORM_PATH="$2"

OLDIFS=$IFS
IFS=','
[ ! -f $INPUT_FILE ] && { echo "$INPUT_FILE file not found"; exit 99; }
while read teamname region subscription
do
    echo "deploying $teamname to $region in $subscription..."
    ./deploy-team.sh $teamname $TERRAFORM_PATH $region $subscription
    echo "deployment completed"
done < $INPUT_FILE
IFS=$OLDIFS