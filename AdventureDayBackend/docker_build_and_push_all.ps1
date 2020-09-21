#!/usr/bin/env pwsh
#Requires -PSEdition Core

param 
(
    [Parameter(Mandatory = $true)]
    [ValidateLength(1,255)]
    [ValidateNotNull()]
    [string]
    $RegistryName,

    [Parameter(Mandatory = $false)]
    [ValidateLength(1,255)]
    [ValidateNotNull()]
    [string]
    $Tag = "latest",

    [switch]
    $LocalBuildOnly
)

Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"


function RunAcrBuild {
    Write-Host "Running ACR Docker Build (and Push)"
    
    Write-Host "Building AdventureDay Runner"
    az acr build -r $RegistryName -f ./portal-api/src/AdventureDay.Runner/Dockerfile -t "$($RegistryName).azurecr.io/adventure-day-runner:$($Tag)" ./portal-api/src/
    if ($LastExitCode -gt 0) { throw "acr docker build error" }
    
    Write-Host "Building AdventureDay Portal Backend"
    az acr build -r $RegistryName -f ./portal-api/src/AdventureDay.PortalApi/Dockerfile -t "$($RegistryName).azurecr.io/adventure-day-portal-api:$($Tag)" ./portal-api/src/
    if ($LastExitCode -gt 0) { throw "acr docker build error" }
    
    Write-Host "Building AdventureDay Portal Frontend"
    az acr build -r $RegistryName -f ./portal-frontend/Dockerfile -t "$($RegistryName).azurecr.io/adventure-day-portal-frontend:$($Tag)" ./portal-frontend
    if ($LastExitCode -gt 0) { throw "acr docker build error" }
}

function RunLocalDockerBuild {
    Write-Host "Running local Docker Build (and no push!)"
    
    Write-Host "Building AdventureDay Runner"
    docker build -f ./portal-api/src/AdventureDay.Runner/Dockerfile -t "$($RegistryName).azurecr.io/adventure-day-runner:$($Tag)" ./portal-api/src/
    if ($LastExitCode -gt 0) { throw "docker build error" }

    Write-Host "Building AdventureDay Portal Backend"
    docker build -f ./portal-api/src/AdventureDay.PortalApi/Dockerfile -t "$($RegistryName).azurecr.io/adventure-day-portal-api:$($Tag)" ./portal-api/src/
    if ($LastExitCode -gt 0) { throw "docker build error" }

    Write-Host "Building AdventureDay Portal Frontend"
    docker build -f ./portal-frontend/Dockerfile -t "$($RegistryName).azurecr.io/adventure-day-portal-frontend:$($Tag)" ./portal-frontend
    if ($LastExitCode -gt 0) { throw "docker build error" }

}


Write-Verbose "Target Azure Container Registry Name: $RegistryName"

if ($LocalBuildOnly) 
{
    RunLocalDockerBuild
}
else
{
    RunAcrBuild
}

