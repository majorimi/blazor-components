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

	//Inject Marker Clusterer JS - load WITHOUT defer so it's available before Google Maps callback
	let importedMarkerClusterer = document.createElement('script');
	importedMarkerClusterer.src = "https://unpkg.com/@googlemaps/markerclusterer/dist/index.min.js";
	document.head.appendChild(importedMarkerClusterer);
}

// Helper function to wait for MarkerClusterer library to be available
function waitForMarkerClusterer() {
	return new Promise((resolve) => {
		// Check every 100ms for markerClusterer availability
		const checkInterval = setInterval(() => {
			if (window.markerClusterer && window.markerClusterer.MarkerClusterer) {
				clearInterval(checkInterval);
				resolve();
			}
		}, 100);

		// Fallback: resolve after 5 seconds anyway (in case library fails to load)
		setTimeout(() => {
			clearInterval(checkInterval);
			resolve();
		}, 5000);
	});
}


//Global function for Google Js callback. It will be called when "https://maps.googleapis.com/maps/api/js" loaded.
window.initGoogleMaps = async () => {
	// Wait for MarkerClusterer to be available
	await waitForMarkerClusterer();

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

		//Marker clusters - initialize with empty markers array, will be populated when markers are added
		if (window.markerClusterer && window.markerClusterer.MarkerClusterer) {
			_mapsElementDict[i].value.clusterer = new window.markerClusterer.MarkerClusterer({ 
				markers: _mapsElementDict[i].value.mapMarkers, 
				map: map 
			});
			console.log('MarkerClusterer initialized for map:', elementId);
		} else {
			console.warn('MarkerClusterer library not available for map:', elementId);
		}

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
			value: {
				ref: dotnetRef,
				map: null,
				clusterer: null,
				mapMarkers: [],
				polylines: [],
				circles: [],
				rectangles: [],
                polygons: [],
				bgColor: backgroundColor,
				ctrSize: controlSize,
				restriction: restriction
			}
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
				mapWithDotnetRef.mapMarkers.push(marker); //Add to per-map markers array

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
							infoWindow.setPosition(event.latLng);
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

		//Rebuild marker clusterer with all markers for this map to trigger proper clustering
		if (mapWithDotnetRef.clusterer && mapWithDotnetRef.mapMarkers.length > 0) {
			mapWithDotnetRef.clusterer.clearMarkers();
			mapWithDotnetRef.clusterer.addMarkers(mapWithDotnetRef.mapMarkers);
			console.log('MarkerClusterer updated with', mapWithDotnetRef.mapMarkers.length, 'markers for map:', elementId);
		} else if (!mapWithDotnetRef.clusterer) {
			console.warn('No clusterer available for map:', elementId, '- markers will not be clustered');
		}
	}
}
export function removeMarkers(elementId, markers) {
	if (elementId && markers && markers.length) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {

			for (var i = 0; i < markers.length; i++) {
				let markerData = markers[i];

				//Remove from per-map markers array
				mapWithDotnetRef.mapMarkers.forEach( (element, index) => {
					if (markerData.id == element.id) {
						google.maps.event.clearInstanceListeners(element); // Remove all event listeners
						element.setMap(null);
						mapWithDotnetRef.mapMarkers.splice(index, 1);
						return;
					}
				});
			}

			//Rebuild marker clusterer after removing markers
			if (mapWithDotnetRef.clusterer) {
				mapWithDotnetRef.clusterer.clearMarkers();
				if (mapWithDotnetRef.mapMarkers.length > 0) {
					mapWithDotnetRef.clusterer.addMarkers(mapWithDotnetRef.mapMarkers);
				}
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

//Drawing Polylines
export function createPolylines(elementId, polylineOptions) {
	if (elementId && polylineOptions && polylineOptions.length) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);

		if (mapWithDotnetRef && mapWithDotnetRef.map) {

			for (var i = 0; i < polylineOptions.length; i++) {
				let options = polylineOptions[i];

				let polyline = new google.maps.Polyline(options);
				polyline.setMap(mapWithDotnetRef.map);
				mapWithDotnetRef.polylines.push(polyline);

				//Polyline events
				if (options.clickable) {
					//Create infoWindow
					let infoWindow = null;
					if (options.infoWindow) {
						infoWindow = new google.maps.InfoWindow({
							content: options.infoWindow.content,
							maxWidth: options.infoWindow.maxWidth
						});
					}

					polyline.addListener("click", (event) => {
						mapWithDotnetRef.ref.invokeMethodAsync("PolylineClicked", options.id);

						//If polyline has info window
						if (infoWindow) {
							infoWindow.setPosition(event.latLng);
							infoWindow.open(mapWithDotnetRef.map);
						}
					});
				}
				if (options.draggable) {
					polyline.addListener("drag", () => {
						polylineDragEvents("PolylineDrag", options.id, polyline.getPath().getAt(0).toJSON());
					});
					polyline.addListener("dragend", () => {
						polylineDragEvents("PolylineDragEnd", options.id, polyline.getPath().getAt(0).toJSON());
					});
					polyline.addListener("dragstart", () => {
						polylineDragEvents("PolylineDragStart", options.id, polyline.getPath().getAt(0).toJSON());
					});

					function polylineDragEvents(callBackName, id, pos) {
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
export function removePolylines(elementId, polylineOptions) {
	if (elementId && polylineOptions && polylineOptions.length) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {

			for (var i = 0; i < polylineOptions.length; i++) {
				let options = polylineOptions[i];

				mapWithDotnetRef.polylines.forEach((element, index) => {
					if (options.id == element.id) {
						google.maps.event.clearInstanceListeners(element);
						element.setMap(null);
						mapWithDotnetRef.polylines.splice(index, 1);
						return;
					}
				});
			}
		}
	}
}

//Drawing Circles
export function createCircles(elementId, circleOptions) {
	if (elementId && circleOptions && circleOptions.length) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);

		if (mapWithDotnetRef && mapWithDotnetRef.map) {

			for (var i = 0; i < circleOptions.length; i++) {
				let options = circleOptions[i];

				let circle = new google.maps.Circle({
					id: options.id,
					strokeColor: options.strokeColor,
					strokeOpacity: options.strokeOpacity,
					strokeWeight: options.strokeWeight,
					fillColor: options.fillColor,
					fillOpacity: options.fillOpacity,
					center: { lat: options.center.latitude, lng: options.center.longitude },
					radius: options.radius,
					clickable: options.clickable,
					draggable: options.draggable,
					editable: options.editable,
					visible: options.visible,
					zIndex: options.zIndex
				});
				circle.setMap(mapWithDotnetRef.map);
				mapWithDotnetRef.circles.push(circle);

				//Circle events
					if (options.clickable) {
						//Create infoWindow
						let infoWindow = null;
						if (options.infoWindow) {
							infoWindow = new google.maps.InfoWindow({
								content: options.infoWindow.content,
								maxWidth: options.infoWindow.maxWidth
							});
						}

						circle.addListener("click", (event) => {
							mapWithDotnetRef.ref.invokeMethodAsync("CircleClicked", options.id);

							//If circle has info window
							if (infoWindow) {
								infoWindow.setPosition(event.latLng);
								infoWindow.open(mapWithDotnetRef.map);
							}
						});
					}
					if (options.draggable) {
						circle.addListener("drag", () => {
							circleDragEvents("CircleDrag", options.id, circle.getCenter().toJSON());
						});
						circle.addListener("dragend", () => {
							circleDragEvents("CircleDragEnd", options.id, circle.getCenter().toJSON());
						});
						circle.addListener("dragstart", () => {
							circleDragEvents("CircleDragStart", options.id, circle.getCenter().toJSON());
						});

						function circleDragEvents(callBackName, id, pos) {
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
export function removeCircles(elementId, circleOptions) {
	if (elementId && circleOptions && circleOptions.length) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {

			for (var i = 0; i < circleOptions.length; i++) {
				let options = circleOptions[i];

				mapWithDotnetRef.circles.forEach((element, index) => {
					if (options.id == element.id) {
						google.maps.event.clearInstanceListeners(element);
						element.setMap(null);
						mapWithDotnetRef.circles.splice(index, 1);
						return;
					}
				});
			}
		}
	}
}

//Drawing Rectangles
export function createRectangles(elementId, rectangleOptions) {
	if (elementId && rectangleOptions && rectangleOptions.length) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);

		if (mapWithDotnetRef && mapWithDotnetRef.map) {

			for (var i = 0; i < rectangleOptions.length; i++) {
				let options = rectangleOptions[i];

				let rectangle = new google.maps.Rectangle({
					id: options.id,
					strokeColor: options.strokeColor,
					strokeOpacity: options.strokeOpacity,
					strokeWeight: options.strokeWeight,
					fillColor: options.fillColor,
					fillOpacity: options.fillOpacity,
					bounds: {
						north: options.bounds.northEast.lat,
						south: options.bounds.southWest.lat,
						east: options.bounds.northEast.lng,
						west: options.bounds.southWest.lng
					},
					clickable: options.clickable,
					draggable: options.draggable,
					editable: options.editable,
					visible: options.visible,
					zIndex: options.zIndex
				});
				rectangle.setMap(mapWithDotnetRef.map);
				mapWithDotnetRef.rectangles.push(rectangle);

				//Rectangle events
				if (options.clickable) {
					//Create infoWindow
					let infoWindow = null;
					if (options.infoWindow) {
						infoWindow = new google.maps.InfoWindow({
							content: options.infoWindow.content,
							maxWidth: options.infoWindow.maxWidth
						});
					}

					rectangle.addListener("click", (event) => {
						mapWithDotnetRef.ref.invokeMethodAsync("RectangleClicked", options.id);

						//If rectangle has info window
						if (infoWindow) {
							infoWindow.setPosition(event.latLng);
							infoWindow.open(mapWithDotnetRef.map);
						}
					});
				}
				if (options.draggable) {
					rectangle.addListener("drag", () => {
						rectangleDragEvents("RectangleDrag", options.id, rectangle.getBounds().getCenter().toJSON());
					});
					rectangle.addListener("dragend", () => {
						rectangleDragEvents("RectangleDragEnd", options.id, rectangle.getBounds().getCenter().toJSON());
					});
					rectangle.addListener("dragstart", () => {
						rectangleDragEvents("RectangleDragStart", options.id, rectangle.getBounds().getCenter().toJSON());
					});

					function rectangleDragEvents(callBackName, id, pos) {
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
export function removeRectangles(elementId, rectangleOptions) {
	if (elementId && rectangleOptions && rectangleOptions.length) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {

			for (var i = 0; i < rectangleOptions.length; i++) {
				let options = rectangleOptions[i];

				mapWithDotnetRef.rectangles.forEach((element, index) => {
					if (options.id == element.id) {
						google.maps.event.clearInstanceListeners(element);
						element.setMap(null);
						mapWithDotnetRef.rectangles.splice(index, 1);
						return;
					}
				});
			}
		}
	}
}

//Drawing Polygons (triangle, square, etc.)
export function createPolygons(elementId, polygonOptions) {
	if (elementId && polygonOptions && polygonOptions.length) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);

		if (mapWithDotnetRef && mapWithDotnetRef.map) {

			for (var i = 0; i < polygonOptions.length; i++) {
				let options = polygonOptions[i];

				// Convert paths from C# format to Google Maps format
				let paths = [];
				if (options.paths && options.paths.length) {
					for (var j = 0; j < options.paths.length; j++) {
						paths.push({ lat: options.paths[j].lat, lng: options.paths[j].lng });
					}
				}

				let polygon = new google.maps.Polygon({
					id: options.id,
					strokeColor: options.strokeColor,
					strokeOpacity: options.strokeOpacity,
					strokeWeight: options.strokeWeight,
					fillColor: options.fillColor,
					fillOpacity: options.fillOpacity,
					paths: paths,
					clickable: options.clickable,
					draggable: options.draggable,
					editable: options.editable,
					geodesic: options.geodesic,
					visible: options.visible,
					zIndex: options.zIndex
				});
				polygon.setMap(mapWithDotnetRef.map);
				mapWithDotnetRef.polygons.push(polygon);

				//Polygon events
				if (options.clickable) {
					//Create infoWindow
					let infoWindow = null;
					if (options.infoWindow) {
						infoWindow = new google.maps.InfoWindow({
							content: options.infoWindow.content,
							maxWidth: options.infoWindow.maxWidth
						});
					}

					polygon.addListener("click", (event) => {
						mapWithDotnetRef.ref.invokeMethodAsync("PolygonClicked", options.id);

						//If polygon has info window
						if (infoWindow) {
							infoWindow.setPosition(event.latLng);
							infoWindow.open(mapWithDotnetRef.map);
						}
					});
				}
				if (options.draggable) {
					polygon.addListener("drag", () => {
						polygonDragEvents("PolygonDrag", options.id, polygon.getPath().getAt(0).toJSON());
					});
					polygon.addListener("dragend", () => {
						polygonDragEvents("PolygonDragEnd", options.id, polygon.getPath().getAt(0).toJSON());
					});
					polygon.addListener("dragstart", () => {
						polygonDragEvents("PolygonDragStart", options.id, polygon.getPath().getAt(0).toJSON());
					});

					function polygonDragEvents(callBackName, id, pos) {
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
export function removePolygons(elementId, polygonOptions) {
	if (elementId && polygonOptions && polygonOptions.length) {
		let mapWithDotnetRef = getElementIdWithDotnetRef(_mapsElementDict, elementId);
		if (mapWithDotnetRef && mapWithDotnetRef.map) {

			for (var i = 0; i < polygonOptions.length; i++) {
				let options = polygonOptions[i];

				mapWithDotnetRef.polygons.forEach((element, index) => {
					if (options.id == element.id) {
						google.maps.event.clearInstanceListeners(element);
						element.setMap(null);
						mapWithDotnetRef.polygons.splice(index, 1);
						return;
					}
				});
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
		if (mapWithDotnetRef.clusterer) {
			mapWithDotnetRef.clusterer.clearMarkers();
			mapWithDotnetRef.clusterer = null;
		}
		if (mapWithDotnetRef.mapMarkers) {
			mapWithDotnetRef.mapMarkers = [];
		}
		if (mapWithDotnetRef.polylines) {
			mapWithDotnetRef.polylines = [];
		}
		if (mapWithDotnetRef.circles) {
			mapWithDotnetRef.circles = [];
		}
		if (mapWithDotnetRef.rectangles) {
			mapWithDotnetRef.rectangles = [];
		}
		if (mapWithDotnetRef.polygons) {
			mapWithDotnetRef.polygons = [];
		}
		mapWithDotnetRef.map = null;
		mapWithDotnetRef.ref = null;

		removeElementIdWithDotnetRef(_mapsElementDict, elementId);
	}
}