export function detectLanguage() {
    var language = navigator.languages && navigator.languages[0] || // Chrome / Firefox
        navigator.language ||   // All browsers
        navigator.userLanguage; // IE <= 10

    return language;
}