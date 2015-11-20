var log = require('./bog.js');

log.info('Something wonderful has happened Your AMIGA is alive !!!');

// enable 'T' and timezone
log.config().includeTimeDesignator = true;
log.config().includeTimeZone = true;

log.info('Something wonderful has happened Your AMIGA is alive !!!');

log.config().format = function (level, args) {
    if (level === 'warn') {
        args.unshift('pre');
        args.push('post');
    }
    return args;
};

log.warn('this');
