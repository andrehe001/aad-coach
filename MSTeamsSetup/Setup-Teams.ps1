
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

$coaches = Import-Csv -Path .\coaches.csv -Delimiter ';'
For ($i=1; $i -le 12; $i++) {
    $teamName = "Team $('{0:d2}' -f $i)"
    New-TeamChannel -GroupId $team.GroupId `
                    -DisplayName $teamName `
                    -MembershipType Private
    
    foreach($coach in $coaches) {
        Write-Output "Adding coach $($coach.Mail) to Team and to Channel '$($teamName)'"
    
        Add-TeamChannelUser -GroupId $team.GroupId `
                            -DisplayName $teamName `
                            -User $coach.Mail
    }
}               

$attendees = Import-Csv -Path .\attendees.csv -Delimiter ';'
foreach($attendee in $attendees) {
    Write-Output "Adding attendee $($attendee.Mail) to Team and to Channel '$($attendee.TeamName)'"

    Add-TeamUser -GroupId $team.GroupId `
                 -User $attendee.Mail
  
    Add-TeamChannelUser -GroupId $team.GroupId `
                        -DisplayName $attendee.TeamName `
                        -User $attendee.Mail
}

