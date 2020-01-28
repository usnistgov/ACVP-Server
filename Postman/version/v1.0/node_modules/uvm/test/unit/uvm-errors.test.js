describe('uvm errors', function () {
    var uvm = require('../../lib');

    it('must raise an error if sandbox disconnect is somehow broken', function (done) {
        var context = uvm.spawn();

        // delete context._disconnect to further sabotage the bridge
        delete context._disconnect;

        context.on('error', function (err) {
            expect(err).be.ok();
            expect(err).to.have.property('message', 'uvm: cannot disconnect, communication bridge is broken');
            done();
        });

        context.disconnect(null);
    });

    it('must dispatch cyclic object', function (done) {
        var context = uvm.spawn({
                bootCode: `
                    bridge.on('transfer', function (data) {
                        bridge.dispatch('transfer', data);
                    });
                `
            }),

            cyclic,
            subcycle;

        context.on('error', done);
        context.on('transfer', function (data) {
            expect(data).be.an('object');
            expect(data).have.property('child');
            expect(data.child).have.property('parent');
            expect(data.child.parent).eql(data);
            done();
        });

        // create a cyclic object
        cyclic = {};
        subcycle = { parent: cyclic };
        cyclic.child = subcycle;

        context.dispatch('transfer', cyclic);
    });

    it('must not allow bridge raw interfaces to be accessed', function (done) {
        uvm.spawn({
            bootCode: `
                bridge.on('probe', function () {
                    bridge.dispatch('result', {
                        typeofEmitter: typeof __uvm_emit,
                        typeofDispatcher: typeof __uvm_dispatch
                    });
                });
            `
        }, function (err, context) {
            if (err) { return done(err); }
            context.on('error', done);
            context.on('result', function (test) {
                expect(test).be.an('object');
                expect(test).not.have.property('typeofEmitter', 'function');
                expect(test).not.have.property('typeofDispatcher', 'function');
                done();
            });
            context.dispatch('probe');
        });
    });

    it('must allow escape sequences in arguments to be dispatched', function (done) {
        uvm.spawn({
            bootCode: `
                bridge.on('loopback', function (data) {
                    bridge.dispatch('loopback', data);
                });
            `
        }, function (err, context) {
            expect(err).not.be.an('object');

            context.on('error', done);
            context.on('loopback', function (data) {
                // eslint-disable-next-line no-useless-escape
                expect(data).be('this has \n "escape" \'characters\"');
                done();
            });

            // eslint-disable-next-line no-useless-escape
            context.dispatch('loopback', 'this has \n "escape" \'characters\"');
        });
    });
});
