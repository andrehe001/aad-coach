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
    $Namespace = "adventure-day-backend"
)

Write-Verbose "================================================================================"
Write-Verbose "= Configuration                                                                ="
Write-Verbose "================================================================================"
Write-Verbose "  Environment:                   $Environment"
Write-Verbose "  Namespace:                     $Namespace"
Write-Verbose "  ImageTag:                      $ImageTag"
Write-Verbose "================================================================================"

helm delete adventure-day-runner --namespace $Namespace
helm delete adventure-day-runner-api --namespace $Namespace
helm delete adventure-day-frontend --namespace $Namespace
if ($LastExitCode -gt 0) { throw "helm error." }
