var encoder = require('../../encoder');

suite('encodeHost()', function () {
    var arrayHost = ['foo', 'bar', 'baz', 'com'],
        stringHost = 'foo.bar.baz.com',
        unicodeHost = '😎.郵便屋さん.com',
        encodedHost = 'xn--s28h.xn--48jwgn17gdel797d.com';

    scenario('with host as array', function () {
        encoder.encodeHost(arrayHost);
    });

    scenario('with host as string', function () {
        encoder.encodeHost(stringHost);
    });

    scenario('with unicode host', function () {
        encoder.encodeHost(unicodeHost);
    });

    scenario('with punycode encoded host', function () {
        encoder.encodeHost(encodedHost);
    });
});

suite('encodePath()', function () {
    var arrayPath = ['foo', 'bar', 'baz'],
        stringPath = '/foo/bar/baz',
        unicodePath = '/😎/郵便屋さん',
        encodedPath = '/%F0%9F%98%8E/%E9%83%B5%E4%BE%BF%E5%B1%8B%E3%81%95%E3%82%93',
        longPath = '/this/is/a/very/very/very/loooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooong/path';

    scenario('with path as array', function () {
        encoder.encodePath(arrayPath);
    });

    scenario('with path as string', function () {
        encoder.encodePath(stringPath);
    });

    scenario('with unicode path', function () {
        encoder.encodePath(unicodePath);
    });

    scenario('with encoded path', function () {
        encoder.encodePath(encodedPath);
    });

    scenario('with long path', function () {
        encoder.encodePath(longPath);
    });
});

suite('encodeQueryParams()', function () {
    var queryArray = ['v1', '(v2)', '你ス'],
        queryObject = {
            q1: 'v1',
            q2: '(v2)',
            q3: '你ス'
        },
        multiValuedQueryObject = {
            q1: ['𝌆й', '你ス'],
            q2: ['foo', 'bar'],
            q3: 'v3'
        },
        longMultiValueQuery = {
            q1: ['v1', 'v2', 'v3', 'v4', 'v5', 'v6', 'v7', 'v8', 'v9', 'v10', 'foo', 'bar', 'baz', '𝌆й', '你ス', '😎']
        };

    scenario('with query as array', function () {
        encoder.encodeQueryParams(queryArray);
    });

    scenario('with query as object', function () {
        encoder.encodeQueryParams(queryObject);
    });

    scenario('with multi value query object', function () {
        encoder.encodeQueryParams(multiValuedQueryObject);
    });

    scenario('with long multivalue array', function () {
        encoder.encodeQueryParams(longMultiValueQuery);
    });
});

suite('percentEncode()', function () {
    var asciiString = 'a quick brown fox jumps over the lazy dog. A QUICK BROWN FOX JUMPS OVER THE LAZY DOG.',
        encodedString = '%E9%80%9F%E3%81%84%E8%8C%B6%E8%89%B2%E3%81%AE%E3%82%AD%E3%83%84',
        unicodeString = '速い茶色のキツネが怠zyな犬を飛び越えます。一只快速的棕色狐狸跳过了那只懒狗。',
        customEncodeSet = new encoder.EncodeSet(['a', 'b', 'c', 'd', 'e', 'f']);

    scenario('with ASCII string', function () {
        encoder.percentEncode(asciiString);
    });

    scenario('with encoded string', function () {
        encoder.percentEncode(encodedString);
    });

    scenario('with unicode string', function () {
        encoder.percentEncode(unicodeString);
    });

    scenario('with custom encode set', function () {
        encoder.percentEncode(asciiString, customEncodeSet);
    });
});
