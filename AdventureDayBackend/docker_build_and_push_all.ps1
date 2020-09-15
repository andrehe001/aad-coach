#!/usr/bin/env pwsh
#Requires -PSEdition Core

param 
(
    [Parameter(Mandatory = $true)]
    [ValidateLength(1,255)]
    [ValidateNotNull()]
    [string]
    $RegistryName,

    [Parameter(Mandatory = $true)]
    [ValidateLength(1,255)]
    [ValidateNotNull()]
    [string]
    $Tag,

    [switch]
    $Local
)

Write-Verbose "  RegistryName: $RegistryName"
Write-Verbose "Building AdventureDayRunner"
az acr build -r $RegistryName -f ./portal-api/src/AdventureDayRunner/Dockerfile -t "$($RegistryName).azurecr.io/adventure-day-runner:$($Tag)" ./portal-api/src
Write-Verbose "Building AdventureDayPortalApi"
az acr build -r $RegistryName -f ./portal-api/src/team-management-api/Dockerfile -t "$($RegistryName).azurecr.io/adventure-day-portal-api:$($Tag)" ./portal-api/src
Write-Verbose "Building AdventureDayPortal"
az acr build -r $RegistryName -f ./portal-frontend/Dockerfile -t "$($RegistryName).azurecr.io/adventure-day-portal-frontend:$($Tag)" ./portal-frontend
