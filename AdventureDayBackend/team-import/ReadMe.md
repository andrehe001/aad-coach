# Team import from csv file

## Team import
A team can be imported from csv file by running the `import-teams.sh` script with the following parameters:

```
chmod +x ./import-teams.sh
TEAMFILE=`pwd`/teams_real.csv
APIURL=http://azure-adventure-day-prod-6d0b53.northeurope.cloudapp.azure.com
ADMIN_USERNAME=admin
ADMIN_PASSWORT=AdminPassword
./import-teams.sh $TEAMFILE $APIURL $ADMIN_USERNAME $ADMIN_PASSWORT
```

Structure of the csv file

```
teamname;tenantid;subscriptionid;teampassword;gameengineuri
```

## Team member import

A list of team members can be imported from csv file by running the `import-members.sh` script with the following parameters:

```
chmod +x ./import-members.sh
MEMBERFILE=`pwd`/teams-members_real.csv
APIURL=http://azure-adventure-day-prod-6d0b53.northeurope.cloudapp.azure.com
ADMIN_USERNAME=admin
ADMIN_PASSWORT=AdminPassword
./import-members.sh $MEMBERFILE $APIURL $ADMIN_USERNAME $ADMIN_PASSWORT
```

Structure of the csv file

```
teamname;username;password
```

## Activating Security center 

The security center activation is using the `activate-security-center.sh` script with the following parameters:

```
chmod +x ./activate-security-center.sh
TEAMFILE=`pwd`/teams_real.csv
ADMINUSERNAME=Coach01@asmw13.onmicrosoft.com
ADMINPASSWORD=D9HbhcAx6!
./activate-security-center.sh $TEAMFILE $ADMINUSERNAME $ADMINPASSWORD
```

Structure of the csv file

```
teamname;tenantid;subscriptionid;teampassword;gameengineuri
```