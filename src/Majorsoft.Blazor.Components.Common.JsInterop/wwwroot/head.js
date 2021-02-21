export function getHtmlTitle() {
	return document.title;
}
export function setHtmlTitle(newTitle) {
	if (newTitle) {
		document.title = newTitle;
	}
}

//function checkIfTagExistsInHead() {
//	let headTags = document.querySelectorAll('head');
//	headTags.forEach(tag => {
//		//if (scriptTag.getAttribute('src').startsWith("https://maps.googleapis.com/maps/api/js?key=")) {
//		//	scriptsIncluded = true;
//		//	return;
//		//}
//	});
//}