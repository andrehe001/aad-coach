#!/usr/bin/env pwsh
#Requires -PSEdition Core

param 
(
    [Parameter(Mandatory = $true)]
    [ValidateLength(3,63)]
    [ValidatePattern('^[^$()]+$', ErrorMessage = "{0} is not valid.")]
    [string]
    $ContainerRegistryName,
    
    [Parameter(Mandatory = $true)]
    [ValidateLength(1,255)]
    [ValidatePattern('^[^$()]+$', ErrorMessage = "{0} is not valid.")]
    [string]
    $Label,

    [switch]
    $Local
)

Write-Verbose "  ContainerRegistryName: $ContainerRegistryName"
az acr build -r $ContainerRegistryName -f ./AdventureDayRunner/Dockerfile -t "$($ContainerRegistryName).azurecr.io/adventure-day-runner:$($Label)" .
az acr build -r $ContainerRegistryName -f ./AdventureDayRunnerAPI/Dockerfile -t "$($ContainerRegistryName).azurecr.io/adventure-day-runner-api:$($Label)" .
az acr build -r $ContainerRegistryName -f ./portal-frontend/Dockerfile -t "$($ContainerRegistryName).azurecr.io/adventure-day-portal-frontend:$($Label)" ./portal-frontend
az acr build -r $ContainerRegistryName -f ./portal-api/Dockerfile -t "$($ContainerRegistryName).azurecr.io/adventure-day-portal-api:$($Label)" ./portal-api
