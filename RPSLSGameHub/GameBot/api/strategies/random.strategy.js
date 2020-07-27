const os = require("os");
const { RPSLSChoices } = require('../settings/rpslsOptions');

class RandomStrategy {

    pick() {
        const choiceIndex = Math.floor((Math.random() * RPSLSChoices.length - 1) + 1);

        var bet = null;
        if (process.env.FF_BETS) {
            bet = Math.random();
        }

        return {
            "player":  os.hostname(),
            "playerType": "node",
            "text": RPSLSChoices[choiceIndex],
            "bet": bet,
            "value": choiceIndex
        };
    }
}

module.exports = RandomStrategy;