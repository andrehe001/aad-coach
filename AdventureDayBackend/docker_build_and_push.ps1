#!/usr/bin/env pwsh
#Requires -PSEdition Core

param 
(
    [Parameter(Mandatory = $true)]
    [ValidateLength(1,255)]
    [ValidatePattern('^[^$()]+$', ErrorMessage = "{0} is not valid. Is it perhaps a non-set Azure DevOps Variable?")]
    [string]
    $ProjectName,

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

function Convert-ToDockerImageName {
    param (
        [Parameter(Mandatory = $true)]
        [string]
        $InputString
    )

    $Converted = $InputString.Replace(".", "-")
    $Converted = $Converted.ToLower()

    return $Converted
}

Write-Verbose "================================================================================"
Write-Verbose "= Configuration                                                                ="
Write-Verbose "================================================================================"
Write-Verbose "  ProjectName:                    $ProjectName"
Write-Verbose "  ContainerRegistryName:          $ContainerRegistryName"
Write-Verbose "================================================================================"

$DockerfileAbsolutePath = Resolve-Path $ProjectName/Dockerfile
$ImageName = Convert-ToDockerImageName -Input $ProjectName

az acr login --name $ContainerRegistryName
if ($LastExitCode -gt 0) { throw "az CLI error." }

docker build -f $DockerfileAbsolutePath -t "$($ContainerRegistryName).azurecr.io/$($ImageName):$($Label)" .
if ($LastExitCode -gt 0) { throw "docker error." }

docker build -f $DockerfileAbsolutePath -t "$($ContainerRegistryName).azurecr.io/$($ImageName):latest" .
if ($LastExitCode -gt 0) { throw "docker error." }

docker push "$($ContainerRegistryName).azurecr.io/$($ImageName):$($Label)"
if ($LastExitCode -gt 0) { throw "docker error." }

Write-Verbose "================================================================================"
Write-Verbose "= Outputs                                                                      ="
Write-Verbose "================================================================================"
Write-Verbose "  Docker Image:     $($ContainerRegistryName).azurecr.io/$($ImageName):$($Label)" 
Write-Verbose "================================================================================"
