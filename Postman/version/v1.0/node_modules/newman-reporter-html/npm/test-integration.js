#!/usr/bin/env node
/* global test, mkdir, rm, exit */
require('shelljs/global');
require('colors');

var fs = require('fs'),
    path = require('path'),
    Mocha = require('mocha'),
    shell = require('shelljs'),
    expect = require('chai').expect,
    recursive = require('recursive-readdir'),

    NYC = require('nyc'),

    COV_REPORT_PATH = '.coverage',

    /**
     * The directory containing integration test specs.
     *
     * @type {String}
     */
    SPEC_SOURCE_DIR = './test/integration';

module.exports = function (exit) {
    // banner line
    console.info('Running Integration tests using mocha and shelljs...'.yellow.bold);

    test('-d', COV_REPORT_PATH) && rm('-rf', COV_REPORT_PATH);
    mkdir('-p', COV_REPORT_PATH);

    var mocha = new Mocha({ timeout: 60000 }),
        nyc = new NYC({
            reporter: ['text', 'text-summary', 'lcov'],
            reportDir: COV_REPORT_PATH,
            tempDirectory: COV_REPORT_PATH
        }),
        tempDir = path.join(__dirname, '..', '.temp'),
        newmanPkg,
        reporterPkg;

    nyc.wrap();

    recursive(SPEC_SOURCE_DIR, function (err, files) {
        if (err) {
            console.error(err);

            return exit(1);
        }

        files.filter(function (file) {
            return (file.substr(-8) === '.test.js');
        }).forEach(function (file) {
            mocha.addFile(file);
        });

        // create a temp directory to test the reporter with newman
        fs.existsSync(tempDir) && shell.rm('-rf', tempDir);
        fs.mkdirSync(tempDir);

        console.info('\n  Installing newman & newman-reporter-html into the temp directory'.gray);
        newmanPkg = shell.exec('npm pack ../node_modules/newman', { cwd: tempDir, silent: true }).stdout.trim();
        reporterPkg = shell.exec('npm pack ../', { cwd: tempDir, silent: true }).stdout.trim();
        shell.exec(`npm i --prefix . ${newmanPkg}`, { cwd: tempDir, silent: true });
        shell.exec(`npm i --prefix . ${reporterPkg}`, { cwd: tempDir, silent: true });

        // start the mocha run
        global.expect = expect; // for easy reference
        // eslint-disable-next-line security/detect-non-literal-require
        global.newman = require(path.join(tempDir, 'node_modules', 'newman'));

        mocha.run(function (err) {
            // remove temp directory
            shell.rm('-rf', tempDir);

            // clear references and overrides
            delete global.expect;
            delete global.newman;

            nyc.reset();
            nyc.writeCoverageFile();
            nyc.report();
            exit(err);
        });
        mocha = null; // cleanup
    });
};

// ensure we run this script exports if this is a direct stdin.tty run
!module.parent && module.exports(exit);
