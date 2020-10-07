//Create event handler
function createEventHandler(dotnetRef, animationName, invokeMethodName) {
    let eventCallback = function (args) {
        //console.log(args);
        let name = args['propertyName'];

        if (!animationName || name === animationName) {
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
function storeEventHandler(dict, element, animationName, eventCallback) {
    let elementFound = false;
    for (let i = 0; i < dict.length; i++) {
        if (dict[i].key === element) {
            if (dict[i].value) {
                dict[i].value.push({ prop: animationName, handler: eventCallback });
            }
            else {
                dict[i].value = [{ prop: animationName, handler: eventCallback }];
            }

            elementFound = true;
            break;
        }
    }

    if (!elementFound) {
        dict.push({
            key: element,
            value: [{ prop: animationName, handler: eventCallback }]
        });
    }
}
//Remove element with event
function removeAndReturnEventHandler(dict, element, animationName) {
    let eventCallback;

    for (let i = 0; i < dict.length; i++) {
        if (dict[i].key === element) {
            let val = dict[i].value;
            for (let j = 0; j < val.length; j++) {
                if (val[j].prop === animationName) {
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

let _animationStartDict = [];
let _animationIterationDict = [];
let _animationEndDict = [];


//animationstart event args:
//////////////////////////////////////
/*
animationName: "mymove"
bubbles: true
cancelBubble: false
cancelable: true
composed: false
currentTarget: null
defaultPrevented: false
elapsedTime: 0
eventPhase: 0
isTrusted: true
path: (5) [div#myDIV, body, html, document, Window]
pseudoElement: ""
returnValue: true
srcElement: div#myDIV
target: div#myDIV
timeStamp: 8376.204999862239
type: "animationstart"
*/

//animationiteration event args:
//////////////////////////////////////
/*
animationName: "mymove"
bubbles: true
cancelBubble: false
cancelable: true
composed: false
currentTarget: null
defaultPrevented: false
elapsedTime: 4
eventPhase: 0
isTrusted: true
path: (5) [div#myDIV, body, html, document, Window]
pseudoElement: ""
returnValue: true
srcElement: div#myDIV
target: div#myDIV
timeStamp: 12359.359999885783
type: "animationiteration"
*/

//animationend event args:
//////////////////////////////////////
/*
animationName: "mymove"
bubbles: true
cancelBubble: false
cancelable: true
composed: false
currentTarget: null
defaultPrevented: false
elapsedTime: 8
eventPhase: 0
isTrusted: true
path: (5) [div#myDIV, body, html, document, Window]
pseudoElement: ""
returnValue: true
srcElement: div#myDIV
target: div#myDIV
timeStamp: 16359.52499997802
type: "animationend"
*/