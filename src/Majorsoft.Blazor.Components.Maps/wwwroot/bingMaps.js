export function init(key, elementId, dotnetRef) {
    if (!key || !elementId || !dotnetRef) return;

    // store dotnet ref
    storeElementIdWithDotnetRef(_bingMapsElementDict, elementId, dotnetRef);

    let srcPrefix = "https://www.bing.com/api/maps/mapcontrol?key=";
    let scriptsIncluded = false;

    let scriptTags = document.querySelectorAll('head > script');
    scriptTags.forEach(scriptTag => {
        if (scriptTag) {
            let srcAttribute = scriptTag.getAttribute('src');
            if (srcAttribute && srcAttribute.startsWith(srcPrefix)) {
                scriptsIncluded = true;
                return;
            }
        }
    });

    if (scriptsIncluded) {
        if (window.Microsoft && window.Microsoft.Maps) {
            window.initBingMaps();
        }
        return;
    }

    let imported = document.createElement('script');
    imported.src = srcPrefix + key + "&callback=initBingMaps";
    imported.defer = true;
    document.head.appendChild(imported);
}

window.initBingMaps = () => {
    for (let i = 0; i < _bingMapsElementDict.length; i++) {
        let elementId = _bingMapsElementDict[i].key;
        let mapInfo = _bingMapsElementDict[i].value;

        if (_bingMapsElementDict[i].value.map) continue;

        let map = new Microsoft.Maps.Map(document.getElementById(elementId), {});
        map.elementId = elementId;
        _bingMapsElementDict[i].value.map = map;
        _bingMapsElementDict[i].value.mapMarkers = [];
        _bingMapsElementDict[i].value.clusterLayer = null;
        _bingMapsElementDict[i].value.enableMarkerClustering = true;

        // mouse events
        Microsoft.Maps.Events.addHandler(map, 'click', function (e) {
            if (e && e.location) {
                let arg = { Latitude: e.location.latitude, Longitude: e.location.longitude };
                let mapWithDotnetRef = getElementIdWithDotnetRef(_bingMapsElementDict, elementId);
                if (mapWithDotnetRef) mapWithDotnetRef.ref.invokeMethodAsync('MapClicked', arg);
            }
        });

        Microsoft.Maps.Events.addHandler(map, 'dblclick', function (e) {
            if (e && e.location) {
                let arg = { Latitude: e.location.latitude, Longitude: e.location.longitude };
                let mapWithDotnetRef = getElementIdWithDotnetRef(_bingMapsElementDict, elementId);
                if (mapWithDotnetRef) mapWithDotnetRef.ref.invokeMethodAsync('MapDoubleClicked', arg);
            }
        });

        Microsoft.Maps.Events.addHandler(map, 'contextmenu', function (e) {
            if (e && e.location) {
                let arg = { Latitude: e.location.latitude, Longitude: e.location.longitude };
                let mapWithDotnetRef = getElementIdWithDotnetRef(_bingMapsElementDict, elementId);
                if (mapWithDotnetRef) mapWithDotnetRef.ref.invokeMethodAsync('MapContextMenu', arg);
            }
        });

        Microsoft.Maps.Events.addHandler(map, 'mouseup', function (e) {
            if (e && e.location) {
                let arg = { Latitude: e.location.latitude, Longitude: e.location.longitude };
                let mapWithDotnetRef = getElementIdWithDotnetRef(_bingMapsElementDict, elementId);
                if (mapWithDotnetRef) mapWithDotnetRef.ref.invokeMethodAsync('MapMouseUp', arg);
            }
        });

        Microsoft.Maps.Events.addHandler(map, 'mousedown', function (e) {
            if (e && e.location) {
                let arg = { Latitude: e.location.latitude, Longitude: e.location.longitude };
                let mapWithDotnetRef = getElementIdWithDotnetRef(_bingMapsElementDict, elementId);
                if (mapWithDotnetRef) mapWithDotnetRef.ref.invokeMethodAsync('MapMouseDown', arg);
            }
        });

        Microsoft.Maps.Events.addHandler(map, 'mousemove', function (e) {
            if (e && e.location) {
                let arg = { Latitude: e.location.latitude, Longitude: e.location.longitude };
                let mapWithDotnetRef = getElementIdWithDotnetRef(_bingMapsElementDict, elementId);
                if (mapWithDotnetRef) mapWithDotnetRef.ref.invokeMethodAsync('MapMouseMove', arg);
            }
        });

        Microsoft.Maps.Events.addHandler(map, 'mouseover', function () {
            let mapWithDotnetRef = getElementIdWithDotnetRef(_bingMapsElementDict, elementId);
            if (mapWithDotnetRef) mapWithDotnetRef.ref.invokeMethodAsync('MapMouseOver');
        });

        Microsoft.Maps.Events.addHandler(map, 'mouseout', function () {
            let mapWithDotnetRef = getElementIdWithDotnetRef(_bingMapsElementDict, elementId);
            if (mapWithDotnetRef) mapWithDotnetRef.ref.invokeMethodAsync('MapMouseOut');
        });

        // view change end -> center/zoom changed
        Microsoft.Maps.Events.addHandler(map, 'viewchangeend', function () {
            let mapWithDotnetRef = getElementIdWithDotnetRef(_bingMapsElementDict, elementId);
            if (mapWithDotnetRef && map.getCenter) {
                let center = map.getCenter();
                let arg = { Latitude: center.latitude, Longitude: center.longitude };
                mapWithDotnetRef.ref.invokeMethodAsync('MapCenterChanged', arg);
                mapWithDotnetRef.ref.invokeMethodAsync('MapZoomChanged', map.getZoom());
                mapWithDotnetRef.ref.invokeMethodAsync('MapBoundsChanged');
            }
        });

        // drag events
        Microsoft.Maps.Events.addHandler(map, 'drag', function (e) {
            if (e && e.location) {
                let arg = { Latitude: e.location.latitude, Longitude: e.location.longitude };
                let mapWithDotnetRef = getElementIdWithDotnetRef(_bingMapsElementDict, elementId);
                if (mapWithDotnetRef) mapWithDotnetRef.ref.invokeMethodAsync('MapDrag', arg);
            }
        });

        Microsoft.Maps.Events.addHandler(map, 'dragend', function (e) {
            if (e && e.location) {
                let arg = { Latitude: e.location.latitude, Longitude: e.location.longitude };
                let mapWithDotnetRef = getElementIdWithDotnetRef(_bingMapsElementDict, elementId);
                if (mapWithDotnetRef) mapWithDotnetRef.ref.invokeMethodAsync('MapDragEnd', arg);
            }
        });

        Microsoft.Maps.Events.addHandler(map, 'dragstart', function (e) {
            if (e && e.location) {
                let arg = { Latitude: e.location.latitude, Longitude: e.location.longitude };
                let mapWithDotnetRef = getElementIdWithDotnetRef(_bingMapsElementDict, elementId);
                if (mapWithDotnetRef) mapWithDotnetRef.ref.invokeMethodAsync('MapDragStart', arg);
            }
        });

        // tiles loaded & idle - approximate with viewchange & idle timer
        Microsoft.Maps.Events.addHandler(map, 'viewchange', function () {
            let mapWithDotnetRef = getElementIdWithDotnetRef(_bingMapsElementDict, elementId);
            if (mapWithDotnetRef) {
                // inform tiles loaded
                mapWithDotnetRef.ref.invokeMethodAsync('MapTilesLoaded');
            }
        });

        // idle is not explicit in Bing Maps - use setTimeout on viewchangeend
        Microsoft.Maps.Events.addHandler(map, 'viewchangeend', function () {
            let mapWithDotnetRef = getElementIdWithDotnetRef(_bingMapsElementDict, elementId);
            if (mapWithDotnetRef) {
                setTimeout(function () { mapWithDotnetRef.ref.invokeMethodAsync('MapIdle'); }, 200);
            }
        });

        // resize is not automatic event; provide helper
        _bingMapsElementDict[i].value.ref.invokeMethodAsync('MapInitialized', elementId);
    }
};

