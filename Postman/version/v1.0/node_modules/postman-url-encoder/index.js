/**
 * Implementation of the WHATWG URL Standard.
 *
 * @example
 * const urlEncoder = require('postman-url-encoder')
 *
 * // Encoding URL string to Node.js compatible Url object
 * urlEncoder.toNodeUrl('郵便屋さん.com/foo&bar/{baz}?q=("foo")#`hash`')
 *
 * // Encoding URI component
 * urlEncoder.encode('qüêry štrìng')
 *
 * // Encoding query string object
 * urlEncoder.encodeQueryString({ q1: 'foo', q2: ['bãr', 'baž'] })
 *
 * @module postman-url-encoder
 * @see {@link https://url.spec.whatwg.org}
 */

const sdk = require('postman-collection'),
    querystring = require('querystring'),

    legacy = require('./legacy'),
    encoder = require('./encoder'),
    QUERY_ENCODE_SET = require('./encoder/encode-set').QUERY_ENCODE_SET,

    E = '',
    COLON = ':',
    STRING = 'string',
    OBJECT = 'object',
    FUNCTION = 'function',
    DOUBLE_SLASH = '//',
    DEFAULT_PROTOCOL = 'http',

    QUERY_SEPARATOR = '?',
    SEARCH_SEPARATOR = '#',
    PROTOCOL_SEPARATOR = '://',
    AUTH_CREDENTIALS_SEPARATOR = '@',

    /**
     * Protocols that always contain a // bit.
     *
     * @private
     * @see {@link https://github.com/nodejs/node/blob/v10.17.0/lib/url.js#L91}
     */
    SLASHED_PROTOCOLS = {
        'file:': true,
        'ftp:': true,
        'gopher:': true,
        'http:': true,
        'https:': true,
        'ws:': true,
        'wss:': true
    };

/**
 * Percent-encode the given string using QUERY_ENCODE_SET.
 *
 * @deprecated since version 2.0, use {@link encodeQueryParam} instead.
 *
 * @example
 * // returns 'foo%20%22%23%26%27%3C%3D%3E%20bar'
 * encode('foo "#&\'<=> bar')
 *
 * // returns ''
 * encode(['foobar'])
 *
 * @param {String} value String to percent-encode
 * @returns {String} Percent-encoded string
 */
function encode (value) {
    return encoder.percentEncode(value, QUERY_ENCODE_SET);
}

/**
 * Percent-encode the URL query string or x-www-form-urlencoded body object
 * according to RFC3986.
 *
 * @example
 * // returns 'q1=foo&q2=bar&q2=baz'
 * encodeQueryString({ q1: 'foo', q2: ['bar', 'baz'] })
 *
 * @param {Object} query Object representing query or urlencoded body
 * @returns {String} Percent-encoded string
 */
function encodeQueryString (query) {
    if (!(query && typeof query === OBJECT)) {
        return E;
    }

    // rely upon faster querystring module
    query = querystring.stringify(query);

    // encode characters not encoded by querystring.stringify() according to RFC3986.
    return query.replace(/[!'()*]/g, function (c) {
        return encoder.percentEncodeCharCode(c.charCodeAt(0));
    });
}

/**
 * Converts PostmanUrl / URL string into Node.js compatible Url object.
 *
 * @example <caption>Using URL string</caption>
 * toNodeUrl('郵便屋さん.com/foo&bar/{baz}?q=("foo")#`hash`')
 * // returns
 * // {
 * //     protocol: 'http:',
 * //     slashes: true,
 * //     auth: null,
 * //     host: 'xn--48jwgn17gdel797d.com',
 * //     port: null,
 * //     hostname: 'xn--48jwgn17gdel797d.com',
 * //     hash: '#%60hash%60',
 * //     search: '?q=(%22foo%22)',
 * //     query: 'q=(%22foo%22)',
 * //     pathname: '/foo&bar/%7Bbaz%7D',
 * //     path: '/foo&bar/%7Bbaz%7D?q=(%22foo%22)',
 * //     href: 'http://xn--48jwgn17gdel797d.com/foo&bar/%7Bbaz%7D?q=(%22foo%22)#%60hash%60'
 * //  }
 *
 * @example <caption>Using PostmanUrl instance</caption>
 * toNodeUrl(new sdk.Url({
 *     host: 'example.com',
 *     query: [{ key: 'foo', value: 'bar & baz' }]
 * }))
 *
 * @param {PostmanUrl|String} url
 * @returns {Url}
 */
