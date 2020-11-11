export function consoleLogger(message, logLevel) {
    switch (logLevel) {
        case 0: //Trace
        case 6: //None
            console.trace(message);
            break;
        case 1: //'Debug'
            console.debug(message);
            break;
        case 3: //'Warning'
            console.warn(message);
            break;
        case 4: //'Error':
        case 5: //'Critical':
            console.error(message);
            break;
    }
}