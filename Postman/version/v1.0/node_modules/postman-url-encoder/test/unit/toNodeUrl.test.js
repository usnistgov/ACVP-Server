const fs = require('fs'),
    path = require('path'),
    expect = require('chai').expect,
    PostmanUrl = require('postman-collection').Url,
    parseCsv = require('@postman/csv-parse/lib/sync'),

    toNodeUrl = require('../../').toNodeUrl;

describe('.toNodeUrl', function () {
    it('should accept url string', function () {
        expect(toNodeUrl('cooper@郵便屋さん.com:399/foo&bar/{baz}?q=("foo")#`hash`'))
            .to.eql({
                protocol: 'http:',
                slashes: true,
                auth: 'cooper',
                host: 'xn--48jwgn17gdel797d.com:399',
                port: '399',
                hostname: 'xn--48jwgn17gdel797d.com',
                hash: '#%60hash%60',
                search: '?q=(%22foo%22)',
                query: 'q=(%22foo%22)',
                pathname: '/foo&bar/%7Bbaz%7D',
                path: '/foo&bar/%7Bbaz%7D?q=(%22foo%22)',
                href: 'http://cooper@xn--48jwgn17gdel797d.com:399/foo&bar/%7Bbaz%7D?q=(%22foo%22)#%60hash%60'
            });
    });

    it('should accept url as PostmanUrl', function () {
        var url = new PostmanUrl({
            host: '127.1',
            protocol: 'postman',
            path: ['f00', '#', 'bär'],
            query: [{ key: 'q', value: '(A & B)' }],
            auth: {
                password: '🔒'
            }
        });

        expect(toNodeUrl(url)).to.eql({
            protocol: 'postman:',
            slashes: false,
            auth: ':%F0%9F%94%92',
            host: '127.0.0.1',
            port: null,
            hostname: '127.0.0.1',
            hash: null,
            search: '?q=(A%20%26%20B)',
            query: 'q=(A%20%26%20B)',
            pathname: '/f00/%23/b%C3%A4r',
            path: '/f00/%23/b%C3%A4r?q=(A%20%26%20B)',
            href: 'postman://:%F0%9F%94%92@127.0.0.1/f00/%23/b%C3%A4r?q=(A%20%26%20B)'
        });
    });

    it('should return same result for string url and PostmanUrl', function () {
        var testCases = fs.readFileSync(path.join(__dirname, '../fixtures/urlList.csv'));

        testCases = parseCsv(testCases, {
            columns: true,
            trim: false
        });

        testCases.forEach(function (testcase) {
            var postmanUrl = new PostmanUrl(testcase.url);

            expect(toNodeUrl(testcase.url), testcase.description).to.eql(toNodeUrl(postmanUrl));
        });
    });

    it('should return empty url object on invalid input types', function () {
        var defaultUrl = {
            protocol: null,
            slashes: null,
            auth: null,
            host: null,
            port: null,
            hostname: null,
            hash: null,
            search: null,
            query: null,
            pathname: null,
            path: null,
            href: ''
        };

        expect(toNodeUrl()).to.eql(defaultUrl);
        expect(toNodeUrl(null)).to.eql(defaultUrl);
        expect(toNodeUrl(undefined)).to.eql(defaultUrl);
        expect(toNodeUrl(true)).to.eql(defaultUrl);
        expect(toNodeUrl({})).to.eql(defaultUrl);
        expect(toNodeUrl([])).to.eql(defaultUrl);
        expect(toNodeUrl(Function)).to.eql(defaultUrl);
        expect(toNodeUrl({ host: 'example.com' })).to.eql(defaultUrl);
    });

    describe('PROPERTY', function () {
        describe('.protocol', function () {
            it('should defaults to http:', function () {
                expect(toNodeUrl('example.com')).to.have.property('protocol', 'http:');
                expect(toNodeUrl(new PostmanUrl({
                    host: 'example.com'
                }))).to.have.property('protocol', 'http:');
            });

            it('should convert to lower case', function () {
                expect(toNodeUrl('HTTP://example.com')).to.have.property('protocol', 'http:');
                expect(toNodeUrl('POSTMAN://example.com')).to.have.property('protocol', 'postman:');
            });

            it('should defaults to http: for non-string protocols', function () {
                expect(toNodeUrl(new PostmanUrl({
                    protocol: { protocol: 'https' }
                }))).to.have.property('protocol', 'http:');
            });

            it('should handle custom protocols', function () {
                expect(toNodeUrl('postman://example.com')).to.have.property('protocol', 'postman:');
            });
        });

        describe('.slashes', function () {
            it('should be true for file:, ftp:, gopher:, http:, and ws: protocols', function () {
                expect(toNodeUrl('file://example.com')).to.have.property('slashes', true);
                expect(toNodeUrl('ftp://example.com')).to.have.property('slashes', true);
                expect(toNodeUrl('gopher://example.com')).to.have.property('slashes', true);
                expect(toNodeUrl('http://example.com')).to.have.property('slashes', true);
                expect(toNodeUrl('https://example.com')).to.have.property('slashes', true);
                expect(toNodeUrl('ws://example.com')).to.have.property('slashes', true);
                expect(toNodeUrl('wss://example.com')).to.have.property('slashes', true);
            });

            it('should be false for custom protocols', function () {
                expect(toNodeUrl('postman://example.com')).to.have.property('slashes', false);
            });
        });

        describe('.auth', function () {
            it('should be null if user info is absent', function () {
                expect(toNodeUrl(new PostmanUrl({
                    host: 'example.com'
                }))).to.have.property('auth', null);

                expect(toNodeUrl('example.com')).to.have.property('auth', null);
            });

            it('should preserve characters case', function () {
                expect(toNodeUrl('UsEr:PaSsWoRd@example.com'))
                    .to.have.property('auth', 'UsEr:PaSsWoRd');
            });

            it('should percent-encode the reserved and unicode characters', function () {
                expect(toNodeUrl('`user`:pâ$$@example.com'))
                    .to.have.property('auth', '%60user%60:p%C3%A2$$');
            });

            it('should not double encode the characters', function () {
                expect(toNodeUrl('%22user%22:p%C3%A2$$@example.com'))
                    .to.have.property('auth', '%22user%22:p%C3%A2$$');
            });

            it('should ignore the empty and non-string username', function () {
                expect(toNodeUrl(new PostmanUrl({
                    host: 'example.com',
                    auth: {
                        user: ['root'],
                        password: 'secret'
                    }
                }))).to.have.property('auth', ':secret');

                expect(toNodeUrl(new PostmanUrl({
                    host: 'example.com',
                    auth: {
                        password: 'secret#123'
                    }
                }))).to.have.property('auth', ':secret%23123');

                expect(toNodeUrl('http://:secret@example.com')).to.have.property('auth', ':secret');
            });

            it('should ignore the empty and non-string password', function () {
                expect(toNodeUrl(new PostmanUrl({
                    host: 'example.com',
                    auth: {
                        user: 'root',
                        password: 12345
                    }
                }))).to.have.property('auth', 'root');

                expect(toNodeUrl(new PostmanUrl({
                    host: 'example.com',
                    auth: {
                        user: 'root@domain.com'
                    }
                }))).to.have.property('auth', 'root%40domain.com');

                expect(toNodeUrl('http://root@example.com')).to.have.property('auth', 'root');
            });
        });

        describe('.host and .hostname', function () {
            it('should be empty string if host and port are absent', function () {
                expect(toNodeUrl(new PostmanUrl({
                    path: '/p/a/t/h'
                }))).to.include({
                    host: '',
                    hostname: ''
                });
            });

            it('should convert to lower case', function () {
                expect(toNodeUrl('EXAMPLE.COM')).to.include({
                    host: 'example.com',
                    hostname: 'example.com'
                });
            });

            it('should do punycode ASCII serialization of the domain', function () {
                expect(toNodeUrl('😎.cool')).to.include({
                    host: 'xn--s28h.cool',
                    hostname: 'xn--s28h.cool'
                });

                expect(toNodeUrl('postman.com')).to.include({
                    host: 'postman.com',
                    hostname: 'postman.com'
                });

                expect(toNodeUrl('郵便屋さん.com')).to.include({
                    host: 'xn--48jwgn17gdel797d.com',
                    hostname: 'xn--48jwgn17gdel797d.com'
                });
            });

            it('should handle the IP address shorthands', function () {
                expect(toNodeUrl('0')).to.include({
                    host: '0.0.0.0',
                    hostname: '0.0.0.0'
                });

                expect(toNodeUrl('1234')).to.include({
                    host: '0.0.4.210',
                    hostname: '0.0.4.210'
                });

                expect(toNodeUrl('127.1')).to.include({
                    host: '127.0.0.1',
                    hostname: '127.0.0.1'
                });

                expect(toNodeUrl('255.255.255')).to.include({
                    host: '255.255.0.255',
                    hostname: '255.255.0.255'
                });

            });

            it('should not double encode hostname', function () {
                expect(toNodeUrl('xn--48jwgn17gdel797d.com')).to.include({
                    host: 'xn--48jwgn17gdel797d.com',
                    hostname: 'xn--48jwgn17gdel797d.com'
                });
            });

            it('should handle invalid hostname', function () {
                expect(toNodeUrl('xn:')).to.include({
                    host: 'xn:',
                    hostname: 'xn:'
                });

                expect(toNodeUrl('xn--iñvalid.com')).to.include({
                    host: 'xn--iñvalid.com',
                    hostname: 'xn--iñvalid.com'
                });
            });

            it('should add port to the host but not to the hostname', function () {
                expect(toNodeUrl('example.com:399')).to.include({
                    host: 'example.com:399',
                    hostname: 'example.com'
                });
            });
        });

        describe('.port', function () {
            it('should be null if port is absent', function () {
                expect(toNodeUrl(new PostmanUrl({
                    host: 'example.com'
                }))).to.have.property('port', null);

                expect(toNodeUrl('example.com')).to.have.property('port', null);
            });

            it('should accept port as string', function () {
                expect(toNodeUrl(new PostmanUrl({
                    host: 'example.com',
                    port: '399'
                }))).to.have.property('port', '399');
            });

            it('should accept port as number', function () {
                expect(toNodeUrl(new PostmanUrl({
                    host: 'example.com',
                    port: 399
                }))).to.have.property('port', '399');
            });
        });

        describe('.hash', function () {
            it('should be null if hash is absent', function () {
                expect(toNodeUrl(new PostmanUrl({
                    host: 'example.com'
                }))).to.have.property('hash', null);

                expect(toNodeUrl('example.com')).to.have.property('hash', null);
            });

            it('should preserve characters case', function () {
                expect(toNodeUrl('example.com#HaSh')).to.have.property('hash', '#HaSh');
            });

            it('should percent-encode the reserved and unicode characters', function () {
                expect(toNodeUrl('example.com#(😎)')).to.have.property('hash', '#(%F0%9F%98%8E)');
            });

            it('should not double encode the characters', function () {
                expect(toNodeUrl('example.com#(%F0%9F%98%8E)')).to.have.property('hash', '#(%F0%9F%98%8E)');
            });

            it('should percent-encode SPACE, ("), (<), (>), and (`)', function () {
                expect(toNodeUrl('0# "<>`')).to.have.property('hash', '#%20%22%3C%3E%60');
            });
        });

        describe('.query and .search', function () {
            it('should be null if query is absent', function () {
                expect(toNodeUrl(new PostmanUrl({
                    host: 'example.com'
                }))).to.include({
                    query: null,
                    search: null
                });

                expect(toNodeUrl('example.com')).to.include({
                    query: null,
                    search: null
                });
            });

            it('should preserve characters case', function () {
                expect(toNodeUrl('example.com?UPPER=CASE&lower=case')).to.include({
                    query: 'UPPER=CASE&lower=case',
                    search: '?UPPER=CASE&lower=case'
                });
            });

            it('should percent-encode the reserved and unicode characters', function () {
                expect(toNodeUrl('example.com?q1=(1 2)&q2=𝌆й你ス')).to.include({
                    query: 'q1=(1%202)&q2=%F0%9D%8C%86%D0%B9%E4%BD%A0%E3%82%B9',
                    search: '?q1=(1%202)&q2=%F0%9D%8C%86%D0%B9%E4%BD%A0%E3%82%B9'
                });
            });

            it('should not double encode the characters', function () {
                expect(toNodeUrl('example.com?q1=(1%202)&q2=f%C3%B2%C3%B3')).to.include({
                    query: 'q1=(1%202)&q2=f%C3%B2%C3%B3',
                    search: '?q1=(1%202)&q2=f%C3%B2%C3%B3'
                });
            });

            it('should percent-encode SPACE, ("), (#), (&), (\'), (<), (=), and (>)', function () {
                expect(toNodeUrl(new PostmanUrl({
                    host: 'example.com',
                    query: [' ', '"', '#', '&', '\'', '<', '=', '>']
                }))).to.include({
                    query: '%20=&%22=&%23=&%26=&%27=&%3C=&%3E=',
                    search: '?%20=&%22=&%23=&%26=&%27=&%3C=&%3E='
                });
            });

            it('should not trim trailing whitespace characters', function () {
                expect(toNodeUrl('example.com?q1=v1 \t\r\n\v\f')).to.include({
                    query: 'q1=v1%20%09%0D%0A%0B%0C',
                    search: '?q1=v1%20%09%0D%0A%0B%0C'
                });
            });
        });

        describe('.path and pathname', function () {
            it('should be `/` if path is absent', function () {
                expect(toNodeUrl(new PostmanUrl({
                    host: 'example.com'
                }))).to.include({
                    path: '/',
                    pathname: '/'
                });

                expect(toNodeUrl('example.com')).to.include({
                    path: '/',
                    pathname: '/'
                });
            });

            it('should preserve characters case', function () {
                expect(toNodeUrl('example.com/UPPER_CASE/lower_case')).to.include({
                    path: '/UPPER_CASE/lower_case',
                    pathname: '/UPPER_CASE/lower_case'
                });
            });

            it('should percent-encode the reserved and unicode characters', function () {
                expect(toNodeUrl('example.com/foo/你ス/(⚡️)')).to.include({
                    path: '/foo/%E4%BD%A0%E3%82%B9/(%E2%9A%A1%EF%B8%8F)',
                    pathname: '/foo/%E4%BD%A0%E3%82%B9/(%E2%9A%A1%EF%B8%8F)'
                });
            });

            it('should not double encode the characters', function () {
                expect(toNodeUrl('example.com/foo/%E4%BD%A0%E3%82%B9/(bar)/')).to.include({
                    path: '/foo/%E4%BD%A0%E3%82%B9/(bar)/',
                    pathname: '/foo/%E4%BD%A0%E3%82%B9/(bar)/'
                });
            });

            it('should percent-encode SPACE, ("), (<), (>), (`), (#), (?), ({), and (})', function () {
                expect(toNodeUrl(new PostmanUrl({
                    host: 'example.com',
                    path: [' ', '"', '<', '>', '`', '#', '?', '{', '}']
                }))).to.include({
                    path: '/%20/%22/%3C/%3E/%60/%23/%3F/%7B/%7D',
                    pathname: '/%20/%22/%3C/%3E/%60/%23/%3F/%7B/%7D'
                });
            });

            it('should not trim trailing whitespace characters', function () {
                expect(toNodeUrl('example.com/path ')).to.include({
                    path: '/path%20',
                    pathname: '/path%20'
                });
            });

            it('should add query to the path but not to the pathname', function () {
                expect(toNodeUrl('example.com/foo?q=bar')).to.include({
                    path: '/foo?q=bar',
                    pathname: '/foo'
                });
            });
        });

        describe('.href', function () {
            it('should percent-encode the reserved and unicode characters', function () {
                expect(toNodeUrl('ròót@郵便屋さん.com/[⚡️]?q1=(1 2)#%foo%')).to.include({
                    href: 'http://r%C3%B2%C3%B3t@xn--48jwgn17gdel797d.com/[%E2%9A%A1%EF%B8%8F]?q1=(1%202)#%foo%'
                });
            });

            it('should not double encode the characters', function () {
                expect(toNodeUrl('postman://xn--48jwgn17gdel797d.com/[%E2%9A%A1%EF%B8%8F]?q1=(1%202)')).to.include({
                    href: 'postman://xn--48jwgn17gdel797d.com/[%E2%9A%A1%EF%B8%8F]?q1=(1%202)'
                });
            });
        });
    });

    describe('SECURITY', function () {
        // Refer: https://www.owasp.org/index.php/Double_Encoding
        it('should not double encode the characters', function () {
            expect(toNodeUrl('%22user%22:p%C3%A2$$@xn--48jwgn17gdel797d.com/%E4%BD?q1=(1%202)#(%F0%9F)')).to.include({
                auth: '%22user%22:p%C3%A2$$',
                host: 'xn--48jwgn17gdel797d.com',
                hostname: 'xn--48jwgn17gdel797d.com',
                pathname: '/%E4%BD',
                path: '/%E4%BD?q1=(1%202)',
                query: 'q1=(1%202)',
                search: '?q1=(1%202)',
                hash: '#(%F0%9F)'
            });
        });

        // eslint-disable-next-line max-len
        // Refer: https://docs.google.com/presentation/d/e/2PACX-1vSTFsJ9t0DatXbjmEGL8sKxt53gf6a1djHp_8Wbj2ZeTB6IfR-HsRD537-L5PgzVrs97bJu1tzJ1Smo/pub?slide=id.g32d0ed6ec2_0_45
        it('should handle encoded hostname', function () {
            expect(toNodeUrl('postman.com%60f.society.org')).to.include({
                host: 'postman.com`f.society.org',
                hostname: 'postman.com`f.society.org'
            });
        });
    });
});