function storeElementIdWithDotnetRef(dict, elementId, dotnetRef) {
    for (let i = 0; i < dict.length; i++) {
        if (dict[i].key === elementId) return;
    }
    dict.push({ key: elementId, value: { ref: dotnetRef, map: null } });
}


function getElementIdWithDotnetRef(dict, elementId) {
    for (let i = 0; i < dict.length; i++) {
        if (dict[i].key === elementId) return dict[i].value;
    }
}

let _bingMapsElementDict = [];

export function setCenterCoords(elementId, latitude, longitude) {
    let item = getElementIdWithDotnetRef(_bingMapsElementDict, elementId);
    if (item && item.map) {
        item.map.setView({ center: new Microsoft.Maps.Location(latitude, longitude) });
    }
}

export function setZoom(elementId, zoom) {
    let item = getElementIdWithDotnetRef(_bingMapsElementDict, elementId);
    if (item && item.map) {
        item.map.setView({ zoom: zoom });
    }
}

export function resizeMap(elementId) {
    let item = getElementIdWithDotnetRef(_bingMapsElementDict, elementId);
    if (item && item.map) {
        Microsoft.Maps.Events.invoke(item.map, 'resize');
    }
}

export function setOptions(elementId, options) {
    let item = getElementIdWithDotnetRef(_bingMapsElementDict, elementId);
    if (!item || !item.map) return;

    // Apply common options where possible
    try {
        if (options.maxZoom !== undefined) {
            item.map.setView({ maxZoom: options.maxZoom });
        }
        if (options.minZoom !== undefined) {
            item.map.setView({ minZoom: options.minZoom });
        }
        if (options.zoomControl !== undefined) {
            // Bing Maps has no direct zoomControl toggle; skip
        }
        if (options.center) {
            const c = options.center;
            item.map.setView({ center: new Microsoft.Maps.Location(c.Latitude || c.lat, c.Longitude || c.lng) });
        }
        if (options.heading !== undefined) {
            // No direct setHeading API in Bing Maps v8; skip
        }
        if (options.tilt !== undefined) {
            // Bing tilt handled via map options; attempt setView with heading/tilt if supported
        }
    } catch (e) {
        console.warn('setOptions failed', e);
    }
}

