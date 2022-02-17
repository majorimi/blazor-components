export function init(key, elementId, dotnetRef, backgroundColor, controlSize, restriction) {
	if (!key || !elementId || !dotnetRef) {
		return;
	}

	storeElementIdWithDotnetRef(_mapsElementDict, elementId, dotnetRef, backgroundColor, controlSize, restriction); //Store map info

	let src = "https://maps.googleapis.com/maps/api/js?key=";
	let scriptsIncluded = false;

	let scriptTags = document.querySelectorAll('head > script');
	scriptTags.forEach(scriptTag => {
		if (scriptTag) {
			let srcAttribute = scriptTag.getAttribute('src');
			if (srcAttribute && srcAttribute.startsWith(src)) {
				scriptsIncluded = true;
				return;
			}
		}
	});

	if (scriptsIncluded) { //Prevent adding JS scripts to page multiple times.
		if (window.google) {
			window.initGoogleMaps(); //Page was navigated
		}
		return;
	}

	//Inject required Google JS scripts to HTML (only once!)
	let importedPoly = document.createElement('script');
	importedPoly.src = "https://polyfill.io/v3/polyfill.min.js?features=default";
	document.head.appendChild(importedPoly);

	src = src + key + "&callback=initGoogleMaps&libraries=&v=weekly";
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
		let mapInfo = _mapsElementDict[i].value;

		if (_mapsElementDict[i].value.map) { //Map already created
			continue;
		}

		//Create Map
		let restrict = null;
		if (mapInfo.restriction && mapInfo.restriction.latLngBounds
			&& mapInfo.restriction.latLngBounds.northEast && mapInfo.restriction.latLngBounds.southWest) {
			restrict =
			{
				latLngBounds: {
					south: mapInfo.restriction.latLngBounds.southWest.lat,
					west: mapInfo.restriction.latLngBounds.southWest.lng,
					north: mapInfo.restriction.latLngBounds.northEast.lat,
					east: mapInfo.restriction.latLngBounds.northEast.lng
				},
				strictBounds: mapInfo.restriction.strictBounds,
			};
		}

		let map = new google.maps.Map(document.getElementById(elementId), {
			backgroundColor: mapInfo.bgColor,
			controlSize: mapInfo.ctrSize,
			restriction: restrict,
		});
		map.elementId = elementId;
		_mapsElementDict[i].value.map = map;

		function mouseEventHandlers(mapsMouseEvent, callbackFuncName) {
			if (map && map.elementId && mapsMouseEvent) {
				let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, map.elementId);
				if (mapWithDotnetRef) {
					let coord = mapsMouseEvent.latLng.toJSON();
					let arg = {
						Latitude: coord.lat,
						Longitude: coord.lng
					};

					mapWithDotnetRef.ref.invokeMethodAsync(callbackFuncName, arg);
				}
			}
		}

		//Add Event listeners
		//Mouse
		map.addListener("click", (mapsMouseEvent) => {
			mouseEventHandlers(mapsMouseEvent, "MapClicked");
		});
		map.addListener("dblclick", (mapsMouseEvent) => {
			mouseEventHandlers(mapsMouseEvent, "MapDoubleClicked");
		});
		map.addListener("contextmenu", (mapsMouseEvent) => {
			mouseEventHandlers(mapsMouseEvent, "MapContextMenu");
		});
		map.addListener("mouseup", (mapsMouseEvent) => {
			mouseEventHandlers(mapsMouseEvent, "MapMouseUp");
		});
		map.addListener("mousedown", (mapsMouseEvent) => {
			mouseEventHandlers(mapsMouseEvent, "MapMouseDown");
		});
		map.addListener("mousemove", (mapsMouseEvent) => {
			mouseEventHandlers(mapsMouseEvent, "MapMouseMove");
		});
		map.addListener("mouseover", () => {
			if (map && map.elementId) {
				let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, map.elementId);
				if (mapWithDotnetRef) {
					mapWithDotnetRef.ref.invokeMethodAsync("MapMouseOver");
				}
			}
		});
		map.addListener("mouseout", () => {
			if (map && map.elementId) {
				let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, map.elementId);
				if (mapWithDotnetRef) {
					mapWithDotnetRef.ref.invokeMethodAsync("MapMouseOut");
				}
			}
		});
		//Changes
		map.addListener("center_changed", () => {
			if (map && map.elementId) {
				let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, map.elementId);
				if (mapWithDotnetRef && map.getCenter()) {
					let center = map.getCenter().toJSON();
					let arg = {
						Latitude: center.lat,
						Longitude: center.lng
					};

					mapWithDotnetRef.ref.invokeMethodAsync("MapCenterChanged", arg);
				}
			}
		});
		map.addListener("zoom_changed", () => {
			if (map && map.elementId) {
				let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, map.elementId);
				if (mapWithDotnetRef) {
					mapWithDotnetRef.ref.invokeMethodAsync("MapZoomChanged", map.getZoom());
				}
			}
		});
		map.addListener("maptypeid_changed", () => {
			if (map && map.elementId) {
				let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, map.elementId);
				if (mapWithDotnetRef) {
					mapWithDotnetRef.ref.invokeMethodAsync("MapTypeIdChanged", map.getMapTypeId());
				}
			}
		});
		map.addListener("heading_changed", () => {
			if (map && map.elementId) {
				let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, map.elementId);
				if (mapWithDotnetRef) {
					mapWithDotnetRef.ref.invokeMethodAsync("MapHeadingChanged", map.getHeading());
				}
			}
		});
		map.addListener("tilt_changed", () => {
			if (map && map.elementId) {
				let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, map.elementId);
				if (mapWithDotnetRef) {
					mapWithDotnetRef.ref.invokeMethodAsync("MapTiltChanged", map.getTilt());
				}
			}
		});
		map.addListener("bounds_changed", () => {
			if (map && map.elementId) {
				let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, map.elementId);
				if (mapWithDotnetRef) {
					mapWithDotnetRef.ref.invokeMethodAsync("MapBoundsChanged");
				}
			}
		});
		map.addListener("projection_changed", () => {
			if (map && map.elementId) {
				let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, map.elementId);
				if (mapWithDotnetRef) {
					mapWithDotnetRef.ref.invokeMethodAsync("MapProjectionChanged");
				}
			}
		});
		map.addListener("draggable_changed", () => {
			if (map && map.elementId) {
				let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, map.elementId);
				if (mapWithDotnetRef) {
					mapWithDotnetRef.ref.invokeMethodAsync("MapDraggableChanged");
				}
			}
		});
		map.addListener("streetview_changed", () => {
			if (map && map.elementId) {
				let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, map.elementId);
				if (mapWithDotnetRef) {
					mapWithDotnetRef.ref.invokeMethodAsync("MapStreetviewChanged");
				}
			}
		});
		//Drag
		map.addListener("drag", () => {
			if (map && map.elementId) {
				let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, map.elementId);
				if (mapWithDotnetRef && map.getCenter()) {
					let center = map.getCenter().toJSON();
					let arg = {
						Latitude: center.lat,
						Longitude: center.lng
					};

					mapWithDotnetRef.ref.invokeMethodAsync("MapDrag", arg);
				}
			}
		});
		map.addListener("dragend", () => {
			if (map && map.elementId) {
				let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, map.elementId);
				if (mapWithDotnetRef && map.getCenter()) {
					let center = map.getCenter().toJSON();
					let arg = {
						Latitude: center.lat,
						Longitude: center.lng
					};

					mapWithDotnetRef.ref.invokeMethodAsync("MapDragEnd", arg);
				}
			}
		});
		map.addListener("dragstart", () => {
			if (map && map.elementId) {
				let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, map.elementId);
				if (mapWithDotnetRef && map.getCenter()) {
					let center = map.getCenter().toJSON();
					let arg = {
						Latitude: center.lat,
						Longitude: center.lng
					};

					mapWithDotnetRef.ref.invokeMethodAsync("MapDragStart", arg);
				}
			}
		});
		//Other
		map.addListener("resize", () => {
			if (map && map.elementId) {
				let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, map.elementId);
				if (mapWithDotnetRef) {
					let arg = {
						Width: map.getDiv().offsetWidth,
						Height: map.getDiv().offsetHeight
					};
					mapWithDotnetRef.ref.invokeMethodAsync("MapResized", arg);
				}
			}
		});
		map.addListener("tilesloaded", () => {
			if (map && map.elementId) {
				let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, map.elementId);
				if (mapWithDotnetRef) {
					mapWithDotnetRef.ref.invokeMethodAsync("MapTilesLoaded");
				}
			}
		});
		map.addListener("idle", () => {
			if (map && map.elementId) {
				let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, map.elementId);
				if (mapWithDotnetRef) {
					mapWithDotnetRef.ref.invokeMethodAsync("MapIdle");
				}
			}
		});
		//Init
		_mapsElementDict[i].value.ref.invokeMethodAsync("MapInitialized", elementId);
	}
};

