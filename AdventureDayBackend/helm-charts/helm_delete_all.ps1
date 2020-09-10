#!/usr/bin/env pwsh
#Requires -PSEdition Core

param 
(
    [ValidateLength(1,255)]
    [ValidatePattern('^[^$()]+$', ErrorMessage = "{0} is not valid. Is it perhaps a non-set Azure DevOps Variable?")]
    [string]
    $Environment = "local",

    [ValidateLength(1,255)]
    [ValidatePattern('^[^$()]+$', ErrorMessage = "{0} is not valid. Is it perhaps a non-set Azure DevOps Variable?")]
    [string]
    $Namespace = "adventure-day"
)

helm delete adventure-day-runner --namespace $Namespace
helm delete adventure-day-runner-api --namespace $Namespace
helm delete adventure-day-portal-frontend --namespace $Namespace
helm delete adventure-day-portal-api --namespace $Namespace
