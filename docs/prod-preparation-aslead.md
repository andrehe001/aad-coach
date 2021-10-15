# Preparing a delivery and acting as Lead Coach

## Prerequisites / 1-2 weeks before

### Coaches

* a list of possible coaches can be found at [AzureAdventureDay_Coaches.xlsx](https://microsofteur.sharepoint.com/:x:/t/AzureAdventureDay/EUggwWr3VcpLhL_9mVg4bFABJ6CyCHGN3SAdTc2WYMbidA?e=sccR3Z)
* there you can also find a list of already assigned coaches to delivery dates - ensure that you have enough coaches for the upcoming delivery (1 coach = 2 teams = 3-6 attendees per team)
* send meeting invites to each coach for
  * Azure Adventure Day blocker, 8:30 - 17:00
  * Teams Call: Internal Last Prep Session, 8:30 - 9:00
  * Teams Call: Internal Debriefing, 16:30 - 17:00

### Azure Subscriptions & Azure Accounts

* all Azure Subscriptions (1x Admin, 1x per team) and accounts must already be created and correctly configured before starting the deployment
* please use [AzureAdventureDay_AzurePreparation_Template.xlsx](/AdventureDayBackend/team-import/AzureAdventureDay_AzurePreparation_Template.xlsx) and follow all of the instructions in it

**Attention: We have the possibility to let this be done by a partner / at German public deliveries we are doing exactly this.**

* Test the provided subscriptions with spot checks
  1. Logging in as admin, ensure you can access all subscriptions
  2. Logging in as one team member, ensure you can access your subscription but only it and have only contributor rights
  3. Logging in as the Admin Service Principal, ensure you can access all subscriptions
  4. Logging in as one of the team Service Principals, ensure you can access only your team subscription and have owner rights

#### File share (German public deliveries)

* Please save the file into a delivery folder like "2021.03.10 - Public" at [Planning, Deliveries, Registrations](https://microsofteur.sharepoint.com/:f:/r/teams/AzureAdventureDay/Shared%20Documents/Planning,%20Deliveries,%20Registrations?csf=1&web=1&e=41azRT)


### Microsoft Teams

* a Microsoft Teams *Team* for each individual delivery needs to be created and named accordingly, e.g. "Azure Adventure Day 10.03.2021"
* the following public channels need to be created: *General*, *AskCoaches* and *Team 01* - *Team 12*
* the following private channel needs to created: *CoachesInternal*
* all attendees are added to the *Teams*
* all assigned coaches for this delivery are added to the *Teams* **and** to the private *CoachesInternal* channel

After everything has been properly prepared, post it in the *Azure Adventure Day/Coaches* channel with a list of Coaches and a link to the Teams environment.

## Deployment / 1 week before

### Setting up the Adventure Day PROD Backend

Use the *Admin Subscription* from *Prerequisites* above to [set up the Adventure Day PROD Backend with the provided instructions](/docs/prod-deployment.md).

**Attention: The admin account is currently hard coded to admin:AdminPassword!**

### Import Teams, Azure Subscriptions and Accounts

Using the *AzureAdventureDay_AzurePreparation_Template.xlsx* from *Prerequisites* above the next step is to import Teams, Azure Subscriptions and Accounts into the deployed environment from the step before.

Please follow the provided [Import Teams, Azure Subscriptions and Accounts instructions](/AdventureDayBackend/team-import/ReadMe.md).

After everything has been properly prepared, post it in the *CoachesInternal* with the Environment URL.

### Team Invitation Messages

Prepare individual *Invitation Messages* per team, which will be shared after the General session in the morning into all individual team channels.

Copy them also into the *AzureAdventureDay_AzurePreparation_Template.xlsx* file.

#### Template

```
Hi Team1,

the Team Portal can be found at http://azure-adventure-day-prod-412f86.northeurope.cloudapp.azure.com/.
Please use "Team1" as Username and "nASAtiOn" as Password.

As teamname=username, if you change it, please make a note of the new one :)

And now - have fun and let's play against the Smoorghs!
```

### Prepare the slide deck

All slide decks can be found at [Azure Adventure Day - Coaches](https://microsofteur.sharepoint.com/:f:/r/teams/AzureAdventureDay526-Coaches/Shared%20Documents/Coaches?csf=1&web=1&e=JML7QI).

Ensure that all coaches for this delivery are listed with pictures at *Azure Adventure Day - Attendee presentation.pptx*.

## Be the Lead Coach / during the day

### Timetable

Internal one can be found at *Azure Adventure Day - CoachDeck.pptx* - ensure to stick to it.

### Internal Last Prep Session, 8:30 - 9:00

Before the event starts a last internal call will be held by you to discuss any open questions and ensure every coach is ready.

### Introduction to Cybercity, 9:00 - 9:40

You are opening the event and giving the *Introduction to Cybercity*.

After done this, a 10-15min break wil be held, to assign attendees to teams and coaches to teams. The orga team will help you doing this. Especially at online deliveries it can be challenging and somebody else needs to look during your introduction at the attendee list and makes notes about who is there, who not.

You are publishing the assignment inside the *General* channel and presenting it also after the break to all attendees.

You are also sending the *Team Invitation Messages* to each individual team channel before resuming the call with the attendees.

### During the day / Phases

You are responsible for starting, stopping and changing the phases, including all other general sessions, like Phase Introductions, Solution Discussion and Closing & Award Ceremony.

Please also ensure that you and every coach and attendee is on time at the phase introductions and you

* stop the current phase at the beginning fo each phase introduction
* change to the next phase
* start this next phase at the  beginning of each phase

### During the day / Phase Winners

You are responsible for getting the information from all coaches, when their assigned teams have finished a phase.

Inside the *Azure Adventure Day - CoachDeck.pptx* you can find the requirements for each phase, what needs to be fulfilled to have the phase won.

Update the *Azure Adventure Day - Attendee presentation.pptx* accordingly with the Phase Winner team names and the total Winner (most score).

### Internal Debriefing, 16:30 - 17:00

You are ending the call together with all coaches in an internal debriefing. Let's use this for discussing any ideas / feedback from the day and try to find out, if something new / valuable feedback has come up, which can improve Azure ADventure Day and all future deliveries.

## THANK YOU

**Thank you for running this event as lead coach and thus offering our customers another training opportunity.**
