

MANAGEMENT_IP=localhost:5001

curl -X GET http://$MANAGEMENT_IP/ping

curl --header "Content-Type: application/json" \
  --request POST \
  --data '{"teamname":"xyz","password":"xyz"}' \
  http://$MANAGEMENT_IP/api/team/authenticateadmin


GET api/team/members/1337
only if you are admin or part of team 1337

GET api/team/all
admins get all teams
users get only their own team

POST api/team/new
--data '{"name":"admin1", "subscriptionId": "5abd8123-18f8-427f-a4ae-30bfb82617e5", "password":"AdminPassword"}'
only if you are admin

POST api/team/rename/2/newName43
only if you are admin or part of team 2

POST api/team/renameMember/{memberId}/{newName}

POST api/team/addmemberto/{teamId}
--data '{"displayname":"Dennis", "username": "dzielke", "password":"AdminPassword"}'


POST api/team/removememberfrom/{teamId}/{memberId}

POST api/team/delete/2

GET api/team/all 
all teams without member objects that you have access to

GET api/team/allwithmembers
all teams with member objects that you have access to

GET api/team/byid/2 
get team 2 without members if you access to it or are a member

GET api/team/byidwithmembers/2
get team 2 with members if you access to it or are a member
