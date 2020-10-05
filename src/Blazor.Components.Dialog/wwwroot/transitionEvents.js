function createEventHandler(dotnetRef, transitionPropertyName, invokeMethodName) {
    let eventCallback = function (args) {
        //console.log(args);
        let name = args['propertyName'];

        if (!transitionPropertyName || name === transitionPropertyName) {
            let props = {
                Composed: args.composed,
                ElapsedTime: args.elapsedTime,
                EventPhase: args.eventPhase,
                //"Path": args.path,  //Throws serialization exception for Element
                PropertyName: args.propertyName,
                ReturnValue: args.returnValue,
                //Target: args.target, //Throws serialization exception for Element
                Type: args.type
            }

            dotnetRef.invokeMethodAsync(invokeMethodName, props);
        }
    }

    return eventCallback;
}

let _transitionHandlerDict = [];

export function addTransitionEnd(element, dotnetRef, transitionPropertyName) {
    let eventCallback = createEventHandler(dotnetRef, transitionPropertyName, 'TransitionEnded');

    let elementFound = false;
    for (let i = 0; i < _transitionHandlerDict.length; i++) {
        if (_transitionHandlerDict[i].key === element) {
            if (_transitionHandlerDict[i].value) {
                _transitionHandlerDict[i].value.push({ prop: transitionPropertyName, handler: eventCallback });
            }
            else {
                _transitionHandlerDict[i].value = [{ prop: transitionPropertyName, handler: eventCallback }];
            }

            elementFound = true;
            break;
        }
    }

    if (!elementFound) {
        _transitionHandlerDict.push({
            key: element,
            value: [{ prop: transitionPropertyName, handler: eventCallback }]
        });
    }

    // Code for Safari 3.1 to 6.0
    element.addEventListener("webkitTransitionEnd", eventCallback);
    // Standard syntax
    element.addEventListener("transitionend", eventCallback);
}

export function removeTransitionEnd(element, transitionPropertyName) {
    let eventCallback;

    for (let i = 0; i < _transitionHandlerDict.length; i++) {
        if (_transitionHandlerDict[i].key === element) {
            let val = _transitionHandlerDict[i].value;
            for (let j = 0; i < val.length; j++) {
                if (val[j].prop === transitionPropertyName) {
                    eventCallback = val[j].handler; //associate callback
                    _transitionHandlerDict[i].value.splice(j, 1); //remove handler
                    break;
                }
            }

            break;
        }
    }

    if (!eventCallback) {
        return; //No event handler found
    }

    // Code for Safari 3.1 to 6.0
    element.removeEventListener("webkitTransitionEnd", eventCallback);
    // Standard syntax
    element.removeEventListener("transitionend", eventCallback);
}

export function dispose() {
    _transitionHandlerDict = [];
}

//transitionend event args:
/*
bubbles: true
cancelBubble: false
cancelable: true
composed: false
currentTarget: null
defaultPrevented: false
elapsedTime: 1
eventPhase: 0
isTrusted: true
path: (5)[div#myDIV, body, html, document, Window]
propertyName: "width"
pseudoElement: ""
returnValue: true
srcElement: div#myDIV
target: div#myDIV
timeStamp: 4927.3449999745935
type: "transitionend"
*/