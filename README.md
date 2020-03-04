# azure-game-day

## Brainstorming
* Agenda:
  * 9:00am - 9:30am Registration/Game Rules (30 Min)
  * 9:30am - 11:30am Game Play (120 Min)
  * 11:30am - 12:30am Lunch (60 Min)
  * 12:30am - 2:30pm Game Play (120 Min)
  * 2:30pm - 3:00pm Closing and Award Ceremony (30 Min)
* Participants:
  * min. 12 (3 teams a 4)
  * max. 48 (12 teams a 4)
* Azure Subscriptions: Lab Service (Dennis knows more about it)
* Prerequisite: [AZ-900 Microsoft Azure Fundamentals](https://docs.microsoft.com/en-us/learn/certifications/exams/az-900) or [Azure Fundamentals Online Learning Path](https://docs.microsoft.com/en-us/learn/paths/azure-fundamentals/)

### Phases:
#### 1. Phase: Deployment
* Narrative: It was all deleted after the most important team member left the team - it was his last fight -.-
* Tech: Terraform templates are already there, just needs to be executed

#### 2. Phase: Scaling
* Narrative: A bunch of tweets are hitting up right after going online again - alle your fans in the world are trying to access the side again
* Tech: Scale pod and node count automatically

#### 3. Phase: Change
* Narrative: Multiple support tickets are coming up, asking for a feature which was already there last week. It seems the deployment was not using the latest codebase.
* Tech: Deploy new version of code without any downtime

#### 4. Phase: Monitoring
*AWS is doing a decoupling phase here - we are thinking about replacing it with this monitoring phase*

ich wollte auch gerade zu klein gesetzte resource limits setzen damit die container unter hoher Memory Last gekillt werden

#### 5. Phase: Security
* Narrative: We got an email - a hacker want 1 million dollor from us, otherwise he will 
* Tech: Using Azure Security Center - Pod Security Policy to ensure an bad executable cannot be executed/gain root access

Hab so ne geile Idee wegen Security und Monitoring: wir packen ins basis Image ne exe die raustelefoniert und in der security phase einen account anlegt, um dann diesen zu nutzen wahllos dinge zu zerstören im Cluster xD 

Dh K8s API Firewall kann helfen, vernünftiges RBAC kann helfen, Pod Sec Policy kann helfen 

den fall oben könnte man regeln indem man den pod nur mit berechtigen deployt der keine berechtigungen auf der kubernetes api hat
Das mit den resource limits auf jeden Fall, ist dann monitoring: warum werden meine pods gekillt

#### 6. Phase: Cost
wir schreiben code der bspw besonders CPU intensiv ist, nutzen aber default VMs due nicht CPU lastig sind, dann müssen sie im laufenden einen neuen node Pool erstellen und alles rüber schieben

Was ist nicht weiß : wie bekommen wir die kosten, bei sponsored subscriptions gehen die cost APIs nicht
    aber ich würde das problem im Cluster lösen - mit KubeCost
    es geht ja um die Pods im Cluster und nicht direkt um die azure kosten

#### Ongoing: Dev Challenge
Ich würde gerne auch haben, das ein sehr Dev oder data lastiges Team auf seine Kosten kommt.... Bspw in dem sie AI über die Anfragen laufen lassen und so rausfinden können wie der "Algorithmus" der Kunden ist um öfter zu gewinnen


## Resources

### Based on
* [Rock, Paper, Scissors, Lizard, Spock - Sample Application](https://github.com/microsoft/RockPaperScissorsLizardSpock)

### Background
* [Microsoft OpenHack](https://openhack.microsoft.com/)
  * [Github repos](https://github.com/Azure-Samples?utf8=%E2%9C%93&q=openhack&type=&language=)
* [Microsoft Cloud Workshop (MCW)](https://microsoftcloudworkshop.com/)
* [WhatTheHack](https://github.com/microsoft/whatthehack)

### AWS Background
* https://aws.amazon.com/de/gameday/
* https://go.awspartner.com/GameDaySTL2019
* https://www.fivetalent.com/aws-gameday-event/
* https://blog.newrelic.com/technology/aws-summit-gameday/
* https://medium.com/@isaeefullah/dbs-aws-gameday-experience-9a9fa35db9e6
* https://www.tui-tech.com/de/aws-gameday-tui
* https://d0.awsstatic.com/events/aws-hosted-events/2017/US/GameDay.pdf
* https://jon.sprig.gs/blog/post/1238
* https://aws.amazon.com/de/blogs/apn/how-we-rebuilt-a-mythical-startup-at-aws-gameday/
* https://github.com/fedorovdima/aws-gameday/blob/master/runbook.md
