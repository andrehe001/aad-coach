# Team import from csv file

## Team import
A team can be imported from csv file by running the `import-teams.sh` script with the following parameters:

```
TEAMFILE=`pwd`/teams.csv
APIURL=http://localhost:8000
ADMIN_USERNAME=admin
ADMIN_PASSWORT=AdminPassword
./import-teams.sh $TEAMFILE $APIURL $ADMIN_USERNAME $ADMIN_PASSWORT
```

Structure of the csv file

```
teamname,tenantid,subscriptionid,teampassword,gameengineuri
```

## Team member import

A list of team members can be imported from csv file by running the `import-members.sh` script with the following parameters:

```
TEAMFILE=`pwd`/team-members.csv
APIURL=http://localhost:8000
ADMIN_USERNAME=admin
ADMIN_PASSWORT=AdminPassword
./import-members.sh $TEAMFILE $APIURL $ADMIN_USERNAME $ADMIN_PASSWORT
```

Structure of the csv file

```
teamname,username,password
```