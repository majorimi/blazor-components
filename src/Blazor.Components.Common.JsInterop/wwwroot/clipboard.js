export function copyTextToClipboard(text) {
    if (!navigator.clipboard) {
        return fallbackCopyTextToClipboard(text);
    }

    navigator.clipboard.writeText(text).then(function () {
        //console.log('Async: Copying to clipboard was successful!');
    }, function (err) {
        //console.error('Async: Could not copy text: ', err);
        throw err;
    });

    return true; //method end but Promise is async...
}
function fallbackCopyTextToClipboard(text) {
    let textArea = document.createElement("textarea");
    textArea.value = text;

    // Avoid scrolling to bottom
    textArea.style.top = "0";
    textArea.style.left = "0";
    textArea.style.position = "fixed";

    document.body.appendChild(textArea);
    textArea.focus();
    textArea.select();

    let result;
    try {
        result = document.execCommand('copy');
        //console.log('Fallback: Copying text command was ' + res ? 'successful' : 'unsuccessful');
    } catch (err) {
        //console.error('Fallback: Unable to copy', err);
        return false;
    }

    document.body.removeChild(textArea);
    return result;
}

export function copyElementTextToClipboard(element) {
    if (!element) {
        return false;
    }

    // Select the text field
    element.select();
    element.setSelectionRange(0, 99999); /*For mobile devices*/

    // Copy the text inside the text field
    let result = document.execCommand("copy");

    //Deselect
    if ('selectionStart' in element) {
        element.selectionEnd = element.selectionStart;
    }

    return result;
}