const expect = require('chai').expect,

    toLegacyNodeUrl = require('../../').toLegacyNodeUrl;

describe('.toLegacyNodeUrl', function () {
    it('should accept url string', function () {
        expect(toLegacyNodeUrl('http://郵便屋さん.com:399/foo&bar/{baz}?q=("foo")#`hash`'))
            .to.deep.include({
                protocol: 'http:',
                slashes: true,
                auth: null,
                host: 'xn--48jwgn17gdel797d.com:399',
                port: '399',
                hostname: 'xn--48jwgn17gdel797d.com',
                hash: '#%60hash%60',
                search: '?q=%28%22foo%22%29',
                query: 'q=%28%22foo%22%29',
                pathname: '/foo&bar/%7Bbaz%7D',
                path: '/foo&bar/%7Bbaz%7D?q=%28%22foo%22%29',
                href: 'http://xn--48jwgn17gdel797d.com:399/foo&bar/%7Bbaz%7D?q=%28%22foo%22%29#%60hash%60'
            });
    });

    it('should return empty url object on invalid input types', function () {
        expect(function () { toLegacyNodeUrl(); }).to.throw(TypeError);
        expect(function () { toLegacyNodeUrl(null); }).to.throw(TypeError);
        expect(function () { toLegacyNodeUrl(undefined); }).to.throw(TypeError);
        expect(function () { toLegacyNodeUrl({ host: '127.1' }); }).to.throw(TypeError);
    });
});
