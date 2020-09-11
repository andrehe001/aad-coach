
# Prerequisites
###############
# Install-Module -Name PackageManagement -Force
# Install-Module -Name PowerShellGet -Force
# Install-Module -Name MicrosoftTeams -AllowPrerelease -Force

$login = Connect-MicrosoftTeams

$team = Get-Team -DisplayName "Azure Adventure Day Bertelsmann 17.9.20" `
                 -User $login.Account `
                 -Visibility Private `
                 -Archived $false

New-TeamChannel -GroupId $team.GroupId `
                 -DisplayName "AskCoaches" `
                 -MembershipType Standard      

New-TeamChannel -GroupId $team.GroupId `
                 -DisplayName "CoachesInternal" `
                 -MembershipType Private      

For ($i=1; $i -le 12; $i++) {
    New-TeamChannel -GroupId $team.GroupId `
                    -DisplayName "Team $('{0:d2}' -f $i)" `
                    -MembershipType Private      
}               

$attendees = Import-Csv -Path .\sample.csv -Delimiter ';'
foreach($attendee in $attendees) {
    Write-Output "Adding $($attendee.Mail) to Team and to Channel '$($attendee.TeamName)'"

    Add-TeamUser -GroupId $team.GroupId `
                 -User $attendee.Mail
  
    Add-TeamChannelUser -GroupId $team.GroupId `
                        -DisplayName $attendee.TeamName `
                        -User $attendee.Mail
}

