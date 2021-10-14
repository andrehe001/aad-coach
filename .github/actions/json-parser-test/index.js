const core = require('@actions/core');
const github = require('@actions/github');

try {
    const jsonToTest = core.getInput('json-data');
    console.log(`Testing:`);
    console.log(jsonToTest);

    try {
        this.dom = JSON.parse(jsonToTest);

        console.log(`JSON parsing successful`);

        const payload = JSON.stringify(dom, undefined, 2)
        console.log(`The payload: ${payload}`);
    }
    catch (ex) {
        core.setFailed(`Content is not a valid JSON object. ${ex}`);
    }
} catch (error) {
    core.setFailed(error.message);
}