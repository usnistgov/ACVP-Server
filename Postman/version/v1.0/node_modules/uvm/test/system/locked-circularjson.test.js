var expect = require('expect.js');

// There is a string variant of the library in bridge-client.js
describe('circular-json dependency', function () {
    it('must be version locked, unless modified intentionally', function () {
        expect(require('../../package.json').dependencies['circular-json']).be('0.3.1');
    });
});
