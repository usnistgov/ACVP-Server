var encodeQueryString = require('../..').encodeQueryString;

suite('encodeQueryString()', function () {
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
        encodeQueryString(queryArray);
    });

    scenario('with query as object', function () {
        encodeQueryString(queryObject);
    });

    scenario('with multi value query object', function () {
        encodeQueryString(multiValuedQueryObject);
    });

    scenario('with long multivalue array', function () {
        encodeQueryString(longMultiValueQuery);
    });
});
