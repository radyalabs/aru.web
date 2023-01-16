var MapsGoogleLocationHistory = function () {
    var mapMarker = function (locationHisList) {
        var latCenter = -6.903363;
        var lngCenter = 107.6080523;

        //if (locationHisList.length > 0) {
        //    latCenter = locationHisList[0].latitude;
        //    lngCenter = locationHisList[0].longitude;
        //}

        var map = new GMaps({
            div: '#dashboard-map',
            lat: latCenter,
            lng: lngCenter,
        });

        jQuery.each(locationHisList, function (i, val) {
            
            if (val.longitude == null && val.latitude == null) {
                val.latitude = latCenter;
                val.longitude = lngCenter;
            }
            
            var iconUrl = '';
            if (val.is_active == true) {
                iconUrl = baseUrl + '/images/pin-green-32.png';
            } else {
                iconUrl = baseUrl + '/images/pin-red-36.png';
            }
            
            map.addMarker({
                lat: val.latitude,
                lng: val.longitude,
                title: val.label,
                infoWindow: {
                    content: '<span style="color:#000">' + val.label + '<br /> Last Checkin : ' + val.checkin_date +'</span>'
                },
                icon: iconUrl
            });
        });

        map.setZoom(13);
    }

    return {
        //main function to initiate map samples
        init: function (locationHisList) {
            mapMarker(locationHisList);
        }
    };

}();