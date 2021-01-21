export function init(key, elementId) {
	mapElementId = elementId;

	let importedPoly = document.createElement('script');
	importedPoly.src = "https://polyfill.io/v3/polyfill.min.js?features=default";
	document.head.appendChild(importedPoly);

	let src = "https://maps.googleapis.com/maps/api/js?key=" + key + "&callback=initGoogleMaps&libraries=&v=weekly";
	let importedMaps = document.createElement('script');
	importedMaps.src = src;
	importedMaps.defer = true;
	document.head.appendChild(importedMaps);
}
//Global function for callback
window.initGoogleMaps = () => {
	map = new google.maps.Map(document.getElementById(mapElementId), {
		center: { lat: -34.397, lng: 150.644 },
		zoom: 8,
	});

	//let infoWindow = new google.maps.InfoWindow();
};

let mapElementId;
let map;

export function recenter() {
	if (map) {
		map.setCenter({ lat: 17.397, lng: 49.644 });
	}
}