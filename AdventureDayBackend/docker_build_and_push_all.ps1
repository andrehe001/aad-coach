#!/usr/bin/env pwsh
#Requires -PSEdition Core

param 
(
    [Parameter(Mandatory = $true)]
    [ValidateLength(3,63)]
    [ValidatePattern('^[^$()]+$', ErrorMessage = "{0} is not valid. Is it perhaps a non-set Azure DevOps Variable?")]
    [string]
    $ContainerRegistryName,
    
    [Parameter(Mandatory = $true)]
    [ValidateLength(1,255)]
    [ValidatePattern('^[^$()]+$', ErrorMessage = "{0} is not valid. Is it perhaps a non-set Azure DevOps Variable?")]
    [string]
    $Label
)

Write-Verbose "================================================================================"
Write-Verbose "= Configuration                                                                ="
Write-Verbose "================================================================================"
Write-Verbose "  ContainerRegistryName:          $ContainerRegistryName"
Write-Verbose "================================================================================"

./docker_build_and_push.ps1 -ProjectName AdventureDayRunner -ContainerRegistryName $ContainerRegistryName -Label $Label
./docker_build_and_push.ps1 -ProjectName AdventureDayRunnerAPI -ContainerRegistryName $ContainerRegistryName -Label $Label
