export function init(trackingId) {
	if (!trackingId) {
		return;
	}

	let src = "https://www.googletagmanager.com/gtag/js?id=";
	let scriptTags = document.querySelectorAll('head > script');
	scriptTags.forEach(scriptTag => {
		if (scriptTag.getAttribute('src').startsWith(src)) {
			return;
		}
	});

	//Inject required Google JS scripts to HTML (only once!)
	document.head.appendChild(document.createComment("Global site tag (gtag.js) - Google Analytics"));

	let importedGtag = document.createElement('script');
	importedGtag.src = src + trackingId;
	importedGtag.async = true;
	document.head.appendChild(importedGtag);

	let importedGtagInline = document.createElement('script');
	importedGtagInline.textContent = 
		`window.dataLayer = window.dataLayer || [];
		function gtag() { dataLayer.push(arguments); }
		gtag('js', new Date());

		gtag('config', '{0}');`.replace("{0}", trackingId);
	document.head.appendChild(importedGtagInline);
}

//Google Gtag API: https://developers.google.com/gtagjs/reference/api
export function config(trackingId, data) {
	if (trackingId && window.gtag) {
		gtag('config', trackingId, data);
	}
}
export function get(trackingId, fieldName) {
	if (trackingId && window.gtag) {
		gtag('get', trackingId, fieldName, (result) => {
			//callback method here...
		});
	}
}
export function set(data) {
	if (window.gtag && data) {
		gtag('set', trackingId, data);
	}
}
export function event(eventName, data) {
	if (window.gtag && eventName && data) {
		gtag('event', eventName, data);
	}
}