const expect = require('chai').expect,

    EncodeSet = require('../../../encoder').EncodeSet,
    encodeSet = require('../../../encoder/encode-set'),

    doesExtends = function (originalSet, extendedSet) {
        var i;

        for (i = 0; i < 128; i++) {
            if (originalSet.has(i) && !extendedSet.has(i)) {
                return false;
            }
        }

        return true;
    };

describe('constants', function () {
    describe('C0_CONTROL_ENCODE_SET', function () {
        const SET = encodeSet.C0_CONTROL_ENCODE_SET;

        it('should be sealed EncodeSet', function () {
            expect(SET).to.be.an.instanceof(EncodeSet);
            expect(SET.add(65).has(65)).to.be.false;
        });

        it('should encode C0 control codes', function () {
            var i,
                flag = 1;

            for (i = 0; i < 32; i++) {
                flag &= SET.has(i);
            }

            flag &= SET.has(127);

            expect(flag).to.equal(1);
        });

        it('should not encode printable ASCII codes [32, 126]', function () {
            var i,
                flag = 0;

            for (i = 32; i < 127; i++) {
                flag |= SET.has(i);
            }

            expect(flag).to.equal(0);
        });
    });

    describe('FRAGMENT_ENCODE_SET', function () {
        const SET = encodeSet.FRAGMENT_ENCODE_SET;

        it('should be sealed EncodeSet', function () {
            expect(SET).to.be.an.instanceof(EncodeSet);
            expect(SET.add(65).has(65)).to.be.false;
        });

        it('should extend C0_CONTROL_ENCODE_SET', function () {
            expect(doesExtends(encodeSet.C0_CONTROL_ENCODE_SET, SET)).to.be.true;
        });

        it('should be extend by SPACE, ("), (<), (>), and (`)', function () {
            var i,
                chars = [],
                expected = [' ', '"', '<', '>', '`'];

            for (i = 32; i < 127; i++) {
                SET.has(i) && chars.push(String.fromCharCode(i));
            }

            expect(chars).to.have.all.members(expected);
        });
    });

    describe('PATH_ENCODE_SET', function () {
        const SET = encodeSet.PATH_ENCODE_SET;

        it('should be sealed EncodeSet', function () {
            expect(SET).to.be.an.instanceof(EncodeSet);
            expect(SET.add(65).has(65)).to.be.false;
        });

        it('should extend FRAGMENT_ENCODE_SET', function () {
            expect(doesExtends(encodeSet.FRAGMENT_ENCODE_SET, SET)).to.be.true;
        });

        it('should be extend by (#), (?), ({), and (})', function () {
            var i,
                chars = [],
                expected = ['#', '?', '{', '}'];

            for (i = 32; i < 127; i++) {
                SET.has(i) && chars.push(String.fromCharCode(i));
            }

            expect(chars).to.include.members(expected);
        });
    });

    describe('USERINFO_ENCODE_SET', function () {
        const SET = encodeSet.USERINFO_ENCODE_SET;

        it('should be sealed EncodeSet', function () {
            expect(SET).to.be.an.instanceof(EncodeSet);
            expect(SET.add(65).has(65)).to.be.false;
        });

        it('should extend PATH_ENCODE_SET', function () {
            expect(doesExtends(encodeSet.PATH_ENCODE_SET, SET)).to.be.true;
        });

        it('should be extend by (/), (:), (;), (=), (@), ([), (\\), (]), (^), and (|)', function () {
            var i,
                chars = [],
                expected = ['/', ':', ';', '=', '@', '[', '\\', ']', '^', '|'];

            for (i = 32; i < 127; i++) {
                SET.has(i) && chars.push(String.fromCharCode(i));
            }

            expect(chars).to.include.members(expected);
        });
    });

    describe('QUERY_ENCODE_SET', function () {
        const SET = encodeSet.QUERY_ENCODE_SET;

        it('should be sealed EncodeSet', function () {
            expect(SET).to.be.an.instanceof(EncodeSet);
            expect(SET.add(65).has(65)).to.be.false;
        });

        it('should extend C0_CONTROL_ENCODE_SET', function () {
            expect(doesExtends(encodeSet.C0_CONTROL_ENCODE_SET, SET)).to.be.true;
        });

        it('should be extend by SPACE, ("), (#), (\'), (<), and (>)', function () {
            var i,
                chars = [],
                expected = [' ', '"', '#', '\'', '<', '>'];

            for (i = 32; i < 127; i++) {
                SET.has(i) && chars.push(String.fromCharCode(i));
            }

            expect(chars).to.include.members(expected);
        });
    });
});
