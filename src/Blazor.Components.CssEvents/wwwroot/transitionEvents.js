//Create event handler
function createEventHandler(dotnetRef, transitionPropertyName, invokeMethodName) {
    let eventCallback = function (args) {
        //console.log(args);
        let name = args['propertyName'];

        if (!transitionPropertyName || name === transitionPropertyName) {
            let props = {
                Composed: args.composed,
                ElapsedTime: args.elapsedTime,
                EventPhase: args.eventPhase,
                //Path: args.path.toString(),  //Throws serialization exception for Element
                PropertyName: args.propertyName,
                ReturnValue: args.returnValue,
                //Target: JSON.stringify(args.target), //Throws serialization exception for Element
                Type: args.type
            }

            dotnetRef.invokeMethodAsync(invokeMethodName, props);
        }
    }

    return eventCallback;
}
//Store element with event
function storeEventHandler(dict, element, transitionPropertyName, eventCallback) {
    let elementFound = false;
    for (let i = 0; i < dict.length; i++) {
        if (dict[i].key === element) {
            if (dict[i].value) {
                dict[i].value.push({ prop: transitionPropertyName, handler: eventCallback });
            }
            else {
                dict[i].value = [{ prop: transitionPropertyName, handler: eventCallback }];
            }

            elementFound = true;
            break;
        }
    }

    if (!elementFound) {
        dict.push({
            key: element,
            value: [{ prop: transitionPropertyName, handler: eventCallback }]
        });
    }
}
//Remove element with event
function removeAndReturnEventHandler(dict, element, transitionPropertyName) {
    let eventCallback;

    for (let i = 0; i < dict.length; i++) {
        if (dict[i].key === element) {
            let val = dict[i].value;
            for (let j = 0; j < val.length; j++) {
                if (val[j].prop === transitionPropertyName) {
                    eventCallback = val[j].handler; //associate callback
                    dict[i].value.splice(j, 1); //remove handler

                    if (dict[i].value.length === 0) {
                        dict.splice(i, 1);
                    }
                    break;
                }
            }
            break;
        }
    }

    return eventCallback;
}

let _transitionHandlerDict = [];

export function addTransitionEnd(element, dotnetRef, transitionPropertyName) {
    let eventCallback = createEventHandler(dotnetRef, transitionPropertyName, 'TransitionEvent');
    storeEventHandler(_transitionHandlerDict, element, transitionPropertyName, eventCallback);

    // Code for Safari 3.1 to 6.0
    element.addEventListener("webkitTransitionEnd", eventCallback);
    // Standard syntax
    element.addEventListener("transitionend", eventCallback);

    return eventCallback;
}

export function removeTransitionEnd(element, transitionPropertyName) {
    let eventCallback = removeAndReturnEventHandler(_transitionHandlerDict, element, transitionPropertyName);

    if (!eventCallback) {
        return; //No event handler found
    }

    // Code for Safari 3.1 to 6.0
    element.removeEventListener("webkitTransitionEnd", eventCallback);
    // Standard syntax
    element.removeEventListener("transitionend", eventCallback);
}

export function dispose(elementsWithPropDictionary) {
    if (elementsWithPropDictionary) {
        for (var i = 0; i < elementsWithPropDictionary.length; i++) {
            removeTransitionEnd(elementsWithPropDictionary[i].key, elementsWithPropDictionary[i].value);
        }
    }
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