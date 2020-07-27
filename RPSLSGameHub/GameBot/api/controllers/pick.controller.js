const PickStrategyFactory = require('../services/pickStrategyFactory.service');
const PredictorProxy = require('../services/predictorProxy.service');

const pick = async (req, res) => {
    var Player1Name= req.body.Player1Name;
    var matchId= req.body.MatchId;
    if (Player1Name==undefined || matchId == undefined)
    {
        res.status(400);
        res.send("Player1NamerId or MatchId undefined");
        return;

    }

    else {
        // implement arcade intelligence here
        strategyOption = process.env.PICK_STRATEGY || "RANDOM";
        const result = pickFromDefaultStrategy();
        console.log('Against some user, strategy ' + strategyOption + '  played ' + result.text);
        res.send({"Move":result.text});
    }	
};

const pickFromDefaultStrategy = () => {
    const strategyOption = process.env.PICK_STRATEGY || "RANDOM";
    
    const strategyFactory = new PickStrategyFactory();

    strategyFactory.setDefaultStrategy(strategyOption);
    const strategy = strategyFactory.getStrategy();
    return strategy.pick();
}

module.exports = {
    pick,
}