var MapsGoogle = function () {
    
    var mapMarker = function (name, latitude, longitude) {
        var map = new GMaps({
            div: '#task-map',
            lat: latitude,
            lng: longitude,
        });

        console.log("label : " + name);

        map.addMarker({
            lat: latitude,
            lng: longitude,
            title: name,
            infoWindow: {
                content: '<span style="color:#000">'+name+'</span>'
            },
            icon: baseUrl + '/images/pin-red-36.png'
        });

        map.setZoom(12);
    }
    
    return {
        //main function to initiate map samples
        init: function (name, latitude, longitude) {
            mapMarker(name, latitude, longitude);
        }

    };

}();