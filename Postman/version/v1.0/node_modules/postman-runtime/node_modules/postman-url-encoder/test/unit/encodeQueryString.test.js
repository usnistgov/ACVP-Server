const expect = require('chai').expect,

    encodeQueryString = require('../../').encodeQueryString;

describe('.encodeQueryString', function () {
    it('should accept query as object', function () {
        expect(encodeQueryString({
            q1: 'v1',
            q2: '(v2)'
        })).to.eql('q1=v1&q2=%28v2%29');
    });

    it('should handle query as an array', function () {
        expect(encodeQueryString(['foo', 'bār'])).to.equal('0=foo&1=b%C4%81r');
    });

    it('should handle multi-valued query object', function () {
        expect(encodeQueryString({
            q1: ['𝌆й', '你ス'],
            q2: ''
        })).to.eql('q1=%F0%9D%8C%86%D0%B9&q1=%E4%BD%A0%E3%82%B9&q2=');
    });

    it('should return empty string on invalid input types', function () {
        expect(encodeQueryString()).to.equal('');
        expect(encodeQueryString(null)).to.equal('');
        expect(encodeQueryString(undefined)).to.equal('');
        expect(encodeQueryString(NaN)).to.equal('');
        expect(encodeQueryString(true)).to.equal('');
        expect(encodeQueryString(1234)).to.equal('');
        expect(encodeQueryString({})).to.equal('');
        expect(encodeQueryString('foo=bar')).to.equal('');
        expect(encodeQueryString(Function)).to.equal('');
    });

    it('should encode `!\'()*` characters', function () {
        expect(encodeQueryString({ q: '!\'()*' }))
            .to.eql('q=%21%27%28%29%2A');
    });
});
