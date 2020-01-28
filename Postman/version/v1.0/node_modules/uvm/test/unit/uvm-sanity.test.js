var async = require('async'),
    isNode = (typeof window === 'undefined');

describe('uvm', function () {
    var uvm = require('../../lib');

    it('must connect a new context', function (done) {
        uvm.spawn({}, done);
    });

    describe('context', function () {
        it('must have event emitter interface and .dispatch method', function () {
            var context = uvm.spawn();

            expect(context).be.ok();
            expect(context).to.have.property('dispatch');
            expect(context.dispatch).be.a('function');
            expect(context).to.have.property('emit');
            expect(context.emit).be.a('function');
            expect(context).to.have.property('on');
            expect(context.on).be.a('function');
            expect(context).to.have.property('disconnect');
            expect(context.disconnect).be.a('function');
        });

        it('must allow dispatching events to context', function () {
            var context = uvm.spawn();

            context.dispatch();
            context.dispatch('event-name');
            context.dispatch('event-name', 'event-arg');
        });

        it('must allow receiving events in context', function (done) {
            var sourceData = 'test',
                context = uvm.spawn({
                    bootCode: `
                        bridge.on('loopback', function (data) {
                            bridge.dispatch('loopback', data);
                        });
                    `
                });

            context.on('loopback', function (data) {
                expect(data).be('test');
                done();
            });

            context.dispatch('loopback', sourceData);
        });

        (isNode ? it : it.skip)('must pass load error on broken boot code', function (done) {
            uvm.spawn({
                bootCode: `
                    throw new Error('error in bootCode');
                `
            }, function (err) {
                expect(err).be.an('object');
                expect(err).have.property('message', 'error in bootCode');
                done();
            });
        });

        it('must not overflow dispatches when multiple vm is run', function (done) {
            // create two vms
            async.times(2, function (n, next) {
                uvm.spawn({
                    bootCode: `
                        bridge.on('loopback', function (data) {
                            bridge.dispatch('loopback', ${n}, data);
                        });
                    `
                }, next);
            }, function (err, contexts) {
                if (err) { return done(err); }

                contexts[0].on('loopback', function (source, data) {
                    expect(source).be(0);
                    expect(data).be('zero');

                    setTimeout(done, 100); // wait for other events before going done
                });

                contexts[1].on('loopback', function () {
                    expect.fail('second context receiving message overflowed from first');
                });

                contexts[0].dispatch('loopback', 'zero');

            });
        });

        it('must restore dispatcher if it is deleted', function (done) {
            uvm.spawn({
                bootCode: `
                    bridge.on('deleteDispatcher', function () {
                        __uvm_dispatch = null;
                    });

                    bridge.on('loopback', function (data) {
                        bridge.dispatch('loopback', data);
                    });
                `
            }, function (err, context) {
                expect(err).not.be.an('object');

                context.on('error', done);
                context.on('loopback', function (data) {
                    expect(data).be('this returns');
                    done();
                });

                context.dispatch('deleteDispatcher');
                context.dispatch('loopback', 'this returns');
            });
        });

        it('must trigger error if dispatched post disconnection', function (done) {
            uvm.spawn({
                bootCode: `
                    bridge.on('loopback', function (data) {
                        bridge.dispatch('loopback', data);
                    });
                `
            }, function (err, context) {
                expect(err).not.be.an('object');

                context.on('error', function (err) {
                    expect(err).be.ok();
                    expect(err).have.property('message', 'uvm: unable to dispatch "loopback" post disconnection.');
                    done();
                });

                context.on('loopback', function () {
                    throw new Error('loopback callback was unexpected post disconnection');
                });

                context.disconnect();
                context.dispatch('loopback', 'this never returns');
            });
        });
    });
});
