#!/usr/bin/env pwsh
#Requires -PSEdition Core

param (
$TerraformOutputFile
)

Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

$output = Get-Content -Raw -Path $TerraformOutputFile | ConvertFrom-Json
Write-Verbose "Successfully read terraform output file!"

az aks update -n $output.aks_name.value  -g $output.aks_resource_group_name.value --attach-acr $output.acr_name.value
Write-Verbose "Successfully updated AKS cluster $($output.aks_name.value) with ACR $($output.acr_name.value)."