//Store elementId with .NET Ref
function storeElementIdWithDotnetRef(dict, elementId, dotnetRef, backgroundColor, controlSize, restriction) {
	let elementFound = false;
	for (let i = 0; i < dict.length; i++) {
		if (dict[i].key === elementId) {
			return; //Element has been stored already
		}
	}

	if (!elementFound) {
		dict.push({
			key: elementId,
			value: { ref: dotnetRef, map: null, bgColor: backgroundColor, ctrSize: controlSize, restriction: restriction }
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
let _mapsMarkers = [];

//Google JS Maps Features
export function setCenterCoords(elementId, latitude, longitude) {
	if (elementId) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {
			mapWithDotnetRef.map.setCenter({ lat: latitude, lng: longitude });
		}
	}
}
export function setCenterAddress(elementId, address) {
	if (elementId) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {
			geocodeAddress(address, function (results) {
				if (results) {
					mapWithDotnetRef.map.setCenter(results[0].geometry.location);
				}
			});
		}
	}
}
export function panToCoords(elementId, latitude, longitude) {
	if (elementId) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {
			mapWithDotnetRef.map.panTo({ lat: latitude, lng: longitude });
		}
	}
}
export function panToAddress(elementId, address) {
	if (elementId) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {
			geocodeAddress(address, function (results) {
				if (results) {
					mapWithDotnetRef.map.panTo(results[0].geometry.location);
				}
			});
		}
	}
}
//get methods
export function getBounds(elementId) {
	if (elementId) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {
			let bounds = mapWithDotnetRef.map.getBounds();

			let ret = {
				Center: convertToLatLng(bounds.getCenter()),
				NorthEast: convertToLatLng(bounds.getNorthEast()),
				SouthWest: convertToLatLng(bounds.getSouthWest()),
				Span: convertToLatLng(bounds.toSpan()),
				IsEmpty: bounds.isEmpty(),
			};
			return ret;
		}
	}
}
export function getCenter(elementId) {
	if (elementId) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {
			let center = mapWithDotnetRef.map.getCenter();

			let ret = convertToLatLng(center);
			return ret;
		}
	}
}
export function getDiv(elementId) {
	if (elementId) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {
			var ret = mapWithDotnetRef.map.getDiv();
			return ret;
		}
	}
}
function convertToLatLng(latLngObject) {
	let ret = { lat: latLngObject.lat(), lng: latLngObject.lng() };
	return ret;
}

