Simplest Possible Logging
=========================

`console.log` and `console.error` is really enough and the world has
too many log frameworks. This library should not be used, but if you
decide to anyway, you get the following:

* Timestamped log lines in local time zone.
* Log levels: `DEBUG`, `INFO`, `WARN`, `ERROR`
* Possible to redirect.
* Reasonable defaults.
* Singleton

## Defaults

The default log level is `INFO`. Debug messages are not shown. Info,
warnings and errors are.

By default `DEBUG` and `INFO` go to `console.log` (and thus `stdout`),
while `WARN` and `ERROR` go to `console.error` (thus `stderr`).

*** 1.0.0 - changed to local time zone. Earlier versions times were always in Zulu.

The default time format is
[ISO8601](http://en.wikipedia.org/wiki/ISO_8601) in the local time zone.

## Install
```
npm install bog
```

## Example

```javascript
var log = require('bog');

log.info('Something wonderful has happened Your AMIGA is alive !!!');
```

Would produce:

```
2013-06-13 19:05:35.482 INFO Something wonderful has happened Your AMIGA is alive !!!
```

## API

### log.debug(args...)

Same as `log.info`.

### log.info(args...)

Outputs logging at the `INFO` level. `args` is one or many arguments
which is passed to the output function.

#### Example

```javascript
log.info('and, even', 'better...');
```

Outputs

```
2013-06-13 19:05:35.482 INFO and even better...
```

N.b. The above is conceptually the same as

```javascript
console.log((new Date()).toISOString(), 'INFO', 'and, even', 'better...');
```

### log.warn(args...)

Same as `log.info`, but to `stderr`.

### log.error(args...)

Same as `log.info`, but to `stderr`.

### log.level(level)

Sets the lowest reported log level, for example `warn` would enable
`warn` and `error`, and disable `debug` and `info`.

#### Example:

```javascript
log.level('warn');
```

### log.redirect(out, err)

`out` is the output *function* for `DEBUG` and `INFO` and `err` is the
output *function* for `WARN` and `ERROR`. `null` is appropriate to
turn off output.

#### Example:

```javascript
log.redirect(null, null); // silences all logging
...
log.redirect(console.log, console.error); // reinstates defaults
```

### log.config()

Spills the beans of the internal config. Can be used to alter the formatting.

#### Example

```javascript
log.config().format = function(level, args) {
    if (level === 'info') {
        args.unshift('Prepended');
        args.push('Appended');
    }
    return args;
};
```

### Enable ISO8601 time designator 'T' and timezone

*** Since 1.0.0

It's possible to turn on the time designator and timezone.

```javascript
log.config().includeTimeDesignator = true;
log.config().includeTimeZone = true;

log.info('Something wonderful has happened Your AMIGA is alive !!!');
```

If machine timezone is India this would produce:

```
2013-06-13T19:05:35.482+0530 INFO Something wonderful has happened Your AMIGA is alive !!!
```

Notice that zulu (GMT) time is denoted by a 'Z'.

## License

MIT License (MIT).
