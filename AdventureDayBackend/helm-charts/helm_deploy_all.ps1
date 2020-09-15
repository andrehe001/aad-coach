#!/usr/bin/env pwsh
#Requires -PSEdition Core

param 
(
    [ValidateLength(1,255)]
    [ValidateNotNull()]
    [string]
    $Environment = "prod",

    [ValidateLength(1,255)]
    [ValidateNotNull()]
    [string]
    $Namespace = "adventure-day",

    [Parameter(Mandatory = $true)]
    [ValidateLength(1,255)]
    [ValidateNotNull()]
    [string]
    $RegistryName,

    [Parameter(Mandatory = $true)]
    [ValidateLength(1,255)]
    [ValidateNotNull()]
    [string]
    $Tag 
)

Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

Write-Host "RegistryName: $RegistryName"
Write-Host "Tag: $Tag"

&helm upgrade adventure-day-runner ./adventure-day-runner --install --namespace $Namespace --set "`"image.repository=$($RegistryName).azurecr.io/adventure-day-runner`"" --set "`"image.tag=$($Tag)`"" --create-namespace
&helm upgrade adventure-day-portal-frontend ./adventure-day-portal-frontend --install --namespace $Namespace --set "`"image.repository=$($RegistryName).azurecr.io/adventure-day-portal-frontend`"" --set "`"image.tag=$($Tag)`"" --create-namespace
&helm upgrade adventure-day-portal-api ./adventure-day-portal-api --install --namespace $Namespace --set "`"image.repository=$($RegistryName).azurecr.io/adventure-day-portal-api`"" --set "`"image.tag=$($Tag)`"" --create-namespace