function toNodeUrl (url) {
    var nodeUrl = {
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
        href: E
    };

    // convert URL string to PostmanUrl
    if (typeof url === STRING) {
        url = new sdk.Url(url);
    }

    // bail out if given url is not a PostmanUrl instance
    if (!sdk.Url.isUrl(url)) {
        return nodeUrl;
    }

    // #protocol
    nodeUrl.protocol = (typeof url.protocol === STRING) ?
        url.protocol.replace(PROTOCOL_SEPARATOR, E).toLowerCase() :
        DEFAULT_PROTOCOL;
    nodeUrl.protocol += COLON;

    // #slashes
    nodeUrl.slashes = SLASHED_PROTOCOLS[nodeUrl.protocol] || false;

    // #href = protocol://
    nodeUrl.href = nodeUrl.protocol + DOUBLE_SLASH;

    // #auth
    if (url.auth) {
        if (typeof url.auth.user === STRING) {
            nodeUrl.auth = encoder.encodeUserInfo(url.auth.user);
        }
        if (typeof url.auth.password === STRING) {
            !nodeUrl.auth && (nodeUrl.auth = E);
            nodeUrl.auth += COLON + encoder.encodeUserInfo(url.auth.password);
        }

        // #href = protocol://user:password@
        nodeUrl.auth && (nodeUrl.href += nodeUrl.auth + AUTH_CREDENTIALS_SEPARATOR);
    }

    // #host, #hostname
    nodeUrl.host = nodeUrl.hostname = encoder.encodeHost(url.getHost()).toLowerCase();

    // #href = protocol://user:password@host.name
    nodeUrl.href += nodeUrl.hostname;

    // @todo Add helper in SDK to normalize port
    if (typeof (url.port && url.port.toString) === FUNCTION) {
        // #port
        nodeUrl.port = url.port.toString();

        // #host = (#hostname):(#port)
        nodeUrl.host = nodeUrl.hostname + COLON + nodeUrl.port;

        // #href = protocol://user:password@host.name:port
        nodeUrl.href += COLON + nodeUrl.port;
    }

    // #path, #pathname
    nodeUrl.path = nodeUrl.pathname = encoder.encodePath(url.getPath());

    // #href = protocol://user:password@host.name:port/p/a/t/h
    nodeUrl.href += nodeUrl.pathname;

    if (url.query.count()) {
        // #query
        nodeUrl.query = encoder.encodeQueryParams(url.query.all());

        // #search
        nodeUrl.search = QUERY_SEPARATOR + nodeUrl.query;

        // #path = (#pathname)?(#search)
        nodeUrl.path = nodeUrl.pathname + nodeUrl.search;

        // #href = protocol://user:password@host.name:port/p/a/t/h?q=query
        nodeUrl.href += nodeUrl.search;
    }

    if (url.hash) {
        // #hash
        nodeUrl.hash = SEARCH_SEPARATOR + encoder.encodeFragment(url.hash);

        // #href = protocol://user:password@host.name:port/p/a/t/h?q=query#hash
        nodeUrl.href += nodeUrl.hash;
    }

    return nodeUrl;
}

/**
 * Converts URL string into Node.js compatible Url object using the v1 encoder.
 *
 * @deprecated since version 2.0
 *
 * @param {String} url URL string
 * @returns {Url} Node.js compatible Url object
 */
function toLegacyNodeUrl (url) {
    return legacy.toNodeUrl(url);
}

module.exports = {
    encode,
    toNodeUrl,
    toLegacyNodeUrl,
    encodeQueryString
};
