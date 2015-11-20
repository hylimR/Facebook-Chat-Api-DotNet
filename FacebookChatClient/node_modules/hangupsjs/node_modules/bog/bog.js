/*global process, module*/
/*jshint -W086*/ // no `break` before `case` is OK

// singleton, hashed by pid
var config = {};

// default setup
config[process.pid] = {
    debug: {on: false, out: console.log},
    info: {on: true, out: console.log},
    warn: {on: true, out: console.error},
    error: {on: true, out: console.error},
    includeTimeDesignator: false,
    includeTimeZone: false,
    format: function(level, args) {
        args.unshift(level.toUpperCase());
        args.unshift(localISOString());
        return args;
    }
};

var log = function (level, args) {
    var conf = config[process.pid], _ref;
    if (!(_ref = conf[level]).on || !_ref.out) return;
    args = conf.format(level, args);
    _ref.out.apply(null, args);
};

var slice = Array.prototype.slice;

var debug = function() {
    log('debug', slice.call(arguments));
};
var info = function() {
    log('info', slice.call(arguments));
};
var warn = function() {
    log('warn', slice.call(arguments));
};
var error = function() {
    log('error', slice.call(arguments));
};

var level = function (l) {
    var s = config[process.pid];
    [s.debug, s.info, s.warn, s.error].forEach(function(item) {
        item.on = false;
    });
    switch (l) {
        case 'debug':
        s.debug.on = true;
        case 'info':
        s.info.on = true;
        case 'warn':
        s.warn.on = true;
        case 'error':
        s.error.on = true;
    }
};

var redirect = function (out, err) {
    var s = config[process.pid];
    s.debug.out = out;
    s.info.out = out;
    s.warn.out = err;
    s.error.out = err;
};

// ISO8601 in local time zone
var localISOString = function() {

	var s = config[process.pid]
        , d = new Date()
		, pad = function (n){return n<10 ? '0'+n : n;}
		, tz = typeof s.fixedTimeZoneMinutes === 'number' ? s.fixedTimeZoneMinutes :
            d.getTimezoneOffset() // mins
		, tzs = (tz>0?"-":"+") + pad(parseInt(Math.abs(tz/60)));

    tzs += tz%60 == 0 ? '00' : pad(Math.abs(tz%60));

	if (tz === 0) // Zulu time == UTC
		tzs = 'Z';

	 return d.getFullYear()+'-'
	      + pad(d.getMonth()+1)+'-'
	      + pad(d.getDate())+(s.includeTimeDesignator ? 'T' : ' ')+
	      + pad(d.getHours())+':'
	      + pad(d.getMinutes())+':'
	      + pad(d.getSeconds()) + (s.includeTimeZone ? tzs : '');
};

module.exports = {
    debug: debug,
    info: info,
    warn: warn,
    error: error,
    level: level,
    redirect: redirect,
    config: function() { return config[process.pid]; }
};
