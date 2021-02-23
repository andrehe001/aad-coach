# Team import from csv file

## Generate team passwords
http://codething.ru/passgen/?lang=en

## Team import
A team can be imported from csv file by running the `import-teams.sh` script with the following parameters:

```
chmod +x ./import-teams.sh
TEAMFILE=`pwd`/teams_real.csv
APIURL=http://test2-adventure-day-prod-e6514c.northeurope.cloudapp.azure.com
ADMIN_USERNAME=admin
ADMIN_PASSWORT=AdminPassword!
./import-teams.sh $TEAMFILE $APIURL $ADMIN_USERNAME $ADMIN_PASSWORT
```

Structure of the csv file

```
teamname;subscriptionid;tenantid;teampassword;comment
```

## Team member import

A list of team members can be imported from csv file by running the `import-members.sh` script with the following parameters:

```
chmod +x ./import-members.sh
MEMBERFILE=`pwd`/teams-members_real.csv
APIURL=http://test2-adventure-day-prod-e6514c.northeurope.cloudapp.azure.com
ADMIN_USERNAME=admin
ADMIN_PASSWORT=AdminPassword!
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
ADMIN_SERVICEPRINCIPAL_ID=admin
ADMIN_SERVICEPRINCIPAL_SECRET=AdminPassword!
./activate-security-center.sh $TEAMFILE $ADMIN_SERVICEPRINCIPAL_ID $ADMIN_SERVICEPRINCIPAL_SECRET
```

Structure of the csv file

```
teamname;subscriptionid;tenantid;teampassword;comment
```