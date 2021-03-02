# Player and Scoring Mechanics

## Players & Phases

| 1   | 2   | 3   | 4   | 5   | 6   | Name     | Type                 | Pattern & Logic                                                                          |
| --- | --- | --- | --- | --- | --- | -------- | -------------------- | ---------------------------------------------------------------------------------------- |
| X   | X   | X   | X   | X   | X   | Lachlan  | RandomPlayer         | Random                                                                                   |
| X   | X   | X   | X   | X   | X   | Kye      | FixedPlayer          | Selects randomly one move per runner instance and uses it all the time                   |
|     | X   | X   | X   | X   | X   | William  | BetPlayer            | Random + assumes bet feature                                                             |
|     |     | X   | X   | X   | X   | Dagobert | CostCalculatorPlayer | No pattern + scores Azure costs                                                          |
|     |     |     | X   |     |     | Gloria   | MassPlayer           | Random + simulates extreme CPU and SQL usage                                             |
|     |     |     |     | X   | X   | Kevin    | HackPlayer           | No pattern + tries to hack their env                                                     |
|     |     |     |     |     | X   | Courtney | IterativePlayer      | Iterates always in the same order over the moves                                         |
|     |     |     |     |     | X   | Libby    | PatternPlayer        | First turn is random, subsequent turns are the same moves as from human the turns before |
|     |     |     |     |     | X   | Brain    | EasyPeasyPlayer      | Has as much intelligence as a stone - plays always Rock                                  |

## Possible solutions for increasing scoring

* Kye: Take a look into Kye's first move per match and you know what will be the 2nd or 3rd move
* Courtney: Take a look into Courtney's first move per match - she will iterate for the 2nd or 3rd move (Rock, Paper, Scissors, Metal, Snap)
* Libby: For your 2nd move, take a look what was your 1st move and fight against, same for 3rd
* Brain: Always return Paper or Snap

## Tables

### Team Score Table

| Column | Description                                                                                                                      |
| ------ | -------------------------------------------------------------------------------------------------------------------------------- |
| TeamId | PK, references team                                                                                                              |
| Score  | (calculated column) Wins + Profit - Errors                                                                                       |
| Wins   | Total count of games where the team has won                                                                                      |
| Loses  | Total count of games where the team has lost                                                                                     |
| Errors | Total count of requests where an errors has happened, this includes any Exceptions, Network Issues, Timeouts, Hacker Attacks ... |
| Profit | (calculated column) Income - Costs                                                                                               |
| Income | Every won game gives income, also potential stakes (bets), sum of it                                                             |
| Costs  | Every lost game costs money, also potential stakes (bets), and the Azure Infrastructure also costs money, sum of it              |

### Log Table

| Column       | Description                                                                                                             |
| ------------ | ----------------------------------------------------------------------------------------------------------------------- |
| TeamId       | PK, references team                                                                                                     |
| Timestamp    | PK                                                                                                                      |
| ResponseTime | (calculated column) Wins + Profit - Errors                                                                              |
| Status       | Success (if game was played), Failed (any error), HackerAttack, Canceled (if player only plays if humans provide a bet) |
| Reason       | Includes Reason or Description for the Status                                                                           |

## Log Samples

| STATUS   | REASON                                                                    |
| -------- | ------------------------------------------------------------------------- |
| SUCCESS  | Smoorgh has won $20                                                       |
| SUCCESS  | Human has won $20                                                         |
| FAILED   | HTTP Status Code 500 received                                             |
| FAILED   | Timeout 5s happened                                                       |
| FAILED   | Network Exception received                                                |
| ATTACKED | Hacker was able to get access into the system                             |
| CANCELED | Smoorgh has canceled the game, because the stakes from humans are too low |