//set methods
export function setZoom(elementId, zoom) {
	if (elementId) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {
			mapWithDotnetRef.map.setZoom(zoom);
		}
	}
}
export function setMapType(elementId, mapType) {
	if (elementId) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {
			mapWithDotnetRef.map.setMapTypeId(mapType);
		}
	}
}
export function setHeading(elementId, heading) {
	if (elementId) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {
			mapWithDotnetRef.map.setHeading(heading);
		}
	}
}
export function setTilt(elementId, tilt) {
	if (elementId) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {
			mapWithDotnetRef.map.setTilt(tilt);
		}
	}
}
export function setClickableIcons(elementId, isClickable) {
	if (elementId) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {
			mapWithDotnetRef.map.setClickableIcons(isClickable);
		}
	}
}
//generic set
export function setOptions(elementId, options) {
	if (elementId) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {
			mapWithDotnetRef.map.setOptions(options);
		}
	}
}

export function resizeMap(elementId) {
	if (elementId) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {
			google.maps.event.trigger(mapWithDotnetRef.map, "resize");
		}
	}
}

//Custom controls
export function createCustomControls(elementId, customControls) {
	if (elementId && customControls) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {

			for (var i = 0; i < customControls.length; i++) {
				let control = customControls[i];
				let controlDiv = document.createElement("div");
				controlDiv.innerHTML = control.content;

				mapWithDotnetRef.map.controls[control.controlPosition].push(controlDiv);

				let id = control.id;
				let dotnetRef = mapWithDotnetRef.ref;
				controlDiv.addEventListener("click", () => {
					dotnetRef.invokeMethodAsync("CustomControlClicked", id);
				});
			}
		}
	}
}

