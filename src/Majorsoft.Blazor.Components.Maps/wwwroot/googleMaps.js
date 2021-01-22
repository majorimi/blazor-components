export function init(key, elementId, dotnetRef) {
	if (!key || !elementId || !dotnetRef) {
		return;
	}

	storeElementIdWithDotnetRef(_mapsElementDict, elementId, dotnetRef); //Store maps

	let scriptsIncluded = false;
	let scriptTags = document.querySelectorAll('head > script');
	scriptTags.forEach(scriptTag => {
		if (scriptTag.getAttribute('src').startsWith("https://maps.googleapis.com/maps/api/js?key=")) {
			scriptsIncluded = true;
			return;
		}
	});

	if (scriptsIncluded) { //Prevent adding JS scripts to page multiple times.
		return;
	}

	//Inject required Google JS scripts to HTML (only once!)
	let importedPoly = document.createElement('script');
	importedPoly.src = "https://polyfill.io/v3/polyfill.min.js?features=default";
	document.head.appendChild(importedPoly);

	let src = "https://maps.googleapis.com/maps/api/js?key=" + key + "&callback=initGoogleMaps&libraries=&v=weekly";
	let importedMaps = document.createElement('script');
	importedMaps.src = src;
	importedMaps.defer = true;
	document.head.appendChild(importedMaps);
}

//Global function for Google Js callback
window.initGoogleMaps = () => {
	//var map = new google.maps.Map(document.getElementById(mapElementId), {
	//	center: { lat: -34.397, lng: 150.644 },
	//	zoom: 8,
	//});

	for (let i = 0; i < _mapsElementDict.length; i++) {
		let elementId = _mapsElementDict[i].key;
		let map = new google.maps.Map(document.getElementById(elementId), {});
		_mapsElementDict[i].value.map = map;

		_mapsElementDict[i].value.ref.invokeMethodAsync("MapInitialized", elementId);
	}
};

//Store elementId with .NET Ref
function storeElementIdWithDotnetRef(dict, elementId, dotnetRef) {
	let elementFound = false;
	for (let i = 0; i < dict.length; i++) {
		if (dict[i].key === elementId) {
			return; //Element has been stored already
		}
	}

	if (!elementFound) {
		dict.push({
			key: elementId,
			value: { ref: dotnetRef, map: null }
		});
	}
}
//Remove elementId with data
function removeElementIdWithDotnetRef(dict, elementId) {
	for (let i = 0; i < dict.length; i++) {
		if (dict[i].key === elementId) {
			dict.splice(i, 1);
			break;
		}
	}
}
//Return elementId with data
function getElementIdWithDotnetRef(dict, elementId) {
	for (let i = 0; i < dict.length; i++) {
		if (dict[i].key === elementId) {
			return dict[i].value;
		}
	}
}
let _mapsElementDict = [];

//Google maps Features
export function setCenter(elementId, latitude, longitude) {
	if (elementId) {
		let map = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (map) {
			map.setCenter({ lat: 17.397, lng: 49.644 });
		}
	}
}

export function dispose(elementId) {
	removeElementIdWithDotnetRef(elementId);
}