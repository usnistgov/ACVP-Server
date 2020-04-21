const expect = require('chai').expect,

    encoder = require('../../../encoder');

describe('encoder', function () {
    describe('.encodeHost', function () {
        it('should do punycode ASCII serialization of the domain', function () {
            expect(encoder.encodeHost('😎.cool')).to.equal('xn--s28h.cool');
            expect(encoder.encodeHost('postman.com')).to.equal('postman.com');
            expect(encoder.encodeHost('郵便屋さん.com')).to.equal('xn--48jwgn17gdel797d.com');
        });

        it('should handle the IP address shorthands', function () {
            expect(encoder.encodeHost('0')).to.equal('0.0.0.0');
            expect(encoder.encodeHost('1234')).to.equal('0.0.4.210');
            expect(encoder.encodeHost('127.1')).to.equal('127.0.0.1');
            expect(encoder.encodeHost('255.255.255')).to.equal('255.255.0.255');
        });

        it('should accept hostname as an array', function () {
            expect(encoder.encodeHost([8, 8])).to.equal('8.0.0.8');
            expect(encoder.encodeHost(['🍪', 'example', 'com'])).to.equal('xn--hj8h.example.com');
        });

        it('should not double encode hostname', function () {
            expect(encoder.encodeHost('xn--48jwgn17gdel797d.com')).to.equal('xn--48jwgn17gdel797d.com');
            expect(encoder.encodeHost('255.255.255.0')).to.equal('255.255.255.0');
        });

        it('should return input value on invalid domain', function () {
            expect(encoder.encodeHost('xn:')).to.equal('xn:');
            expect(encoder.encodeHost('example#com')).to.equal('example#com');
            expect(encoder.encodeHost('99999999999')).to.equal('99999999999');
            expect(encoder.encodeHost('xn--iñvalid.com')).to.equal('xn--iñvalid.com');
        });

        it('should return empty string on invalid input types', function () {
            expect(encoder.encodeHost()).to.equal('');
            expect(encoder.encodeHost(null)).to.equal('');
            expect(encoder.encodeHost(undefined)).to.equal('');
            expect(encoder.encodeHost(NaN)).to.equal('');
            expect(encoder.encodeHost(true)).to.equal('');
            expect(encoder.encodeHost(1234)).to.equal('');
            expect(encoder.encodeHost(Function)).to.equal('');
            expect(encoder.encodeHost({ domain: 'home' })).to.equal('');
        });
    });

    describe('.encodePath', function () {
        it('should percent-encode C0 control codes', function () {
            var i,
                char;

            for (i = 0; i < 32; i++) {
                char = String.fromCharCode(i);
                expect(encoder.encodePath(char)).to.equal(encoder.percentEncodeCharCode(i));
            }

            char = String.fromCharCode(127);
            expect(encoder.encodePath(char)).to.equal(encoder.percentEncodeCharCode(127));
        });

        it('should percent-encode SPACE, ("), (<), (>), (`), (#), (?), ({), and (})', function () {
            var i,
                char,
                encoded,
                chars = [],
                expected = [' ', '"', '<', '>', '`', '#', '?', '{', '}'];

            for (i = 32; i < 127; i++) {
                char = String.fromCharCode(i);
                encoded = encoder.encodePath(char);

                if (char !== encoded) {
                    chars.push(char);
                    expect(encoded).to.equal(encodeURIComponent(char));
                }
            }

            expect(chars).to.have.all.members(expected);
        });

        it('should percent-encode unicode characters', function () {
            expect(encoder.encodePath('/𝌆/й/你/ス')).to.eql('/%F0%9D%8C%86/%D0%B9/%E4%BD%A0/%E3%82%B9');
        });

        it('should accept path as an array', function () {
            expect(encoder.encodePath(['🍪'])).to.equal('%F0%9F%8D%AA');
            expect(encoder.encodePath(['foo', 'bar', '(bàz)'])).to.equal('foo/bar/(b%C3%A0z)');
        });

        it('should not double encode characters', function () {
            expect(encoder.encodePath('foo/%2a/%F0%9F%8D%AA')).to.equal('foo/%2a/%F0%9F%8D%AA');
            expect(encoder.encodePath(['foo', '+', '(b%C3%A0r)'])).to.equal('foo/+/(b%C3%A0r)');
        });

        it('should return empty string on invalid input types', function () {
            expect(encoder.encodePath()).to.equal('');
            expect(encoder.encodePath(null)).to.equal('');
            expect(encoder.encodePath(undefined)).to.equal('');
            expect(encoder.encodePath(NaN)).to.equal('');
            expect(encoder.encodePath(true)).to.equal('');
            expect(encoder.encodePath(1234)).to.equal('');
            expect(encoder.encodePath(Function)).to.equal('');
            expect(encoder.encodePath({ path: '/foo' })).to.equal('');
        });
    });

    describe('.encodeUserInfo', function () {
        it('should percent-encode C0 control codes', function () {
            var i,
                char;

            for (i = 0; i < 32; i++) {
                char = String.fromCharCode(i);
                expect(encoder.encodeUserInfo(char)).to.equal(encoder.percentEncodeCharCode(i));
            }

            char = String.fromCharCode(127);
            expect(encoder.encodeUserInfo(char)).to.equal(encoder.percentEncodeCharCode(127));
        });

        it('should percent-encode SPACE, ("), (<), (>), (`), (#), (?), ({), (}), (/),' +
            '(:), (;), (=), (@), ([), (\\), (]), (^), and (|)', function () {
            var i,
                char,
                encoded,
                chars = [],
                expected = [' ', '"', '<', '>', '`', '#', '?', '{', '}', '/', ':',
                    ';', '=', '@', '[', '\\', ']', '^', '|'];

            for (i = 32; i < 127; i++) {
                char = String.fromCharCode(i);
                encoded = encoder.encodeUserInfo(char);

                if (char !== encoded) {
                    chars.push(char);
                    expect(encoded).to.equal(encodeURIComponent(char));
                }
            }

            expect(chars).to.have.all.members(expected);
        });

        it('should percent-encode unicode characters', function () {
            expect(encoder.encodeUserInfo('𝌆й你ス')).to.eql('%F0%9D%8C%86%D0%B9%E4%BD%A0%E3%82%B9');
        });

        it('should not double encode characters', function () {
            expect(encoder.encodeUserInfo('username_%F0%9F%8D%AA')).to.equal('username_%F0%9F%8D%AA');
        });

        it('should return empty string on invalid input types', function () {
            expect(encoder.encodeUserInfo()).to.equal('');
            expect(encoder.encodeUserInfo(null)).to.equal('');
            expect(encoder.encodeUserInfo(undefined)).to.equal('');
            expect(encoder.encodeUserInfo(NaN)).to.equal('');
            expect(encoder.encodeUserInfo(true)).to.equal('');
            expect(encoder.encodeUserInfo(1234)).to.equal('');
            expect(encoder.encodeUserInfo(Function)).to.equal('');
            expect(encoder.encodeUserInfo({ auth: 'secret' })).to.equal('');
        });
    });

    describe('.encodeFragment', function () {
        it('should percent-encode C0 control codes', function () {
            var i,
                char;

            for (i = 0; i < 32; i++) {
                char = String.fromCharCode(i);
                expect(encoder.encodeFragment(char)).to.equal(encoder.percentEncodeCharCode(i));
            }

            char = String.fromCharCode(127);
            expect(encoder.encodeFragment(char)).to.equal(encoder.percentEncodeCharCode(127));
        });

        it('should percent-encode SPACE, ("), (<), (>), and (`)', function () {
            var i,
                char,
                encoded,
                chars = [],
                expected = [' ', '"', '<', '>', '`'];

            for (i = 32; i < 127; i++) {
                char = String.fromCharCode(i);
                encoded = encoder.encodeFragment(char);

                if (char !== encoded) {
                    chars.push(char);
                    expect(encoded).to.equal(encodeURIComponent(char));
                }
            }

            expect(chars).to.have.all.members(expected);
        });

        it('should percent-encode unicode characters', function () {
            expect(encoder.encodeFragment('𝌆й你ス')).to.eql('%F0%9D%8C%86%D0%B9%E4%BD%A0%E3%82%B9');
        });

        it('should not double encode characters', function () {
            expect(encoder.encodeFragment('#search=%F0%9F%8D%AA')).to.equal('#search=%F0%9F%8D%AA');
        });

        it('should return empty string on invalid input types', function () {
            expect(encoder.encodeFragment()).to.equal('');
            expect(encoder.encodeFragment(null)).to.equal('');
            expect(encoder.encodeFragment(undefined)).to.equal('');
            expect(encoder.encodeFragment(NaN)).to.equal('');
            expect(encoder.encodeFragment(true)).to.equal('');
            expect(encoder.encodeFragment(1234)).to.equal('');
            expect(encoder.encodeFragment(Function)).to.equal('');
            expect(encoder.encodeFragment({ hash: 'fragment' })).to.equal('');
        });
    });

    describe('.encodeQueryParam', function () {
        it('should percent-encode C0 control codes', function () {
            var i,
                char;

            for (i = 0; i < 32; i++) {
                char = String.fromCharCode(i);
                expect(encoder.encodeQueryParam(char)).to.equal(encoder.percentEncodeCharCode(i));
            }

            char = String.fromCharCode(127);
            expect(encoder.encodeQueryParam(char)).to.equal(encoder.percentEncodeCharCode(127));
        });

        it('should percent-encode SPACE, ("), (#), (\'), (<), and (>)', function () {
            var i,
                char,
                encoded,
                chars = [],
                expected = [' ', '"', '#', '\'', '<', '>'];

            for (i = 32; i < 127; i++) {
                char = String.fromCharCode(i);
                encoded = encoder.encodeQueryParam(char);

                if (char !== encoded) {
                    chars.push(char);
                    expect(encoded).to.equal(encoder.percentEncodeCharCode(i));
                }
            }

            expect(chars).to.have.all.members(expected);
        });

        it('should percent-encode unicode characters', function () {
            expect(encoder.encodeQueryParam('𝌆й你ス')).to.eql('%F0%9D%8C%86%D0%B9%E4%BD%A0%E3%82%B9');
        });

        it('should not double encode characters', function () {
            expect(encoder.encodeQueryParam('key:%F0%9F%8D%AA')).to.equal('key:%F0%9F%8D%AA');
        });

        it('should return empty string on invalid input types', function () {
            expect(encoder.encodeQueryParam()).to.equal('');
            expect(encoder.encodeQueryParam(null)).to.equal('');
            expect(encoder.encodeQueryParam(undefined)).to.equal('');
            expect(encoder.encodeQueryParam(NaN)).to.equal('');
            expect(encoder.encodeQueryParam(true)).to.equal('');
            expect(encoder.encodeQueryParam(1234)).to.equal('');
            expect(encoder.encodeQueryParam(Function)).to.equal('');
            expect(encoder.encodeQueryParam(['key', 'value'])).to.equal('');
        });

        it('should accept param as key-value object', function () {
            expect(encoder.encodeQueryParam({ key: 'q', value: '(🚀)' })).to.equal('q=(%F0%9F%9A%80)');
        });

        it('should percent-encode SPACE, ("), (#), (&), (\'), (<), (=), and (>) in param key', function () {
            var i,
                char,
                encoded,
                chars = [],
                expected = [' ', '"', '#', '&', '\'', '<', '=', '>'];

            for (i = 32; i < 127; i++) {
                char = String.fromCharCode(i);
                encoded = encoder.encodeQueryParam({ key: char });

                if (char !== encoded) {
                    chars.push(char);
                    expect(encoded).to.equal(encoder.percentEncodeCharCode(i));
                }
            }

            expect(chars).to.have.all.members(expected);
        });

        it('should percent-encode SPACE, ("), (#), (&), (\'), (<), and (>) in param value', function () {
            var i,
                char,
                encoded,
                chars = [],
                expected = [' ', '"', '#', '&', '\'', '<', '>'];

            for (i = 32; i < 127; i++) {
                char = String.fromCharCode(i);
                encoded = encoder.encodeQueryParam({ value: char }).slice(1); // leading `=`

                if (char !== encoded) {
                    chars.push(char);
                    expect(encoded).to.equal(encoder.percentEncodeCharCode(i));
                }
            }

            expect(chars).to.have.all.members(expected);
        });

        it('should handle param object without key', function () {
            expect(encoder.encodeQueryParam({ value: 'bar&=#' })).to.eql('=bar%26=%23');
        });

        it('should handle param object with null key', function () {
            expect(encoder.encodeQueryParam({ key: null, value: 'bar' })).to.eql('=bar');
        });

        it('should handle param object without value', function () {
            expect(encoder.encodeQueryParam({ key: 'foo&=#' })).to.eql('foo%26%3D%23');
        });

        it('should handle param object with null value', function () {
            expect(encoder.encodeQueryParam({ key: 'foo', value: null })).to.eql('foo');
        });

        it('should handle param object with empty value', function () {
            expect(encoder.encodeQueryParam({ key: 'foo', value: '' })).to.eql('foo=');
        });

        it('should handle param object with empty key and empty value', function () {
            expect(encoder.encodeQueryParam({ key: '', value: '' })).to.eql('=');
        });

        it('should return empty string for invalid param object', function () {
            expect(encoder.encodeQueryParam({})).to.eql('');
            expect(encoder.encodeQueryParam({ keys: ['a', 'b'] })).to.eql('');
        });

        it('should ignore non-string value in param object', function () {
            expect(encoder.encodeQueryParam({ key: 'q', value: 123 })).to.eql('q');
            expect(encoder.encodeQueryParam({ value: true })).to.eql('');
        });

        it('should ignore non-string key in param object', function () {
            expect(encoder.encodeQueryParam({ key: 123, value: 'foo' })).to.eql('=foo');
            expect(encoder.encodeQueryParam({ key: true })).to.eql('');
        });

        it('should encode key with unicode characters in param object', function () {
            expect(encoder.encodeQueryParam({ key: 'foo=𝌆й你ス', value: 'bar' }))
                .to.eql('foo%3D%F0%9D%8C%86%D0%B9%E4%BD%A0%E3%82%B9=bar');
        });

        it('should encode value with unicode characters in param object', function () {
            expect(encoder.encodeQueryParam({ key: 'foo', value: '"𝌆й你ス"' }))
                .to.eql('foo=%22%F0%9D%8C%86%D0%B9%E4%BD%A0%E3%82%B9%22');
        });

        it('should not double encode characters in param object', function () {
            expect(encoder.encodeQueryParam({ key: 'foo%20bar', value: '%25' })).to.eql('foo%20bar=%25');
        });
    });

    describe('.encodeQueryParams', function () {
        it('should accept params as object', function () {
            expect(encoder.encodeQueryParams({
                q1: 'v1',
                q2: '(v2)'
            })).to.eql('q1=v1&q2=(v2)');
        });

        it('should accept array of param string', function () {
            expect(encoder.encodeQueryParams(['foo', 'bār'])).to.equal('foo&b%C4%81r');
        });

        it('should accept array of param objects', function () {
            expect(encoder.encodeQueryParams([
                { key: '☝🏻', value: 'v1' },
                { key: '✌🏻', value: 'v2' }
            ])).to.eql('%E2%98%9D%F0%9F%8F%BB=v1&%E2%9C%8C%F0%9F%8F%BB=v2');
        });

        it('should handle params with empty key or value', function () {
            expect(encoder.encodeQueryParams([
                { key: 'get', value: null },
                { key: '', value: 'bar' },
                { key: '', value: '' },
                { key: 'baz', value: '' },
                { key: null, value: null },
                { key: '', value: null }
            ])).to.eql('get&=bar&=&baz=&&');

            expect(encoder.encodeQueryParams({ '': null })).to.eql('');
            expect(encoder.encodeQueryParams({ '': '' })).to.eql('=');
            expect(encoder.encodeQueryParams({ '': [null, null] })).to.eql('&');
            expect(encoder.encodeQueryParams({ '': ['', null] })).to.eql('=&');
            expect(encoder.encodeQueryParams({ '': [null, ''] })).to.eql('&=');
            expect(encoder.encodeQueryParams({ '': ['', ''] })).to.eql('=&=');
        });

        it('should handle multi-valued param object', function () {
            expect(encoder.encodeQueryParams({
                q1: ['𝌆+й', '你-ス'],
                q2: ''
            })).to.eql('q1=%F0%9D%8C%86+%D0%B9&q1=%E4%BD%A0-%E3%82%B9&q2=');
        });

        it('should exclude disabled params by default', function () {
            expect(encoder.encodeQueryParams([
                { key: 'q1', value: 'v1', disabled: true },
                { value: 'v2' }
            ])).to.eql('=v2');
        });

        it('should return empty string on invalid input types', function () {
            expect(encoder.encodeQueryParams()).to.equal('');
            expect(encoder.encodeQueryParams(null)).to.equal('');
            expect(encoder.encodeQueryParams(undefined)).to.equal('');
            expect(encoder.encodeQueryParams(NaN)).to.equal('');
            expect(encoder.encodeQueryParams(true)).to.equal('');
            expect(encoder.encodeQueryParams(1234)).to.equal('');
            expect(encoder.encodeQueryParams('foo=bar')).to.equal('');
            expect(encoder.encodeQueryParams(Function)).to.equal('');
        });
    });

    describe('.percentEncode', function () {
        it('should return the percent-encoded representation of the string', function () {
            expect(encoder.percentEncode('\u001c')).to.equal('%1C');
            expect(encoder.percentEncode('૵')).to.equal('%E0%AB%B5');
            expect(encoder.percentEncode('🎉')).to.equal('%F0%9F%8E%89');
        });

        it('should encode C0 control codes', function () {
            var i,
                char,
                encoded,
                encode = 1;

            for (i = 0; i < 32; i++) {
                char = String.fromCharCode(i);
                encoded = encoder.percentEncode(char);

                encode &= char !== encoded;
                expect(encoded).to.equal(encodeURI(char));
            }

            char = String.fromCharCode(127);
            encoded = encoder.percentEncode(char);

            encode &= char !== encoded;
            expect(encoded).to.equal(encodeURI(char));

            expect(encode).to.equal(1);
        });

        it('should not encode printable ASCII codes [32, 126]', function () {
            var i,
                char,
                encoded,
                encode = 0;

            for (i = 32; i < 127; i++) {
                char = String.fromCharCode(i);
                encoded = encoder.percentEncode(char);

                encode |= char !== encoded;
                expect(encoded).to.equal(char);
            }

            expect(encode).to.equal(0);
        });

        it('should accept a custom EncodeSet', function () {
            var set = new encoder.EncodeSet(['a', 'b', 'c']);

            expect(encoder.percentEncode('abc ABC', set)).to.equal('%61%62%63 ABC');
        });

        it('should handle invalid EncodeSet input', function () {
            var set = new Set(['a', 'b', 'c']);

            expect(encoder.percentEncode('abc ABC', set)).to.equal('abc ABC');
        });
    });

    describe('.percentEncodeCharCode', function () {
        it('should return the percent-encoded representation of the char code', function () {
            expect(encoder.percentEncodeCharCode(0)).to.equal('%00');
            expect(encoder.percentEncodeCharCode(5)).to.equal('%05');
            expect(encoder.percentEncodeCharCode(-0)).to.equal('%00');
            expect(encoder.percentEncodeCharCode(12.00)).to.equal('%0C');
            expect(encoder.percentEncodeCharCode(123)).to.equal('%7B');
            expect(encoder.percentEncodeCharCode(255)).to.equal('%FF');
        });

        it('should return empty string if not in [0x00, 0xFF] range', function () {
            expect(encoder.percentEncodeCharCode(-123)).to.equal('');
            expect(encoder.percentEncodeCharCode(256)).to.equal('');
            expect(encoder.percentEncodeCharCode(28.05)).to.equal('');
            expect(encoder.percentEncodeCharCode(Number.MAX_VALUE)).to.equal('');
        });

        it('should return empty string for non-integers', function () {
            expect(encoder.percentEncodeCharCode(50.50)).to.equal('');
            expect(encoder.percentEncodeCharCode(NaN)).to.equal('');
            expect(encoder.percentEncodeCharCode(Infinity)).to.equal('');
            expect(encoder.percentEncodeCharCode(-Infinity)).to.equal('');
        });

        it('should return empty string on invalid input types', function () {
            expect(encoder.percentEncodeCharCode()).to.equal('');
            expect(encoder.percentEncodeCharCode(null)).to.equal('');
            expect(encoder.percentEncodeCharCode(undefined)).to.equal('');
            expect(encoder.percentEncodeCharCode('123')).to.equal('');
            expect(encoder.percentEncodeCharCode(true)).to.equal('');
            expect(encoder.percentEncodeCharCode(false)).to.equal('');
            expect(encoder.percentEncodeCharCode(Function)).to.equal('');
            expect(encoder.percentEncodeCharCode([Infinity])).to.equal('');
        });
    });
});
