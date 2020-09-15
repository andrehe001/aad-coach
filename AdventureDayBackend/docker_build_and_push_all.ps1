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
Write-Verbose "Building AdventureDayRunner"
az acr build -r $ContainerRegistryName -f ./portal-api/src/AdventureDayRunner/Dockerfile -t "$($ContainerRegistryName).azurecr.io/adventure-day-runner:$($Label)" ./portal-api/src
Write-Verbose "Building AdventureDayPortalApi"
az acr build -r $ContainerRegistryName -f ./portal-api/src/team-management-api/Dockerfile -t "$($ContainerRegistryName).azurecr.io/adventure-day-portal-api:$($Label)" ./portal-api/src
Write-Verbose "Building AdventureDayPortal"
az acr build -r $ContainerRegistryName -f ./portal-frontend/Dockerfile -t "$($ContainerRegistryName).azurecr.io/adventure-day-portal-frontend:$($Label)" ./portal-frontend
