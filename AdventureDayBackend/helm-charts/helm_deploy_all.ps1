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

helm upgrade adventure-day-runner ./adventure-day-runner --install --namespace $Namespace -f ./adventure-day-runner.values.$($Environment).yaml --create-namespace
#helm upgrade adventure-day-portal-frontend ./adventure-day-portal-frontend --install --namespace $Namespace -f ./adventure-day-portal-frontend.values.$($Environment).yaml --create-namespace
#helm upgrade adventure-day-portal-api ./adventure-day-portal-api --install --namespace $Namespace -f ./adventure-day-portal-api.values.$($Environment).yaml --create-namespace