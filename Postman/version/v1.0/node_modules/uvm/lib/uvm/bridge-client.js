/**
 * This is a cross-platform event emitter with bridge interface.
 * It uses Circular JSON as dependency where code is modified slightly to allow loading as a string
 *
 * @note The Circular JSON libraries escape characters have been replaced with variable generated from
 * String.fromCharCode(92);
 */
var STRING = 'string', // constant
    toString = String.prototype.toString; // hold reference to this for security purpose

/**
 * Generate code to be executed inside a VM for bootstrap.
 *
 * @param {String|Buffer} bootCode
 * @return {String}
 */
/* eslint-disable max-len */
module.exports = function (bootCode) {
    return `;
(function (emit) {
    /*! (C) WebReflection Mit Style License */
    var CircularJSON=function(e,t,es){function l(e,t,o){var u=[],f=[e],l=[e],c=[o?n:"[Circular]"],h=e,p=1,d;return function(e,v){return t&&(v=t.call(this,e,v)),e!==""&&(h!==this&&(d=p-a.call(f,this)-1,p-=d,f.splice(p,f.length),u.splice(p-1,u.length),h=this),typeof v=="object"&&v?(a.call(f,v)<0&&f.push(h=v),p=f.length,d=a.call(l,v),d<0?(d=l.push(v)-1,o?(u.push((""+e).replace(s,r)),c[d]=n+u.join(n)):c[d]=c[0]):v=c[d]):typeof v=="string"&&o&&(v=v.replace(r,i).replace(n,r))),v}}function c(e,t){for(var r=0,i=t.length;r<i;e=e[t[r++].replace(o,n)]);return e}function h(e){return function(t,s){var o=typeof s=="string";return o&&s.charAt(0)===n?new f(s.slice(1)):(t===""&&(s=v(s,s,{})),o&&(s=s.replace(u,"$1"+n).replace(i,r)),e?e.call(this,t,s):s)}}function p(e,t,n){for(var r=0,i=t.length;r<i;r++)t[r]=v(e,t[r],n);return t}function d(e,t,n){for(var r in t)t.hasOwnProperty(r)&&(t[r]=v(e,t[r],n));return t}function v(e,t,r){return t instanceof Array?p(e,t,r):t instanceof f?t.length?r.hasOwnProperty(t)?r[t]:r[t]=c(e,t.split(n)):e:t instanceof Object?d(e,t,r):t}function m(t,n,r,i){return e.stringify(t,l(t,n,!i),r)}function g(t,n){return e.parse(t,h(n))}var n="~",r=es+"x"+("0"+n.charCodeAt(0).toString(16)).slice(-2),i=es+r,s=new t(r,"g"),o=new t(i,"g"),u=new t("(?:^|([^"+es+es+"]))"+i),a=[].indexOf||function(e){for(var t=this.length;t--&&this[t]!==e;);return t},f=String;return{stringify:m,parse:g}}(JSON,RegExp,String.fromCharCode(92));

    /*! (C) Postdot Technologies, Inc (Apache-2.0) */
    var arrayProtoSlice = Array.prototype.slice;

    bridge = { // ensure global using no var
        _events: {},
        emit: function (name) {
            var self = this,
                args = arrayProtoSlice.call(arguments, 1);
            this._events[name] && this._events[name].forEach(function (listener) {
                listener.apply(self, args);
            });
        },

        dispatch: function () {
            emit(CircularJSON.stringify(arrayProtoSlice.call(arguments)));
        },

        on: function (name, listener) {
            if (typeof listener !== 'function') { return; }
            !this._events[name] && (this._events[name] = []);
            this._events[name].push(listener);
        },

        off: function (name, listener) {
            var e = this._events[name],
                i = e && e.length || 0;

            if (!e) { return; }
            if (arguments.length === 1) {
                return delete this._events[name];
            }

            if (typeof listener === 'function' && (i >= 1)) {
                while (i >= 0) {
                    (e[i] === listener) && e.splice(i, 1);
                    i -= 1;
                }
            }
            if (!e.length) { delete this._events[name]; }
        }
    };

    // create the dispatch function inside a closure to ensure that actual function references are never modified
    __uvm_dispatch = (function (bridge, bridgeEmit) { // ensure global using no var
        return function (args) {
            bridgeEmit.apply(bridge, CircularJSON.parse(args));
        };
    }(bridge, bridge.emit));

}(__uvm_emit));

// boot code starts hereafter
${(typeof bootCode === STRING) ? toString.call(bootCode) : ''};`;
};
