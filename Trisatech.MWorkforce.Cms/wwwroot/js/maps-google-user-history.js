var MapsGoogleLocationHistory = function () {
    var mapMarker = function (locationHisList) {
        var latCenter = 0;
        var lngCenter = 0;

        if (locationHisList.length > 0) {
            latCenter = locationHisList[0].latitude;
            lngCenter = locationHisList[0].longitude;
        }
        
        var map = new GMaps({
            div: '#task-map',
            lat: latCenter,
            lng: lngCenter,
        });
        
        jQuery.each(locationHisList, function (i, val) {    
            map.addMarker({
                lat: val.latitude,
                lng: val.longitude,
                title: val.label,
                infoWindow: {
                    content: '<span style="color:#000">' + val.label + '</span>'
                }
            });
        });
        
        map.setZoom(12);
    }

    function initMap(locationList, wayRouteData) {
        var directionsService = new google.maps.DirectionsService;
        //var directionsDisplay = new google.maps.DirectionsRenderer;
        var latCenter = 0;
        var lngCenter = 0;

        if (wayRouteData !== null && wayRouteData.length > 0) {
            latCenter = wayRouteData[0].latitude;
            lngCenter = wayRouteData[0].longitude;
        }else if (locationList !== null && locationList.length > 0) {
            latCenter = locationList[0].latitude;
            lngCenter = locationList[0].longitude;
        }

        var map = new google.maps.Map(document.getElementById('task-map'), {
            zoom: 12,
            center: { lat: latCenter, lng: lngCenter }
        });

        drawUserRoute(map, wayRouteData);
        //directionsDisplay.setMap(map);
        //calculateAndDisplayRoute(directionsService, directionsDisplay, locationList);
        
        //document.getElementById('submit').addEventListener('click', function () {
        //    calculateAndDisplayRoute(directionsService, directionsDisplay);
        //});
    }

    function drawUserRoute(map, wayRouteData) {

        var path = [];

        for (var i = 1; i < wayRouteData.length; i++) {
            if (wayRouteData[i] !== null) {
                path.push(new google.maps.LatLng(wayRouteData[i].latitude, wayRouteData[i].longitude));
            }
        }
        
        var userWayRoute = new google.maps.Polyline({
            path: path,
            geodesic: true,
            strokeColor: '#FF0000',
            strokeOpacity: 1.0,
            strokeWeight: 2
        });

        userWayRoute.setMap(map);
    }

    function calculateAndDisplayRoute(directionsService, directionsDisplay, locationList) {
        var waypts = [];
        var startLocation = new google.maps.LatLng(locationList[0].latitude, locationList[0].longitude);
        var endLocation = new google.maps.LatLng(locationList[locationList.length - 1].latitude, locationList[locationList.length - 1].longitude);

        for (var i = 1; i < locationList.length; i++) {
            if (locationList[i] !== null) {
                waypts.push({
                    location: new google.maps.LatLng(locationList[i].latitude, locationList[i].longitude),
                    stopover: true
                });
            }
        }

        directionsService.route({
            origin: startLocation,
            destination: endLocation,
            waypoints: waypts,
            optimizeWaypoints: true,
            travelMode: 'DRIVING'
        }, function (response, status) {
            if (status === 'OK') {
                var totalDistance = 0;
                directionsDisplay.setDirections(response);
                var route = response.routes[0];
                var summaryPanel = document.getElementById('directions-panel');
                summaryPanel.innerHTML = '';
                // For each route, display summary information.
                for (var i = 0; i < route.legs.length; i++) {
                    var routeSegment = i + 1;

                    summaryPanel.innerHTML += '<b>Route Segment: ' + locationList[i].label +
                        '</b><br>';

                    summaryPanel.innerHTML += route.legs[i].start_address + ' to ';
                    summaryPanel.innerHTML += route.legs[i].end_address + '<br>';
                    summaryPanel.innerHTML += route.legs[i].distance.text + '<br><br>';
                    totalDistance += route.legs[i].distance.value;
                }

                console.log(totalDistance);
            } else {
                window.alert('Directions request failed due to ' + status);
            }
        });

    }
    
    return {
        //main function to initiate map samples
        init: function (locationHisList, wayRouteData) {
            mapMarker(locationHisList);
            initMap(locationHisList, wayRouteData);
        }
    };

}();

var MapsGoogleDrawRoute = function () {
    var map;
    var directionsService;
    var directionsDisplay;
    var markerArray = [];
    var stepDisplay;
    var locationHistory = [];

    function initMap(locationHist, wayRouteData) {
        var latCenter = 0;
        var lngCenter = 0;
        locationHistory = locationHist;

        if (locationHist.length > 0) {
            latCenter = locationHist[0].latitude;
            lngCenter = locationHist[0].longitude;
        }

        directionsService = new google.maps.DirectionsService;
        directionsDisplay = new google.maps.DirectionsRenderer;
        map = new google.maps.Map(document.getElementById('task-map'), {
            zoom: 10,
            center: { lat: latCenter, lng: lngCenter }
        });

        directionsDisplay.setMap(map);
        stepDisplay = new google.maps.InfoWindow();

        if (locationHist.length > 1) {
            calculateAndDisplayRoute(locationHist, directionsService, directionsDisplay);
        } else {
            showSteps();
        }
    }

    function calculateAndDisplayRoute(wayRouteData, directionsService, directionsDisplay) {
        var waypts = [];
        var origin = new google.maps.LatLng(0, 0);
        var destination = new google.maps.LatLng(0, 0);

        for (var i = 0; i < wayRouteData.length; i++) {
            if (i === 0) {
                origin = new google.maps.LatLng(wayRouteData[i].latitude, wayRouteData[i].longitude);
            } else if (i === wayRouteData.length - 1) {
                destination = new google.maps.LatLng(wayRouteData[i].latitude, wayRouteData[i].longitude);
            } else {
                waypts.push({
                    location: new google.maps.LatLng(wayRouteData[i].latitude, wayRouteData[i].longitude),
                    stopover: true
                });
            }
        }

        directionsService.route({
            origin: origin,
            destination: destination,
            waypoints: waypts,
            optimizeWaypoints: true,
            travelMode: 'DRIVING'
        },
        function (response, status) {
            if (status === 'OK') {
                directionsDisplay.setDirections(response);
                directionsDisplay.setOptions({ suppressMarkers: true });
                showSteps();
            } else {
                showSteps();
                window.alert('Directions request failed due to ' + status);
            }
        });
    }

    function showSteps() {
        for (var i = 0; i < locationHistory.length; i++) {
            var marker = new google.maps.Marker({
                position: new google.maps.LatLng(locationHistory[i].latitude, locationHistory[i].longitude),
                label: '' + (i + 1) + '',
                title: locationHistory[i].label,    
                map: map
            });
            attachInstructionText(marker, locationHistory[i].label);
            markerArray[i] = marker;
        }
    }

    function attachInstructionText(marker, text) {
        google.maps.event.addListener(marker, 'click', function () {
            stepDisplay.setContent(text);
            stepDisplay.open(map, marker);
        });
    }
    return {
        init: function (locationHist, wayRouteData) {
            console.log(locationHist, wayRouteData);
            initMap(locationHist, wayRouteData);
        }
    };
}();