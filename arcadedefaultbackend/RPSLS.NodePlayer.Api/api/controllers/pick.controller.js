const PickStrategyFactory = require('../services/pickStrategyFactory.service');
const PredictorProxy = require('../services/predictorProxy.service');

const pick = async (req, res) => {
    var challengerId= req.body.challengerId;
    var matchId= req.body.matchId;
    if (challengerId==undefined || matchId == undefined)
    {
        res.status(400);
        res.send("challengerId or matchId undefined");
        return;

    }

    else {
        // implement arcade intelligence here
        const result = pickFromDefaultStrategy();
        const strategyOption = process.env.PICK_STRATEGY || "RANDOM";
        console.log('Against some user, strategy ' + strategyOption + '  played ' + result.text);
        res.send({"Move":result.text});
    }	
};

const pickFromDefaultStrategy = () => {
    const strategyOption = process.env.PICK_STRATEGY;
    const strategyFactory = new PickStrategyFactory();

    strategyFactory.setDefaultStrategy(strategyOption);
    const strategy = strategyFactory.getStrategy();
    return strategy.pick();
}

module.exports = {
    pick,
}