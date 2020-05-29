var _ = require('lodash'),
    expect = require('chai').expect,
    encoder = require('../..'),
    testCases = require('../fixtures/url-resolve-list');

describe('url-resolve', function () {
    it('should resolve all URLs properly', function () {
        _.forEach(testCases, function (test) {
            var base = encoder.toNodeUrl(test.base),
                resolved = encoder.resolveNodeUrl(base, test.relative);

            expect(resolved).to.eql(test.resolved);
        });
    });

    it('should accept string URL as base', function () {
        var base = 'http://postman.com/path/alpha',
            relative = 'foo/bar',
            resolved = 'http://postman.com/path/foo/bar';

        expect(encoder.resolveNodeUrl(base, relative)).to.eql(resolved);
    });

    it('should return relative URL if base URL is undefined', function () {
        expect(encoder.resolveNodeUrl(undefined, '/foo')).to.eql('/foo');
    });

    it('should return base URL if relative URL is not string', function () {
        var base = 'http://postman.com/path/alpha',
            relative = {};

        expect(encoder.resolveNodeUrl(base, relative)).to.eql(base);
    });

    it('should return relative URL if base URL is not valid URL object', function () {
        var base = {},
            relative = 'http://postman.com';

        expect(encoder.resolveNodeUrl(base, relative)).to.eql(relative);
    });
});
