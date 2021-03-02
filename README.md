# Azure Adventure Day

## General Setup
* Agenda:
  * 9:00am - 9:30am Registration/Game Rules (30 Min)
  * 9:30am - 11:30am Game Play (120 Min)
  * 11:30am - 12:30am Lunch (60 Min)
  * 12:30am - 2:30pm Game Play (120 Min)
  * 2:30pm - 3:00pm Closing and Award Ceremony (30 Min)
  * after first test delivery decide if more time is needed
* Participants:
  * min. 12 (3 teams a 4)
  * max. 48 (12 teams a 4)
  * Skill set (level & domain) based mixed team
* Azure Subscriptions: Lab Service (Dennis knows more about it)
* Prerequisite: [AZ-900 Microsoft Azure Fundamentals](https://docs.microsoft.com/en-us/learn/certifications/exams/az-900) or [Azure Fundamentals Online Learning Path](https://docs.microsoft.com/en-us/learn/paths/azure-fundamentals/), or [other Azure certificates](https://www.microsoft.com/en-us/learning/azure-exams.aspx)

## Azure Services
- AKS
- Application Gateway
- Azure Monitor / Log Analytics / App Insights
- Key Vault
- Security Center
- Azure Advisor
- Azure SQL Datenbank (for game leaderboard)
- KubeCost https://github.com/hjacobs/kube-resource-report will be used for measuring economics.
  - also measure SQL Database costs

GitHub
- Repo Terraform + App Source Code
- Teilnehmer forken sich das

Jede Challenge wird unabhängig von einander bewertet
- einen Hauptgewinner
- Phasen Gewinner (Mini Challenges)

### Phases:
#### 1. Phase: Deployment
* Narrative: It was all deleted after the most important team member left the team - it was his last fight -.-
* Tech: Terraform templates are already there, just needs to be processed locally

#### 2. Phase: Change
* Narrative: Multiple support tickets are coming up, asking for a feature which was already there last week. It seems the deployment was not using the latest codebase.
* Tech: Deploy new version of code without any downtime
* Tech: Feature Flag - Issue (w change commit) - can be set per env var
* Tech: GitHub Actions for yaml Deployment
* Builtin Challenge: Learn how to define readiness probes, initialDelaySeconds and upgrade strategy. Learn how to build and deploy containers using github actions.

#### 3. Phase: Monitoring
* Narrative: Management is now looking into Azure Costs - can you monitor it? Wieviele Anfrage / Game Sessions kommen pro Sekunde an? Und Kunden melden immer mal wieder Fehler. Understand performance metrics of your application, identify issues and ensure economic scaling. 
* Tech: Ensure resource quotas and vm types that are just big enough and scale fast.
* Tech: Use application insights codeless attach to evaluate performance inside the cluster.
* Builtin Challenge: The resource quota for some pods will be too small (pods get kill) and others will be too big (waste).

#### 4. Phase: Scaling
* Narrative: A bunch of tweets are hitting up right after going online again - alle your fans in the world are trying to access the side again. 
* Tech: Scale pod and node count automatically
* Tech: Scale SQL Database
* Tech: Backend fährt hoch und runter die Requests pro Sekunde
Background: Limit Sessions per Pod (based on memory)

#### 5. Phase: Security
* Narrative: We got an email - a hacker want 1 million dollor from us, otherwise he will attack the service
* Tech: Using Azure Security Center - Pod Security Policy to ensure an bad executable cannot be executed/gain root access
  * "Pod Security Policies should be defined to reduce the attack vector by removing unnecessary application privileges"
  * "Privileged command run in container"
* Tech: Using Azure Security Center - Pod as Cluster Admin
  * "Role binding to the cluster-admin role detected"
  * "Suspicious request to Kubernetes API"
* AKS Security Center Ref:  
  * [Container recommendations](https://docs.microsoft.com/en-us/azure/security-center/recommendations-reference#recs-containers)
  * [Alerts for containers - AKS clusters](https://docs.microsoft.com/en-us/azure/security-center/alerts-reference#alerts-akscluster)
  * [Alerts for containers - host level](https://docs.microsoft.com/en-us/azure/security-center/alerts-reference#alerts-containerhost)
* Tech: App Gateway WAF needs to be enabled (SQL injection)

Hab so ne geile Idee wegen Security und Monitoring: wir packen ins basis Image ne exe die raustelefoniert und in der security phase einen account anlegt, um dann diesen zu nutzen wahllos dinge zu zerstören im Cluster xD 

den fall oben könnte man regeln indem man den pod nur mit berechtigen deployt der keine berechtigungen auf der kubernetes api hat

#### 6. Phase: Intelligence
* Narrative: Win against the computer and be better than your competition
* Tech: We will send the name of the algorithm in the header of each post. Teams can find out through the logs on how to beat each algorithm by statistics. The challenge can be solved by either building your own smart algorithm or hosting multiple algorithm in cluster cluster and route the request (by logic in the ingress controller) to the one that is most likely going to win against the opponent. So both dev and ops people can win here.
Ich würde gerne auch haben, das ein sehr Dev oder data lastiges Team auf seine Kosten kommt.... Bspw in dem sie AI über die Anfragen laufen lassen und so rausfinden können wie der "Algorithmus" der Kunden ist um öfter zu gewinnen
* Tech: e.g. using Log Analytics for functional logs


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