export function setClickableIcons(elementId, isClickable) {
    // no-op for Bing Maps v8
}

export function setHeading(elementId, heading) {
    // no-op placeholder
}

export function setTilt(elementId, tilt) {
    // no-op placeholder
}

// Markers
export function createMarkers(elementId, markers) {
    if (elementId && markers && markers.length) {
        let mapWithDotnetRef = getElementIdWithDotnetRef(_bingMapsElementDict, elementId);
        if (mapWithDotnetRef && mapWithDotnetRef.map) {
            for (var i = 0; i < markers.length; i++) {
                let markerData = markers[i];
                let location = new Microsoft.Maps.Location(markerData.position.latitude, markerData.position.longitude);
                let pushpin = new Microsoft.Maps.Pushpin(location, {
                    title: markerData.title,
                    draggable: markerData.draggable,
                    text: markerData.label ? (markerData.label.text || '') : undefined
                });

                pushpin.id = markerData.id;
                mapWithDotnetRef.map.entities.push(pushpin);
                mapWithDotnetRef.mapMarkers.push(pushpin);

                if (markerData.clickable) {
                    var h = Microsoft.Maps.Events.addHandler(pushpin, 'click', function (e) {
                        mapWithDotnetRef.ref.invokeMethodAsync('MarkerClicked', markerData.id);
                    });
                    pushpin._handlers = pushpin._handlers || [];
                    pushpin._handlers.push(h);
                }
                if (markerData.draggable) {
                    var hDrag = Microsoft.Maps.Events.addHandler(pushpin, 'drag', function (e) {
                        if (e && e.location) mapWithDotnetRef.ref.invokeMethodAsync('MarkerDrag', markerData.id, { Latitude: e.location.latitude, Longitude: e.location.longitude });
                    });
                    var hDragEnd = Microsoft.Maps.Events.addHandler(pushpin, 'dragend', function (e) {
                        if (e && e.location) mapWithDotnetRef.ref.invokeMethodAsync('MarkerDragEnd', markerData.id, { Latitude: e.location.latitude, Longitude: e.location.longitude });
                    });
                    var hDragStart = Microsoft.Maps.Events.addHandler(pushpin, 'dragstart', function (e) {
                        if (e && e.location) mapWithDotnetRef.ref.invokeMethodAsync('MarkerDragStart', markerData.id, { Latitude: e.location.latitude, Longitude: e.location.longitude });
                    });
                    pushpin._handlers = pushpin._handlers || [];
                    pushpin._handlers.push(hDrag, hDragEnd, hDragStart);
                }
            }

            // basic clustering approach: if Microsoft.Maps.ClusterLayer exists, use it
            if (mapWithDotnetRef.enableMarkerClustering && window.Microsoft && Microsoft.Maps && Microsoft.Maps.ClusterLayer) {
                if (!mapWithDotnetRef.clusterLayer) {
                    mapWithDotnetRef.clusterLayer = new Microsoft.Maps.ClusterLayer(mapWithDotnetRef.mapMarkers);
                    mapWithDotnetRef.map.layers.insert(mapWithDotnetRef.clusterLayer);
                } else {
                    mapWithDotnetRef.clusterLayer.setPushpins(mapWithDotnetRef.mapMarkers);
                }
            }
        }
    }
}

export function removeMarkers(elementId, markers) {
    if (elementId && markers && markers.length) {
        let mapWithDotnetRef = getElementIdWithDotnetRef(_bingMapsElementDict, elementId);
        if (mapWithDotnetRef && mapWithDotnetRef.map) {
            for (var i = 0; i < markers.length; i++) {
                let id = markers[i].id || markers[i];
                for (var j = mapWithDotnetRef.mapMarkers.length - 1; j >= 0; j--) {
                    if (mapWithDotnetRef.mapMarkers[j].id === id) {
                        // remove attached event handlers
                        var mp = mapWithDotnetRef.mapMarkers[j];
                        if (mp._handlers && mp._handlers.length) {
                            for (var hh = 0; hh < mp._handlers.length; hh++) {
                                try { Microsoft.Maps.Events.removeHandler(mp._handlers[hh]); } catch (ex) { }
                            }
                        }
                        mapWithDotnetRef.map.entities.remove(mp);
                        mapWithDotnetRef.mapMarkers.splice(j, 1);
                    }
                }
            }

            if (mapWithDotnetRef.clusterLayer) {
                mapWithDotnetRef.clusterLayer.setPushpins(mapWithDotnetRef.mapMarkers);
            }
        }
    }
}

export function dispose(elementId) {
    for (let i = 0; i < _bingMapsElementDict.length; i++) {
        if (_bingMapsElementDict[i].key === elementId) {
            if (_bingMapsElementDict[i].value.map) {
                _bingMapsElementDict[i].value.map.dispose();
            }
            _bingMapsElementDict.splice(i, 1);
            break;
        }
    }
}