//Markers
export function createMarkers(elementId, markers) {
	if (elementId && markers && markers.length) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {

			for (var i = 0; i < markers.length; i++) {

				let markerData = markers[i];
				let marker = new google.maps.Marker({
					id: markerData.id, //Custom id to track Markers
					//some property does not work after set...
					crossOnDrag: markerData.crossOnDrag,
					optimized: markerData.optimized,
				});

				marker.setMap(mapWithDotnetRef.map);
				setMarkerData(markerData, marker);
				_mapsMarkers.push(marker);

				//Marker events
				if (markerData.clickable) {
					//Create infoWindow
					let infoWindow = null;
					if (markerData.infoWindow) {
						infoWindow = new google.maps.InfoWindow({
							content: markerData.infoWindow.content,
							maxWidth: markerData.infoWindow.maxWidth
						}); 
					}

					marker.addListener("click", () => {
						mapWithDotnetRef.ref.invokeMethodAsync("MarkerClicked", markerData.id);

						//If marker has info window
						if (infoWindow) {
							infoWindow.open(mapWithDotnetRef.map, marker);
						}
					});
				}
				if (markerData.draggable) {
					marker.addListener("drag", () => {
						markerDragEvents("MarkerDrag", markerData.id, marker.getPosition().toJSON());
					});

					marker.addListener("dragend", () => {
						markerDragEvents("MarkerDragEnd", markerData.id, marker.getPosition().toJSON());
					});

					marker.addListener("dragstart", () => {
						markerDragEvents("MarkerDragStart", markerData.id, marker.getPosition().toJSON());
					});

					function markerDragEvents(callBackName, id, pos) {
						let arg = {
							Latitude: pos.lat,
							Longitude: pos.lng
						};
						mapWithDotnetRef.ref.invokeMethodAsync(callBackName, id, arg);
					}
				}
			}
		}
	}
}
//export function updateMarkers(elementId, markers) {
//	if (elementId && markers && markers.length) {
//		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
//		if (mapWithDotnetRef && mapWithDotnetRef.map) {

//			for (var i = 0; i < markers.length; i++) {
//				let markerData = markers[i];

//				_mapsMarkers.forEach(element => {
//					if (markerData.id == element.id) {
//						setMarkerData(markerData, element);
//						return;
//					}
//				});
//			}
//		}
//	}
//}
export function removeMarkers(elementId, markers) {
	if (elementId && markers && markers.length) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {

			for (var i = 0; i < markers.length; i++) {
				let markerData = markers[i];

				_mapsMarkers.forEach( (element, index) => {
					if (markerData.id == element.id) {
						element.setMap(null);
						_mapsMarkers.splice(index, 1);
						return;
					}
				});
			}
		}
	}
}
function setMarkerData(markerData, marker) {
	if (!marker || !markerData) {
		return;
	}

	//required
	marker.setPosition({ lat: markerData.position.latitude, lng: markerData.position.longitude });
	//optional
	//marker.setAnchorPoint(markerData.anchorPoint ? { x: markerData.anchorPoint.x, y: markerData.anchorPoint.y } : null);
	marker.anchorPoint = markerData.anchorPoint ? { x: markerData.anchorPoint.x, y: markerData.anchorPoint.y } : null;
	marker.setAnimation(markerData.animation);
	marker.setClickable(markerData.clickable);
	//marker.setCrossOnDrag(markerData.crossOnDrag);
	marker.crossOnDrag = markerData.crossOnDrag;
	marker.setCursor(markerData.cursor);
	marker.setDraggable(markerData.draggable);
	marker.setIcon(markerData.icon);
	marker.setLabel(markerData.label);
	marker.setOpacity(markerData.opacity);
	//marker.setOptimized(markerData.optimized);
	marker.optimized = markerData.optimized;
	marker.setShape(markerData.shape);
	marker.setTitle(markerData.title);
	marker.setVisible(markerData.visible);
	marker.setZIndex(markerData.zIndex);
}

//Drawing
export function polylineSetMap(elementId, polylineOptions) {
	if (elementId && polylineOptions && polylineOptions.length) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {

			for (var i = 0; i < polylineOptions.length; i++) {
				let options = polylineOptions[i];

				let polyline = new google.maps.Polyline(options);
				polyline.setMap(mapWithDotnetRef.map);
			}
		}
	}
}

//Google GeoCoder
export function getAddressCoordinates(elementId, address) {
	geocodeAddress(address, function (results) {
		if (results) {
			let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
			if (mapWithDotnetRef && mapWithDotnetRef.map) {
				//TODO: map .NET object
				mapWithDotnetRef.ref.invokeMethodAsync("AddressSearch", results);
			}
		}
	});
}
function geocodeAddress(address, successCallback) {
	let geocoder = new google.maps.Geocoder();
	geocoder.geocode({
		'address': address
	}, function (results, status) {
		if (status == google.maps.GeocoderStatus.OK) {
			successCallback(results);
		}
	});
}

//Dispose
export function dispose(elementId) {
	if (elementId) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		mapWithDotnetRef.map = null;
		mapWithDotnetRef.ref = null;

		removeElementIdWithDotnetRef(_mapsElementDict, elementId);
	}
}