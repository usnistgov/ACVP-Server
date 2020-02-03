'use strict';

class TeamcityReporter {
    constructor(emitter, reporterOptions, options) {
        this.reporterOptions = reporterOptions;
        this.options = options;
        this.flowId = `${new Date().getTime() + Math.random()}`;
        const events = 'start beforeIteration iteration beforeItem item beforePrerequest prerequest beforeScript script beforeRequest request beforeTest test beforeAssertion assertion console exception beforeDone done'.split(' ');
        events.forEach((e) => { if (typeof this[e] == 'function') emitter.on(e, (err, args) => this[e](err, args)) });
    }

    start(err, args) {
        console.log(`##teamcity[testSuiteStarted name='${this.escape(this.options.collection.name)}' flowId='${this.flowId}']`);
    }

    beforeItem(err, args) {
        this.currItem = {name: this.itemName(args.item, args.cursor), passed: true, failedAssertions: []};
        console.log(`##teamcity[testStarted name='${this.currItem.name}' captureStandardOutput='true' flowId='${this.flowId}']`);
    }

    request(err, args) {
        if (!err) {
            this.currItem.response = args.response;
        }
    }

    assertion(err, args) {
        if (err) {
            this.currItem.passed = false;
            this.currItem.failedAssertions.push(args.assertion);
        }
    }

    item(err, args) {
        if (!this.currItem.passed) {
            const msg = this.escape(this.currItem.failedAssertions.join(", "));
            const responseCode = (this.currItem.response && this.currItem.response.code) || "-";
            const reason = (this.currItem.response && this.currItem.response.reason()) || "-";
            const details = this.escape(`Response code: ${responseCode}, reason: ${reason}`);
            console.log(`##teamcity[testFailed name='${this.currItem.name}' message='${msg}' details='${msg} - ${details}' flowId='${this.flowId}']`);
        }
        const duration = (this.currItem.response && this.currItem.response.responseTime) || 0;
        console.log(`##teamcity[testFinished name='${this.currItem.name}' duration='${duration}' flowId='${this.flowId}']`);
    }

    done(err, args) {
        console.log(`##teamcity[testSuiteFinished name='${this.options.collection.name}' flowId='${this.flowId}']`);
    }

    /* HELPERS */
    itemName(item, cursor) {
        const parentName = item.parent && item.parent() && item.parent().name ? item.parent().name : "";
        const folderOrEmpty = (!parentName || parentName === this.options.collection.name) ? "" : parentName + "/";
        const iteration = cursor && cursor.cycles > 1 ? "/" + cursor.iteration : "";
        return this.escape(folderOrEmpty + item.name + iteration);
    }

    /**
     * Escape special patterns as defined at:
     * https://confluence.jetbrains.com/display/TCD9/Build+Script+Interaction+with+TeamCity
     */
    escape(string) {
        return string
            .replace(/['|\[\]]/g, '|$&')
            .replace('\n', '|n')
            .replace('\r', '|r')
            .replace(/[\u0100-\uffff]/g, (c) => `|0x${c.charCodeAt(0).toString(16).padStart(4, "0")}`);
    }
}

module.exports = TeamcityReporter;
