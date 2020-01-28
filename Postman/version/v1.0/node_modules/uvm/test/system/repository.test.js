/**
 * @fileOverview This test specs runs tests on the package.json file of repository. It has a set of strict tests on the
 * content of the file as well. Any change to package.json must be accompanied by valid test case in this spec-sheet.
 */
var _ = require('lodash'),
    expect = require('expect.js'),
    parseIgnore = require('parse-gitignore');

/* global describe, it */
describe('project repository', function () {
    var fs = require('fs');

    describe('package.json', function () {
        var content,
            json;

        it('should exist', function (done) {
            fs.stat('./package.json', done);
        });

        it('should have readable content', function () {
            expect(content = fs.readFileSync('./package.json').toString()).to.be.ok();
        });

        it('should have valid JSON content', function () {
            expect(json = JSON.parse(content)).to.be.ok();
        });

        describe('package.json JSON data', function () {
            it('should have valid name, description and author', function () {
                expect(json.name).to.equal('uvm');
                expect(json.description)
                    .to.equal('Universal Virtual Machine for Node and Browser');
                expect(json.author).to.equal('Postman Labs <help@getpostman.com> (=)');
                expect(json.license).to.equal('Apache-2.0');
            });

            it('should have a valid version string in form of <major>.<minor>.<revision>', function () {
                expect(json.version)
                    // eslint-disable-next-line max-len
                    .to.match(/^((\d+)\.(\d+)\.(\d+))(?:-([\dA-Za-z-]+(?:\.[\dA-Za-z-]+)*))?(?:\+([\dA-Za-z-]+(?:\.[\dA-Za-z-]+)*))?$/);
            });
        });

        describe('script definitions', function () {
            it('should have valid, existing files', function () {
                var scriptRegex = /^node\snpm\/.+\.js$/;

                expect(json.scripts).to.be.ok();
                json.scripts && Object.keys(json.scripts).forEach(function (scriptName) {
                    expect(scriptRegex.test(json.scripts[scriptName])).to.be.ok();
                    expect(fs.statSync('npm/' + scriptName + '.js')).to.be.ok();
                });
            });

            it('should have the hashbang defined', function () {
                json.scripts && Object.keys(json.scripts).forEach(function (scriptName) {
                    var fileContent = fs.readFileSync('npm/' + scriptName + '.js').toString();
                    expect(/^#!\/(bin\/bash|usr\/bin\/env\snode)[\r\n][\W\w]*$/g.test(fileContent)).to.be.ok();
                });
            });
        });

        describe('devDependencies', function () {
            it('should exist', function () {
                expect(json.devDependencies).to.be.a('object');
            });

            it('should point to a valid semver', function () {
                Object.keys(json.devDependencies).forEach(function (dependencyName) {
                    expect(json.devDependencies[dependencyName]).to.match(new RegExp('((\\d+)\\.(\\d+)\\.(\\d+))(?:-' +
                        '([\\dA-Za-z\\-]+(?:\\.[\\dA-Za-z\\-]+)*))?(?:\\+([\\dA-Za-z\\-]+(?:\\.[\\dA-Za-z\\-]+)*))?$'));
                });
            });
        });

        describe('main entry script', function () {
            it('should point to a valid file', function (done) {
                expect(json.main).to.equal('index.js');
                fs.stat(json.main, done);
            });
        });
    });

    describe('README.md', function () {
        it('should exist', function (done) {
            fs.stat('./README.md', done);
        });

        it('should have readable content', function () {
            expect(fs.readFileSync('./README.md').toString()).to.be.ok();
        });
    });

    describe('LICENSE.md', function () {
        it('should exist', function (done) {
            fs.stat('./LICENSE.md', done);
        });

        it('should have readable content', function () {
            expect(fs.readFileSync('./LICENSE.md').toString()).to.be.ok();
        });
    });

    describe('.ignore files', function () {
        var gitignorePath = '.gitignore',
            npmignorePath = '.npmignore',
            npmignore = parseIgnore(npmignorePath),
            gitignore = parseIgnore(gitignorePath);

        describe(gitignorePath, function () {
            it('must exist', function (done) {
                fs.stat(gitignorePath, done);
            });

            it('must have valid content', function () {
                expect(_.isEmpty(gitignore)).to.not.be.ok();
            });
        });

        describe(npmignorePath, function () {
            it('must exist', function (done) {
                fs.stat(npmignorePath, done);
            });

            it('must have valid content', function () {
                expect(_.isEmpty(npmignore)).to.not.be.ok();
            });
        });

        it('.gitignore coverage must be a subset of .npmignore coverage', function () {
            expect(_.intersection(gitignore, npmignore)).to.eql(gitignore);
        });
    });
});
