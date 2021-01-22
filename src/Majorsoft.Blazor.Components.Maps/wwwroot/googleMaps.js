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

//Global function for Google Js callback. It will be called when "https://maps.googleapis.com/maps/api/js" loaded.
//TODO: multiple instances of Js Maps if registered must be stored before callback happens. In the future it might causes timing issues...
window.initGoogleMaps = () => {
	for (let i = 0; i < _mapsElementDict.length; i++) {
		let elementId = _mapsElementDict[i].key;
		let map = new google.maps.Map(document.getElementById(elementId), { });
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
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {
			mapWithDotnetRef.map.setCenter({ lat: 17.397, lng: 49.644 });
			//mapWithDotnetRef.map.setZoom(6);
		}
	}
}
export function setZoom(elementId, zoom) {
	if (elementId) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {
			mapWithDotnetRef.map.setZoom(6);
		}
	}
}

//Dispose
export function dispose(elementId) {
	removeElementIdWithDotnetRef(elementId);
}