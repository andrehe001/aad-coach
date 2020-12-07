
# Tables

## Team Score Table
| Column | Description |
| --- | --- |
| TeamId | PK, references team |
| Score | (calculated column) Wins + Profit - Errors |
| Wins | Total count of games where the team has won |
| Loses | Total count of games where the team has lost |
| Errors | Total count of requests where an errors has happened, this includes any Exceptions, Network Issues, Timeouts, Hacker Attacks ... |
| Profit | (calculated column) Income - Costs |
| Income | Every won game gives income, also potential stakes (bets), sum of it  |
| Costs | Every lost game costs money, also potential stakes (bets), and the Azure Infrastructure also costs money, sum of it |

## Log Table
| Column | Description |
| --- | --- |
| TeamId | PK, references team |
| Timestamp | PK |
| ResponseTime | (calculated column) Wins + Profit - Errors |
| Status | Success (if game was played), Failed (any error), HackerAttack, Canceled (if player only plays if humans provide a bet) |
| Reason | Includes Reason or Description for the Status |

# Log Samples

| STATUS | REASON |
| --- | --- |
| SUCCESS | Smoorgh has won $20 |
| SUCCESS | Human has won $20 |
| FAILED | HTTP Status Code 500 received |
| FAILED | Timeout 5s happened |
| FAILED | Network Exception received |
| ATTACKED | Hacker was able to get access into the system |
| CANCELED | Smoorgh has canceled the game, because the stakes from humans are too low |

# Logic

## Players

| Type | Name | Pattern |
| --- | --- | --- |
| FixedPlayer | Kye | Randomly choose one move during init and uses it all the time |
| PatternPlayer | Libby | first turn is random, second turn is the same move as from human the first turn move |
| IterativePlayer | Courtney | Iterates all possible moves per team |
| RandomPlayer | Lachlan | Random |
| BetPlayer | William | Random |
| MassPlayer | Gloria | Random |
| HackPlayer | Kevin | NO |
| CostCalculatorPlayer | Dagobert | NO |

## Phase 1: Deployment
* RandomPlayer tries to play with the team environment
* FixedPlayer, see [FixedStrategy.cs](https://github.com/microsoft/RockPaperScissorsLizardSpock/blob/main/Source/Services/RPSLS.DotNetPlayer.Api/Strategies/FixedStrategy.cs)

## Phase 2: Change
* RandomPlayer see above
* FixedPlayer see above
* BetPlayer will only play if bet if present, cancels it otherwise

## Phase 3: Monitoring
* RandomPlayer see above
* FixedPlayer see above
* BetPlayer see above
* CostCalculatorPlayer access Azure environment behind team environment, calculates costs and write it back

### Azure costs
* look into AKS node pool SKU and node count
* look into Azure SQL Database SKU

## Phase 4: Scale
* RandomPlayer see above
* FixedPlayer see above
* BetPlayer see above
* CostCalculatorPlayer see above
* MassPlayer plays 10x as much as the other players (rps), in addition it triggers hidden functionality inside the GameEngine, which leads to wait times and 10x SQL calls

## Phase 5: Security
* RandomPlayer see above
* FixedPlayer see above
* BetPlayer see above
* CostCalculatorPlayer see above
* MassPlayer will NOT run
* HackPlayer tries to use the vulnerability endpoint to check if he still has access, logs successful attempts as HackerAttack

## Phase 6: Intelligence
* RandomPlayer see above
* FixedPlayer see above
* BetPlayer see above
* CostCalculatorPlayer see above
* MassPlayer will NOT run
* HackPlayer see above
* IterativePlayer, see [IterativePickStrategy.java](https://github.com/microsoft/RockPaperScissorsLizardSpock/blob/main/Source/Services/RPSLS.JavaPlayer.Api/src/main/java/RPSLS/JavaPlayer/Api/Strategy/IterativePickStrategy.java)
* PatternPlayer, always uses a fixed answer to the played human match before
