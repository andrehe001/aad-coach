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
    $Namespace = "adventure-day-backend",

    [Parameter(Mandatory = $true)]
    [ValidateLength(1,255)]
    [ValidatePattern('^[^$()]+$', ErrorMessage = "{0} is not valid. Is it perhaps a non-set Azure DevOps Variable?")]
    [string]
    $ImageTag
)

Write-Verbose "================================================================================"
Write-Verbose "= Configuration                                                                ="
Write-Verbose "================================================================================"
Write-Verbose "  Environment:                   $Environment"
Write-Verbose "  Namespace:                     $Namespace"
Write-Verbose "  ImageTag:                      $ImageTag"
Write-Verbose "================================================================================"

helm upgrade adventure-day-runner ./adventure-day-runner --install --namespace $Namespace -f ./adventure-day-runner.values.$($Environment).yaml --create-namespace
helm upgrade adventure-day-runner-api ./adventure-day-runner-api --install --namespace $Namespace -f ./adventure-day-runner-api.values.$($Environment).yaml --create-namespace
if ($LastExitCode -gt 0) { throw "helm error." }
